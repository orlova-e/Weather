using System.Reflection;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Weather.Services.MappingModels;
using Weather.Services.Models;

namespace Weather.Services.Files;

public class ExcelService<T> : IFileService<T>
    where T : class, IMapModel, new()
{
    private readonly ILogger<ExcelService<T>> _logger;

    public ExcelService(ILogger<ExcelService<T>> logger)
    {
        _logger = logger;
    }

    private static MapModelInfo<T> GetProperties<T>()
        where T : class, IMapModel, new()
    {
        var entity = new T();
        var entityType = entity.GetType();
                    
        var mapProperties = entityType.GetProperties(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.SetProperty |
                BindingFlags.GetProperty)?
            .Where(x => x.GetCustomAttribute<ColumnMapAttribute>() is not null)?
            .Select(x => new
            {
                Info = x,
                Attribute = x.GetCustomAttribute<ColumnMapAttribute>()
            })
            .OrderBy(x => x.Attribute.Index)
            .Select(x => x.Info)
            .ToList();
        
        return new MapModelInfo<T>()
        {
            Model = entity,
            PropertyInfos = mapProperties
        };
    }
    
    public FileReadMessages Validate(
        Stream file,
        int headerColumns = 1)
    {
        return TryReadFile(file, out _, headerColumns);
    }

    public FileReadMessages TryReadFile(
        Stream file,
        out IList<T> entities,
        int skipRows = 1)
    {
        entities = new List<T>();
        IWorkbook workbook;
        
        try
        {
            workbook = new XSSFWorkbook(file);
        }
        catch (Exception exc)
        {
            _logger.LogError("Couldn't open file:\n{message}\n{stackTrace}", 
                exc.Message, exc.StackTrace);
            
            return FileReadMessages.UnableToOpen;
        }

        try
        {
            for (int s = 0; s < workbook.NumberOfSheets; s++)
            {
                var sheet = workbook.GetSheetAt(s);
                
                for (int r = 0; r <= sheet.LastRowNum; r++)
                {
                    if(r < skipRows)
                        continue;

                    var mapModelInfo = GetProperties<T>();

                    var row = sheet.GetRow(r);
                    var cells = row.Cells;

                    for (int c = 0; c < cells.Count; c++)
                    {
                        var propertyInfo = mapModelInfo.PropertyInfos.ElementAt(c);

                        if (cells[c].CellType is
                            CellType.Formula or
                            CellType.Unknown or
                            CellType.Error)
                        {
                            _logger.LogError("Cell value should have a primitive type");
                            return FileReadMessages.UnableToMap;
                        }

                        dynamic propertyValue = cells[c].CellType switch
                        {
                            CellType.Boolean => cells[c].BooleanCellValue,
                            CellType.Numeric => cells[c].NumericCellValue,
                            CellType.String => cells[c].StringCellValue,
                            CellType.Blank => propertyInfo.PropertyType.IsValueType ?
                                Activator.CreateInstance(propertyInfo.PropertyType) :
                                null,
                            _ => throw new TypeInitializationException(propertyInfo.PropertyType.FullName, null)
                        };

                        if (propertyValue is string && 
                            string.IsNullOrWhiteSpace(propertyValue))
                        {
                            propertyValue = propertyInfo.PropertyType.IsValueType
                                ? Activator.CreateInstance(propertyInfo.PropertyType)
                                : null;
                            
                            propertyInfo.SetValue(mapModelInfo.Model, propertyValue, null);
                            
                            continue;
                        }
                        
                        var valueWithChangedType = Convert.ChangeType(
                            propertyValue,
                            Nullable.GetUnderlyingType(propertyInfo.PropertyType) ??
                                propertyInfo.PropertyType);
                        
                        propertyInfo.SetValue(mapModelInfo.Model, valueWithChangedType, null);
                    }
                    
                    entities.Add(mapModelInfo.Model);
                }
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("File doesn't match the format\n{message}\n{stackTrace}", 
                exc.Message, exc.StackTrace);
            
            return FileReadMessages.UnableToRead;
        }

        return FileReadMessages.Normal;
    }
}
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

    private static MethodInfo CachedPropertiesMethod { get; } = typeof(ExcelService<T>)
        .GetMethod(nameof(ExcelService<T>.GetProperties));

    private static Func<(T, List<PropertyInfo>)> GetPropertiesCachedDelegate { get; } =
        (Func<(T, List<PropertyInfo>)>) Delegate
            .CreateDelegate(typeof(Func<(T, List<PropertyInfo>)>), CachedPropertiesMethod);

    private static (T, List<PropertyInfo>) GetProperties<T>()
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
        
        return (entity, mapProperties);
    }
    
    public FileReadMessages Validate(
        FileStream file,
        int headerColumns = 1)
    {
        return TryReadFile(file, out _, headerColumns);
    }

    public FileReadMessages TryReadFile(
        FileStream file,
        out IList<T> entities,
        int headerColumns = 1)
    {
        entities = new List<T>();
        IWorkbook workbook;
        
        try
        {
            workbook = new XSSFWorkbook(file);
        }
        catch (Exception exc)
        {
            _logger.LogError("Couldn't open file '{name}':\n{message}\n{stackTrace}", 
                file.Name, exc.Message, exc.StackTrace);
            
            return FileReadMessages.UnableToOpen;
        }

        try
        {
            for (int s = 0; s < workbook.NumberOfSheets; s++)
            {
                var sheet = workbook.GetSheetAt(s);
                
                for (int r = 1; r <= sheet.LastRowNum; r++)
                {
                    if(r <= headerColumns)
                        continue;

                    var (entity, mapProperties) = GetPropertiesCachedDelegate();

                    var row = sheet.GetRow(r);
                    var cells = row.Cells;

                    for (int c = 0; c < cells.Count; c++)
                    {
                        var propertyInfo = mapProperties.ElementAt(c);

                        if (cells[c].CellType is CellType.Blank or
                            CellType.Formula or
                            CellType.Unknown or
                            CellType.Error)
                        {
                            _logger.LogError("Cell value should have a primitive type");
                            return FileReadMessages.UnableToMap;
                        }
                        
                        var propertyValue = cells[c].StringCellValue;
                        var valueWithChangedType = Convert.ChangeType(propertyValue, propertyInfo.PropertyType);
                        propertyInfo.SetValue(entity, valueWithChangedType, null);
                    }
                    
                    entities.Add(entity);
                }
            }
        }
        catch (Exception exc)
        {
            _logger.LogError("File '{name}' doesn't match the format\n{message}\n{stackTrace}", 
                file.Name, exc.Message, exc.StackTrace);
            
            return FileReadMessages.UnableToRead;
        }

        return FileReadMessages.Normal;
    }
}
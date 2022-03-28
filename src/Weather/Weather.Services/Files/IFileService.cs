using Weather.Services.MappingModels;
using Weather.Services.Models;

namespace Weather.Services.Files;

public interface IFileService<T>
    where T : class, IMapModel, new()
{
    FileReadMessages Validate(Stream file, int headerColumns = 1);

    FileReadMessages TryReadFile(Stream file, out IList<T> entities, int skipRows = 1);
}
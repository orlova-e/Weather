using Weather.Services.MappingModels;
using Weather.Services.Models;

namespace Weather.Services.Files;

public interface IFileService<T>
    where T : class, IMapModel, new()
{
    FileReadMessages Validate(FileStream file);

    FileReadMessages TryReadFile(FileStream file, out IList<T> entities, int headerColumns = 1);
}
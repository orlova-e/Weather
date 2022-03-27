using System.Reflection;

namespace Weather.Services.MappingModels;

public class MapModelInfo<T>
    where T : IMapModel
{
    public T Model { get; set; }
    public List<PropertyInfo> PropertyInfos { get; set; }
}
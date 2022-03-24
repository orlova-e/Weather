namespace Weather.Services.MappingModels;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnMapAttribute : Attribute
{
    public int Index { get; }
    public string Column { get; }

    public ColumnMapAttribute(int index, string column)
    {
        if (index < 1)
            throw new ArgumentException("Index must be a positive value", nameof(index));
        
        Index = index;
        Column = column;
    }
}
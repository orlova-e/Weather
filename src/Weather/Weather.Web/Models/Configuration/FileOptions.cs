namespace Weather.Web.Models.Configuration;

public class FileSettings
{
    public int SkipRows { get; set; }
    public IEnumerable<string> Extensions { get; set; }
}
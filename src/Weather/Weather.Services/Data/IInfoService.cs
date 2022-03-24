namespace Weather.Services.Data;

public interface IInfoService
{
    Task<IEnumerable<int>> GetAvailableYears();
    Task<IEnumerable<int>> GetAvailableMonths();
}
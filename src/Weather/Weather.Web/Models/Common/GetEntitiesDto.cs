using Weather.Infrastructure.Conditions;

namespace Weather.Web.Models.Common;

public class GetEntitiesDto
{
    public int Page { get; set; } = 1;
    public int ItemsNumber { get; set; } = 20;
    public SortBy SortBy { get; set; } = SortBy.Desc;
}
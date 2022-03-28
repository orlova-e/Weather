using MediatR;
using Microsoft.AspNetCore.Mvc;
using Weather.Web.Models.Common;
using Weather.Web.Models.Weather;
using Weather.Web.Services.Commands.Weather;
using Weather.Web.Services.Commands;

namespace Weather.Web.Controllers;

public class WeatherController : Controller
{
    private readonly IMediator _mediator;

    public WeatherController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("load")]
    public async Task<IActionResult> LoadAsync([FromForm] LoadWeatherDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);
        
        var result = await _mediator.Send(new LoadWeatherDataRequest(dto));
        return this.Redirect(result, "List", "Weather");
    }
    
    [HttpGet]
    [Route("load")]
    public IActionResult Load()
    {
        return View();
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> ListAsync([FromQuery] GetEntitiesDto dto)
    {
        var result = await _mediator.Send(new GetWeatherConditionsRequest(dto));
        return this.AsViewResult(result);
    }
}
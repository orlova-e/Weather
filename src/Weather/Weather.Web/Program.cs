using Weather.Infrastructure.Configuration;
using Weather.Services.Configuration;
using Weather.Web.Services.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services
    .AddInfrastructure(builder.Configuration)
    .AddServices()
    .AddWebServices(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app
        .UseDeveloperExceptionPage()
        .UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting();

app.MapDefaultControllerRoute();

app.Run();
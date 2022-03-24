var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app
        .UseExceptionHandler("/Home/Error")
        .UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting();

app.MapDefaultControllerRoute();

app.Run();
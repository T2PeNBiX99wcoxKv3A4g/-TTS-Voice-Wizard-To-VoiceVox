using System.Net;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using TTSToVOICEVOX.Configs;
using TTSToVOICEVOX.Controllers;

Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppDomain.CurrentDomain.BaseDirectory,
    WebRootPath = AppDomain.CurrentDomain.BaseDirectory
});

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

// ReSharper disable StringLiteralTypo
builder.Configuration.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ttssettings.json"), false, true)
    .AddJsonFile(
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ttssettings.{builder.Environment.EnvironmentName}.json"), true,
        true);
// ReSharper restore StringLiteralTypo

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddCommandLine(args);

builder.Services.Configure<ApiConfig>(builder.Configuration.GetSection(ApiConfig.Name));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = ApiController.Title, Version = ApiController.Version });
});
builder.Services.AddHttpLogging(_ => { });

builder.WebHost.ConfigureKestrel((_, serverOptions) => { serverOptions.Listen(IPAddress.Loopback, 8124); });

builder.Logging.AddSimpleConsole(options =>
{
    // options.IncludeScopes = true; // <- これ
    options.SingleLine = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.fff] ";
});

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = ApiController.Title;
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.ConfigObject.Urls =
        [new UrlDescriptor { Name = $"{ApiController.Title} {ApiController.Version}", Url = "v1/swagger.json" }];
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Map("/docs", () => Results.Redirect("/swagger"));

app.Run();
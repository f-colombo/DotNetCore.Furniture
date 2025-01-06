using HealthChecks.UI.Client;
using DotNetCore.Furniture.Data;
using DotNetCore.Furniture.Service;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Furniture.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, config) =>
{
    config.Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration);

});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title="DotNetCore.Furniture", Version="v1" });
    }
);

builder.Services.AddHealthChecks();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.AddConfiguration();
builder.Services.AddDataDependencies(builder.Configuration);
//builder.Services.AddServiceDependencies(builder.Configuration);

builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
});

//builder.Services.AddHealthChecksUI(opt =>
//{
//    opt.SetEvaluationTimeInSeconds(builder.Configuration.GetValue<int>("HealthCheckConfig:EvaluationTimeInSeconds")); //time in seconds between check    
//    opt.MaximumHistoryEntriesPerEndpoint(builder.Configuration.GetValue<int>("HealthCheckConfig:MaxHistoryPerEndpoint")); //maximum history of checks    
//    opt.SetApiMaxActiveRequests(builder.Configuration.GetValue<int>("HealthCheckConfig:ApiMaxActiveRequest")); //api requests concurrency    
//    opt.AddHealthCheckEndpoint("default api", builder.Configuration.GetValue<string>("HealthCheckConfig:HealthCheckEndpoint")); //map health check api  

//    //bypass ssl
//    opt.UseApiEndpointHttpMessageHandler(sp =>
//    {
//        return new HttpClientHandler
//        {
//            ClientCertificateOptions = ClientCertificateOption.Manual,
//            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
//        };
//    });
//}).AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(x =>
    //{
    //    x.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetCore.Furniture");
    //    x.RoutePrefix = string.Empty;
    //});
}

app.UseSerilogRequestLogging();

app.UseCors("corsapp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseHealthChecksUI();

app.MapHealthChecks(builder.Configuration.GetValue<string>("HealthCheckConfig:HealthCheckEndpoint"), new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Data;
using WebApi.Needed;
using WebApi.Service;

var builder = WebApplication.CreateBuilder(args);
// add services to DI container
var services = builder.Services;
var env = builder.Environment;

builder.Services.AddSwaggerGen();
services.AddDbContext<DataContext>();
services.AddCors();
services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();


app.Run();

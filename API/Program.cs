using API;
using API.Extensions;
using API.Middleware;
using Application;
using Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(((context, configuration) =>
{
    configuration.WriteTo.Console();
    configuration.ReadFrom.Configuration(context.Configuration);
}));

builder.Services.AddSwagger();
builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

builder.Services.AddCorsExtension();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseLockedOutMiddleware();
app.MapControllers();

app.UseSerilogRequestLogging();

app.Run();
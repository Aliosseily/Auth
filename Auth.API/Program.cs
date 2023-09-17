using Auth.API.Extensions;
using Auth.API.Middlewares;
using Auth.Core.Authentication.Jwt;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.


// Read configuration from appsettings.json
var serilogConfiguration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(serilogConfiguration)
	.CreateLogger();



builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddApplicationServices(configuration);
builder.Services.AddIdentityServices(configuration);

builder.Services.ConfigureOptions<JwtOptionsSetup>();

builder.Services.AddTransient<ExceptionMiddleware>();



builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = "localhost:6379";
	options.InstanceName = "MyRedisCache";
});



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<SensitiveEndpointLoggingMiddleware>();


app.MapControllers();

app.Run();

using PriceScoutAPI.Helpers;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

//--------------------------------------------------------------------------------
// --- Log System ----------------------------------------------------------------
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/Walks_Log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Error()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//--------------------------------------------------------------------------------
// --- Add services to the container ---------------------------------------------
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
//--------------------------------------------------------------------------------

//--------------------------------------------------------------------------------
// --- Add services Helpers (Inject IConfiguration )  ----------------------------
builder.Services.AddScoped<AmazonHelper>();
builder.Services.AddScoped<CurrencyHelper>();
builder.Services.AddScoped<AliExpressHelper>();

//--------------------------------------------------------------------------------
//--------------------------------------------------------------------------------


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

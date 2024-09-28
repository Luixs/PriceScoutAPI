
using Polly;
using PriceScoutAPI.Helpers;
using PriceScoutAPI.Interfaces;
using Serilog;


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
// --- Http Clients configurations -----------------------------------------------
builder.Services.AddHttpClient<IAliExpressHelper, AliExpressHelper>().AddPolicyHandler(
    Policy.Handle<HttpRequestException>()
    .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)))
    .AsAsyncPolicy<HttpResponseMessage>()
);

builder.Services.AddHttpClient<IAmazonHelper, AmazonHelper>().AddPolicyHandler(
    Policy.Handle<HttpRequestException>()
    .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)))
    .AsAsyncPolicy<HttpResponseMessage>()
);
//--------------------------------------------------------------------------------
// --- Add services to the container ---------------------------------------------
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
//--------------------------------------------------------------------------------

//--------------------------------------------------------------------------------
// --- Add services Helpers (Inject IConfiguration )  ----------------------------
builder.Services.AddScoped<AmazonHelper>();
builder.Services.AddScoped<CurrencyHelper>();
//builder.Services.AddScoped<AliExpressHelper>();

// --- Adding the interface, insted of the class
builder.Services.AddScoped<IBestOptionHelper, BestOptionHelper>(); // --- Linked both!
builder.Services.AddScoped<IAliExpressHelper, AliExpressHelper>(); // --- Linked both!
builder.Services.AddScoped<IAmazonHelper, AmazonHelper>(); // --- Linked both!

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
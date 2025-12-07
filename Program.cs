var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddHttpClient<IWeatherService, WeatherService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache(); // session uses this
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});


var app = builder.Build();
app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();


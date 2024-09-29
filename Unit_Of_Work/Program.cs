using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Unit_Of_Work.Common;
using Unit_Of_Work.Data;
using Unit_Of_Work.Repository;
using Unit_Of_Work.Repository.Interface;
using Unit_Of_Work.Services;
using Unit_Of_Work.Settings;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var cacheDurationInHours = builder.Configuration.GetValue<int>("CacheSettings:CacheDurationInHours");
var cacheDuration = TimeSpan.FromHours(cacheDurationInHours);
builder.Services.AddSingleton(new CacheSetting
{
    Duration = cacheDuration
});
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379,abortConnect=false";
});

#region Redis
var redisConfiguration = new RedisConfiguration();
builder.Configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
if (string.IsNullOrEmpty(redisConfiguration.ConnectionString) || !redisConfiguration.Enabled)
{
    return;
}
builder.Services.AddSingleton(redisConfiguration);
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    return ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString);
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConfiguration.ConnectionString;
});
builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
builder.Services.Configure<RedisConfiguration>(builder.Configuration.GetSection("RedisConfiguration"));


#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<QlSinhVienContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DbSinhVien"));
});

builder.Services.AddScoped<ISinhVienRepository, SinhVienRepository>();
builder.Services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandleMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

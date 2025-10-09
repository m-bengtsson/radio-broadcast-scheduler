using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<Schedule>();
// DotNetEnv.Env.Load();

builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAll", policy =>
   {
      policy.AllowAnyOrigin();
   });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddDbContext<RadioSchedulerContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// builder.Services.AddDbContext<RadioSchedulerContext>(options => options.UseSqlite("DataSource=RadioScheduler.db"));
var app = builder.Build();

app.UseCors("AllowAll");
app.MapControllers();

app.MapGet("/api/broadcasts", (RadioSchedulerContext context) =>
{
   return context.Broadcasts.ToList();
});

app.Run();
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<Schedule>();

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

var app = builder.Build();

app.UseCors("AllowAll");
app.MapControllers();

app.Run();
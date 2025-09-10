var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddSwaggerUi();
var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
   app.MapOpenApi();
   app.UseSwagger();
   app.UseSwaggerUI();
}

var schedule = new Schedule();
schedule.CreateSchedule();

app.MapGet("/api/schedule", () => schedule.WeeklySchedule);


app.Run();

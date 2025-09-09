var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var schedule = new Schedule();
schedule.CreateSchedule();

app.MapGet("/api/schedule", () => schedule.WeeklySchedule);

app.Run();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAll", policy =>
   {
      policy.AllowAnyOrigin();
   });
});

var app = builder.Build();
app.UseCors("AllowAll");

// GET Schedule
var schedule = new Schedule();
schedule.CreateSchedule();

app.MapGet("/api/schedule", () => schedule.WeeklySchedule);

// GET Today's Schedule
app.MapGet("/api/today", () =>
{
   var today = DateOnly.FromDateTime(DateTime.Now);
   var daySchedule = schedule.WeeklySchedule.FirstOrDefault(ds => ds.Date == today);

   if (daySchedule != null)
   {
      return Results.Ok(daySchedule);
   }
   else
   {
      return Results.NotFound(new { Message = "No schedule found for today." });
   }

});


app.Run();
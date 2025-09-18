using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.OpenApi.Any;
using RadioScheduler;

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
Schedule schedule = new Schedule();
schedule.CreateSchedule();

List<DaySchedule> weekSchedule = schedule.WeeklySchedule;

app.MapGet("/api/schedule", () => weekSchedule);

// GET Today's Schedule
app.MapGet("/api/today", () =>
{
   var today = DateOnly.FromDateTime(DateTime.Now);
   var daySchedule = weekSchedule.FirstOrDefault(ds => ds.Date == today);

   if (daySchedule != null)
   {
      return Results.Ok(daySchedule);
   }
   else
   {
      return Results.NotFound(new { Message = "No schedule found for today." });
   }

});

// GET Event 
app.MapGet("/api/event/{id}", (Guid id) =>
{
   var flatList = weekSchedule.SelectMany(i => i.Broadcasts).Select(i => i.Id);

   foreach (var existingId in flatList)
   {
      if (existingId == id)
      {
         var broadcasts = weekSchedule.SelectMany(i => i.Broadcasts);
         return Results.Ok(broadcasts.FirstOrDefault(i => i.Id == id));
      }
   }
   return Results.Ok(flatList);
});


app.Run();
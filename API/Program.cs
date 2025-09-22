using System.Text.Json.Serialization;
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

builder.Services.AddControllers().AddJsonOptions(options =>
{
   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); ;
});


var app = builder.Build();
app.UseCors("AllowAll");
app.MapControllers();

Schedule schedule = new Schedule();

// GET Schedule
app.MapGet("/api/schedule", () => schedule.broadcasts);

// Get today's schedule
app.MapGet("/api/today", () =>
{
   DateOnly today = DateOnly.FromDateTime(DateTime.Now);
   List<BroadcastContent> todaysBroadcasts = schedule.broadcasts.Where(b => b.Date == today).OrderBy(b => b.StartTime).ToList();
   return todaysBroadcasts;
});




// GET Today's Schedule
// app.MapGet("/api/today", () =>
// {
//    // Get today's date
//    var today = DateOnly.FromDateTime(DateTime.Now);
//    // Find the DaySchedule for today
//    var daySchedule = weekSchedule.FirstOrDefault(ds => ds.Date == today);

//    if (daySchedule != null)
//    {
//       return Results.Ok(daySchedule);
//    }
//    else
//    {
//       return Results.NotFound(new { Message = "No schedule found for today." });
//    }

// });

// // GET Event 
// app.MapGet("/api/event/{id}", (Guid id) =>
// {
//    // Flatten the list of broadcasts for the week into a single list
//    var flatList = weekSchedule.SelectMany(i => i.Broadcasts).Select(i => i.Id);

//    // Check if the provided ID exists in the flattened list
//    foreach (var existingId in flatList)
//    {
//       // If id matches, return the corresponding BroadcastContent
//       if (existingId == id)
//       {
//          var broadcasts = weekSchedule.SelectMany(i => i.Broadcasts);
//          return Results.Ok(broadcasts.FirstOrDefault(i => i.Id == id));
//       }
//       else
//       {
//          return Results.NotFound(new { Message = "Event not found." });
//       }
//    }
//    return Results.Ok();
// });

// Add event to schedule


// Remove event from schedule
// Reschedule event




//                     HOST               
// Add host to event
// Remove host from event

//                     GUEST
// Add guest to event
// Remove guest from event

app.Run();
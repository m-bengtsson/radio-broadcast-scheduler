using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/schedule")]
public class ScheduleController : ControllerBase
{
   private readonly Schedule _schedule;

   public ScheduleController(Schedule schedule)
   {
      _schedule = schedule;
   }

   [HttpGet]
   public IActionResult GetSchedule()
   {
      // TODO Order by date and time
      return Ok(_schedule.broadcasts);
   }

   [HttpGet("today")]
   public IActionResult GetTodaysSchedule()
   {
      DateOnly today = DateOnly.FromDateTime(DateTime.Now);
      var todaysBroadcasts = _schedule.broadcasts
         .Where(b => b.Date == today)
         .OrderBy(b => b.StartTime)
         .ToList();
      return Ok(todaysBroadcasts);
   }

   [HttpGet("{id}")]
   public IActionResult GetBroadcastById(Guid id)
   {
      var broadcast = _schedule.broadcasts.FirstOrDefault(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      return Ok(broadcast);
   }
}
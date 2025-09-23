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

   [HttpPost]
   public IActionResult AddBroadcast([FromBody] BroadcastDto dto)
   {
      // TODO validate date and time to avoid conflicts
      // TODO validate required fields
      BroadcastContent newBroadcast;

      switch (dto.Type)
      {
         case "LiveSession":
            if (string.IsNullOrEmpty(dto.Host))
            {
               return BadRequest(new { Message = "Host is required for LiveSession." });
            }
            newBroadcast = new LiveSession(dto.Date, dto.Title, dto.StartTime, dto.Duration, dto.Host, dto.CoHost, dto.Guest);
            break;
         case "Reportage":
            newBroadcast = new Reportage(dto.Date, dto.Title, dto.StartTime, dto.Duration);
            break;
         case "Music":
            newBroadcast = new Music(dto.Date, dto.Title, dto.StartTime, dto.Duration);
            break;
         default:
            return BadRequest(new { Message = "Invalid broadcast type." });
      }

      _schedule.broadcasts.Add(newBroadcast);
      return CreatedAtAction(nameof(GetBroadcastById), new { id = newBroadcast.Id }, newBroadcast);

   }
}
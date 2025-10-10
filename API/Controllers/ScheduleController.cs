using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/schedule")]
public class ScheduleController : ControllerBase
{
   private RadioSchedulerContext _db;

   public ScheduleController(RadioSchedulerContext db)
   {
      _db = db;
   }
   // Get full schedule
   [HttpGet]

   public async Task<IActionResult> GetSchedule()
   {
      // TODO Order by date and time
      return Ok(_db.Broadcasts.ToList());
   }
   // Get today's schedule
   [HttpGet("today")]
   public IActionResult GetTodaysSchedule()
   {
      DateOnly today = DateOnly.FromDateTime(DateTime.Now);
      var todaysBroadcasts = _db.Broadcasts
         .Where(b => b.Date == today)
         .OrderBy(b => b.StartTime)
         .ToList();
      return Ok(todaysBroadcasts);
   }
   // Get broadcast by ID
   [HttpGet("{id}")]
   public IActionResult GetBroadcastById(Guid id)
   {
      var broadcast = _db.Broadcasts.FirstOrDefault(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      return Ok(broadcast);
   }
   // Add new broadcast
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
      _db.Broadcasts.Add(newBroadcast);
      _db.SaveChanges();

      return CreatedAtAction(nameof(GetBroadcastById), new { id = newBroadcast.Id }, newBroadcast);

   }

   // Delete broadcast
   [HttpDelete("{id}")]
   public IActionResult DeleteBroadcast(Guid id)
   {
      var broadcast = _db.Broadcasts.FirstOrDefault(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      _db.Broadcasts.Remove(broadcast);
      _db.SaveChanges();
      return NoContent();
   }
   // Reschedule broadcast
   [HttpPatch("{id}")]
   public IActionResult RescheduleBroadcast(Guid id, [FromBody] RescheduleDto dto)
   {
      var broadcast = _db.Broadcasts.FirstOrDefault(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      broadcast.Date = dto.Date;
      broadcast.StartTime = dto.StartTime;
      _db.SaveChanges();
      return Ok(broadcast);
   }
   // Add cohost to LiveSession
   [HttpPatch("cohost/{id}")]
   public IActionResult AddCoHost(Guid id, [FromBody] UpdateBroadcastDto updateDto)
   {
      var broadcast = _db.Broadcasts.FirstOrDefault(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (broadcast is LiveSession liveSession)
      {
         liveSession.CoHost = updateDto.CoHost;
         return Ok(liveSession);
      }
      _db.SaveChanges();

      return BadRequest(new { Message = "Cohost can only be added to LiveSession." });
   }
   // Remove cohost from LiveSession
   [HttpDelete("cohost/{id}")]
   public IActionResult RemoveCoHost(Guid id)
   {
      var broadcast = _db.Broadcasts.FirstOrDefault(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (broadcast is LiveSession liveSession)
      {
         liveSession.CoHost = null;
         return Ok(liveSession);
      }
      _db.SaveChanges();
      return BadRequest(new { Message = "Cohost can only be removed from LiveSession." });
   }
   // Add guest to LiveSession
   [HttpPatch("guest/{id}")]
   public IActionResult AddGuest(Guid id, [FromBody] UpdateBroadcastDto updateDto)
   {
      var broadcast = _db.Broadcasts.FirstOrDefault(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (broadcast is LiveSession liveSession)
      {
         liveSession.Guest = updateDto.Guest;
         return Ok(liveSession);
      }
      _db.SaveChanges();
      return BadRequest(new { Message = "Guest can only be added to LiveSession." });
   }
   // Remove guest from LiveSession
   [HttpDelete("guest/{id}")]
   public IActionResult RemoveGuest(Guid id)
   {
      var broadcast = _db.Broadcasts.FirstOrDefault(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (broadcast is LiveSession liveSession)
      {
         liveSession.Guest = null;
         return Ok(liveSession);
      }
      _db.SaveChanges();
      return BadRequest(new { Message = "Guest can only be removed from LiveSession." });
   }
}
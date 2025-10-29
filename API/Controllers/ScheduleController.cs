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
      try
      {
         var broadcasts = await _db.Broadcasts.ToListAsync();
         return Ok(broadcasts);

      }
      catch (Exception ex)
      {
         return StatusCode(500, new { Message = "An error occurred while retrieving the schedule from the database.", Error = ex.Message });
      }
   }
   // Get today's schedule
   [HttpGet("today")]
   public async Task<IActionResult> GetTodaysSchedule()
   {
      try
      {
         DateOnly today = DateOnly.FromDateTime(DateTime.Now);
         var todaysBroadcasts = await _db.Broadcasts
            .Where(b => b.Date == today)
            .OrderBy(b => b.StartTime)
            .ToListAsync();
         return Ok(todaysBroadcasts);

      }
      catch (Exception ex)
      {
         return StatusCode(500, new { Message = "An error occurred while retrieving today's schedule from the database.", Error = ex.Message });
      }
   }
   // Get broadcast by ID
   [HttpGet("{id}")]
   public async Task<IActionResult> GetBroadcastById(Guid id)
   {
      try
      {
         var broadcast = await _db.Broadcasts.FirstOrDefaultAsync(b => b.Id == id);
         if (broadcast == null)
         {
            return NotFound(new { Message = "Broadcast not found." });
         }
         return Ok(broadcast);

      }
      catch (Exception ex)
      {
         return StatusCode(500, new { Message = "An error occurred while retrieving the broadcast from the database.", Error = ex.Message });
      }
   }
   // Add new broadcast
   [HttpPost]
   public async Task<IActionResult> AddBroadcast([FromBody] BroadcastDto dto)
   {
      // TODO validate date and time to avoid conflicts
      // TODO validate required fields
      BroadcastContent newBroadcast;
      try
      {
         if (string.IsNullOrEmpty(dto.Type) || string.IsNullOrEmpty(dto.Title) || dto.Date == default || dto.StartTime == default || dto.Duration == TimeSpan.Zero)
         {
            return BadRequest(new { Message = "Missing required fields." });
         }

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
         await _db.Broadcasts.AddAsync(newBroadcast);
         await _db.SaveChangesAsync();

         return CreatedAtAction(nameof(GetBroadcastById), new { id = newBroadcast.Id }, newBroadcast);

      }
      catch (Exception ex)
      {
         return BadRequest(new { Message = "Invalid broadcast.", Error = ex.Message });

      }
   }

   // Delete broadcast
   [HttpDelete("{id}")]
   public async Task<IActionResult> DeleteBroadcast(Guid id)
   {
      var broadcast = await _db.Broadcasts.FirstOrDefaultAsync(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      _db.Broadcasts.Remove(broadcast);
      await _db.SaveChangesAsync();
      return NoContent();
   }
   // Reschedule broadcast
   [HttpPatch("{id}")]
   public async Task<IActionResult> RescheduleBroadcast(Guid id, [FromBody] RescheduleDto dto)
   {
      var broadcast = await _db.Broadcasts.FirstOrDefaultAsync(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      broadcast.Date = dto.Date;
      broadcast.StartTime = dto.StartTime;

      await _db.SaveChangesAsync();
      return Ok(broadcast);
   }
   // Add cohost to LiveSession
   [HttpPatch("cohost/{id}")]
   public async Task<IActionResult> AddCoHost(Guid id, [FromBody] UpdateBroadcastDto updateDto)
   {
      var broadcast = await _db.Broadcasts.FirstOrDefaultAsync(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (broadcast is LiveSession liveSession)
      {
         liveSession.CoHost = updateDto.CoHost;
         await _db.SaveChangesAsync();

         return Ok(liveSession);
      }

      return BadRequest(new { Message = "Cohost can only be added to LiveSession." });
   }
   // Remove cohost from LiveSession
   [HttpDelete("cohost/{id}")]
   public async Task<IActionResult> RemoveCoHost(Guid id)
   {
      var broadcast = await _db.Broadcasts.FirstOrDefaultAsync(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (broadcast is LiveSession liveSession)
      {
         liveSession.CoHost = null;
         await _db.SaveChangesAsync();

         return Ok(liveSession);
      }
      //_db.SaveChanges();
      return BadRequest(new { Message = "Cohost can only be removed from LiveSession." });
   }
   // Add guest to LiveSession
   [HttpPatch("guest/{id}")]
   public async Task<IActionResult> AddGuest(Guid id, [FromBody] UpdateBroadcastDto updateDto)
   {
      var broadcast = await _db.Broadcasts.FirstOrDefaultAsync(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (broadcast is LiveSession liveSession)
      {
         liveSession.Guest = updateDto.Guest;
         await _db.SaveChangesAsync();

         return Ok(liveSession);
      }
      return BadRequest(new { Message = "Guest can only be added to LiveSession." });
   }
   // Remove guest from LiveSession
   [HttpDelete("guest/{id}")]
   public async Task<IActionResult> RemoveGuest(Guid id)
   {
      var broadcast = await _db.Broadcasts.FirstOrDefaultAsync(b => b.Id == id);
      if (broadcast == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (broadcast is LiveSession liveSession)
      {
         liveSession.Guest = null;
         await _db.SaveChangesAsync();

         return Ok(liveSession);
      }
      return BadRequest(new { Message = "Guest can only be removed from LiveSession." });
   }
}
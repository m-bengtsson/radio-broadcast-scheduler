using Microsoft.AspNetCore.Authorization;
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
   [AllowAnonymous]
   public async Task<IActionResult> GetSchedule()
   {
      // TODO Order by date and time
      try
      {
         var broadcasts = await _db.Broadcasts
                  .OrderBy(b => b.Date)
                  .ThenBy(b => b.StartTime)
                  .ToListAsync();

         var groupedBroadcasts = broadcasts
         .GroupBy(b => b.Date)
         .Select(g => new
         {
            Date = g.Key,
            Broadcasts = g
            .Select(b => BroadcastMapper.ToDto(b)).ToList()
         })
         .ToList();

         return Ok(groupedBroadcasts);
      }
      catch (Exception ex)
      {
         return StatusCode(500, new { Message = "An error occurred while retrieving the schedule from the database.", Error = ex.Message });
      }
   }
   // Get today's schedule
   [HttpGet("today")]
   [AllowAnonymous]
   public async Task<IActionResult> GetTodaysSchedule()
   {
      try
      {
         DateOnly today = DateOnly.FromDateTime(DateTime.Now);


         var todaysBroadcasts = await _db.Broadcasts
           .Where(b => b.Date == today)
            .OrderBy(b => b.StartTime)
            .GroupBy(b => b.Date)   // this will create one group
            .Select(g => new
            {
               Date = g.Key,
               Broadcasts = g.ToList()
            })
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
   [HttpPost]
   public async Task<IActionResult> AddBroadcast([FromBody] AddBroadcastDto dto)
   {
      try
      {
         var broadcast = BroadcastFactory.Create(dto);

         _db.Broadcasts.Add(broadcast);
         await _db.SaveChangesAsync();

         return CreatedAtAction(nameof(GetBroadcastById),
            new { id = broadcast.Id },
            BroadcastMapper.ToDto(broadcast));
      }
      catch (ArgumentException ex)
      {
         return BadRequest(new { Message = ex.Message });
      }
      catch (Exception ex)
      {
         return StatusCode(500, new
         {
            Message = "An error occurred while adding the broadcast.",
            Error = ex.Message
         });
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
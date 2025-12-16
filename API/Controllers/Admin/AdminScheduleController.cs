using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/admin/schedule")]
public class AdminScheduleController : ControllerBase
{
   private RadioSchedulerContext _db;

   public AdminScheduleController(RadioSchedulerContext db)
   {
      _db = db;
   }

   [HttpPost]
   public async Task<IActionResult> AddBroadcast([FromBody] AddBroadcastDto dto)
   {
      try
      {
         var broadcast = BroadcastFactory.Create(dto);

         _db.Broadcasts.Add(broadcast);
         await _db.SaveChangesAsync();

         return CreatedAtAction(
            nameof(ScheduleController.GetBroadcastById),
            "Schedule",
            new { id = broadcast.Id },
            BroadcastMapper.ToDto(broadcast)
         );
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Policy = "AdminOnly")]
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
   public async Task<IActionResult> AddEvent([FromBody] AddEventDto dto)
   {
      try
      {
         var ev = EventFactory.Create(dto);

         _db.Events.Add(ev);
         await _db.SaveChangesAsync();

         return CreatedAtAction(
            nameof(ScheduleController.GetEventById),
            "Schedule",
            new { id = ev.Id },
            EventMapper.ToDto(ev)
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
            Message = "An error occurred while adding the ev.",
            Error = ex.Message
         });
      }
   }

   // Delete ev
   [HttpDelete("{id}")]
   public async Task<IActionResult> DeleteEvent(Guid id)
   {
      try
      {
         var ev = await _db.Events.FirstOrDefaultAsync(b => b.Id == id);
         if (ev == null)
         {
            return NotFound(new { Message = "Event not found." });
         }

         _db.Events.Remove(ev);
         await _db.SaveChangesAsync();

         return NoContent();
      }
      catch (ArgumentException ex)
      {
         return BadRequest(new { Message = ex.Message });
      }
      catch (Exception ex)
      {
         return StatusCode(500, new
         {
            Message = "An error occurred while deleting the ev.",
            Error = ex.Message
         });
      }


   }
   // Reschedule ev
   [HttpPatch("{id}")]
   public async Task<IActionResult> RescheduleEvent(Guid id, [FromBody] RescheduleDto dto)
   {
      var ev = await _db.Events.FirstOrDefaultAsync(b => b.Id == id);
      if (ev == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      ev.Date = dto.Date;
      ev.StartTime = dto.StartTime;

      await _db.SaveChangesAsync();
      return Ok(ev);
   }
   // Add cohost to LiveSession
   [HttpPatch("cohost/{id}")]
   public async Task<IActionResult> AddCoHost(Guid id, [FromBody] UpdateEventDto updateDto)
   {
      var ev = await _db.Events.FirstOrDefaultAsync(b => b.Id == id);
      if (ev == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (ev is LiveSession liveSession)
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
      var ev = await _db.Events.FirstOrDefaultAsync(b => b.Id == id);
      if (ev == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (ev is LiveSession liveSession)
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
   public async Task<IActionResult> AddGuest(Guid id, [FromBody] UpdateEventDto updateDto)
   {
      var ev = await _db.Events.FirstOrDefaultAsync(b => b.Id == id);
      if (ev == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (ev is LiveSession liveSession)
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
      var ev = await _db.Events.FirstOrDefaultAsync(b => b.Id == id);
      if (ev == null)
      {
         return NotFound(new { Message = "Event not found." });
      }
      if (ev is LiveSession liveSession)
      {
         liveSession.Guest = null;
         await _db.SaveChangesAsync();

         return Ok(liveSession);
      }
      return BadRequest(new { Message = "Guest can only be removed from LiveSession." });
   }
}

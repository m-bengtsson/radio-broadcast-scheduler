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
   [AllowAnonymous]
   [HttpGet]
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
   [AllowAnonymous]
   [HttpGet("today")]
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
   [AllowAnonymous]
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
}
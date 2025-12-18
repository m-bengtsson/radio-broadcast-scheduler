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
         var events = await _db.Events
                  .OrderBy(b => b.Date)
                  .ThenBy(b => b.StartTime)
                  .ToListAsync();

         var groupedEvents = events
         .GroupBy(b => b.Date)
         .Select(g => new
         {
            Date = g.Key,
            Events = g
            .Select(b => EventMapper.ToDto(b)).ToList()
         })
         .ToList();

         return Ok(groupedEvents);
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


         var todaysEvents = await _db.Events
           .Where(b => b.Date == today)
            .OrderBy(b => b.StartTime)
            .GroupBy(b => b.Date)   // this will create one group
            .Select(g => new
            {
               Date = g.Key,
               Events = g.ToList()
            })
            .ToListAsync();
         return Ok(todaysEvents);

      }
      catch (Exception ex)
      {
         return StatusCode(500, new { Message = "An error occurred while retrieving today's schedule from the database.", Error = ex.Message });
      }
   }
   // Get ev by ID
   [AllowAnonymous]
   [HttpGet("{id}")]
   public async Task<IActionResult> GetEventById(Guid id)
   {
      try
      {
         var ev = await _db.Events.FirstOrDefaultAsync(b => b.Id == id);
         if (ev == null)
         {
            return NotFound(new { Message = "Event not found." });
         }
         return Ok(ev);

      }
      catch (Exception ex)
      {
         return StatusCode(500, new { Message = "An error occurred while retrieving the ev from the database.", Error = ex.Message });
      }
   }
}
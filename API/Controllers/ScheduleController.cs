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
      return Ok(_schedule.broadcasts);
   }
}
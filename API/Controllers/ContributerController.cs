using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/contributor")]
public class ContributorController : ControllerBase
{
   private RadioSchedulerContext _db;

   public ContributorController(RadioSchedulerContext db)
   {
      _db = db;
   }

   [HttpGet]

   public async Task<IActionResult> GetContributors()
   {
      try
      {
         var contributors = await _db.Contributors
         .Include(c => c.Payments)
         .Select(c => new ContributorDto
         {
            Id = c.Id,
            Name = c.Name,
            Address = c.Address,
            PhoneNumber = c.PhoneNumber,
            Email = c.Email,
            Payments = c.Payments.Select(p => new PaymentDto
            {
               Id = p.Id,
               Amount = p.Amount,
               Date = p.Date
            }).ToList()
         })
         .ToListAsync();
         return Ok(contributors);

      }
      catch (Exception ex)
      {
         return StatusCode(500, new { Message = "An error occurred while retrieving the contributors from the database.", Error = ex.Message });
      }
   }
}
using Microsoft.AspNetCore.Authorization;
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

   // Get logged in contributer profile ( "my profile" )
   [HttpGet("me")]
   public async Task<IActionResult> GetMyProfile()
   {
      try
      {
         // For demo purposes, we just get the first contributor
         var contributor = await _db.Contributors
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
               NetAmount = p.NetAmount,
               BillingPeriod = p.BillingPeriod
            }).ToList()
         })
         .FirstOrDefaultAsync();

         if (contributor == null)
         {
            return NotFound(new { Message = "Contributor profile not found." });
         }

         return Ok(contributor);
      }
      catch (Exception ex)
      {
         return StatusCode(500, new { Message = "An error occurred while retrieving the contributor profile from the database.", Error = ex.Message });
      }
   }

   // Change contributer details
   // [HttpPatch]
   // public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateContributorDto dto){

   // }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Policy = "AdminOnly")]
[ApiController]
[Route("api/admin/contributor")]
public class AdminContributorController : ControllerBase
{
   private RadioSchedulerContext _db;
   private UserManager<IdentityUser> _userManager;

   public AdminContributorController(RadioSchedulerContext db, UserManager<IdentityUser> userManager)
   {
      _db = db;
      _userManager = userManager;
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
               NetAmount = p.NetAmount,
               BillingPeriod = p.BillingPeriod
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

   [HttpPost]
   public async Task<IActionResult> CreateContributor([FromBody] CreateContributorDto dto)
   {
      try
      {
         var user = new IdentityUser
         {
            UserName = dto.Email,
            Email = dto.Email
         };

         var result = await _userManager.CreateAsync(user, dto.Password);

         if (!result.Succeeded)
         {
            return BadRequest(new { Message = "Failed to create user.", Errors = result.Errors });
         }
         var contributer = new Contributor(user, dto.Name, dto.Address, dto.PhoneNumber, dto.Email);

         _db.Contributors.Add(contributer);
         await _db.SaveChangesAsync();

         return Ok(new { Message = "Contributor created successfully.", ContributorId = contributer.Id });
      }
      catch (Exception ex)
      {
         return StatusCode(500, new { Message = "An error occurred while creating the contributor.", Error = ex.Message });
      }

   }
}
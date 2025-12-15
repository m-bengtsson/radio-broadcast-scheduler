using System.Net.Mail;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<Schedule>();

// Add cors policy
builder.Services.AddCors(options =>
{
   options.AddPolicy("RefPolicy", policy =>
   {
      policy.AllowAnyOrigin();
      policy.AllowAnyHeader();
      policy.AllowAnyMethod();
   });
});

// Configure JSON options to handle enum as strings
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

});

builder.Services.AddControllers(options =>
{
   options.Filters.Add(new AuthorizeFilter());
});
// Configure DbContext with SQLite
builder.Services.AddDbContext<RadioSchedulerContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity services
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
   .AddEntityFrameworkStores<RadioSchedulerContext>();

var app = builder.Build();

// 
using (var scope = app.Services.CreateScope())
{
   var context = scope.ServiceProvider.GetRequiredService<RadioSchedulerContext>();
   // Drop the database file
   context.Database.EnsureDeleted();

   // Recreate tables 
   context.Database.EnsureCreated();
   DbSeeder.SeedDatabase(context);

   var contributor = new Contributor(
      "Natalie Brooks",
       "Some Street 1",
       "0701234567",
       "test@example.com"
   );

   var billingPeriod = new DateOnly(2025, 10, 26);

   var payment = new Payment(
       billingPeriod,
       contributor,
       5,
       2

   );

   contributor.AddPayment(payment);

   // Save to DB
   context.Contributors.Add(contributor);
   context.SaveChanges();

   Console.WriteLine("Test contributor and payment saved!");

   // ---------------------------------------------------------
   //   👉 CONFIRM: READ BACK FROM DATABASE
   // ---------------------------------------------------------
   var savedContributor = context.Contributors
       .Include(c => c.Payments)
       .FirstOrDefault();

   if (savedContributor != null)
   {
      Console.WriteLine("=== CONFIRMATION ===");
      Console.WriteLine($"Contributor ID: {savedContributor.Id}");
      Console.WriteLine($"Address: {savedContributor.Address}");
      Console.WriteLine($"Phone: {savedContributor.PhoneNumber}");
      Console.WriteLine($"Email: {savedContributor.Email}");

      Console.WriteLine($"Payments count: {savedContributor.Payments.Count}");

      foreach (var p in savedContributor.Payments)
      {
         Console.WriteLine($" -> Payment ID: {p.Id}");
         Console.WriteLine($"    Amount: {p.NetAmount}");
         Console.WriteLine($"    Date: {p.BillingPeriod}");
      }
   }
   else
   {
      Console.WriteLine("No contributor found in database!");
   }
   // -------------------------------- -------------------------
}

app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();

app.UseCors("RefPolicy");
app.MapControllers();

app.Run();
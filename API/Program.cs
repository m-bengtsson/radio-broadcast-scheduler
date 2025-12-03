using System.Net.Mail;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<Schedule>();


builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAll", policy =>
   {
      policy.AllowAnyOrigin();
      policy.AllowAnyHeader();
      policy.AllowAnyMethod();
   });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

});
builder.Services.AddDbContext<RadioSchedulerContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

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

   var payment = new Payment(
       DateTime.Now,
       1000,
       contributor
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
         Console.WriteLine($"    Amount: {p.Amount}");
         Console.WriteLine($"    Date: {p.Date}");
      }
   }
   else
   {
      Console.WriteLine("No contributor found in database!");
   }
   // ---------------------------------------------------------
}


app.UseCors("AllowAll");
app.MapControllers();

app.Run();
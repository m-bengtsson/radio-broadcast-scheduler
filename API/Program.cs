using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;


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

// Configure Authorization policies
builder.Services.AddAuthorization(options =>
{
   options.AddPolicy("AdminOnly", policy =>
       policy.RequireClaim("AccountType", "Admin"));

   options.AddPolicy("AuthenticatedUser", policy =>
       policy.RequireAuthenticatedUser());
});


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
   var context = scope.ServiceProvider.GetRequiredService<RadioSchedulerContext>();
   var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
   // Drop the database file
   context.Database.EnsureDeleted();

   // Recreate tables 
   context.Database.EnsureCreated();
   DbSeeder.SeedDatabase(context);

   // Seed an admin IdentityUser if it doesn't exist
   var adminEmail = "admin@example.com";
   var adminUser = await userManager.FindByEmailAsync(adminEmail);
   if (adminUser == null)
   {
      adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
      await userManager.CreateAsync(adminUser, "Password123!");  // Set a test password

      // Add Admin role claim
      await userManager.AddClaimAsync(adminUser, new Claim("AccountType", "Admin"));
   }

   // Seed a test contributor linked to IdentityUser
   var contributorEmail = "contrib@example.com";
   var contributorUser = await userManager.FindByEmailAsync(contributorEmail);
   if (contributorUser == null)
   {
      contributorUser = new IdentityUser { UserName = contributorEmail, Email = contributorEmail };
      await userManager.CreateAsync(contributorUser, "Password123!");
   }

   if (!context.Contributors.Any(c => c.Email == contributorEmail))
   {
      var contributor = new Contributor(
          contributorUser, // link IdentityUser
          "Test Contributor",
          "Street 1",
          "0701234567",
          contributorEmail
      );


      await userManager.AddClaimAsync(contributorUser, new Claim("AccountType", "Contributor"));
      context.Contributors.Add(contributor);
      await context.SaveChangesAsync();
   }
}

app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();

app.UseCors("RefPolicy");
app.MapControllers();

app.Run();
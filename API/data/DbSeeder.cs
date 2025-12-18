public static class DbSeeder
{
   public static void SeedDatabase(RadioSchedulerContext context)
   {
      // Only seed if empty
      if (!context.Events.Any())
      {
         var today = DateOnly.FromDateTime(DateTime.Now);

         var events = new List<EventContent>
            {
                // Day 1
            new Reportage(today, "Climate Report", new TimeOnly(10, 0), TimeSpan.FromMinutes(30)),
            new LiveSession(today, "Morning Live", new TimeOnly(12, 0), TimeSpan.FromMinutes(60), "Anna"),
            new Music(today, "Morning Hits", new TimeOnly(9, 0), TimeSpan.FromMinutes(30)),

            // Day 2
            new Reportage(today.AddDays(1), "Tech Update", new TimeOnly(11, 0), TimeSpan.FromMinutes(30)),
            new Music(today.AddDays(1), "Afternoon Tunes", new TimeOnly(15, 0), TimeSpan.FromMinutes(45)),

            // Day 3
            new Reportage(today.AddDays(2), "Economy News", new TimeOnly(10, 0), TimeSpan.FromMinutes(30)),
            new LiveSession(today.AddDays(2), "Midday Live", new TimeOnly(12, 0), TimeSpan.FromMinutes(60), "Bjorn"),
            new Music(today.AddDays(2), "Classic Hour", new TimeOnly(14, 0), TimeSpan.FromMinutes(60)),

            // Day 4
            new Reportage(today.AddDays(3), "Sports Roundup", new TimeOnly(9, 30), TimeSpan.FromMinutes(30)),
            new Music(today.AddDays(3), "Evening Vibes", new TimeOnly(18, 0), TimeSpan.FromMinutes(45)),

            // Day 5
            new Reportage(today.AddDays(4), "Weekly Review", new TimeOnly(17, 0), TimeSpan.FromMinutes(30)),
            new LiveSession(today.AddDays(4), "Evening Live", new TimeOnly(19, 0), TimeSpan.FromMinutes(60), "Carla"),
            new Music(today.AddDays(4), "Night Melodies", new TimeOnly(21, 0), TimeSpan.FromMinutes(45))
            };

         // Day 6 
         events.AddRange(new List<EventContent>
            {
                new Reportage(today.AddDays(5), "Health Update", new TimeOnly(10, 0), TimeSpan.FromMinutes(30)),
                new Music(today.AddDays(5), "Afternoon Beats", new TimeOnly(14, 0), TimeSpan.FromMinutes(45)),
            });

         // Day 7
         events.AddRange(new List<EventContent>
            {
                new Reportage(today.AddDays(6), "Travel News", new TimeOnly(11, 0), TimeSpan.FromMinutes(30)),
                new LiveSession(today.AddDays(6), "Weekend Live", new TimeOnly(13, 0), TimeSpan.FromMinutes(60), "Derek"),
                new Music(today.AddDays(6), "Sunday Classics", new TimeOnly(16, 0), TimeSpan.FromMinutes(60)),
            });

         context.Events.AddRange(events);
         context.SaveChanges();
      }
   }
}

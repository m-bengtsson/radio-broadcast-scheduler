using RadioScheduler;
class Program
{

   static void Main(string[] args)
   {
      Schedule schedule = new Schedule();
      schedule.CreateSchedule();

      List<DaySchedule> weeklySchedule = schedule.WeeklySchedule;

      DateOnly dateOfNewDay = weeklySchedule[weeklySchedule.Count - 1].Date.AddDays(1); // Get date of last day and add one day ;

      DaySchedule newDay = new DaySchedule(dateOfNewDay);

      newDay.AddContent(new LiveSession("Traffic today", new TimeOnly(6, 0), TimeSpan.FromMinutes(120), "Rudy"));

      newDay.AddContent(new Reportage("Coding 101", new TimeOnly(8, 30), TimeSpan.FromMinutes(30)));

      schedule.AddDaySchedule(newDay);
      int counter = 1;

      // Print schedule
      foreach (var day in weeklySchedule)
      {
         Console.WriteLine($"{counter} Date: {day.Date}");
         counter++;
         var broadcasts = day.Broadcasts.OrderBy(b => b.StartTime).ToList();
         TimeOnly currentTime = new TimeOnly(0, 0);
         foreach (var broadcast in broadcasts)
         {
            // If there's a gap before this broadcast, play music
            if (broadcast.StartTime > currentTime)
            {
               var gap = broadcast.StartTime - currentTime;
               Console.WriteLine($" - {currentTime:HH:mm}: Music ({gap.TotalMinutes} mins)");
            }
            if (broadcast is LiveSession liveSession)
            {
               Console.WriteLine($" - {broadcast.StartTime:HH:mm}: Studio {liveSession.StudioNumber}: {broadcast.Title} ({broadcast.Duration.TotalMinutes} mins)");
            }
            else
            {
               Console.WriteLine($" - {broadcast.StartTime:HH:mm}: {broadcast.Title} ({broadcast.Duration.TotalMinutes} mins)");
            }
            currentTime = broadcast.EndTime;
         }
         // If there's time left in the day after last broadcast, play music
         // Only play music if currentTime is before 23:59 and last broadcast does not wrap to next day
         if (currentTime < new TimeOnly(23, 59) && currentTime > new TimeOnly(0, 0))
         {
            var gap = new TimeOnly(23, 59) - currentTime;
            Console.WriteLine($" - {currentTime:HH:mm}: Music ({gap.TotalMinutes} mins)");
         }
      }
      Console.ReadLine();
   }


}
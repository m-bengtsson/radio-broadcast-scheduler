using RadioScheduler;

// Todo
// Add timer, to track current time, remove day and add new date when day is over (00:00)
class Program
{

   static void Main(string[] args)
   {
      Schedule schedule = new Schedule();
      schedule.CreateSchedule();

      List<DaySchedule> weeklySchedule = schedule.WeeklySchedule;

      // Print schedule
      foreach (var day in weeklySchedule)
      {
         Console.WriteLine($"Date: {day.Date}");
         foreach (var broadcast in day.Broadcasts)
         {
            Console.WriteLine($" - {broadcast.StartTime}: {broadcast.Title} ({broadcast.Duration.TotalMinutes} mins)");

            // If no broadcast, play music, else next broadcast

         }
      }
      Console.ReadLine();
   }

   class Schedule
   {
      public List<DaySchedule> WeeklySchedule { get; set; } = new List<DaySchedule>();

      public void AddDaySchedule(DaySchedule daySchedule)
      {
         if (WeeklySchedule.Count >= 7)
         {
            WeeklySchedule.RemoveAt(0); // Remove the oldest day
         }
         WeeklySchedule.Add(daySchedule);
      }

      public void CreateSchedule()
      {

         DaySchedule day1 = new DaySchedule(DateOnly.FromDateTime(DateTime.Now));

         DaySchedule day2 = new DaySchedule(DateOnly.FromDateTime(DateTime.Now.AddDays(7)));


         day1.AddContent(new LiveSession("Morning Show", new TimeOnly(6, 0), TimeSpan.FromMinutes(120), "Alice"));

         day1.AddContent(new Reportage("Local News", new TimeOnly(9, 0), TimeSpan.FromMinutes(30)));

         // Add to days schedule
         AddDaySchedule(day1);
         AddDaySchedule(day2);
      }
   }
}
using System.Dynamic;
using RadioScheduler;

class Program
{
   static void Main(string[] args)
   {
      Schedule weeklySchedule = new Schedule();

      DaySchedule todaySchedule = new DaySchedule(DateOnly.FromDateTime(DateTime.Now));

      todaySchedule.AddContent(new Reportage("Local News", new TimeOnly(9, 0), TimeSpan.FromMinutes(30)));

      // Add to days schedule
      weeklySchedule.AddDaySchedule(todaySchedule);
      weeklySchedule.AddDaySchedule(todaySchedule);
      weeklySchedule.AddDaySchedule(todaySchedule);


      // Print schedule
      foreach (var day in weeklySchedule.WeeklySchedules)
      {
         Console.WriteLine($"Date: {day.Date}");
         foreach (var broadcast in day.Broadcasts)
         {
            Console.WriteLine($" - {broadcast.StartTime}: {broadcast.Title} ({broadcast.Duration.TotalMinutes} mins)");
         }
      }


      Console.ReadLine();
   }
}

// ListBroadcasts()
class Schedule
{
   // Weekly schedule, monday-friday FIFO
   public List<DaySchedule> WeeklySchedules { get; set; } = new List<DaySchedule>();

   public void AddDaySchedule(DaySchedule daySchedule)
   {
      WeeklySchedules.Add(daySchedule);
   }
}
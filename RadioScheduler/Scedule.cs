using RadioScheduler;
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
      // Add sample data for seven days

      DaySchedule day1 = new DaySchedule();
      day1.AddContent(new LiveSession("Morning Show", new TimeOnly(6, 0), TimeSpan.FromMinutes(120), "Alice"));
      day1.AddContent(new Reportage("City News", new TimeOnly(8, 30), TimeSpan.FromMinutes(30)));
      AddDaySchedule(day1);

      DaySchedule day2 = new DaySchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(1));
      day2.AddContent(new LiveSession("Evening Show", new TimeOnly(18, 0), TimeSpan.FromMinutes(180), "Bob"));
      day2.AddContent(new Reportage("Tech Update", new TimeOnly(21, 30), TimeSpan.FromMinutes(20)));
      AddDaySchedule(day2);

      DaySchedule day3 = new DaySchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(2));
      day3.AddContent(new LiveSession("Night Show", new TimeOnly(22, 0), TimeSpan.FromMinutes(120), "Charlie"));
      day3.AddContent(new Reportage("Sports Recap", new TimeOnly(20, 0), TimeSpan.FromMinutes(25)));
      AddDaySchedule(day3);

      DaySchedule day4 = new DaySchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(3));
      day4.AddContent(new LiveSession("Afternoon Jazz", new TimeOnly(15, 0), TimeSpan.FromMinutes(90), "Dana"));
      day4.AddContent(new Reportage("Culture Beat", new TimeOnly(16, 45), TimeSpan.FromMinutes(15)));
      AddDaySchedule(day4);

      DaySchedule day5 = new DaySchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(4));
      day5.AddContent(new LiveSession("Lunch Hour", new TimeOnly(12, 0), TimeSpan.FromMinutes(60), "Eli"));
      day5.AddContent(new Reportage("Health Tips", new TimeOnly(13, 15), TimeSpan.FromMinutes(20)));
      AddDaySchedule(day5);

      DaySchedule day6 = new DaySchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(5));
      day6.AddContent(new LiveSession("Drive Time", new TimeOnly(17, 0), TimeSpan.FromMinutes(120), "Fay"));
      day6.AddContent(new Reportage("Travel Guide", new TimeOnly(19, 30), TimeSpan.FromMinutes(25)));
      AddDaySchedule(day6);

      DaySchedule day7 = new DaySchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(6));
      day7.AddContent(new LiveSession("Late Night", new TimeOnly(23, 0), TimeSpan.FromMinutes(60), "Gus"));
      day7.AddContent(new Reportage("Science Hour", new TimeOnly(21, 30), TimeSpan.FromMinutes(30)));
      AddDaySchedule(day7);

   }
}
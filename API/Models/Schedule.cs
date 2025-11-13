public class Schedule
{
   public List<BroadcastContent> broadcasts { get; set; } = new List<BroadcastContent>();

   public Schedule()
   {
      InitializeDefaultSchedule();
   }
   public void InitializeDefaultSchedule()
   {
      DateOnly today = DateOnly.FromDateTime(DateTime.Now);

      for (int i = 0; i < 7; i++)
      {
         DateOnly scheduleDate = today.AddDays(i);
         // Add default music for each day
         broadcasts.Add(new Music(scheduleDate, "Default music", new TimeOnly(0, 0), TimeSpan.FromHours(24)));
      }
   }
}
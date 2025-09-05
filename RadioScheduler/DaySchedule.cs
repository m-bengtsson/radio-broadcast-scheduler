namespace RadioScheduler;

public class DaySchedule
{
   public DateOnly Date { get; set; }
   public List<BroadcastContent> Broadcasts { get; set; } = new List<BroadcastContent>();

   public DaySchedule(DateOnly date)
   {
      Date = date;
   }

   public DaySchedule()
   {
      Date = DateOnly.FromDateTime(DateTime.Now);
   }

   public void SortBroadcasts()
   {
      Broadcasts = Broadcasts.OrderBy(b => b.StartTime).ToList();
   }

   public void AddContent(BroadcastContent content)
   {
      Broadcasts.Add(content);

      SortBroadcasts();
   }



   // If list is empty, play music

   // RemoveBroadcast()

}
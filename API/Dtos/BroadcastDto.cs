public class BroadcastDto
{
   public Guid Id { get; set; }
   public DateOnly Date { get; set; }
   public string Title { get; set; }
   public TimeOnly StartTime { get; set; }
   public TimeSpan Duration { get; set; } // in minutes
   public TimeOnly EndTime { get; set; }
   public string Type { get; set; }
   // Specific properties for LiveSession
   public string? Host { get; set; }
   public string? CoHost { get; set; }
   public string? Guest { get; set; }
   public int? StudioNumber { get; set; }

   // Constructor to map from BroadcastContent to BroadcastDto
   public BroadcastDto(BroadcastContent broadcast)
   {
      Id = broadcast.Id;
      Date = broadcast.Date;
      Title = broadcast.Title;
      StartTime = broadcast.StartTime;
      Duration = broadcast.Duration;
      EndTime = broadcast.EndTime;
      Type = broadcast.Type;

      if (broadcast is LiveSession liveSession)
      {
         Host = liveSession.Host;
         CoHost = liveSession.CoHost;
         Guest = liveSession.Guest;
         StudioNumber = liveSession.StudioNumber;
      }
   }
}
using System.Text.Json.Serialization;

public enum BroadcastType
{
   LiveSession,
   Reportage,
   Music
}

public abstract class BroadcastContent
{
   public Guid Id { get; private set; }
   public DateOnly Date { get; set; }
   public string Title { get; set; }
   public TimeOnly StartTime { get; set; }
   public TimeSpan Duration { get; set; } // in minutes
   public TimeOnly EndTime
   {
      get
      {
         return StartTime.Add(Duration);
      }
   }
   // abstract property to be implemented by derived classes

   [JsonIgnore]
   protected abstract BroadcastType TypeName { get; }

   [JsonPropertyName("type")]
   public string Type => TypeName.ToString();

   public BroadcastContent(DateOnly date, string title, TimeOnly startTime, TimeSpan duration)
   {
      Id = Guid.NewGuid();
      Date = date;
      Title = title;
      StartTime = startTime;
      Duration = duration;
   }
}
public class Reportage : BroadcastContent
{
   protected override BroadcastType TypeName => BroadcastType.Reportage;
   public Reportage(DateOnly date, string title, TimeOnly startTime, TimeSpan duration)
      : base(date, title, startTime, duration)
   { }
}

public class LiveSession : BroadcastContent
{
   public string Host { get; set; }
   public string? CoHost { get; set; }
   public string? Guest { get; set; }
   // public int StudioNumber { get; set; }
   public int StudioNumber
   {
      get
      {
         return !string.IsNullOrEmpty(CoHost) ? 2 : 1;
      }
   }
   protected override BroadcastType TypeName => BroadcastType.LiveSession;

   public LiveSession(DateOnly date, string title, TimeOnly startTime, TimeSpan duration, string host, string? coHost = null, string? guest = null)
         : base(date, title, startTime, duration)
   {
      Host = host;
      CoHost = coHost;
      Guest = guest;
   }
}
public class Music : BroadcastContent
{
   protected override BroadcastType TypeName => BroadcastType.Music;
   public Music(DateOnly date, string title, TimeOnly startTime, TimeSpan duration)
      : base(date, title, startTime, duration)
   { }
}
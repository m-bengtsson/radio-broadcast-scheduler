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
   //  public abstract BroadcastType Type { get; }

   [JsonIgnore]
   public abstract BroadcastType Type { get; }

   [JsonPropertyName("type")]
   public string TypeName => Type.ToString();

   public BroadcastContent(DateOnly date, string title, TimeOnly startTime, TimeSpan duration)
   {
      Id = Guid.NewGuid();
      Date = date;
      Title = title;
      StartTime = startTime;
      Duration = duration;
   }
}
class Reportage : BroadcastContent
{
   public override BroadcastType Type => BroadcastType.Reportage;
   public Reportage(DateOnly date, string title, TimeOnly startTime, TimeSpan duration)
      : base(date, title, startTime, duration)
   { }
}

class LiveSession : BroadcastContent
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
   public override BroadcastType Type => BroadcastType.LiveSession;

   public LiveSession(DateOnly date, string title, TimeOnly startTime, TimeSpan duration, string host, string? coHost = null, string? guest = null)
         : base(date, title, startTime, duration)
   {
      Host = host;
      CoHost = coHost;
      Guest = guest;
   }
}
class Music : BroadcastContent
{
   public override BroadcastType Type => BroadcastType.Music;
   public Music(DateOnly date, string title, TimeOnly startTime, TimeSpan duration)
      : base(date, title, startTime, duration)
   { }
}
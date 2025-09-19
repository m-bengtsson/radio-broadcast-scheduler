public enum BroadcastType
{
   LiveSession,
   Reportage,
   Music
}

public abstract class BroadcastContent
{
   public Guid Id { get; private set; }
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
   public abstract BroadcastType Type { get; }

   public BroadcastContent(string title, TimeOnly startTime, TimeSpan duration)
   {
      Id = Guid.NewGuid();
      Title = title;
      StartTime = startTime;
      Duration = duration;
   }
}
class Reportage : BroadcastContent
{
   public override BroadcastType Type => BroadcastType.Reportage;
   public Reportage(string title, TimeOnly startTime, TimeSpan duration)
      : base(title, startTime, duration)
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

   public LiveSession(string title, TimeOnly startTime, TimeSpan duration, string host, string? coHost = null, string? guest = null)
         : base(title, startTime, duration)
   {
      Host = host;
      CoHost = coHost;
      Guest = guest;
   }
}
class Music : BroadcastContent
{
   public override BroadcastType Type => BroadcastType.Music;
   public Music(string title, TimeOnly startTime, TimeSpan duration)
      : base(title, startTime, duration)
   { }
}
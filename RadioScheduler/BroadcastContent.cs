using System.Net.Mime;

abstract public class BroadcastContent
{
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

   public BroadcastContent(string title, TimeOnly startTime, TimeSpan duration)
   {
      Title = title;
      StartTime = startTime;
      Duration = duration;
   }
}
class Reportage : BroadcastContent
{
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
   public Music(string title, TimeOnly startTime, TimeSpan duration)
      : base(title, startTime, duration)
   { }
}
using System.Text.Json.Serialization;

public enum EventType
{
   LiveSession,
   Reportage,
   Music
}
[JsonDerivedType(typeof(Reportage), typeDiscriminator: "Reportage")]
[JsonDerivedType(typeof(LiveSession), typeDiscriminator: "LiveSession")]
[JsonDerivedType(typeof(Music), typeDiscriminator: "Music")]
public abstract class EventContent
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
   public List<Contributor> Contributors { get; set; }
   // abstract property to be implemented by derived classes

   [JsonIgnore]
   protected abstract EventType TypeName { get; }

   [JsonPropertyName("type")]
   public string Type => TypeName.ToString();

   public EventContent(DateOnly date, string title, TimeOnly startTime, TimeSpan duration)
   {
      Id = Guid.NewGuid();
      Date = date;
      Title = title;
      StartTime = startTime;
      Duration = duration;
   }
}
public class Reportage : EventContent
{
   protected override EventType TypeName => EventType.Reportage;
   public Reportage(DateOnly date, string title, TimeOnly startTime, TimeSpan duration)
      : base(date, title, startTime, duration)
   { }
}

public class LiveSession : EventContent
{
   public string Host { get; set; }
   public string? CoHost { get; set; }
   public string? Guest { get; set; }
   public int StudioNumber
   {
      get
      {
         return !string.IsNullOrEmpty(CoHost) ? 2 : 1;
      }
   }
   protected override EventType TypeName => EventType.LiveSession;

   public LiveSession(DateOnly date, string title, TimeOnly startTime, TimeSpan duration, string host, string? coHost = null, string? guest = null)
         : base(date, title, startTime, duration)
   {
      Host = host;
      CoHost = coHost;
      Guest = guest;
   }
}
public class Music : EventContent
{
   protected override EventType TypeName => EventType.Music;
   public Music(DateOnly date, string title, TimeOnly startTime, TimeSpan duration)
      : base(date, title, startTime, duration)
   { }
}
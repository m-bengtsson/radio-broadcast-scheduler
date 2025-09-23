using System.Text.Json.Serialization;

public class BroadcastDto
{
   public DateOnly Date { get; set; }
   public string Title { get; set; }
   public TimeOnly StartTime { get; set; }
   public TimeSpan Duration { get; set; } // in minutes
   public string Type { get; set; }
   // Specific properties for LiveSession
   public string? Host { get; set; }
   public string? CoHost { get; set; }
   public string? Guest { get; set; }
}

public class RescheduleDto
{
   public DateOnly Date { get; set; }
   public TimeOnly StartTime { get; set; }
}
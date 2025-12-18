public class EventDto
{
   public Guid Id { get; set; }
   public string Type { get; set; }
   public DateOnly Date { get; set; }
   public string Title { get; set; }
   public TimeOnly StartTime { get; set; }
   public TimeSpan Duration { get; set; } // in minutes

   // // Specific properties for LiveSession
   public string? Host { get; set; }
   public string? CoHost { get; set; }
   public string? Guest { get; set; }
}

public class AddEventDto
{
   public string Type { get; set; }
   public DateOnly Date { get; set; }
   public string Title { get; set; }
   public TimeOnly StartTime { get; set; }
   public TimeSpan Duration { get; set; }

   public string Host { get; set; }
   public string? CoHost { get; set; }
   public string? Guest { get; set; }
}
public class LiveSessionDto : EventDto
{
   public string Host { get; set; }
   public string? CoHost { get; set; }
   public string? Guest { get; set; }
}
// public class ReportageDto : EventDto { }
// public class MusicDto : EventDto { }


public class RescheduleDto
{
   public DateOnly Date { get; set; }
   public TimeOnly StartTime { get; set; }
}
public class UpdateEventDto
{
   public string? Title { get; set; }
   public TimeSpan? Duration { get; set; } // in minutes
   public string? Host { get; set; }
   public string? CoHost { get; set; }
   public string? Guest { get; set; }
}
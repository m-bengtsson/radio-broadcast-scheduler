public class BroadcastDto
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

public class AddBroadcastDto
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
public class LiveSessionDto : BroadcastDto
{
   public string Host { get; set; }
   public string? CoHost { get; set; }
   public string? Guest { get; set; }
}
// public class ReportageDto : BroadcastDto { }
// public class MusicDto : BroadcastDto { }


public class RescheduleDto
{
   public DateOnly Date { get; set; }
   public TimeOnly StartTime { get; set; }
}
public class UpdateBroadcastDto
{
   public string? Title { get; set; }
   public TimeSpan? Duration { get; set; } // in minutes
   public string? Host { get; set; }
   public string? CoHost { get; set; }
   public string? Guest { get; set; }
}
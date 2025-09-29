public interface IScheduleService
{
   List<BroadcastContent> GetAllBroadcasts();
   List<BroadcastContent> GetTodaysBroadcasts();
   // List<BroadcastContent> GetBroadcastsByDate(DateOnly date);
   BroadcastContent? GetBroadcastById(Guid id);
   void AddBroadcast(BroadcastDto dto);
   void RemoveBroadcast(Guid id);
   void RescheduleBroadCast(Guid id, DateOnly newDate, TimeOnly newTime);
}

public class ScheduleService : IScheduleService
{
   private readonly Schedule _schedule;

   public void AddBroadcast(BroadcastDto dto)
   {
      BroadcastContent newBroadcast = dto.Type switch
      {
         "LiveSession" => new LiveSession(dto.Date, dto.Title, dto.StartTime, dto.Duration, dto.Host ?? throw new ArgumentNullException("Host is required for LiveSession."), dto.CoHost, dto.Guest),
         "Reportage" => new Reportage(dto.Date, dto.Title, dto.StartTime, dto.Duration),
         "Music" => new Music(dto.Date, dto.Title, dto.StartTime, dto.Duration),
         _ => throw new ArgumentException("Invalid broadcast type.")
      };
      _schedule.broadcasts.Add(newBroadcast);

   }

   public List<BroadcastContent> GetAllBroadcasts()
   {
      return _schedule.broadcasts;
   }

   public BroadcastContent? GetBroadcastById(Guid id)
   {
      return _schedule.broadcasts.FirstOrDefault(broadcast => broadcast.Id == id);
   }
   public void RemoveBroadcast(Guid id)
   {
      BroadcastContent? broadcast = GetBroadcastById(id);

      if (broadcast != null)
      {
         _schedule.broadcasts.Remove(broadcast);
      }
      else
      {
         throw new KeyNotFoundException("Broadcast not found.");
      }
   }

   public List<BroadcastContent> GetTodaysBroadcasts()
   {
      DateOnly today = DateOnly.FromDateTime(DateTime.Now);
      var todaysBroadcasts = _schedule.broadcasts
         .Where(b => b.Date == today)
         .OrderBy(b => b.StartTime)
         .ToList();

      return todaysBroadcasts;
   }

   public void RescheduleBroadCast(Guid id, DateOnly newDate, TimeOnly newStartTime)
   {
      var broadcast = GetBroadcastById(id);
      if (broadcast != null)
      {
         broadcast.Date = newDate;
         broadcast.StartTime = newStartTime;
      }

   }
}
// List<BroadcastContent> IScheduleService.GetBroadcastsByDate(DateOnly date)
// {
//    throw new NotImplementedException();
// }
// public interface IScheduleService
// {
//    List<EventContent> GetAllEvents();
//    List<EventContent> GetTodaysEvents();
//    // List<EventContent> GetEventsByDate(DateOnly date);
//    EventContent? GetEventById(Guid id);
//    void AddEvent(EventDto dto);
//    void RemoveEvent(Guid id);
//    void RescheduleBroadCast(Guid id, DateOnly newDate, TimeOnly newTime);
// }

// public class ScheduleService : IScheduleService
// {
//    private readonly Schedule _schedule;

//    public void AddEvent(EventDto dto)
//    {
//       EventContent newEvent = dto.Type switch
//       {
//          "LiveSession" => new LiveSession(dto.Date, dto.Title, dto.StartTime, dto.Duration, dto.Host ?? throw new ArgumentNullException("Host is required for LiveSession."), dto.CoHost, dto.Guest),
//          "Reportage" => new Reportage(dto.Date, dto.Title, dto.StartTime, dto.Duration),
//          "Music" => new Music(dto.Date, dto.Title, dto.StartTime, dto.Duration),
//          _ => throw new ArgumentException("Invalid ev type.")
//       };
//       _schedule.events.Add(newEvent);

//    }

//    public List<EventContent> GetAllEvents()
//    {
//       return _schedule.events;
//    }

//    public EventContent? GetEventById(Guid id)
//    {
//       return _schedule.events.FirstOrDefault(ev => ev.Id == id);
//    }
//    public void RemoveEvent(Guid id)
//    {
//       EventContent? ev = GetEventById(id);

//       if (ev != null)
//       {
//          _schedule.events.Remove(ev);
//       }
//       else
//       {
//          throw new KeyNotFoundException("Event not found.");
//       }
//    }

//    public List<EventContent> GetTodaysEvents()
//    {
//       DateOnly today = DateOnly.FromDateTime(DateTime.Now);
//       var todaysEvents = _schedule.events
//          .Where(b => b.Date == today)
//          .OrderBy(b => b.StartTime)
//          .ToList();

//       return todaysEvents;
//    }

//    public void RescheduleBroadCast(Guid id, DateOnly newDate, TimeOnly newStartTime)
//    {
//       var ev = GetEventById(id);
//       if (ev != null)
//       {
//          ev.Date = newDate;
//          ev.StartTime = newStartTime;
//       }

//    }
// }
// // List<EventContent> IScheduleService.GetEventsByDate(DateOnly date)
// // {
// //    throw new NotImplementedException();
// // }
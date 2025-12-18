public static class EventMapper
{
   public static EventDto ToDto(EventContent ev)
   {
      var dto = new EventDto
      {
         Id = ev.Id,
         Type = ev.Type,
         Date = ev.Date,
         Title = ev.Title,
         StartTime = ev.StartTime,
         Duration = ev.Duration,
      };

      switch (ev)
      {
         case LiveSession ls:
            dto.Host = ls.Host;
            dto.CoHost = ls.CoHost;
            dto.Guest = ls.Guest;
            break;

         case Reportage:
            // No extra properties
            break;

         case Music:
            // No extra properties
            break;
      }

      return dto;
   }
}
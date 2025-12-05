public static class BroadcastMapper
{
   public static BroadcastDto ToDto(BroadcastContent broadcast)
   {
      var dto = new BroadcastDto
      {
         Id = broadcast.Id,
         Type = broadcast.Type,
         Date = broadcast.Date,
         Title = broadcast.Title,
         StartTime = broadcast.StartTime,
         Duration = broadcast.Duration,
      };

      switch (broadcast)
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
public static class BroadcastFactory
{
   public static BroadcastContent Create(AddBroadcastDto dto)
   {
      return dto.Type switch
      {
         "Reportage" => new Reportage(dto.Date, dto.Title, dto.StartTime, dto.Duration),

         "Music" => new Music(dto.Date, dto.Title, dto.StartTime, dto.Duration),

         "LiveSession" => new LiveSession(
             dto.Date,
             dto.Title,
             dto.StartTime,
             dto.Duration,
             dto.Host ?? throw new ArgumentException("LiveSession requires Host"),
             dto.CoHost,
             dto.Guest
         ),

         _ => throw new ArgumentException("Invalid broadcast type")
      };
   }
}

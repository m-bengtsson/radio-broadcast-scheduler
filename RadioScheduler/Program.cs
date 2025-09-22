using RadioScheduler;

Schedule schedule = new Schedule();

foreach (var broadcast in schedule.broadcasts)
{
   Console.WriteLine($"{broadcast.Date} - {broadcast.StartTime:HH:mm} - {broadcast.Title} ({broadcast.Duration.TotalMinutes} mins) [{broadcast.Type}]");
}
Console.ReadLine();
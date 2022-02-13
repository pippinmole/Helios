namespace Helios.Core;

public class ScheduleConfig<T> : IScheduleConfig<T> {
    public string CronExpression { get; set; }
    public TimeZoneInfo TimeZoneInfo { get; set; }
}
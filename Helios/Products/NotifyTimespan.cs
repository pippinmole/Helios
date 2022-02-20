namespace Helios.Products;

public static class NotifyTimespan {
    public static readonly List<TimeSpan> All = new() {
        TimeSpan.FromMinutes(5),
        TimeSpan.FromMinutes(15),
        TimeSpan.FromMinutes(30),
        TimeSpan.FromMinutes(60),
        TimeSpan.FromHours(6),
        TimeSpan.FromDays(1),
        TimeSpan.FromDays(7)
    };
}
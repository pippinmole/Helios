using Helios.Data.Users;

namespace Helios.Products;

public class Product {
    public string Name { get; }
    public int Id { get; }
    public string BriefDescription { get; }
    public float PriceUsd { get; set; }
    public TimeSpan MinimumNotifyTimespan { get; set; }

    public static readonly Product FreeTier = new("Free", 0, 0f, TimeSpan.FromMinutes(60));
    public static readonly Product ProTier = new("Pro", 1, 4.99f, TimeSpan.FromMinutes(30));
    public static readonly Product EnterpriseTier = new("Enterprise", 2, 14.99f, TimeSpan.FromMinutes(5));

    public static readonly List<Product> Tiers = new() {
        FreeTier,
        ProTier,
        EnterpriseTier
    };

    public Product(string name, int id, float priceUsd, TimeSpan minNotifyTimespan) {
        Name = name;
        Id = id;
        PriceUsd = priceUsd;
        MinimumNotifyTimespan = minNotifyTimespan;
    }
}
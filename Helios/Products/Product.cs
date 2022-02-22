using Helios.Data.Users;

namespace Helios.Products;

public class Product {
    public string Name { get; }
    public int Id { get; }
    public string BriefDescription { get; }
    public float PriceHnt { get; set; }
    public TimeSpan MinimumNotifyTimespan { get; set; }

    public static readonly Product FreeTier = new("Free", 0, 0f, TimeSpan.FromMinutes(60));
    public static readonly Product ProTier = new("Pro", 1, 0.21f, TimeSpan.FromMinutes(30));
    public static readonly Product EnterpriseTier = new("Enterprise", 2, 0.64f, TimeSpan.FromMinutes(5));

    public static readonly List<Product> Tiers = new() {
        FreeTier,
        ProTier,
        EnterpriseTier
    };

    public Product(string name, int id, float priceHnt, TimeSpan minNotifyTimespan) {
        Name = name;
        Id = id;
        PriceHnt = priceHnt;
        MinimumNotifyTimespan = minNotifyTimespan;
    }
}
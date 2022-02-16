using Helios.Data.Users;

namespace Helios.Products;

public class Product {
    public string Name { get; }
    public int Id { get; }
    public string BriefDescription { get; }
    public float PriceUsd { get; set; }

    public static readonly Product FreeTier = new("Free", 0, 0f);
    public static readonly Product ProTier = new("Pro", 1, 4.99f);
    public static readonly Product EnterpriseTier = new("Enterprise", 2, 14.99f);
    
    public static readonly List<Product> Tiers = new() {
        FreeTier,
        ProTier,
        EnterpriseTier
    };

    public Product(string name, int id, float priceUsd) {
        Name = name;
        Id = id;
        PriceUsd = priceUsd;
    }
}
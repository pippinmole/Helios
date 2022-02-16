using MongoDB.Bson.Serialization.Attributes;

namespace Helios.Paypal;

public class PaypalOrder {
    
    [BsonId]
    public Guid Id { get; set; }
    public string OrderId { get; private set; }
    public int Sku { get; private set; }

    public PaypalOrder(int sku, string orderId) {
        Sku = sku;
        OrderId = orderId;
    }
}
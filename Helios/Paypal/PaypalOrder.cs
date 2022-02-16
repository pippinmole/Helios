using MongoDB.Bson.Serialization.Attributes;

namespace Helios.Paypal;

public enum EOrderState {
    Created,
    Completed
}

public class PaypalOrder {
    
    [BsonId]
    public Guid Id { get; set; }
    public string OrderId { get; private set; }
    public int Sku { get; private set; }
    public EOrderState State { get; set; }

    public PaypalOrder(int sku, string orderId) {
        Sku = sku;
        OrderId = orderId;
        State = EOrderState.Created;
    }
}
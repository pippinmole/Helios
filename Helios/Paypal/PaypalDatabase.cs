using Helios.Database;
using MongoDB.Driver;

namespace Helios.Paypal; 

public class PaypalDatabase : IPaypalDatabase {
    private readonly ILogger<PaypalDatabase> _logger;
    private readonly IDatabaseContext _context;
    private readonly IMongoCollection<PaypalOrder> _collection;

    public PaypalDatabase(ILogger<PaypalDatabase> logger, IDatabaseContext context) {
        _logger = logger;
        _context = context;

        var databaseName = MongoUrl.Create(context.ConnectionString).DatabaseName;
        _collection = context.GetDatabase(databaseName).GetCollection<PaypalOrder>("orders");
    }

    public async Task<PaypalOrder> GetOrderById(string orderId) {
        var result = await _collection.FindAsync(x => x.OrderId == orderId);
        return result.FirstOrDefault();
    }

    public async Task AddOrder(PaypalOrder paypalOrder) {
        await _collection.InsertOneAsync(paypalOrder);
    }
}

public interface IPaypalDatabase {
    Task<PaypalOrder> GetOrderById(string orderId);
    Task AddOrder(PaypalOrder paypalOrder);
}
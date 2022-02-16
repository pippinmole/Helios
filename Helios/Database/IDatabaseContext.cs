using MongoDB.Driver;

namespace Helios.Database;

public interface IDatabaseContext {
    IMongoClient Client { get; }
    string ConnectionString { get; }
    IMongoDatabase GetDatabase(string name) => Client.GetDatabase(name);
}
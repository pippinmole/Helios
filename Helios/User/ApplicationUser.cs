using AspNetCore.Identity.Mongo.Model;
using Helios.Helium;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Helios.Data.Users;

[BsonIgnoreExtraElements]
public sealed class ApplicationUser : MongoUser<ObjectId> {
    
    public List<HeliumMiner> Devices { get; set; }
    public EAccountType AccountType { get; set; } = EAccountType.Free;

    public ApplicationUser() : base() { }

    public ApplicationUser(string username, string email) : base(username) {
        Email = email;
    }

    public static readonly ApplicationUser NoUser = new("Unknown", "unknown@unknown.com");
}
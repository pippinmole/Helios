using AspNetCore.Identity.Mongo.Model;
using Helios.Helium;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Helios.Data.Users;

[BsonIgnoreExtraElements]
public sealed class ApplicationUser : MongoUser<ObjectId> {
    public List<HeliumMiner> Devices { get; set; } = new();
    public EAccountType AccountType { get; set; } = EAccountType.Free;
    public TimeSpan DowntimeNotifyRate { get; set; } = TimeSpan.FromMinutes(30);
    public bool ReceiveEmails { get; set; } = true;
    public DateTime LastEmailDate { get; set; }
    public List<string> PreviousOrderHashes { get; set; } = new();

    public ApplicationUser() : base() { }

    public ApplicationUser(string username, string email) : base(username) {
        Email = email;
    }

    public bool CanSendEmail() => LastEmailDate + DowntimeNotifyRate < DateTime.Now;

    public static readonly ApplicationUser NoUser = new("Unknown", "unknown@unknown.com");
}
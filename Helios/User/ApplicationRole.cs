using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson;

namespace Helios.Data.Users;

public class ApplicationRole : MongoRole<ObjectId> {
    public ApplicationRole() : base() {}

    public ApplicationRole(string roleName) : base(roleName) {}
}
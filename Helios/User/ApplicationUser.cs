using AspNetCore.Identity.Mongo.Model;

namespace Helios.Data.Users;

public sealed class ApplicationUser : MongoUser<Guid> {
    public string? Username { get; set; }

    public ApplicationUser() : base() { }

    public ApplicationUser(string? username, string email) : base(username) {
        Username = username;
        Email = email;
    }

    public static readonly ApplicationUser NoUser = new("Unknown", "unknown@unknown.com");
}

public class ApplicationRole : MongoRole<Guid> {
    public ApplicationRole() : base() {}

    public ApplicationRole(string roleName) : base(roleName) {}
}
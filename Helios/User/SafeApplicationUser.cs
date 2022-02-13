using System.Globalization;
using MongoDB.Bson.Serialization.Attributes;

namespace Helios.Data.Users; 

public class SafeApplicationUser {
    [BsonId]
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool EmailConfirmed { get; set; }

    public string FullName() => $"{FirstName} {LastName}";
    public string FirstNameFormat() =>  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(FirstName.ToLower());
}
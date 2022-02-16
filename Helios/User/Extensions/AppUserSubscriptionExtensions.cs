namespace Helios.Data.Users.Extensions; 

public static class AppUserSubscriptionExtensions {
    public static int MaxDevicesAllowed(this EAccountType role) {
        return role switch {
            EAccountType.Enterprise => 999,
            EAccountType.Pro => 5,
            EAccountType.Free => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}
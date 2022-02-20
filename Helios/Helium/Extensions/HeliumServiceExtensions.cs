namespace Helios.Helium.Extensions; 

public static class HeliumServiceExtensions {

    public static Task<bool> HasUserPaid(this IHeliumService heliumService, string memo) {
        return Task.FromResult(false);
    }

}
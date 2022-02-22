namespace Helios.Helium.Helpers; 

public static class HeliumHelper {
    public static bool IsValidHash(string hash) {
        if ( string.IsNullOrEmpty(hash) ) return false;
        if(hash.Contains(' ')) return false;
        if ( hash.Length != 43 ) return false;

        return true;
    }
}
namespace Helios.Core.Extensions; 

public static class DateTimeExtensions {

    public static int ToUnixTimeSeconds(this DateTime time) {
        return (int) ((DateTimeOffset) time).ToUnixTimeSeconds();
    }
    
}
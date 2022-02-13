namespace Helios.Paypal; 

public class PaypalOptions {
    public static readonly string Name = "PaypalSettings";
    
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}
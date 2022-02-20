namespace Helios.Helium.Schemas; 

public class TransactionPage {
    public List<Transaction> data { get; set; }
    public string cursor { get; set; }
}
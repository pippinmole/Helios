namespace Helios.Helium.Schemas; 

public class TransactionPage {
    public List<Block> data { get; set; }
    public string cursor { get; set; }
}
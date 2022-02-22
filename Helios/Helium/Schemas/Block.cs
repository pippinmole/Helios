using System.Text;

namespace Helios.Helium.Schemas;

public class BlockRoot {
    public Block data { get; set; }
}

public class Block {
    public string type { get; set; }
    public int time { get; set; }
    public List<Payment> payments { get; set; }
    public string payer { get; set; }
    public int nonce { get; set; }
    public int height { get; set; }
    public string hash { get; set; }
    public int fee { get; set; }
}

public class Payment {
    public string payee { get; set; }
    public string memo { get; set; }
    public int amount { get; set; }

    public float AmountHnt => amount / 100000000f;

    public string GetDecodedMemo() {
        if ( string.IsNullOrEmpty(memo) )
            return null;
        
        var valueBytes = Convert.FromBase64String(memo);
        return Encoding.UTF8.GetString(valueBytes)
            .Trim();
    }
}
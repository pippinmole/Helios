﻿namespace Helios.Helium.Schemas; 

public class Transaction {
    public string type { get; set; }
    public int time { get; set; }
    public string role { get; set; }
    public int height { get; set; }
    public string hash { get; set; }
}
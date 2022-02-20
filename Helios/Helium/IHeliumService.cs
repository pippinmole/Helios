﻿using Helios.Helium.Schemas;

namespace Helios.Helium;

public interface IHeliumService {
    Task<HotspotReport> GetHotspotByAnimalName(string name);
    Task<TransactionPage> GetTransactionsFromAddress(string address, string cursor = null);
}
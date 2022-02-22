using Helios.Helium.Schemas;

namespace Helios.Helium;

public interface IHeliumService {
    Task<HotspotReport> GetHotspotByAnimalName(string name);
    Task<(Block, Payment)> GetTransactionFromHash(string hash, CancellationToken cancellationToken = default);
}
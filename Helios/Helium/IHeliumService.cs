using Helios.Helium.Schemas;

namespace Helios.Helium;

public interface IHeliumService {
    Task<Hotspot?> GetHotspotByAnimalName(string name);
}
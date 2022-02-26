using Helios.Helium.Schemas;

namespace Helios.Helium; 

public class HeliumMiner {

    public Guid Id { get; set; }
    public string AnimalName { get; set; }
    public HotspotReport LastReport { get; private set; }
    public DateTime LastReportDate { get; private set; }

    public HeliumMiner(string animalName, HotspotReport report) {
        Id = Guid.NewGuid();
        AnimalName = animalName;

        UpdateReport(report);
    }

    public void UpdateReport(HotspotReport report) {
        LastReport = report;
        LastReportDate = DateTime.Now;
    }
    
    public TimeSpan TimeSinceLastReport() => DateTime.Now - LastReportDate;
}
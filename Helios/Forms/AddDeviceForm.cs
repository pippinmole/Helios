using System.ComponentModel.DataAnnotations;

namespace Helios.Forms; 

public class AddDeviceForm {
    [Required]
    public string AnimalName { get; set; }
}
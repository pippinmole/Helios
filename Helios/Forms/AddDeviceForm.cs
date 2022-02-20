using System.ComponentModel.DataAnnotations;

namespace Helios.Forms; 

public class AddDeviceForm {
    [Required, MaxLength(50)]
    public string AnimalName { get; set; }
}
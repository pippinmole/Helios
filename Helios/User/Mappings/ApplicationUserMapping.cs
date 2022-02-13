using AutoMapper;

namespace Helios.Data.Users;

public class ApplicationUserMapping : Profile {
    public ApplicationUserMapping() {
        CreateMap<ApplicationUser, SafeApplicationUser>();
    }
}
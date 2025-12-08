using Domain.AppUser;

namespace Service.AppUser;

public class AppUser(IAppUserRepository appUserRepository) : IAppUser
{
    public int GetRemainingConvertions(string userId)
    {
        return appUserRepository.GetRemainingConvertions(userId);
    }
}
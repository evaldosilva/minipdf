using Domain.AppUser;

namespace Service.AppUser;

public class AppUser(IAppUserRepository appUserRepository) : IAppUser
{
    public int GetRemainingConvertions(string userId)
    {
        return appUserRepository.GetRemainingConvertions(userId);
    }

    public bool HasAvailableConvertions(string userId, int filesToBeConverted)
    {
       return GetRemainingConvertions(userId) >= filesToBeConverted;
    }
}
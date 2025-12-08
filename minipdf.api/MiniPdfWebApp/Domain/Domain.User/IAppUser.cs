namespace Domain.AppUser;

public interface IAppUser
{
    int GetRemainingConvertions(string userId);

    bool HasAvailableConvertions(string userId, int filesToBeConverted);
}
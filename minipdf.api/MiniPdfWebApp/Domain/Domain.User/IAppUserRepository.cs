namespace Domain.AppUser;

public interface IAppUserRepository
{
    int GetRemainingConvertions(string userId);
    bool UpdateConvertions(string userId, int quantity);
    bool RefreshConvertions(string userId);
}
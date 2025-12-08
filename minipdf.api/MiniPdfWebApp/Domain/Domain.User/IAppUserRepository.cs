namespace Domain.AppUser;

public interface IAppUserRepository
{
    int GetRemainingConvertions(string userId);
}

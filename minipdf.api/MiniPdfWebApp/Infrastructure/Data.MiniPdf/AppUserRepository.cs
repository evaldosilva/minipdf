using Domain.AppUser;
using Microsoft.Extensions.Configuration;

namespace Data.MiniPdf;

public class AppUserRepository(IConfiguration configuration) : IAppUserRepository
{
    public int GetRemainingConvertions(string userId)
    {
        var connStr = configuration.GetConnectionString("AppMiniPdfConn");
        return 2;
    }
}
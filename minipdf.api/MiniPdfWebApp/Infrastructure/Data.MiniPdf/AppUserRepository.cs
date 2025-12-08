using Dapper;
using Domain.AppUser;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.MiniPdf;

public class AppUserRepository(IConfiguration configuration) : IAppUserRepository
{
    private const int refreshQuantity = 500;
    private const string updateCommand = "UPDATE AppUser SET remainingCompressions = @quantity WHERE id = @userId";

    public int GetRemainingConvertions(string userId)
    {
        var connStr = configuration.GetConnectionString("AppMiniPdfConn");
        return 299;
    }

    public bool RefreshConvertions(string userId)
    {
        var connStr = configuration.GetConnectionString("AppMiniPdfConn");
        using var connection = new SqlConnection(connStr);
        int affectedRows = connection.Execute(updateCommand, new { quantity = refreshQuantity, userId });
        return affectedRows > 0;
    }

    public bool UpdateConvertions(string userId, int quantity)
    {
        int currentQuantity = GetRemainingConvertions(userId);
        int newQuantity = currentQuantity - quantity;

        var connStr = configuration.GetConnectionString("AppMiniPdfConn");
        using var connection = new SqlConnection(connStr);
        int affectedRows = connection.Execute(updateCommand, new { quantity = newQuantity, userId });
        return affectedRows > 0;
    }
}
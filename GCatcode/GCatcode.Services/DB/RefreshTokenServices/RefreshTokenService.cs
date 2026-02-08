using Dapper;
using GCatcode.Repository.DB.RefreshTokenServices.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Repository.DB.RefreshTokenServices
{
    public class RefreshTokenService : DBService
    {
        public RefreshTokenService(SqlConnection dbConnection)
            : base(dbConnection, null)
        {
        }

        public RefreshTokenService(SqlConnection dbConnection, IDbTransaction transaction)
            : base(dbConnection, transaction)
        {
        }

        public RefreshTokenDTO Add(RefreshTokenInsert data)
        {
            var query = @"
                INSERT INTO [dbo].[RefreshTokens] 
                (UserId, Token, ExpiryDate, IsRevoked, CreatedAt, CreatedByIp)
                VALUES 
                (@UserId, @Token, @ExpiryDate, 0, @CreatedAt, @CreatedByIp);
                
                SELECT * FROM [dbo].[RefreshTokens] 
                WHERE RefreshTokenId = SCOPE_IDENTITY();";

            return DbConnection.QueryFirstOrDefault<RefreshTokenDTO>(
                query,
                new
                {
                    data.UserId,
                    data.Token,
                    data.ExpiryDate,
                    CreatedAt = DateTime.UtcNow,
                    data.CreatedByIp
                },
                Transaction);
        }

        public RefreshTokenDTO GetByToken(string token)
        {
            var query = @"
                SELECT * FROM [dbo].[RefreshTokens] 
                WHERE Token = @Token";

            return DbConnection.QueryFirstOrDefault<RefreshTokenDTO>(query, new { Token = token }, Transaction);
        }

        public IEnumerable<RefreshTokenDTO> GetActiveTokensByUserId(int userId)
        {
            var query = @"
                SELECT * FROM [dbo].[RefreshTokens] 
                WHERE UserId = @UserId 
                AND IsRevoked = 0 
                AND ExpiryDate > @Now
                ORDER BY CreatedAt DESC";

            return DbConnection.Query<RefreshTokenDTO>(
                query,
                new { UserId = userId, Now = DateTime.UtcNow },
                Transaction);
        }

        public bool RevokeToken(string token, string revokedByIp)
        {
            var query = @"
                UPDATE [dbo].[RefreshTokens]
                SET IsRevoked = 1,
                    RevokedAt = @RevokedAt,
                    RevokedByIp = @RevokedByIp
                WHERE Token = @Token";

            var rowsAffected = DbConnection.Execute(
                query,
                new
                {
                    Token = token,
                    RevokedAt = DateTime.UtcNow,
                    RevokedByIp = revokedByIp
                },
                Transaction);

            return rowsAffected > 0;
        }

        public bool RevokeAllUserTokens(int userId, string revokedByIp)
        {
            var query = @"
                UPDATE [dbo].[RefreshTokens]
                SET IsRevoked = 1,
                    RevokedAt = @RevokedAt,
                    RevokedByIp = @RevokedByIp
                WHERE UserId = @UserId 
                AND IsRevoked = 0";

            var rowsAffected = DbConnection.Execute(
                query,
                new
                {
                    UserId = userId,
                    RevokedAt = DateTime.UtcNow,
                    RevokedByIp = revokedByIp
                },
                Transaction);

            return rowsAffected > 0;
        }

        public bool RevokeTokenAndReplace(string oldToken, string newToken, string revokedByIp)
        {
            var query = @"
                UPDATE [dbo].[RefreshTokens]
                SET IsRevoked = 1,
                    RevokedAt = @RevokedAt,
                    RevokedByIp = @RevokedByIp,
                    ReplacedByToken = @ReplacedByToken
                WHERE Token = @Token";

            var rowsAffected = DbConnection.Execute(
                query,
                new
                {
                    Token = oldToken,
                    RevokedAt = DateTime.UtcNow,
                    RevokedByIp = revokedByIp,
                    ReplacedByToken = newToken
                },
                Transaction);

            return rowsAffected > 0;
        }

        public void RemoveExpiredTokens()
        {
            var query = @"
                DELETE FROM [dbo].[RefreshTokens]
                WHERE ExpiryDate < @Now";

            DbConnection.Execute(query, new { Now = DateTime.UtcNow }, Transaction);
        }
    }
}

-- =============================================
-- Procedimiento para limpiar tokens expirados (opcional)
-- Ejecutar periodicamente con un job de SQL Server
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_CleanExpiredRefreshTokens]
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @DeletedCount INT;
    
    DELETE FROM [dbo].[RefreshTokens]
    WHERE ExpiryDate < GETUTCDATE();
    
    SET @DeletedCount = @@ROWCOUNT;
    
    PRINT CONCAT('Tokens expirados eliminados: ', @DeletedCount);
END
GO

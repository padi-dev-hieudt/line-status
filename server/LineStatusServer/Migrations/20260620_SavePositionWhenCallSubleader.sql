CREATE TABLE CallSubleaderHistory(
	Id INT PRIMARY KEY IDENTITY(1,1),
	LineCode nvarchar(50) not null,
	Position nvarchar(255) not null,
	CreatedDate datetime not null,
)
GO;

 GO
-- =============================================
-- Author:		Hieu dt
-- Create date: 20/06/2026
-- Description:	insert lich su trang thai goi va vi tri
-- =============================================
CREATE PROCEDURE [dbo].[sp_CallSubleaderHistory_Insert]
  @LineCode       NVARCHAR(50),
  @Timestamp      DATETIME,
  @Position       NVARCHAR(255)
AS
BEGIN
    -- 20/06/2026 : insert lich su goi 
    IF TRIM(@Position) <> ''
    BEGIN
         INSERT INTO CallSubleaderHistory(LineCode, Position, CreatedDate)
        VALUES (@LineCode, TRIM(@Position), @Timestamp)
    END
END



 GO
-- =============================================
-- Author:		Hieu dt
-- Create date: 20/06/2026
-- Description:	kiem tra lich su trang thai goi va vi tri
-- =============================================
ALTER PROCEDURE [dbo].[sp_CallSubleaderHistory_Search]
AS
BEGIN
    SELECT
        a.LineCode,
        l.Line_nm           AS LineName,
        a.Position,
        COUNT(*)            AS CallCount,
        MAX(a.CreatedDate)  AS CallTime
    FROM CallSubleaderHistory a
    JOIN Line_mst l ON l.Line_c = a.LineCode
    GROUP BY a.LineCode, l.Line_nm, a.Position
    ORDER BY CallTime DESC
END
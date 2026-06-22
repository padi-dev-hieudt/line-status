-- =============================================
-- Author:		Hieu dt
-- Create date: 21/06/2026
-- Description:	tim kiem lich su goi theo bo loc
-- =============================================
CREATE PROCEDURE [dbo].[sp_CallSubleaderHistory_SearchByFilter]
    @DateFrom  DATETIME,
    @DateTo    DATETIME,
    @LineCode  NVARCHAR(50)  = '',
    @Position  NVARCHAR(255) = ''
AS
BEGIN
    SELECT
        a.Id,
        a.LineCode,
        l.Line_nm  AS LineName,
        a.Position,
        a.CreatedDate
    FROM CallSubleaderHistory a
    JOIN Line_mst l ON l.Line_c = a.LineCode
    WHERE a.CreatedDate >= @DateFrom
      AND a.CreatedDate <= @DateTo
      AND (@LineCode = '' OR a.LineCode = @LineCode)
      AND (@Position = '' OR a.Position LIKE '%' + @Position + '%')
    ORDER BY a.CreatedDate DESC
END

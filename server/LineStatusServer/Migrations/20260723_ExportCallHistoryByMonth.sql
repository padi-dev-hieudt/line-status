USE CallASSYDB;
GO

-- =============================================
-- Author:		Hieu dt
-- Create date: 23/07/2026
-- Description:	xuat lich su goi theo thang (tat ca chuyen)
-- =============================================
CREATE PROCEDURE [dbo].[sp_CallSubleaderHistory_ExportByMonth]
    @Year  INT,
    @Month INT
AS
BEGIN
    DECLARE @DateFrom DATETIME = DATEFROMPARTS(@Year, @Month, 1);
    DECLARE @DateTo   DATETIME = DATEADD(MONTH, 1, @DateFrom);

    SELECT
        a.Id,
        a.LineCode,
        l.Line_nm  AS LineName,
        a.Position,
        a.CreatedDate,
        MAX(ws.ShiftName) as ShiftName,
        COUNT(1) OVER (PARTITION BY a.LineCode, a.Position) AS TotalCount
    FROM CallSubleaderHistory a
    JOIN Line_mst l ON l.Line_c = a.LineCode
    LEFT JOIN WorkShift ws on ws.ID = a.WorkShiftId
    WHERE a.CreatedDate >= @DateFrom
      AND a.CreatedDate < @DateTo
    GROUP BY a.Id, a.LineCode, l.Line_nm, a.Position, a.CreatedDate
    ORDER BY a.CreatedDate ASC
END
GO

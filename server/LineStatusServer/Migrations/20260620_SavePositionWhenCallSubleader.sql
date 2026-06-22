USE CallASSYDB;
GO

CREATE TABLE CallSubleaderHistory(
	Id INT PRIMARY KEY IDENTITY(1,1),
	LineCode nvarchar(50) not null,
	Position nvarchar(255) not null,
    WorkShiftId int null,
	CreatedDate datetime not null,

    foreign key(WorkShiftId) references [WorkShift]([ID])
)
GO

-- =============================================
-- Author:		Hieu dt
-- Create date: 20/06/2026
-- Description:	insert lich su trang thai goi va vi tri
-- =============================================
CREATE PROCEDURE [dbo].[sp_CallSubleaderHistory_Insert]
  @LineCode       NVARCHAR(50),
  @Timestamp      DATETIME,
  @Position       NVARCHAR(255),
  @WorkShiftId INT
AS
BEGIN
    -- 20/06/2026 : insert lich su goi 
    IF TRIM(@Position) <> ''
    BEGIN
         INSERT INTO CallSubleaderHistory(LineCode, Position, WorkShiftId, CreatedDate)
        VALUES (@LineCode, TRIM(@Position), @WorkShiftId, @Timestamp)
    END
END



 GO
-- =============================================
-- Author:		Hieu dt
-- Create date: 20/06/2026
-- Description:	kiem tra lich su trang thai goi va vi tri
-- =============================================
CREATE PROCEDURE [dbo].[sp_CallSubleaderHistory_Search]
AS
BEGIN
    SELECT
        a.LineCode,
        l.Line_nm           AS LineName,
        a.Position,
        COUNT(*)            AS CallCount,
        MAX(a.CreatedDate)  AS CallTime,
        MAX(ws.ShiftName) as ShiftName
    FROM CallSubleaderHistory a
    JOIN Line_mst l ON l.Line_c = a.LineCode
    LEFT JOIN WorkShift ws on ws.ID = a.WorkShiftId
    WHERE 
        a.CreatedDate >= DATEADD(DAY, -3, CAST(GETDATE() AS DATE)) -- (3 ngày gần nhất)
    GROUP BY a.LineCode, l.Line_nm, a.Position
    ORDER BY CallTime DESC
END
GO


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
        a.CreatedDate,
        MAX(ws.ShiftName) as ShiftName,
        COUNT(1) OVER (PARTITION BY LineCode, Position) AS TotalCount
    FROM CallSubleaderHistory a
    JOIN Line_mst l ON l.Line_c = a.LineCode
    LEFT JOIN WorkShift ws on ws.ID = a.WorkShiftId
    WHERE a.CreatedDate >= @DateFrom
      AND a.CreatedDate <= @DateTo
      AND (@LineCode = '' OR a.LineCode = @LineCode)
      AND (@Position = '' OR a.Position LIKE '%' + @Position + '%')
    GROUP BY a.Id, a.LineCode, l.Line_nm, a.Position, a.CreatedDate
    ORDER BY a.CreatedDate DESC
END

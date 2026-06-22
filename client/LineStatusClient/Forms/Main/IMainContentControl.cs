using System.Threading.Tasks;

namespace LineStatusClient.Forms.Main
{
    /// <summary>
    /// Hợp đồng cho các UserControl được hiển thị trong pnlContent của FormMain.
    /// FormMain điều phối vòng đời (nạp dữ liệu + theo dõi realtime) để việc chuyển
    /// chế độ mượt mà: thao tác chặn UI (dừng SqlDependencyEx) được đẩy ra luồng nền.
    /// </summary>
    public interface IMainContentControl
    {
        /// <summary>Nạp dữ liệu và bắt đầu theo dõi realtime (timer/SqlDependencyEx).</summary>
        Task ActivateAsync();

        /// <summary>Dừng theo dõi realtime; phần dừng SqlDependencyEx (chặn) chạy ở luồng nền.</summary>
        Task DeactivateAsync();

        /// <summary>Nạp lại dữ liệu cho grid (nút Làm mới).</summary>
        Task ReloadAsync();
    }
}

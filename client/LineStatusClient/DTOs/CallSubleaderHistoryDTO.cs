using System;

namespace LineStatusClient.DTOs
{
    public class CallSubleaderHistoryDTO
    {
        public int Id { get; set; }
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public string Position { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

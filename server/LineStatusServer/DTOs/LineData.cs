using System;

namespace LineStatusServer.DTOs
{
    public class LineData
    {
        public string LineCode { get; set; }
        public int Status { get; set; }
        public DateTime Timestamp { get; set; }
        public int ProductCount { get; set; }
        public int shift { get; set; }

        /// <summary>
        /// Gia tri tacktime
        /// </summary>
        public string Tacktime { get; set; }
        /// <summary>
        /// Vi tri
        /// </summary>
        public string Sub { get; set; }

    }
}
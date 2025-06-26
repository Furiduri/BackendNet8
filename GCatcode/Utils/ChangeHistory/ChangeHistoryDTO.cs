namespace GCatcode.Utils.ChangeHistory
{
    /// <summary>
    /// ChangeHistory DB class.
    /// </summary>
    public class ChangeHistoryDTO
    {
        /// <summary>
        /// Gets or sets ChangeHistory identifier.
        /// </summary>
        public int ChangeHistoryId { get; set; }

        /// <summary>
        /// Gets or sets ChangeType identifier.
        /// </summary>
        public int ChangeTypeId { get; set; }

        /// <summary>
        /// Gets or sets date Time.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets user Modifying identifier.
        /// </summary>
        public int UserModifying { get; set; }

        /// <summary>
        /// Gets or sets ChangedData.
        /// </summary>
        public string ChangedData { get; set; }

        /// <summary>
        /// Gets or sets Observations.
        /// </summary>
        public string Observations { get; set; }
    }
}
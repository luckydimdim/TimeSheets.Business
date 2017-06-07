namespace Cmas.BusinessLayers.TimeSheets.Entities
{
    /// <summary>
    /// Вложение
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Attachment name
        /// </summary>
        public string Name;

        /// <summary>
        /// Attachment MIME type
        /// </summary>
        public string Content_type;

        /// <summary>
        /// Real attachment size in bytes
        /// </summary>
        public int Length;

        /// <summary>
        /// Attachment data
        /// </summary>
        public byte[] Data;

        public int Revpos;

        public string Digest;

        public bool Stub;
    }
}

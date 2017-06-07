using Cmas.Infrastructure.Domain.Commands;

namespace Cmas.BusinessLayers.TimeSheets.CommandsContexts
{
    /// <summary>
    /// Контекст комманды на добавление вложения
    /// </summary>
    public class AddAttachmentCommandContext : ICommandContext
    {
        /// <summary>
        /// ID документа, к которому прикладываем вложение
        /// </summary>
        public string Id;

        /// <summary>
        /// Ревизия документа, к которому прикладываем вложение
        /// </summary>
        public string RevId;

        /// <summary>
        /// Наименование вложения
        /// </summary>
        public string Name;

        /// <summary>
        /// Тип контента
        /// </summary>
        public string ContentType;

        /// <summary>
        /// Содержимое вложения
        /// </summary>
        public byte[] Content;

        /// <summary>
        /// ID созданного вложения
        /// </summary>
        public string AttachmentId;
    }
}
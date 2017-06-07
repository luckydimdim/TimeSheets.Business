using Cmas.Infrastructure.Domain.Commands;

namespace Cmas.BusinessLayers.TimeSheets.CommandsContexts
{
    /// <summary>
    /// Удалить вложение из документа
    /// </summary>
    public class DeleteAttachmentCommandContext : ICommandContext
    {
        /// <summary>
        /// ID документа, которому принадлежит вложение
        /// </summary>
        public string Id;

        /// <summary>
        /// Ревизия документа, которому принадлежит вложение
        /// </summary>
        public string RevId;

        /// <summary>
        /// Наименование вложения
        /// </summary>
        public string Name;
    }
}
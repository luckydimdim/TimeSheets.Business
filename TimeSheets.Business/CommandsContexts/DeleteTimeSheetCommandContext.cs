using Cmas.Infrastructure.Domain.Commands;

namespace Cmas.BusinessLayers.TimeSheets.CommandsContexts
{
    public class DeleteTimeSheetCommandContext : ICommandContext
    {
        /// <summary>
        /// ID заявки
        /// </summary>
        public string Id;

    }
}

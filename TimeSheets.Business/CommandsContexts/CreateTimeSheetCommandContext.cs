using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.Infrastructure.Domain.Commands;

namespace Cmas.BusinessLayers.TimeSheets.CommandsContexts
{
    public class CreateTimeSheetCommandContext : ICommandContext
    {
        /// <summary>
        /// ID созданной сущности
        /// </summary>
        public string Id;

        public TimeSheet TimeSheet;
    }
}

using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.Infrastructure.Domain.Commands;

namespace Cmas.BusinessLayers.TimeSheets.CommandsContexts
{
    public class UpdateTimeSheetCommandContext : ICommandContext
    {
        public TimeSheet TimeSheet;
    }
}

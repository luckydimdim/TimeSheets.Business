using Cmas.Infrastructure.Domain.Commands;

namespace Cmas.BusinessLayers.TimeSheets.CommandsContexts
{
    public class CreateTempAttachmentTokenContext : ICommandContext
    {
        public string TimeSheetId;

        public string FileName;

        public string Token;
    }
}

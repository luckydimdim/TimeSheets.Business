using Cmas.Infrastructure.Domain.Commands;

namespace Cmas.BusinessLayers.TimeSheets.CommandsContexts
{
    public class DeleteTempAttachmentTokenContext : ICommandContext
    {
        public string FileName;
        public string TimeSheetId;
        public string Token;
        
    }
}

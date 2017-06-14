using Cmas.Infrastructure.Domain.Criteria;

namespace Cmas.BusinessLayers.TimeSheets.Criteria
{
    /// <summary>
    ///  
    /// </summary>
    public class IsTempAttachmentTokenValid : ICriterion
    {
        public string TimeSheetId;

        public string FileName;

        public string Token;

        public IsTempAttachmentTokenValid(string timeSheetId, string fileName, string token)
        {
            TimeSheetId = timeSheetId;
            FileName = fileName;
            Token = token;
        }
    }
}
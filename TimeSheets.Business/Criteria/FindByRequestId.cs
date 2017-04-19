using Cmas.Infrastructure.Domain.Criteria;

namespace Cmas.BusinessLayers.TimeSheets.Criteria
{
    public class FindByRequestId : ICriterion
    {
        public FindByRequestId(string requestId)
        {
            RequestId = requestId;
        }

        public string RequestId { get; set; }
    }
}

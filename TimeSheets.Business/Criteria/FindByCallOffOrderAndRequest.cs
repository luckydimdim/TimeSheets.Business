using Cmas.Infrastructure.Domain.Criteria;

namespace Cmas.BusinessLayers.TimeSheets.Criteria
{
    public class FindByCallOffOrderAndRequest : ICriterion
    {
        public FindByCallOffOrderAndRequest(string callOffOrderId, string requestId)
        {
            CallOffOrderId = callOffOrderId;
            RequestId = requestId;
        }

        public string CallOffOrderId { get; set; }
        public string RequestId { get; set; }
    }
}

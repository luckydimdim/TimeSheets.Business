using Cmas.Infrastructure.Domain.Criteria;

namespace Cmas.BusinessLayers.TimeSheets.Criteria
{
    public class FindByCallOffOrderId : ICriterion
    {
        public FindByCallOffOrderId(string callOffOrderId = null)
        {
            CallOffOrderId = callOffOrderId;
        }

        public string CallOffOrderId { get; set; }
    }
}

using Cmas.Infrastructure.Domain.Criteria;

namespace Cmas.BusinessLayers.TimeSheets.Criteria
{
    /// <summary>
    /// Получить вложения по табелю
    /// </summary>
    public class GetAttachments : ICriterion
    {
        /// <summary>
        /// ID документа, которому принадлежит вложение
        /// </summary>
        public string Id;
    }
}
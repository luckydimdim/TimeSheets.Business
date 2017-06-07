using Cmas.Infrastructure.Domain.Criteria;

namespace Cmas.BusinessLayers.TimeSheets.Criteria
{
    /// <summary>
    /// Получить содержимое вложения
    /// </summary>
    public class GetAttachment : ICriterion
    {
        /// <summary>
        /// ID документа, которому принадлежит вложение
        /// </summary>
        public string Id;

        /// <summary>
        /// Ревизия документа, которому принадлежит вложение
        /// </summary>
        public string RevId;

        /// <summary>
        /// Наименование вложения
        /// </summary>
        public string Name;
    }
}
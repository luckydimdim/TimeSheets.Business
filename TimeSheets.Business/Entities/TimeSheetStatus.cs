 
namespace Cmas.BusinessLayers.TimeSheets.Entities
{
    /// <summary>
    /// Статус табеля
    /// </summary>
    public enum TimeSheetStatus
    {
        /// <summary>
        ///  Нет никакого статуса
        /// </summary>
        None = 0,

        /// <summary>
        ///  Пустой табель
        /// </summary>
        Empty = 1,

        /// <summary>
        ///  В процессе составления (было редактирование)
        /// </summary>
        Creation = 2,

        /// <summary>
        /// На проверке
        /// </summary>
        Validation = 3,

        /// <summary>
        /// На исправлении
        /// </summary>
        Correction = 4,

        /// <summary>
        /// Проверена, согласована
        /// </summary>
        Done = 5,
    }
}

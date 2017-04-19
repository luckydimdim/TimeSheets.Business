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
        Creating = 2,

        /// <summary>
        ///  Редактирование завершено
        /// </summary>
        Created = 3,

        /// <summary>
        /// На проверке
        /// </summary>
        Validation = 4,

        /// <summary>
        /// На исправлении
        /// </summary>
        Correction = 5,

        /// <summary>
        /// Проверена, согласована
        /// </summary>
        Done = 6,
    }
}
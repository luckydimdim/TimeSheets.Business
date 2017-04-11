using System;
using System.Collections.Generic;

namespace Cmas.BusinessLayers.TimeSheets.Entities
{
    /// <summary>
    /// Заявка на проверку
    /// </summary>
    public class TimeSheet
    {
        /// <summary>
        /// Уникальный внутренний идентификатор
        /// </summary>
        public string Id;

        /// <summary>
        /// Номер ревизии
        /// </summary>
        public string RevId;

        /// <summary>
        /// Идентификатор наряд заказа
        /// </summary>
        public string CallOffOrderId;

        /// <summary>
        /// Дата и время создания
        /// </summary>
        public DateTime CreatedAt;

        /// <summary>
        /// Дата и время обновления
        /// </summary>
        public DateTime UpdatedAt;

        /// <summary>
        /// Примечания
        /// </summary>
        public string Notes;

        /// <summary>
        /// Год
        /// </summary>
        public int? Year;

        /// <summary>
        /// Месяц
        /// </summary>
        public int? Month;
         
        /// <summary>
        /// Рабочее время в разрезе работ
        /// Dictionary<{ID ставки}, IEnumerable<{время по каждому дню в месяце}>>
        /// </summary>
        public Dictionary<string, IEnumerable<double>> Times;

        public TimeSheetStatus Status;

        public TimeSheet()
        {
            Times = new Dictionary<string, IEnumerable<double>>();
            Status = TimeSheetStatus.None;
        }

    }
}

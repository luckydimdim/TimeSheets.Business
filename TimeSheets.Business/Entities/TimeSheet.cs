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
        /// Идентификатор заявки на проверку
        /// </summary>
        public string RequestId;

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
        /// Период - начало
        /// </summary>
        public DateTime From;

        /// <summary>
        /// Период - окончание
        /// </summary>
        public DateTime Till;

        /// <summary>
        /// Потраченное время
        /// Dictionary<{ID ставки}, IEnumerable<{время по каждому дню в месяце}>>
        /// </summary>
        public Dictionary<string, IEnumerable<double>> SpentTime;

        /// <summary>
        /// Статус табеля
        /// </summary>
        public TimeSheetStatus Status;

        /// <summary>
        /// Сумма по табелю
        /// </summary>
        public double Amount;

        /// <summary>
        /// Валюта
        /// </summary>
        public string CurrencySysName;

        public TimeSheet()
        {
            SpentTime = new Dictionary<string, IEnumerable<double>>();
            Status = TimeSheetStatus.None;
            RequestId = null;
        }

    }
}

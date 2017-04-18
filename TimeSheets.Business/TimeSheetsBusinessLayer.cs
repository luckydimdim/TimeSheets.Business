using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cmas.BusinessLayers.TimeSheets
{
    public class TimeSheetsBusinessLayer
    {
        private readonly ICommandBuilder _commandBuilder;
        private readonly IQueryBuilder _queryBuilder;

        public TimeSheetsBusinessLayer(ICommandBuilder commandBuilder, IQueryBuilder queryBuilder)
        {
            _commandBuilder = commandBuilder;
            _queryBuilder = queryBuilder;
        }

        /// <summary>
        /// Получить название статуса.
        /// TODO: Перенести в класс - локализатор
        /// </summary>
        private static string GetStatusName(TimeSheetStatus status)
        {
            switch (status)
            {
                case TimeSheetStatus.Empty:
                    return "Не заполнен";
                case TimeSheetStatus.Creation:
                    return "Заполнен";
                case TimeSheetStatus.Validation:
                    return "На проверке";
                case TimeSheetStatus.Correction:
                    return "Содержит ошибки";
                case TimeSheetStatus.Done:
                    return "Проверена";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Создать табель учета рабочего времени
        /// </summary>
        /// <param name="callOffOrderId">ID наряд заказа</param>
        /// <returns>ID созданного табеля</returns>
        public async Task<string> CreateTimeSheet(string callOffOrderId, int month, int year, string requestId = null)
        {
            var timeSheet = new TimeSheet();

            timeSheet.CreatedAt = DateTime.UtcNow;
            timeSheet.UpdatedAt = DateTime.UtcNow;
            timeSheet.CallOffOrderId = callOffOrderId;
            timeSheet.Month = month;
            timeSheet.Year = year;
            timeSheet.RequestId = requestId;
            timeSheet.Status = TimeSheetStatus.Empty;

            var context = new CreateTimeSheetCommandContext
            {
                TimeSheet = timeSheet
            };

            context = await _commandBuilder.Execute(context);

            return context.Id;
        }

        /// <summary>
        /// Получить табель по ID
        /// </summary>
        public async Task<TimeSheet> GetTimeSheet(string timeSheetId)
        {
            return await _queryBuilder.For<Task<TimeSheet>>().With(new FindById(timeSheetId));
        }

        /// <summary>
        /// Получить все табели
        /// </summary>
        public async Task<IEnumerable<TimeSheet>> GetTimeSheets()
        {
            return await _queryBuilder.For<Task<IEnumerable<TimeSheet>>>().With(new AllEntities());
        }

        /// <summary>
        /// Получить табели по наряд заказу
        /// </summary>
        /// <param name="callOffOrderId">ID наряд заказа</param
        public async Task<IEnumerable<TimeSheet>> GetTimeSheetsByCallOffOrderId(string callOffOrderId)
        {
            return await _queryBuilder.For<Task<IEnumerable<TimeSheet>>>()
                .With(new FindByCallOffOrderId(callOffOrderId));
        }

        /// <summary>
        /// Получить табель по наряд заказу и заявке
        /// </summary>
        /// <param name="callOffOrderId">ID наряд заказа</param
        /// <param name="requestId">ID заявки</param
        public async Task<TimeSheet> GetTimeSheetByCallOffOrderAndRequest(string callOffOrderId, string requestId)
        {
            return await _queryBuilder.For<Task<TimeSheet>>()
                .With(new FindByCallOffOrderAndRequest(callOffOrderId, requestId));
        }

        /// <summary>
        /// Удалить табель учета рабочего времени
        /// </summary>
        /// <param name="timeSheetId">ID табеля</param>
        public async Task<string> DeleteTimeSheet(string timeSheetId)
        {
            var context = new DeleteTimeSheetCommandContext
            {
                Id = timeSheetId
            };

            context = await _commandBuilder.Execute(context);

            return context.Id;
        }

        /// <summary>
        /// Обновить табель
        /// </summary> 
        /// <returns>ID табеля</returns>
        public async Task<string> UpdateTimeSheet(TimeSheet timeSheet)
        {
            if (timeSheet.Status == TimeSheetStatus.Done)
                throw new Exception("Cannot update time sheet with status " + timeSheet.Status.ToString());

            timeSheet.UpdatedAt = DateTime.UtcNow;
            timeSheet.Status = TimeSheetStatus.Creation;

            var context = new UpdateTimeSheetCommandContext
            {
                TimeSheet = timeSheet
            };

            context = await _commandBuilder.Execute(context);

            return context.TimeSheet.Id;
        }

        public static double GetAmount(double rate, TimeUnit timeUnit, IEnumerable<double> spentTimes)
        {
            double result = 0;
            int day = 1;

            if (spentTimes.Count() > 31)
                throw new Exception(String.Format("Wrong days count: ({0})", spentTimes.Count()));

            foreach (double spentTime in spentTimes)
            {
                switch (timeUnit)
                {
                    case TimeUnit.Day:
                        if (spentTime > 1 || spentTime < 0)
                            throw new Exception(String.Format("Wrong day: {0}", spentTime));
                        break;
                    case TimeUnit.Hour:
                        if (spentTime > 24 || spentTime < 0)
                            throw new Exception(String.Format("Wrong hours: {0}", spentTime));
                        break;
                }

                result += rate * spentTime;

                day++;
            }

            return result;
        }
    }
}
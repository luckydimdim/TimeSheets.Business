using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.Infrastructure.Domain.Queries;
using Cmas.Infrastructure.ErrorHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cmas.Infrastructure.Security;
using System.IO;

namespace Cmas.BusinessLayers.TimeSheets
{
    public class TimeSheetsBusinessLayer
    {
        private readonly ICommandBuilder _commandBuilder;
        private readonly IQueryBuilder _queryBuilder;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public TimeSheetsBusinessLayer(IServiceProvider serviceProvider, ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
            _commandBuilder = (ICommandBuilder) serviceProvider.GetService(typeof(ICommandBuilder));
            _queryBuilder = (IQueryBuilder) serviceProvider.GetService(typeof(IQueryBuilder));
        }

        /// <summary>
        /// Получить название статуса.
        /// TODO: Перенести в класс - локализатор
        /// </summary>
        public static string GetStatusName(TimeSheetStatus status)
        {
            switch (status)
            {
                case TimeSheetStatus.Empty:
                    return "Пустой";
                case TimeSheetStatus.Creating:
                    return "Редактирование";
                case TimeSheetStatus.Created:
                    return "Редактирование завершено";
                case TimeSheetStatus.Corrected:
                    return "Исправление завершено";
                case TimeSheetStatus.Approving:
                    return "На проверке";
                case TimeSheetStatus.Correcting:
                    return "Содержит ошибки";
                case TimeSheetStatus.Approved:
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
        public async Task<string> CreateTimeSheet(string callOffOrderId, DateTime from, DateTime till, string requestId,
            string currencySysName)
        {
            if (string.IsNullOrEmpty(callOffOrderId))
            {
                throw new ArgumentException("callOffOrderId");
            }

            if (string.IsNullOrEmpty(requestId))
            {
                throw new ArgumentException("requestId");
            }

            var timeSheet = new TimeSheet();

            timeSheet.CreatedAt = DateTime.UtcNow;
            timeSheet.UpdatedAt = DateTime.UtcNow;
            timeSheet.CallOffOrderId = callOffOrderId;
            timeSheet.From = from;
            timeSheet.Till = till;
            timeSheet.RequestId = requestId;
            timeSheet.CurrencySysName = currencySysName;
            timeSheet.Status = TimeSheetStatus.Empty;
            timeSheet.Id = null;

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
            if (string.IsNullOrEmpty(timeSheetId))
            {
                throw new ArgumentException("timeSheetId");
            }

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
            if (string.IsNullOrEmpty(callOffOrderId))
            {
                throw new ArgumentException("callOffOrderId");
            }

            return await _queryBuilder.For<Task<IEnumerable<TimeSheet>>>()
                .With(new FindByCallOffOrderId(callOffOrderId));
        }

        /// <summary>
        /// Получить количество табелей по наряд заказу
        /// </summary>
        /// <param name="callOffOrderId">ID наряд заказа</param
        public async Task<int> CountTimeSheetsByCallOffOrderId(string callOffOrderId)
        {
            if (string.IsNullOrEmpty(callOffOrderId))
            {
                throw new ArgumentException("callOffOrderId");
            }

            return await _queryBuilder.For<Task<int>>().With(new FindByCallOffOrderId(callOffOrderId));
        }

        /// <summary>
        /// Получить табели по заявке
        /// </summary>
        /// <param name="requestId">ID заявки</param
        public async Task<IEnumerable<TimeSheet>> GetTimeSheetsByRequestId(string requestId)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                throw new ArgumentException("requestId");
            }

            return await _queryBuilder.For<Task<IEnumerable<TimeSheet>>>()
                .With(new FindByRequestId(requestId));
        }

        /// <summary>
        /// Получить табель по наряд заказу и заявке
        /// </summary>
        /// <param name="callOffOrderId">ID наряд заказа</param
        /// <param name="requestId">ID заявки</param
        public async Task<TimeSheet> GetTimeSheetByCallOffOrderAndRequest(string callOffOrderId, string requestId)
        {
            if (string.IsNullOrEmpty(callOffOrderId))
            {
                throw new ArgumentException("callOffOrderId");
            }

            if (string.IsNullOrEmpty(requestId))
            {
                throw new ArgumentException("requestId");
            }

            return await _queryBuilder.For<Task<TimeSheet>>()
                .With(new FindByCallOffOrderAndRequest(callOffOrderId, requestId));
        }

        /// <summary>
        /// Удалить табель учета рабочего времени
        /// </summary>
        /// <param name="timeSheetId">ID табеля</param>
        public async Task<string> DeleteTimeSheet(string timeSheetId)
        {
            if (string.IsNullOrEmpty(timeSheetId))
            {
                throw new ArgumentException("timeSheetId");
            }

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
            timeSheet.UpdatedAt = DateTime.UtcNow;

            if (timeSheet.SpentTime.Count == 0)
                timeSheet.Amount = 0;

            if (timeSheet.Status == TimeSheetStatus.Empty)
            {
                timeSheet.Status = TimeSheetStatus.Creating;
            }

            var context = new UpdateTimeSheetCommandContext
            {
                TimeSheet = timeSheet
            };

            context = await _commandBuilder.Execute(context);

            return context.TimeSheet.Id;
        }

        /// <summary>
        /// Обновление статуса табеля
        /// </summary>
        /// <param name="timeSheet">Табель</param>
        /// <param name="status">Новый статус</param>
        public async Task UpdateTimeSheetStatus(TimeSheet timeSheet, TimeSheetStatus status)
        {
            if (status == TimeSheetStatus.None)
                throw new ArgumentException("status");

            if (status == timeSheet.Status)
                return;

            switch (status)
            {
                case TimeSheetStatus.Empty:
                    throw new GeneralServiceErrorException($"cannot set '{status}' status from '{timeSheet.Status}'");
                case TimeSheetStatus.Creating:
                    if (timeSheet.Status != TimeSheetStatus.Empty && timeSheet.Status != TimeSheetStatus.Created)
                        throw new GeneralServiceErrorException(
                            $"cannot set '{status}' status from '{timeSheet.Status}'");
                    else
                        timeSheet.Status = status;
                    break;
                case TimeSheetStatus.Created:
                    if (timeSheet.Status != TimeSheetStatus.Empty && timeSheet.Status != TimeSheetStatus.Creating &&
                        timeSheet.Status != TimeSheetStatus.Approving)
                        throw new GeneralServiceErrorException(
                            $"cannot set '{status}' status from '{timeSheet.Status}'");
                    else
                        timeSheet.Status = status;
                    break;
                case TimeSheetStatus.Approving:
                    if (timeSheet.Status != TimeSheetStatus.Corrected && timeSheet.Status != TimeSheetStatus.Created)
                        throw new GeneralServiceErrorException(
                            $"cannot set '{status}' status from '{timeSheet.Status}'");
                    else
                        timeSheet.Status = status;
                    break;
                case TimeSheetStatus.Correcting:
                    if (timeSheet.Status != TimeSheetStatus.Approving && timeSheet.Status != TimeSheetStatus.Corrected)
                        throw new GeneralServiceErrorException(
                            $"cannot set '{status}' status from '{timeSheet.Status}'");
                    else
                    {
                        if (timeSheet.Status == TimeSheetStatus.Approving &&
                            !_claimsPrincipal.HasAnyRole(new[] {Role.Customer}))
                            throw new ForbiddenErrorException();

                        timeSheet.Status = status;
                    }
                    break;
                case TimeSheetStatus.Corrected:
                    if (timeSheet.Status != TimeSheetStatus.Correcting)
                        throw new GeneralServiceErrorException(
                            $"cannot set '{status}' status from '{timeSheet.Status}'");
                    else
                    {
                        timeSheet.Status = status;
                    }
                    break;
                case TimeSheetStatus.Approved:
                    if (timeSheet.Status != TimeSheetStatus.Approving)
                        throw new GeneralServiceErrorException(
                            $"cannot set '{status}' status from '{timeSheet.Status}'");
                    else
                    {
                        if (!_claimsPrincipal.HasAnyRole(new[] {Role.Customer}))
                            throw new ForbiddenErrorException();

                        timeSheet.Status = status;
                    }
                    break;

                default:
                    throw new GeneralServiceErrorException("unknown status");
            }

            await UpdateTimeSheet(timeSheet);
        }

        /// <summary>
        /// Получить сумму по табелю для ед.изм - день
        /// </summary>
        public static double GetDayAmount(double rate, IEnumerable<double> spentTimes)
        {
            double result = 0;

            if (spentTimes.Count() > 31)
                throw new ArgumentException($"Wrong days count: ({spentTimes.Count()})");

            foreach (double spentTime in spentTimes)
            {
                result += rate * (spentTime > 0 ? 1 : 0);
            }

            return result;
        }

        /// <summary>
        /// Получить сумму по табелю для ед.изм - час
        /// </summary>
        public static double GetHourAmount(double rate, IEnumerable<double> spentTimes)
        {
            double result = 0;

            if (spentTimes.Count() > 31)
                throw new ArgumentException($"Wrong days count: ({spentTimes.Count()})");

            foreach (double spentTime in spentTimes)
            {
                result += rate * spentTime;
            }

            return result;
        }

        /// <summary>
        /// Получить сумму по табелю для ед.изм - месяц
        /// </summary>
        public static double GetMonthAmount(double rate, IEnumerable<double> spentTimes, DateTime from, DateTime till)
        {
            if (spentTimes.Count() > 31)
                throw new ArgumentException($"Wrong days count: ({spentTimes.Count()})");

            int monthDaysCount = (int)Math.Round((till - from).TotalDays, MidpointRounding.AwayFromZero);

            if (monthDaysCount == 0)
                return 0;


            var workedDays = 0;
            foreach (double spentTime in spentTimes)
            {
                workedDays += spentTime > 0 ? 1 : 0;
            }

            return ((double)workedDays / monthDaysCount) * rate;
        }

        /// <summary>
        /// Добавить вложение
        /// </summary>
        public async Task<string> AddAttachment(TimeSheet timeSheet, string fileName, Stream stream, string contentType)
        {
            if (timeSheet == null)
            {
                throw new ArgumentException("timeSheet");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("fileName");
            }

            var context = new AddAttachmentCommandContext()
            {
                Id = timeSheet.Id,
                RevId = timeSheet.RevId,
                ContentType = contentType,
                Name = fileName
            };

            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                context.Content = ms.ToArray();
            }

            context = await _commandBuilder.Execute(context);

            return context.AttachmentId;
        }

        /// <summary>
        /// Удалить вложение
        /// </summary>
        /// <param name="timeSheet"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DeleteAttachmentAsync(TimeSheet timeSheet, string fileName)
        {
            if (timeSheet == null)
            {
                throw new ArgumentException("timeSheet");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("fileName");
            }

            var context = new DeleteAttachmentCommandContext()
            {
                Id = timeSheet.Id,
                RevId = timeSheet.RevId,
                Name = fileName
            };

            await _commandBuilder.Execute(context);
        }

        /// <summary>
        /// Получить вложение
        /// </summary>
        public async Task<Attachment> GetAttachmentAsync(TimeSheet timeSheet, string fileName)
        {
            if (timeSheet == null)
            {
                throw new ArgumentException("timeSheet");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("fileName");
            }

            return await _queryBuilder.For<Task<Attachment>>()
                .With(new GetAttachment {Id = timeSheet.Id, RevId = timeSheet.RevId, Name = fileName});
        }

        /// <summary>
        /// Получить вложения (без данных)
        /// </summary>
        public async Task<Attachment[]> GetAttachmentsAsync(string timeSheetId)
        {
            if (timeSheetId == null)
            {
                throw new ArgumentException("timeSheetId");
            }

            return await _queryBuilder.For<Task<Attachment[]>>()
                .With(new GetAttachments {Id = timeSheetId});
        }
    }
}
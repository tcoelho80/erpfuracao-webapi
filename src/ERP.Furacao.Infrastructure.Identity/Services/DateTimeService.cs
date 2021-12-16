using System;
using ERP.Furacao.Application.Services;

namespace ERP.Furacao.Infrastructure.Identity.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}

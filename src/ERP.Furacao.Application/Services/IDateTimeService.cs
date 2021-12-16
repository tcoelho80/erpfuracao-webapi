using System;

namespace ERP.Furacao.Application.Services
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}

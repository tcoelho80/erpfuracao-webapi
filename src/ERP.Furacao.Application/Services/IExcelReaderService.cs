using System.Collections.Generic;

namespace ERP.Furacao.Application.Services
{
    public interface IExcelReaderService
    {
        List<T> ReadFromFile<T>(string path, int initialLine, List<string> columnsConfig, string culture) where T : class, new();
    }
}

using System.Collections.Generic;

namespace ERP.Furacao.Infrastructure.Data.Settings
{
    public class ExcelReaderSettings
    {
        /// <summary>
        /// Informação de regionalização para conversão de valores e datas
        /// </summary>
        public string Culture { get; set; } = "pt-BR";

        /// <summary>
        /// Linha que será iniciada a leitura do arquivo excel
        /// </summary>
        public int InitialLine { get; set; }

        /// <summary>
        /// Extensões permitidas para realizar a importação
        /// </summary>
        public string[] Extensions { get; set; }

        /// <summary>
        /// Binding Name e posição das colunas que serão importadas
        /// </summary>
        public List<string> ColumnsConfig { get; set; }
    }
}

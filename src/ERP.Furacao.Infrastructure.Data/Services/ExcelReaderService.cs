using ClosedXML.Excel;
using ERP.Furacao.Application.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ERP.Furacao.Infrastructure.Data.Services
{
    public class ExcelReaderService : IExcelReaderService
    {
        private  CultureInfo _cultureInfo;
        public List<T> ReadFromFile<T>(string path, int initialLine, List<string> columnsConfig, string culture) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(path) || initialLine == 0 || columnsConfig == null || !columnsConfig.Any())
                throw new ArgumentException();

            _cultureInfo = new CultureInfo(culture);
            List<T> listOutPut = new List<T>();

            var xls = new XLWorkbook(path);
            var planilha = xls.Worksheets.First();
            var totalLinhas = planilha.Rows().Count();

            for (int l = initialLine; l <= totalLinhas; l++)
            {
                T item = new T();

                foreach (var config in columnsConfig)
                {
                    var configs = config.Split("|");
                    var bindingName = configs[0];
                    var position = configs[1];

                    SetEntityProperty(item, bindingName, planilha.Cell($"{position}{l}").Value);
                }
                    
                listOutPut.Add(item);
            }

            return listOutPut;
        }

        private void SetEntityProperty<T>(T entity, string propertyName, object value) where T : class, new()
        {
            if (value == null) return;

            var props = entity.GetType().GetProperties();

            var property = props.FirstOrDefault(p => p.Name == propertyName);
            if (property == null) return;

            var convertedValue = ToType(value, property.PropertyType);
            property.SetValue(entity, convertedValue);
        }

        public object ToType(object value, Type conversionType)
        {
            switch (Type.GetTypeCode(conversionType))
            {
                case TypeCode.Boolean:
                    return (bool)value;
                case TypeCode.DateTime:
                    return Convert.ToDateTime(value, _cultureInfo);
                case TypeCode.Decimal:
                    return Convert.ToDecimal(value, _cultureInfo);
                case TypeCode.Double:
                    return Convert.ToDouble(value, _cultureInfo);
                case TypeCode.Int16:
                    return Convert.ToInt16(value);
                case TypeCode.Int32:
                    return Convert.ToInt32(value);
                case TypeCode.Int64:
                    return Convert.ToInt64(value);
                default:
                    return value.ToString();

            };
        }
    }
}

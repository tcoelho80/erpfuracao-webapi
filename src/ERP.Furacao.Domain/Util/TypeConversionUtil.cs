using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace ERP.Furacao.Domain.Util
{
    public class TypeConversionLayoutUtil : IEnumerable<TypeConversionLayoutItemUtil>
    {
        /// <summary>
        /// Instacia da Colleção do tipo "TypeConversionLayoutItemUtil".
        /// </summary>
        public HashSet<TypeConversionLayoutItemUtil> innerList = new HashSet<TypeConversionLayoutItemUtil>();

        /// <summary>
        /// Adiciona um item na coleção.
        /// </summary>
        /// <param name="item">Item</param>
        public void Add(TypeConversionLayoutItemUtil item)
        {
            this.innerList.Add(item);

        }

        /// <summary>
        /// Adiciona uma instancia de um item na coleção. 
        /// </summary>
        /// <param name="type">Tipo do item</param>
        /// <param name="sourceName">Nome da fonte</param>
        /// <param name="targetName">Nome alvo</param>
        public void Add(Type type, String sourceName, String targetName)
        {
            this.Add(new TypeConversionLayoutItemUtil { SourceName = sourceName, TargetName = targetName });
        }

        /// <summary>
        /// Abre um arquivo "TypeConversionLayout", através do nome do arquivo.
        /// </summary>
        /// <param name="fileName">Nome do arquivo</param>
        /// <returns>Um tipo de "TypeConversionLayout"</returns>
        public static TypeConversionLayoutUtil OpenTypeConversionLayoutFromFile(String fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TypeConversionLayoutUtil));

            TypeConversionLayoutUtil result = new TypeConversionLayoutUtil();

            try
            {
                XmlReader xmlReader = XmlReader.Create(fileName);

                result = (TypeConversionLayoutUtil)serializer.Deserialize(xmlReader);
            }
            catch
            {
            }
            return result;
        }

        #region IEnumerable<TypeConversionLayoutItemUtil> Members

        IEnumerator<TypeConversionLayoutItemUtil> IEnumerable<TypeConversionLayoutItemUtil>.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Classe de item do layout para conversão de tipos.
    /// </summary>
    public class TypeConversionLayoutItemUtil
    {
        /// <summary>
        /// Nome da fonte.
        /// </summary>
        public String SourceName { get; set; }
        /// <summary>
        /// Nome alvo.
        /// </summary>
        public String TargetName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ERP.Furacao.Domain.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Formata uma string de acordo com os argumentos passados.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <param name="args">Um array de objeto opcional</param>
        /// <returns>Uma string formatada</returns>
        public static String Fmt(this String owner, params Object[] args)
        {
            return String.Format(owner, args);
        }

        /// <summary>
        /// Verifica se o tipo esta nulo ou vazio.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <returns>Verdadeiro ou falso</returns>
        public static Boolean IsNullOrEmpty(this String owner)
        {
            return String.IsNullOrEmpty(owner);
        }

        /// <summary>
        /// Verifica se o tipo esta nulo, vazio ou contém somente espaços em branco.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <returns>Verdadeiro ou falso</returns>
        public static Boolean IsNullOrWhiteSpace(this String owner)
        {
            return String.IsNullOrWhiteSpace(owner);
        }

        /// <summary>
        /// Obtem somente os números de uma string.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <returns>Uma string com os números</returns>
        public static String OnlyNumbers(this String owner)
        {
            return Regex.Replace(owner, @"[^\d]", "", RegexOptions.Compiled);
        }

        /// <summary>
        /// Verifica o tamanho da string.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <param name="lenght">Tamanho do valor do tipo</param>
        /// <param name="message">Mensagem para a exceção</param>
        /// <returns>O valor da string</returns>
        public static String ValidateLenght(this String owner, Int32 lenght, String message)
        {
            if (!owner.Length.Equals(lenght))
                throw new ArgumentException(message, "Lenght");

            return owner;
        }

        /// <summary>
        ///  Verifica o tamanho da string e formata a mensagem.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <param name="lenght">Tamanho do valor da string</param>
        /// <returns>O valor da string</returns>
        public static String ValidateLenght(this String owner, Int32 lenght)
        {
            return owner.ValidateLenght(lenght, "Tamanho da string não respeita o pattern \n String: {0} \n Tamanho: {1}"
                                                .Fmt(owner, lenght));
        }

        /// <summary>
        /// Quebra uma string através de um delimitador e faz uma busca na mesma através de uma chave.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <param name="key">Chave de busca</param>
        /// <returns>Uma string com o resultado da busca</returns>
        public static String SubstringByKey(this String owner, String key)
        {
            return SubstringByKey(owner, key, ',');
        }

        /// <summary>
        /// Sobrecarga do método SubstringByKey.
        /// Quebra uma string através de um delimitador e faz uma busca na mesma através de uma chave.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <param name="key">Chave de busca</param>
        /// <param name="delimiter">Delimitador para a quebra da string</param>
        /// <returns>Uma string com o resultado da busca</returns>
        public static String SubstringByKey(this String owner, String key, Char delimiter)
        {
            var split = owner.Split(delimiter);

            var item = split
                        .Where(x => x.Contains(key + '='))
                        .Select(x => x).FirstOrDefault();

            if (item == null)
                return String.Empty;

            var itemLenght = (key + '=').Length;

            return item
                    .Substring(
                        itemLenght,
                        item.Length - itemLenght
                    );
        }

        /// <summary>
        /// Obtem uma parte de uma string, tendo como partida para o 
        /// subString o argumento passado.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <param name="startIndex">Ponto inicial para o subString</param>
        /// <returns>Uma string com o resultado da subString</returns>
        public static String TrySubstring(this String owner, Int32 startIndex)
        {
            return TrySubstring(owner, startIndex, owner.Length - startIndex);
        }

        /// <summary>
        /// Sobrecarga do método TrySubstring.
        /// Obtem uma parte de uma string, através dos argumentos passados.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <param name="startIndex">Ponto inicial para o subString</param>
        /// <param name="length">Diferença entre o length da string e o startIndex</param>
        /// <returns>Uma string com o resultado da subString</returns>
        public static String TrySubstring(this String owner, Int32 startIndex, Int32 length)
        {
            return owner.IsNullOrEmpty() ? owner : (owner.Length > length ? owner.Substring(startIndex, length) : owner);
        }


        public static String HtmlEncode(this String owner)
        {
            return System.Security.SecurityElement.Escape(
                System.Web.HttpUtility.HtmlDecode(owner)
                );
        }

        public static String HtmlDecode(this String owner)
        {
            return System.Web.HttpUtility.HtmlDecode(owner);
        }

        /// <summary>
        /// Remove caracteres especiais do valor da string em questão.
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <returns>Uma sem determinados caracteres especiais</returns>
        public static String RemoveSpecialCharacters(this String owner, Boolean trim = false)
        {
            string retorno;
            var specials = new Dictionary<Int32, String>();
            specials.Add(38, "amp");

            if (owner != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in owner)
                {
                    if (specials.ContainsKey(c)) // special chars
                        sb.Append(String.Format("&{0};", specials[c]));
                    else
                        sb.Append(c);
                }
                retorno = sb.ToString();
            }
            else
                retorno = owner;

            if (trim)
                retorno = string.IsNullOrEmpty(retorno) ? retorno : retorno.Trim();

            return retorno;
        }

        public static String RemoveNonAlphaNumericChars(this String owner)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(owner, String.Empty);
        }

        /// <summary>
        /// <para>Remove Acentuação da string em questão.
        /// os caracteres acentuados são trocados por não acentuados
        /// os caracteres de acento são trocados por espaço, os seguintes carateres
        /// especiais são trocados por espaço por padrão:</para>
        /// <para>UnicodeCategory.ModifierSymbol, 
        ///   UnicodeCategory.MathSymbol,
        ///   UnicodeCategory.OpenPunctuation,
        ///   UnicodeCategory.ClosePunctuation,
        ///   UnicodeCategory.DashPunctuation,
        ///   UnicodeCategory.ConnectorPunctuation,
        ///   UnicodeCategory.OtherPunctuation,
        ///   UnicodeCategory.CurrencySymbol</para>
        /// <para>Podem ser incluidos outros na lista, ou considerados somente os passados no método.</para> 
        /// </summary>
        /// <param name="owner">Tipo extendido</param>
        /// <param name="ignoreImplicitCategoriesToRemove">Indica se é para ignorar a lista de categorias trocadas por espaço por padrão.</param>
        /// <param name="otherCategoriesToRemove">Indica um Array com outras categorias a considerar trocar por espaço</param>
        /// <returns>Uma sem caracteres acentuados especiais</returns>
        public static String ReplaceAccents(this String owner, bool ignoreImplicitCategoriesToRemove = false, UnicodeCategory[] otherCategoriesToRemove = null)
        {


            UnicodeCategory[] implicityCategoriesToRemove = {  UnicodeCategory.ModifierSymbol,
                                                               UnicodeCategory.MathSymbol,
                                                               UnicodeCategory.OpenPunctuation,
                                                               UnicodeCategory.ClosePunctuation,
                                                               UnicodeCategory.DashPunctuation,
                                                               UnicodeCategory.ConnectorPunctuation,
                                                               UnicodeCategory.OtherPunctuation,
                                                               UnicodeCategory.CurrencySymbol };

            UnicodeCategory[] categoriesToRemove = (implicityCategoriesToRemove);

            if (otherCategoriesToRemove != null)
                if (ignoreImplicitCategoriesToRemove)
                    categoriesToRemove = otherCategoriesToRemove;
                else
                    categoriesToRemove = categoriesToRemove.AsEnumerable<UnicodeCategory>().Concat(otherCategoriesToRemove.AsEnumerable<UnicodeCategory>()).ToArray<UnicodeCategory>();

            string s = owner.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    if (categoriesToRemove.AsEnumerable<UnicodeCategory>().Contains(uc))
                        sb.Append(' ');
                    else
                        sb.Append(s[k]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputCamelCaseString"></param>
        /// <returns></returns>
        public static String SeparateCamelCase(this String inputCamelCaseString)
        {
            return System.Text.RegularExpressions.Regex.Replace(inputCamelCaseString,
                                                                    "([A-Z])",
                                                                    " $1",
                                                                    System.Text.RegularExpressions.RegexOptions.Compiled
                                                                ).Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static StringBuilder AppendAfterLine(this StringBuilder owner, String value)
        {
            owner.Append(System.Environment.NewLine);
            owner.Append(value);
            return owner;
        }

        /// <summary>
        /// Obtem o valor absoluto de uma string para ser usado com o operador ??
        /// </summary>
        /// <param name="owner">String a obter o valor</param>
        /// <returns>Se a string for nula ou vazia, retorna null. Do contrario, retorna seu valor</returns>
        public static String Abs(this String owner)
        {
            return owner.IsNullOrEmpty() ? null : owner;
        }

        private static Int32 OccurrenceIndexOfAnyOccurrence(this String owner, String value, Int32 occurrence)
        {
            var result =
                    owner
                        .Select(
                            (c, i) => { return new KeyValuePair<String, Int32>(owner.Substring(i), i); }
                        )
                        .Where(
                            x => x.Key.StartsWith(value)
                        );

            return result.Count() < occurrence
                ? -1
                : result.Select(x => x.Value)
                    .ElementAtOrDefault(occurrence - 1);
        }

        private static int OccurrenceIndexOfExclusive(this String owner, String value, int occurrenceNumber)
        {
            Match match = Regex.Match(owner, "((" + value + ").*?){" + occurrenceNumber + "}");

            if (match.Success)
                return match.Groups[2].Captures[occurrenceNumber - 1].Index;
            else
                return -1;
        }

        public static Boolean IsValidRegex(this String owner, String regex)
        {
            return Regex.IsMatch(owner, regex, RegexOptions.Compiled);
        }

        public static String Reverse(this String owner)
        {
            Char[] charArray = owner.ToCharArray();

            Array.Reverse(charArray);

            return new String(charArray);
        }

        public static String PadLeftEx(this String owner, int length, char paddingChar)
        {
            string s = owner ?? String.Empty;

            if (s.Length <= length)
                return s.PadLeft(length, paddingChar);
            else
                return s.Substring(0, length);
        }

        public static String PadRightEx(this String owner, int length, char paddingChar)
        {
            string s = owner ?? String.Empty;

            if (s.Length <= length)
                return s.PadRight(length, paddingChar);
            else
                return s.Substring(0, length);
        }

        public static bool IsNumeric(this String owner)
        {
            return (owner.Count(c => char.IsNumber(c)) == owner.Length);
        }

        public static string TrocaCaracteresEspeciais(this String texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[k]);
                }
            }
            return sb.ToString();
        }


        public static string GZip(this String input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return Convert.ToBase64String(mso.ToArray());
            }

        }

        public static string GUnZip(this String input)
        {
            var bytes = Convert.FromBase64String(input);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static string EncodeStringTo64(this string baseString)
        {
            var bytes = Encoding.UTF8.GetBytes(baseString ?? string.Empty);
            return Convert.ToBase64String(bytes);
        }

        public static string EncodeArrayOfStringTo64(this string[] arrayOfString)
        {
            var stringBuilder = new StringBuilder();

            foreach (var element in arrayOfString)
            {
                stringBuilder.Append(element);
            }

            return stringBuilder.ToString().EncodeStringTo64();
        }

        public static bool GreaterThan(this string s, string other)
        {
            return string.Compare(s, other) > 0;
        }
        public static bool GreaterThanOrEqual(this string s, string other)
        {
            return string.Compare(s, other) >= 0;
        }
        public static bool LessThan(this string s, string other)
        {
            return string.Compare(s, other) < 0;
        }
        public static bool LessThanOrEqual(this string s, string other)
        {
            return string.Compare(s, other) <= 0;
        }

        public static string ReplaceExtendedAphabeticalChars(this string owner, string extendedCharsToPreserve = "")
        {
            var comacentos = "áéíóúÁÉÍÓÚàèìòùÀÈÌÒÙãeiõuÃEIÕUâêîôûÂÊÎÔÛäëïöüÄËÏÖÜçÇ";
            var semacentos = "aeiouAEIOUaeiouAEIOUaeiouAEIOUaeiouAEIOUaeiouAEIOUcC";

            for (int i = 0; i < comacentos.Length; i++)
            {
                owner = owner.Replace(comacentos[i], semacentos[i]);
            }

            Regex rgx = new Regex($"[^a-zA-Z0-9{extendedCharsToPreserve.Trim()} -]");

            return rgx.Replace(owner, String.Empty);
        }

        public static string ToPascalCase(this string owner, Boolean removeWhiteSpaces = true)
        {
            if (!removeWhiteSpaces)
                return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(owner);
            else
                return String.Join((removeWhiteSpaces ? String.Empty : " "), owner.Split(' ').Select(x => Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(x)));
        }
    }
}

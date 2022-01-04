namespace ERP.Furacao.Domain.Entities
{
    public class FornecedorItem : EntityBase
    {
        public string CodigoItem { get; set; }
        public string CodigoFornecedor { get; set; }
        public string NumeradorOriginal { get; set; }
        public string ItemSimilar { get; set; }
        public string Descricao { get; set; }
        public string VeiculosAplicacao { get; set; }
        public string Categoria { get; set; }
        public string PercentualDeVenda { get; set; }
        public string UnidadeMedida { get; set; }
        public double PrecoBruto { get; set; }
        public string NCM { get; set; }
        public string IPI { get; set; }
        public string ST { get; set; }
        public string Importado { get; set; }
        public string CurvaDeVenda { get; set; }
        public string Peso { get; set; }
        public string Altura { get; set; }
        public string Largura { get; set; }
        public string Comprimento { get; set; }
        public string CodigoDeBarras { get; set; }
        public string Cor { get; set; }
        public string Lado { get; set; }
        public string Amperagem { get; set; }
        public string Voltagem { get; set; }
        public string Potencia { get; set; }
        public string Segmento { get; set; }
        public string MultiploDeCompras { get; set; }
        public string QuantidadeDePecas { get; set; }

    }
}

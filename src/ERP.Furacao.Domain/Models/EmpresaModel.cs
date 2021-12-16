namespace ERP.Furacao.Domain.Models
{
    public class EmpresaModel
    {
        public int IdUsuario { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string NomeUsuario { get; set; }

        public int CodigoPessoa { get; set; }

        public string RazaoSocial { get; set; }

        public string CNPJ { get; set; }

        public string CodigoMasterFile { get; set; }
    }
}

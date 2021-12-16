using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Furacao.WebApi.Controllers.v1.Fornecedores
{
    /// <summary>
    /// Conta Controller
    /// </summary>
   
    [ApiVersion("1.0")]
    [ControllerName("Fornecedores")]
    public class FornecedoresController : BaseController
    {
        private readonly IEnumerable<string> _extensions = new List<string>{ ".xls" , ".xlsx"};

        [HttpPost("cadastrar-itens-upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> CadastrarItensUpload()
        {
            var file = Request?.Form?.Files[0];
            if(file  == null || file.Length == 0) return BadRequest();

            var extension = Path.GetExtension(file.FileName);
            if (!_extensions.Contains(extension.ToLower())) return BadRequest();
            
            return Ok();
        }
    }

}

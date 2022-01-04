using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Application.Services;
using ERP.Furacao.Domain.Entities;
using ERP.Furacao.Infrastructure.Data.Settings;
using ERP.Furacao.WebApi.Converters;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IExcelReaderService _excelReaderService;
        private readonly ExcelReaderSettings _excelReaderSettings;
        private readonly IFornecedorItemRepository _fornecedorItemRepository;
        private readonly IActionResultConverter _actionResultConverter;

        public FornecedoresController(
            IExcelReaderService excelReaderService,
            ExcelReaderSettings excelReaderSettings,
            IFornecedorItemRepository fornecedorItemRepository,
            IActionResultConverter actionResultConverter)
        {
            _excelReaderService = excelReaderService;
            _excelReaderSettings = excelReaderSettings;
            _fornecedorItemRepository = fornecedorItemRepository;
            _actionResultConverter = actionResultConverter;
        }

        [HttpPost("cadastrar-itens-upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> CadastrarItensUpload()
        {
            var file = Request?.Form?.Files[0];
            if (file == null || file.Length == 0) return BadRequest();

            var extension = Path.GetExtension(file.FileName);
            if (!_excelReaderSettings.Extensions.Contains(extension.ToLower())) return BadRequest();

            var filePath = $"{Path.GetTempFileName()}{extension}";

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var itens = _excelReaderService.ReadFromFile<FornecedorItem>(
                filePath, _excelReaderSettings.InitialLine, _excelReaderSettings.ColumnsConfig, _excelReaderSettings.Culture);

            var ok = await _fornecedorItemRepository.IncluirItensEmMassa(itens);

            if (ok) return _actionResultConverter.AcceptedResult(ok);

            return _actionResultConverter.BusinessErrorResult(new string[] { "Erro ao incluir dados da planilha" });
        }

        [HttpGet("listar-itens")]
        public async Task<IActionResult> ListarItens([FromHeader] int page, [FromHeader] int pageSize)
        {
            var result = await _fornecedorItemRepository.ListarItens(page, pageSize);
            return _actionResultConverter.PaginatedObjectResult(result.Data, result.TotalItems, result.Page, result.PageSize);
        }
    }

}

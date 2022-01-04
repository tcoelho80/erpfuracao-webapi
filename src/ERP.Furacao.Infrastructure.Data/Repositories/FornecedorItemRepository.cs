using Dapper;
using ERP.Furacao.Application.Pagination;
using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Domain.Entities;
using ERP.Furacao.Infrastructure.Data.Settings;
using Infrastructure.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace ERP.Furacao.Infrastructure.Data.Repositoriess
{
    public class FornecedorItemRepository : DapperBaseRepository<FornecedorItem>, IFornecedorItemRepository
    {
        public FornecedorItemRepository(DapperDbSettings dbSettings) : base(dbSettings)
        {
        }

        public async Task<bool> IncluirItensEmMassa(List<FornecedorItem> fornecedorItems)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                foreach (var item in fornecedorItems)
                {
                    var sql = @"INSERT INTO ERP.TBL_FORNECEDOR_ITEM
                              (ID, CODIGOITEM, NUMERADORORIGINAL, ITEMSIMILAR, DESCRICAO, VEICULOSAPLICACAO, CATEGORIA, PERCENTUALDEVENDA, UNIDADEMEDIDA, PRECOBRUTO, NCM, IPI, ST, IMPORTADO, CURVADEVENDA, PESO, ALTURA, LARGURA, COMPRIMENTO, CODIGODEBARRAS, COR, LADO, AMPERAGEM, VOLTAGEM, POTENCIA, SEGMENTO, MULTIPLODECOMPRAS, QUANTIDADEDEPECAS)
                              VALUES 
                              (SQ_TBL_FORNECEDOR.Nextval, :CodigoItem, :NumeradorOriginal, :ItemSimilar, :Descricao, :VeiculosAplicacao, :Categoria, :PercentualDeVenda, :UnidadeMedida, :PrecoBruto, :NCM, :IPI, :ST, :Importado, :CurvaDeVenda, :Peso, :Altura, :Largura, :Comprimento, :CodigoDeBarras, :Cor, :Lado, :Amperagem, :Voltagem, :Potencia, :Segmento, :MultiploDeCompras, :QuantidadeDePecas)";

                    var parameters = new DynamicParameters();
                    parameters.Add(":CodigoItem", item.CodigoItem, System.Data.DbType.String);
                    parameters.Add(":NumeradorOriginal", item.NumeradorOriginal, System.Data.DbType.String);
                    parameters.Add(":ItemSimilar", item.ItemSimilar, System.Data.DbType.String);
                    parameters.Add(":Descricao", item.Descricao, System.Data.DbType.String);
                    parameters.Add(":VeiculosAplicacao", item.VeiculosAplicacao, System.Data.DbType.String);
                    parameters.Add(":Categoria", item.Categoria, System.Data.DbType.String);
                    parameters.Add(":PercentualDeVenda", item.PercentualDeVenda, System.Data.DbType.String);
                    parameters.Add(":UnidadeMedida", item.UnidadeMedida, System.Data.DbType.String);
                    parameters.Add(":PrecoBruto", item.PrecoBruto, System.Data.DbType.Double);
                    parameters.Add(":NCM", item.NCM, System.Data.DbType.String);
                    parameters.Add(":IPI", item.IPI, System.Data.DbType.String);
                    parameters.Add(":ST", item.ST, System.Data.DbType.String);
                    parameters.Add(":Importado", item.Importado, System.Data.DbType.String);
                    parameters.Add(":CurvaDeVenda", item.CurvaDeVenda, System.Data.DbType.String);
                    parameters.Add(":Peso", item.Peso, System.Data.DbType.String);
                    parameters.Add(":Altura", item.Altura, System.Data.DbType.String);
                    parameters.Add(":Largura", item.Largura, System.Data.DbType.String);
                    parameters.Add(":Comprimento", item.Comprimento, System.Data.DbType.String);
                    parameters.Add(":CodigoDeBarras", item.CodigoDeBarras, System.Data.DbType.String);
                    parameters.Add(":Cor", item.Cor, System.Data.DbType.String);
                    parameters.Add(":Lado", item.Lado, System.Data.DbType.String);
                    parameters.Add(":Amperagem", item.Amperagem, System.Data.DbType.String);
                    parameters.Add(":Voltagem", item.Voltagem, System.Data.DbType.String);
                    parameters.Add(":Potencia", item.Potencia, System.Data.DbType.String);
                    parameters.Add(":Segmento", item.Segmento, System.Data.DbType.String);
                    parameters.Add(":MultiploDeCompras", item.MultiploDeCompras, System.Data.DbType.String);
                    parameters.Add(":QuantidadeDePecas", item.QuantidadeDePecas, System.Data.DbType.String);

                    await DbConnection.ExecuteAsync(sql, parameters);
                }

                transaction.Complete();
                return true;

            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<PaginatedResult<FornecedorItem>> ListarItens(int page, int pageSize)
        {
            return new PaginatedResult<FornecedorItem>()
            {
                Data = new List<FornecedorItem>() {
                    new FornecedorItem() { CodigoItem = "1234" },
                    new FornecedorItem() { CodigoItem = "2345" }
                },
                TotalItems = 2,
                Page = page,
                PageSize = pageSize
            };

            //var sql = @"SELECT ID, CODIGOITEM, NUMERADORORIGINAL, ITEMSIMILAR, DESCRICAO, VEICULOSAPLICACAO, CATEGORIA, PERCENTUALDEVENDA, UNIDADEMEDIDA, PRECOBRUTO, NCM, IPI, ST, IMPORTADO, CURVADEVENDA, PESO, ALTURA, LARGURA, COMPRIMENTO, CODIGODEBARRAS, COR, LADO, AMPERAGEM, VOLTAGEM, POTENCIA, SEGMENTO, MULTIPLODECOMPRAS, QUANTIDADEDEPECAS
            //            FROM ERP.TBL_FORNECEDOR_ITEM";
        }
    }
}

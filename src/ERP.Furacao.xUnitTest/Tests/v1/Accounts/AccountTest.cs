using Moq;
using ERP.Furacao.Domain.Models;
using ERP.Furacao.Infrastructure.Data.Contexts;
using ERP.Furacao.Infrastructure.Data.Repositories;
using System.Linq;
using Xunit;

namespace ERP.Furacao.xUnitTest.Tests.v1.Accounts
{
    public class AccountTest
    {

        [Fact]
        public void Register()
        {

            //Arrange
            var data = new[] { new EmpresaModel { Login = "Angelo" }, new EmpresaModel { Login = "Tamyres" } }.AsQueryable();
            var mock = new Mock<ApplicationContext>();
            mock.Setup(x => x.Set<EmpresaModel>().AsQueryable()).Returns(data);
            var context = mock.Object;
            var repository = new EmpresaRepository(context);
            //Act
            var blogs = repository.GetAll();
            //Assert
            Assert.Equal(data, blogs);

        }
    }
}

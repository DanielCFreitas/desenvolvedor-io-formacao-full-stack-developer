using AppSemTemplate.Controllers;
using AppSemTemplate.Data;
using AppSemTemplate.Models;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace Testes
{
    public class ControllerTests
    {
        [Fact]
        public void TesteController_Index_Sucesso()
        {
            // Arrange
            var controller = new TesteController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ProdutoController_Index_Sucesso()
        {
            // ===> Arrange

            // Resolvendo o AppDbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var ctx = new AppDbContext(options);

            ctx.Produtos.Add(new Produto() { Id = 1, Nome = "Produto 1", Valor = 10m });
            ctx.Produtos.Add(new Produto() { Id = 2, Nome = "Produto 2", Valor = 10m });
            ctx.Produtos.Add(new Produto() { Id = 3, Nome = "Produto 3", Valor = 10m });
            ctx.SaveChanges();

            // Resolvendo Identity
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(m => m.Name).Returns("teste@teste.com");

            var principal = new ClaimsPrincipal(mockClaimsIdentity.Object);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.User).Returns(principal);

            // Imagem Service
            var imagemService = new Mock<IImageUploadService>();

            // Controller
            var controller = new ProdutosController(ctx, imagemService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            // ====> Act
            var result = controller.Index().Result;

            // ====> Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ProdutoController_CriarProduto_Sucesso()
        {
            // ===> Arrange

            // Resolvendo o AppDbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Contexto
            var ctx = new AppDbContext(options);

            //IFormFile
            var fileMock = new Mock<IFormFile>();
            var fileName = "test.jpg";
            fileMock.Setup(_ => _.FileName).Returns(fileName);

            // Imagem Service
            var imagemService = new Mock<IImageUploadService>();
            imagemService.Setup(s => s.UploadArquivo(
                new ModelStateDictionary(), fileMock.Object, It.IsAny<string>())
            ).ReturnsAsync(true);

            // Produto
            var produto = new Produto()
            {
                Id = 1,
                ImagemUpload = fileMock.Object,
                Nome = "Teste",
                Valor = 50
            };

            // Controller
            var controller = new ProdutosController(ctx, imagemService.Object);

            // ====> Act
            var result = controller.CriarProduto(produto).Result;

            // ====> Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ProdutoController_CriarProduto_ErroValidacaoProduto()
        {
            // ===> Arrange

            // Resolvendo o AppDbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Contexto
            var ctx = new AppDbContext(options);

            // Imagem Service
            var imagemService = new Mock<IImageUploadService>();

            // Controller
            var controller = new ProdutosController(ctx, imagemService.Object);
            controller.ModelState.AddModelError("Nome", "O campo nome é requerido");

            // Produto
            var produto = new Produto()
            {
            };

            // ====> Act
            var result = controller.CriarProduto(produto).Result;

            // ====> Assert
            Assert.False(controller.ModelState.IsValid);
            Assert.IsType<ViewResult>(result);
        }
    }
}
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppSemTemplate.Services
{
    public interface IImageUploadService
    {
        public Task<bool> UploadArquivo(ModelStateDictionary modelState, IFormFile arquivo, string imgPrefixo);
    }
}

using ImageRecognize.API.Helpers;
using ImageRecognize.API.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Tesseract;

namespace ImageRecognize.API.Controllers
{
    /// <summary>
    /// API Recognize CNH
    /// </summary>
    [ApiController]
    [Route("[controller]")]    
    public class CNHRecognizeController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Recognize CNH Controller
        /// </summary>
        /// <param name="env"></param>
        public CNHRecognizeController(IWebHostEnvironment env)
        {
            _env = env;
        }
        /// <summary>
        /// Recognize CNH
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost]
        public CNH Index(IFormFile image)
        {
            CNH cnh = new CNH();
            string textSearch = string.Empty;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();           
                using (var engine = new TesseractEngine((_env.ContentRootPath + "/tessdata"), "por", EngineMode.Default))
                {
                    using var imgPix = Pix.LoadFromMemory(fileBytes);
                    using var page = engine.Process(imgPix);
                    textSearch = page.GetText();
                }

                var split = textSearch.Split("\n");
                split = split.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();

                if (split.Count() > 0)
                {
                    cnh.Nome = FilterBlocks.GetName(split);
                    cnh.Identidade = FilterBlocks.GetIdentidade(split);
                    cnh.OrgaoEmissor = FilterBlocks.GetOrgaoEmissor(split);
                    cnh.UF = FilterBlocks.GetUF(split);
                    cnh.CPF = FilterBlocks.GetCPF(split);
                    cnh.DataNascimento = FilterBlocks.GetDataNascimento(split);
                    cnh.Pai = FilterBlocks.GetPai(split);
                    cnh.Mae = FilterBlocks.GetMae(split);
                    cnh.NumeroRegistro = FilterBlocks.GetNumeroRegistro(split);
                    cnh.Validade = FilterBlocks.GetValidade(split);
                    cnh.PrimeiraHabilitacao = FilterBlocks.GetPrimeiraHabilitacao(split);
                    cnh.DataEmissao = FilterBlocks.GetDataEmissao(split);
                    cnh.LocalEmissao = FilterBlocks.GetLocalEmissao(split);
                }
            }
            return cnh;
        }

    }
}

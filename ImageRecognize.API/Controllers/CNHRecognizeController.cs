using ImageRecognize.API.Helpers;
using ImageRecognize.API.Model;
using IronOcr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ImageRecognize.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CNHRecognizeController : ControllerBase
    {
        /// <summary>
        /// Recognize CNH
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost]
        public CNH Index(IFormFile image)
        {
            CNH cnh = new CNH();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                using var img = Image.FromStream(memoryStream);
                var bmp = (Bitmap)img;
                var Ocr = new IronTesseract
                {
                    Language = OcrLanguage.Portuguese,
                    Configuration = new TesseractConfiguration
                    {
                        EngineMode = TesseractEngineMode.TesseractAndLstm,
                        TesseractVersion = TesseractVersion.Tesseract5,
                        BlackListCharacters  = "~`$#^*_}{][|\\",
                        PageSegmentationMode = TesseractPageSegmentationMode.Auto
                    }
                };

                bmp = ImagePreProcessing.OptimizeOCR(bmp);
                bmp = ImagePreProcessing.Grayscale(bmp);
                bmp = ImagePreProcessing.Contrast(bmp, 70);

                using (var Input = new OcrInput())
                {
                    Input.AddImage(bmp);
                    Input.Deskew();           

                    var Result = Ocr.Read(Input);

                    cnh.Nome = FilterBlocks.GetName(Result.Blocks);
                    cnh.Identidade = FilterBlocks.GetIdentidade(Result.Blocks);
                    cnh.OrgaoEmissor = FilterBlocks.GetOrgaoEmissor(Result.Blocks);
                    cnh.UF = FilterBlocks.GetUF(Result.Blocks);       
                    cnh.CPF = FilterBlocks.GetCPF(Result.Blocks);
                    cnh.DataNascimento = FilterBlocks.GetDataNascimento(Result.Blocks);
                    cnh.Pai = FilterBlocks.GetPai(Result.Blocks);
                    cnh.Mae = FilterBlocks.GetMae(Result.Blocks);
                    cnh.Validade = FilterBlocks.GetValidade(Result.Blocks);
                    cnh.PrimeiraHabilitacao = FilterBlocks.GetPrimeiraHabilitacao(Result.Blocks);
                    cnh.DataEmissao = FilterBlocks.GetDataEmissao(Result.Blocks);
                }

                string file = DateTime.Now.ToString("ddMMyyyyHHmmss")+".jpg";
                bmp.Save("C:/ImageProcesssing/"+ file);
            }

            
            return cnh;
        }


    }
}

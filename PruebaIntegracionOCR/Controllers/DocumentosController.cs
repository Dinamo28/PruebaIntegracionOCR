using ClbModOCR;
using ClbModOCR.Common;
using ClbNegOCR;
using Microsoft.AspNetCore.Mvc;

namespace PruebaIntegracionOCR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentosController : Controller
    {
        private readonly ClsNegDocumento _clsNegDocumento = null;
        public DocumentosController()
        {
            _clsNegDocumento = new ClsNegDocumento();
        }

        [HttpPost("GuardaDocumento")]
        public ClsModResult GuardaDocumento(IFormFile file, string nombre)
        {
            ClsModDocumentos doc = new ClsModDocumentos();
            doc.NombreArchivo = nombre;
            doc.NombreOriginalDoc = file.FileName;
            doc.DocumentContent = file.ContentType;
            doc.Extension = file.ContentDisposition.Trim('"').Split(".")[1];


            return _clsNegDocumento.GuardaDocumento(file, doc);
        }
    }
}

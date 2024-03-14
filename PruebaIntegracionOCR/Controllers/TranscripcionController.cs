using ClbModOCR;
using ClbModOCR.Common;
using ClbNegOCR;
using Microsoft.AspNetCore.Mvc;

namespace PruebaIntegracionOCR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranscripcionController : Controller
    {
        private readonly ClsNegTranscripcion _clsNegTranscripcion = null;
        public TranscripcionController()
        {
            _clsNegTranscripcion = new ClsNegTranscripcion();
        }


        [HttpPost("Analyze")]
        [RequestSizeLimit(200_000_000)]
        public async Task<ClsModResult> TranscribirDocumento([FromForm] IFormFile file)
        {
            ClsModResult clsModResult = new ClsModResult();
            try
            {
                clsModResult = await _clsNegTranscripcion.TranscribirArchivo(file);
            }
            catch (Exception ex)
            {
                clsModResult.MsgError = ex.Message;
            }
            return clsModResult;

        }
        [HttpPost("Guardar")]
        public ClsModResult GuardarTranscripcion(ClsModTranscripcion transcripcion)
        {
            return _clsNegTranscripcion.GuardarTranscripcion(transcripcion.IdRelDocumento, transcripcion.IdRelEmpresa, transcripcion.NombreTranscripcion, transcripcion.PorcentajeConfianza, transcripcion.IdUsuarioCreacion, transcripcion.Transcripcion);
        }

        [HttpPost("Coincidencias")]
        public ClsModResult GetListCoincidences(int[] ids)
        {
            return _clsNegTranscripcion.GetListCoincidences(ids);
        }

        [HttpGet("Get")]
        public IEnumerable<ClsModLegibilidad> GetLegibilidad()
        {
            return _clsNegTranscripcion.GetLegibilidad();
        }

        [HttpDelete("Borrar")]
        public ClsModResult BorrarTranscripcion(int idDocumento, int idUsuario)
        {
            Console.WriteLine("EntroBorrar: IdTransc" + idDocumento + " | IdUser " + idUsuario);
            return _clsNegTranscripcion.BorrarTranscripcion(idDocumento, idUsuario);
        }
    }
}

using ClbDatOCR;
using ClbModOCR;
using ClbModOCR.Common;
using Microsoft.AspNetCore.Http;

namespace ClbNegOCR
{
    public class ClsNegTranscripcion
    {
        private readonly ClsDatTranscripcion _clsDatTranscripcion = new ClsDatTranscripcion();
        public ClsNegTranscripcion()
        {
            _clsDatTranscripcion = new ClsDatTranscripcion();
        }

        public async Task<ClsModResult> TranscribirArchivo(IFormFile file)
        {
            return await _clsDatTranscripcion.TranscribirArchivo(file);
        }

        public ClsModResult GuardarTranscripcion(int IdRelDocumento, int IdRelEmpresa, string NombreTranscripcion, double PorcentajeConfianza, int IdUsuarioCreacion, string Transcripcion)
        {
            return _clsDatTranscripcion.GuardarTranscripcion(IdRelDocumento, IdRelEmpresa, NombreTranscripcion, PorcentajeConfianza, IdUsuarioCreacion, Transcripcion);
        }

        public ClsModResult GetListCoincidences(int[] ids)
        {
            return _clsDatTranscripcion.GetListCoincidences(ids);
        }

        public IEnumerable<ClsModLegibilidad> GetLegibilidad()
        {
            return _clsDatTranscripcion.GetLegibilidad();
        }

        public ClsModResult BorrarTranscripcion(int idDocumento, int idUsuario)
        {
            return _clsDatTranscripcion.BorrarTranscripcion(idDocumento, idUsuario);
        }

        //public string CreatePdfFile(string text, string nombre)
        //{
        //    return _clsDatTranscripcion.CreatePdfFile(text, nombre);
        //}
    }
}

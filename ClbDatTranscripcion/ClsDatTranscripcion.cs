
using ClbDatOCR.Common;
using ClbModOCR;
using ClbModOCR.Common;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace ClbDatOCR
{
    public class ClsDatTranscripcion : DatabaseConnections
    {
        private readonly string TAG = typeof(ClsDatTranscripcion).ToString();
        private readonly AzureFunctions _azureFunctions;

        public ClsDatTranscripcion()
        {
            _azureFunctions = new AzureFunctions();
        }
        public async Task<ClsModResult> TranscribirArchivo(IFormFile file)
        {
            ClsModResult result = new ClsModResult();
            try
            {
                TempTranscript Transcript = await _azureFunctions.GetTextFromImage(file);
                result.Object = new { porcentaje = Transcript.percentage, transcrito = Transcript.transcript, nombre = file.FileName };
            }
            catch (Exception ex)
            {
                result.MsgError = $"{TAG} - TranscribirArchivo() : {ex.Message}";
            }

            return result;
        }

        public ClsModResult GuardarTranscripcion(int IdRelDocumento, int IdRelEmpresa, string NombreTranscripcion, double PorcentajeConfianza, int IdUsuarioCreacion, string Transcripcion)
        {
            ClsModResult result = new();

            string[] respuesta = _azureFunctions.CreatePdfFile(Transcripcion, NombreTranscripcion);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdRelDocumento", IdRelDocumento);
            parameters.Add("@IdRelEmpresa", IdRelEmpresa);
            parameters.Add("@NombreTranscripcion", respuesta[1]);
            parameters.Add("@PorcentajeConfianza", PorcentajeConfianza);
            parameters.Add("@RutaTranscripcion", respuesta[0]);
            parameters.Add("@FechaCreacion", DateTime.Now);
            parameters.Add("@IdUsuarioCreacion", IdUsuarioCreacion);
            parameters.Add("@Transcripcion", Transcripcion);
            try
            {
                using (var conexion = new SqlConnection(base.GetConnectionString()))
                {
                    int res = conexion.QueryFirstOrDefault<int>("SpdGuardarTranscripcion", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    result.Object = res != 0 ? res : null;
                }
            }
            catch (Exception ex)
            {
                result.MsgError = $"{TAG} - GuardarTranscripcion() : {ex.Message}";
            }
            return result;
        }


        public ClsModResult GetListCoincidences(int[] ids)
        {
            ClsModResult result = new ClsModResult();
            try
            {
                string array = string.Join(",", ids);
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@array", array);

                using (var conexion = new SqlConnection(base.GetConnectionString()))
                {
                    var res = conexion.Query("SpdGetListCoincidences", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    result.Object = res;
                }
            }
            catch (Exception ex)
            {
                result.MsgError = $"{TAG} - GetListCoincidences() : {ex.Message}";
            }


            return result;
        }

        public IEnumerable<ClsModLegibilidad> GetLegibilidad()
        {
            try
            {
                using (var conexion = new SqlConnection(base.GetConnectionString()))
                {
                    return conexion.Query<ClsModLegibilidad>("SpdGetLegibilidad", commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{TAG} - GetLegibilidad() : {ex.Message}");
                return null;
            }
        }
    }
}

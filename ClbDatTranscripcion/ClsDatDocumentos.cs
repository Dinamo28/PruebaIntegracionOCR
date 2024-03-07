using ClbDatOCR.Common;
using ClbModOCR;
using ClbModOCR.Common;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace ClbDatOCR
{

    public class ClsDatDocumentos : DatabaseConnections
    {
        private readonly string TAG = typeof(ClsDatDocumentos).ToString();
        public ClsModResult GuardarDocumento(ClsModDocumentos documento, IFormFile file)
        {
            ClsModResult result = new ClsModResult();

            Stream stream = file.OpenReadStream();
            stream.CopyTo(File.Create("C:\\Proyectos\\Documentos\\Docs\\" + file.FileName));
            documento.Ruta = "C:\\Proyectos\\Documentos\\Docs\\" + file.FileName;
            documento.Activo = true;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@NombreArchivo", documento.NombreArchivo);
            parameters.Add("@NombreOriginalDoc", documento.NombreOriginalDoc);
            parameters.Add("@DocumentContent", documento.DocumentContent);
            parameters.Add("@Extension", documento.Extension);
            parameters.Add("@Ruta", documento.Ruta);
            parameters.Add("@FechaCreacion", DateTime.Now);
            parameters.Add("@FechaModificacion", DateTime.Now);
            parameters.Add("@IdEstatus", documento.IdEstatus);
            parameters.Add("@Activo", documento.Activo);
            parameters.Add("@FechaDocumento", DateTime.Now);
            parameters.Add("@FechaVencimiento", DateTime.Now.AddMonths(1));
            parameters.Add("@IdTablaRelacion", documento.IdTablaRelacion);
            parameters.Add("@IdRelacion", documento.IdRelacion);
            parameters.Add("@IdDocumentoRelacion", documento.IdDocumentoRelacion);
            parameters.Add("@Latitud", documento.Latitud);
            parameters.Add("@Longitud", documento.Longitud);
            parameters.Add("@Comentario", documento.Comentario);
            parameters.Add("@IdRuta", documento.IdRutaDocumento);
            parameters.Add("@Carpeta", documento.Carpeta);
            parameters.Add("@IdClaveDesencriptar", documento.IdClaveDesencriptar);
            parameters.Add("@NombreOriginalDoc", documento.NombreOriginalDoc);
            parameters.Add("@CodCategoriaRelTabla", documento.CodCategoriaRelTabla);


            using (var conexion = new SqlConnection(base.GetConnectionString()))
            {
                var res = conexion.QueryFirstOrDefault("SpdInsertarDocumento", parameters, commandType: System.Data.CommandType.StoredProcedure);
                result.Object = res != 0 ? res : null;
            }

            return result;
        }
    }


}

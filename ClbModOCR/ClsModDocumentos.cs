using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClbModOCR
{
    
    public class ClsModDocumentos
    {
        [Key]
        public int IdDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string DocumentContent { get; set; }
        public string Extension { get; set; }
        public string Ruta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int IdEstatus { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaDocumento { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int IdTablaRelacion { get; set; }
        public int IdRelacion { get; set; }
        public int IdDocumentoRelacion { get; set; }
        public int Latitud { get; set; }
        public int Longitud { get; set; }
        public string Comentario { get; set; }
        public int IdRutaDocumento { get; set; }
        public string Carpeta { get; set; }
        public int IdClaveDesencriptar { get; set; }
        public string NombreOriginalDoc { get; set; }
        public int CodCategoriaRelTabla { get; set; }

    }
}

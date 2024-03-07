using System.ComponentModel.DataAnnotations;

namespace ClbModOCR
{
    public class ClsModTranscripcion
    {
        [Key]
        public int IdTranscripcion { get; set; }
        public int IdRelDocumento { get; set; }
        public int IdRelEmpresa { get; set; }
        public string NombreTranscripcion { get; set; }
        public float PorcentajeConfianza { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public string Transcripcion { get; set; }
    }
}

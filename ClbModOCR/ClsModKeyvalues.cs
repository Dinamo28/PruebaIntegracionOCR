using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClbModOCR
{
    public class ClsModKeyvalues
    {
        [Key]
        public int IdKeyValues { get; set; }
        public int IdRelDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string KeyValues { get; set; }
        public DateTime FechaExtraccion { get; set; }
        public int IdUsuarioPeticion { get; set; }
    }
}

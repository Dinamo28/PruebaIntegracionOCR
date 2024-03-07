using ClbDatOCR;
using ClbModOCR;
using ClbModOCR.Common;
using Microsoft.AspNetCore.Http;

namespace ClbNegOCR
{
    public class ClsNegDocumento
    {
        private readonly ClsDatDocumentos _clsDatDocumentos = null;
        public ClsNegDocumento()
        {
            _clsDatDocumentos = new ClsDatDocumentos();
        }

        public ClsModResult GuardaDocumento(IFormFile file, ClsModDocumentos doc)
        {
            return _clsDatDocumentos.GuardarDocumento(doc, file);
        }


    }
}

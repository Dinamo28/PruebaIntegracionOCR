
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClbModOCR.Common
{
    public class ClsModResult
    {
        public ClsModResult()
        {
            MsgError = string.Empty;
        }
        public dynamic Object { get; set; }
        public string MsgError { get; set; }
        public bool isError
        {
            get
            {
                return MsgError != "" ? true : false;
            }
        }
        public dynamic Items { get; set; }
        public int Cant { get; set; }
    }
}

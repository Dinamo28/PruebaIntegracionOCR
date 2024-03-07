using System.Configuration;

namespace ClbDatOCR.Common
{
    public class DatabaseConnections
    {
        public string GetConnectionString()
        {
            var varConec = ConfigurationManager.ConnectionStrings["Conexion"];

            if (varConec != null)
            {
                return varConec.ToString();
            }

            return string.Empty;
        }
        public string GetAzureEndpoint()
        {
            var varConec = ConfigurationManager.ConnectionStrings["endpoint"];

            if (varConec != null)
            {
                return varConec.ToString();
            }

            return string.Empty;
        }
        public string GetAzureKey()
        {
            var varConec = ConfigurationManager.ConnectionStrings["key"];

            if (varConec != null)
            {
                return varConec.ToString();
            }

            return string.Empty;
        }
    }
}

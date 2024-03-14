using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PruebaIntegracionOCR.Middleware
{
    public class ClsEncryptionMiddleware
    {
        private readonly string _Key;
        private readonly string _IV;
        private readonly RequestDelegate _next;

        public ClsEncryptionMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _Key = config.GetSection("AppSettings")["KeyEncrypt"];
            _IV = config.GetSection("AppSettings")["IVEncriptar"];
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.ToString() != "GET"
                    && !context.Request.Path.Value.Contains("/Analyze")
                    && !context.Request.Path.Value.Contains("/TestPostFile"))
            {
                using var reader = new StreamReader(context.Request.Body);
                var requestBody = await reader.ReadToEndAsync();
                //MemoryStream msDecrypt = new MemoryStream(requestBody);
                string txt = DecryptDataAES(requestBody, _Key, _IV);

                try
                {
                    int valor = Convert.ToInt32(txt);
                    context.Request.Body = new MemoryStream(BitConverter.GetBytes(valor));

                }
                catch (Exception)
                {

                    var objeto = JsonSerializer.Deserialize<JsonObject>(txt);

                    string cadena = JsonSerializer.Serialize<JsonObject>(objeto);

                    context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(cadena));
                }

            }
            else
            {


            }
            if (!context.Request.Path.Value.Contains("/Analyze")
                    && !context.Request.Path.Value.Contains("/TestPostFile"))
            {
                var originalBody = context.Response.Body;

                //await _next(context);
                // Aquí interceptamos la respuesta antes de que se envíe de vuelta al cliente


                using (var memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;

                    await _next(context);

                    //if (context.Response.StatusCode == 200 && context.Response.ContentType?.Contains("application/json") == true)
                    //{

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(memoryStream))
                    {
                        var responseBody = await reader.ReadToEndAsync();
                        var encryptedBody = EncryptDataAES(responseBody, _Key, _IV);
                        var obj = new { response = encryptedBody };
                        string cadena = JsonSerializer.Serialize(obj);

                        context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(cadena));
                        context.Response.ContentType = "application/json";

                        await context.Response.Body.CopyToAsync(originalBody);
                    }
                    //}
                }
            }
            else
            {
                await _next(context);
            }
        }


        private string EncryptDataAES(string strTextoPlano, string strKey, string strIV)
        {
            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(strTextoPlano);

                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.Key = Convert.FromBase64String(strKey);
                    aes.IV = Convert.FromBase64String(strIV);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                        {
                            cs.Write(plainBytes, 0, plainBytes.Length);
                            cs.FlushFinalBlock();
                        }

                        // Convertir el texto cifrado a Base64 para que sea fácilmente legible
                        string encryptedText = Convert.ToBase64String(ms.ToArray());
                        return encryptedText;
                    }
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante la encriptación.
                Console.WriteLine("Error durante la encriptación: " + ex.Message);
                return null;
            }
        }


        private string DecryptDataAES(string strTextoEncriptado, string strKey, string strIV)
        {
            try
            {
                var objeto = JsonSerializer.Deserialize<Datos>(strTextoEncriptado);

                string cadena = objeto.Encrypt;

                string decrypted = null;
                byte[] cipher = Convert.FromBase64String(cadena);

                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.Key = Convert.FromBase64String(strKey);
                    aes.IV = Convert.FromBase64String(strIV);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform dec = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(cipher))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, dec, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                decrypted = sr.ReadToEnd();
                            }
                        }
                    }
                }
                return decrypted;
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante la desencriptación.
                Console.WriteLine("Error durante la desencriptación: " + ex.Message);
                return null;
            }

        }
    }
    public class Datos
    {
        public string Encrypt { get; set; }
    }
}

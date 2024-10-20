using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Shree_API_AWS.Attributes
{
    public class EncryptResponseAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is OkObjectResult okResult)
            {
                var plainText = okResult.Value;

                if (plainText != null)
                {
                    // Stringify the object or array into JSON
                    var jsonString = JsonConvert.SerializeObject(plainText);

                    // Encrypt the response
                    var (encryptedData, key, iv) = EncryptString(jsonString);
                    context.Result = new OkObjectResult(new { EncryptedData = encryptedData, Key = key, IV = iv });
                }
            }
        }

        private (string EncryptedData, byte[] Key, byte[] IV) EncryptString(string plainText)
        {
            // Generate a random key and IV
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[32]; // 32 bytes for AES-256
                rng.GetBytes(key);

                byte[] iv = new byte[16]; // 16 bytes for AES
                rng.GetBytes(iv);

                // Create the AES encryption object
                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            {
                                using (var writer = new StreamWriter(cs))
                                {
                                    writer.Write(plainText);
                                }
                                return (Convert.ToBase64String(ms.ToArray()), key, iv);
                            }
                        }
                    }
                }
            }
        }
    }
}

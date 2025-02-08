using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Shree_API_AWS.Attributes
{
    public class EncryptResponseAttribute : ActionFilterAttribute
    {
        private static readonly byte[] StaticKey = Convert.FromBase64String("rW5zPz4aF7XhU7FQh5mXqFmdXoFtPrD0SpLZa5DhS2c="); // Replace with a 32-byte Base64 key
        private static readonly byte[] StaticIV = Convert.FromBase64String("D7K1vOJHQ5+7XcAsxu7Q3w=="); // Replace with a 16-byte Base64 IV

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
                    var encryptedData = EncryptString(jsonString);
                    context.Result = new OkObjectResult(new { EncryptedData = encryptedData });
                }
            }
        }

        private string EncryptString(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = StaticKey;
                aes.IV = StaticIV;

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
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }
    }
}

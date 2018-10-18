using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using IRunes.Services.Contracts;

namespace IRunes.Services
{
    public class UserCookieService : IUserCookieService
    {
        public const string EncryptKey = "E246C8DF278CD5931069B522E695D4F2";

        public string GetUserCookie(string userName)
        {
            string cookieContent = EncryptString(userName, EncryptKey);

            return cookieContent;
        }

        public string GetUserData(string cookieContent)
        {
            string username = DecryptString(cookieContent, EncryptKey);

            return username;
        }

        public static string EncryptString(string text, string keyString)
        {
            byte[] key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(text);
                            }
                        }

                        byte[] iv = aesAlg.IV;

                        byte[] decryptedContent = msEncrypt.ToArray();

                        byte[] result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            byte[] iv = new byte[16];
            byte[] cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);

            byte[] key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;

                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}

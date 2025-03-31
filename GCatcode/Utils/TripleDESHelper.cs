using System.Security.Cryptography;
using System.Text;

namespace GCatcode.Utils
{
    public static class TripleDESHelper
    {
        private static readonly string key = "^TeBNGb0LhR#\"ou_/|$V0zh+"; // Debe ser de 24 bytes

        public static string Encrypt(string plainText)
        {
            try
            {
                using (TripleDES tripleDES = TripleDES.Create())
                {
                    tripleDES.Key = Encoding.UTF8.GetBytes(key);
                    tripleDES.IV = new byte[8]; // Inicialización en cero

                    ICryptoTransform encryptor = tripleDES.CreateEncryptor(tripleDES.Key, tripleDES.IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                return plainText;
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                using (TripleDES tripleDES = TripleDES.Create())
                {
                    tripleDES.Key = Encoding.UTF8.GetBytes(key);
                    tripleDES.IV = new byte[8]; // Inicialización en cero

                    ICryptoTransform decryptor = tripleDES.CreateDecryptor(tripleDES.Key, tripleDES.IV);

                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return cipherText;
            }
        }
    }
}
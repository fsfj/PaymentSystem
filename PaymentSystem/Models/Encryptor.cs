using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Models
{
    public class Encryptor
    {
        public string EncryptText(string plainText, string secretKey, string salt, int keySize, RijndaelManaged algorithm)
        {
            string encryptedPassword = string.Empty;

            using (MemoryStream outputStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(outputStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] inputBuffer = Encoding.Unicode.GetBytes(plainText);
                    cryptoStream.Write(inputBuffer, 0, inputBuffer.Length);
                    cryptoStream.FlushFinalBlock();
                    encryptedPassword = Convert.ToBase64String(outputStream.ToArray());
                }
            }

            return encryptedPassword;
        }

        public string DecryptText(string encryptedBytes, string secretKey, string salt, int keySize, RijndaelManaged algorithm)
        {
            string plainText = string.Empty;

            using (MemoryStream inputStream = new MemoryStream(Convert.FromBase64String(encryptedBytes)))
            {
                using (CryptoStream cryptoStream = new CryptoStream(inputStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] outputBuffer = new byte[Convert.ToInt32(inputStream.Length - 1) + 1];
                    int readBytes = cryptoStream.Read(outputBuffer, 0, Convert.ToInt32(inputStream.Length));
                    plainText = Encoding.Unicode.GetString(outputBuffer, 0, readBytes);
                }
            }

            return plainText;
        }

        public RijndaelManaged getAlgorithm(string secretKey, string salt, int keySize)
        {
            Rfc2898DeriveBytes keyBuilder = new Rfc2898DeriveBytes(secretKey, Encoding.Unicode.GetBytes(salt));
            RijndaelManaged algorithm = new RijndaelManaged();
            algorithm.KeySize = keySize;
            algorithm.IV = keyBuilder.GetBytes(Convert.ToInt32(algorithm.BlockSize / 8));
            algorithm.Key = keyBuilder.GetBytes(Convert.ToInt32(algorithm.KeySize / 8));
            algorithm.Padding = PaddingMode.PKCS7;

            return algorithm;
        }

        public EncryptionKeyUpdated EncryptionKey()
        {
            EncryptionKeyUpdated ek = new EncryptionKeyUpdated();

            try
            {
                ek.oSecretKey = "mySecretKey";
                ek.oSalt = "mySecretSalt";
                ek.oKeySize = 256;
                ek.oAlgorithm = getAlgorithm(ek.oSecretKey, ek.oSalt, ek.oKeySize);
                ek.WithEncryption = true;
            }
            catch
            {
                ek.oSecretKey = "";
                ek.oSalt = "";
                ek.oKeySize = 0;
                ek.oAlgorithm = new RijndaelManaged();
                ek.WithEncryption = false;
            }

            return ek;
        }

        public string Encrypt(string txt, EncryptionKeyUpdated ek)
        {
            string encryptString = string.Empty;

            try
            {
                if (ek.WithEncryption == true)
                {
                    encryptString = (string.IsNullOrEmpty(txt)) ? "" : EncryptText(txt, ek.oSecretKey, ek.oSalt, ek.oKeySize, ek.oAlgorithm);
                }
                else
                {
                    encryptString = txt;
                }
            }
            catch
            {
                encryptString = "ERROR";
            }

            return encryptString;
        }

        public string Decrypt(string txt, EncryptionKeyUpdated ek)
        {
            string decryptString = string.Empty;

            try
            {
                if (ek.WithEncryption == true)
                {
                    decryptString = (string.IsNullOrEmpty(txt)) ? "" : DecryptText(txt, ek.oSecretKey, ek.oSalt, ek.oKeySize, ek.oAlgorithm);
                }
                else
                {
                    decryptString = txt;
                }
            }
            catch
            {
                decryptString = "ERROR";
            }

            return decryptString;
        }
    }

    public class EncryptionKeyUpdated
    {
        public string oSecretKey { get; set; }
        public string oSalt { get; set; }
        public int oKeySize { get; set; }
        public RijndaelManaged oAlgorithm { get; set; }
        public bool WithEncryption { get; set; }
    }
}

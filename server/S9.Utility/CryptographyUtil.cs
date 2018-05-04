using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using System.IO;



namespace S9.Utility
{
    public class CryptographyUtil<T> where T: class
    {
        public static string PASSWORD = "S9123###";


        #region "Hash Cryptography"
        ////**************
        // Asymetric encryption (hasing) is one way, cannot get the original plain text
        // Algorithms     size digest
        // MD5
        // SHA1           28 byte
        // SHA256         44 byte
        // SHA384         64 byte
        // SHA512         88 byte
        ////***************
        public static string EncryptedMD5(T obj)
        {
            string serializedObj = JSON<T>.Serialize(obj);

            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] hashedBytes = encoder.GetBytes(serializedObj);
            //Dim hashedBytes() As Byte = Encoding.ASCII.GetBytes(txtdata)
            byte[] encryptedData = md5Hasher.ComputeHash(hashedBytes);
            string txtencrypted = null;
            txtencrypted = Convert.ToBase64String(encryptedData);
            return txtencrypted;
        }

        public static string EncryptedSHA1(T obj)
        {
            string serializedObj = JSON<T>.Serialize(obj);

            SHA1Managed sha1Hasher = new SHA1Managed();
            byte[] hashedBytes = Encoding.ASCII.GetBytes(serializedObj);
            byte[] encryptedData = sha1Hasher.ComputeHash(hashedBytes);
            string txtencrypted = null;
            txtencrypted = Convert.ToBase64String(encryptedData);
            return txtencrypted;
        }

        public static string EncryptedSHA256(T obj)
        {
            string serializedObj = JSON<T>.Serialize(obj);

            SHA256Managed sha256Hasher = new SHA256Managed();
            byte[] hashedBytes = Encoding.ASCII.GetBytes(serializedObj);
            byte[] encryptedData = sha256Hasher.ComputeHash(hashedBytes);
            string txtencrypted = null;
            txtencrypted = Convert.ToBase64String(encryptedData);
            return txtencrypted;
        }

        public static string EncryptedSHA384(T obj)
        {
            string serializedObj = JSON<T>.Serialize(obj);

            SHA384Managed sha384Hasher = new SHA384Managed();
            byte[] hashedBytes = Encoding.ASCII.GetBytes(serializedObj);
            byte[] encryptedData = sha384Hasher.ComputeHash(hashedBytes);
            string txtencrypted = null;
            txtencrypted = Convert.ToBase64String(encryptedData);
            return txtencrypted;
        }

        public static string EncryptedSHA512(T obj)
        {
            string serializedObj = JSON<T>.Serialize(obj);

            SHA512Managed sha512Hasher = new SHA512Managed();
            byte[] hashedBytes = Encoding.ASCII.GetBytes(serializedObj);
            byte[] encryptedData = sha512Hasher.ComputeHash(hashedBytes);
            string txtencrypted = null;
            txtencrypted = Convert.ToBase64String(encryptedData);
            return txtencrypted;
        }

        #endregion



        #region Encrypt / Decrypt Symmetric

        /// <summary>
        /// // Symetric encryption is two way, can get the original plain text
        /// // Sample code string
        ///             string encrypted = CryptographyUtil<string>.Encrypt("hello", "1234");
        ///             string decrypted = CryptographyUtil<string>.Decrypt(encrypted, "1234");
        /// // Sample code object
        ///             Car c = new Car();
        ///             c.ID = 1;
        ///             c.AreaID = 1;
        ///             c.Brand = "Toyota";
        ///             c.Color = "black";
        ///             c.PlateNumber = "1234";
        ///             string encryptedObj = CryptographyUtil<Car>.Encrypt(c, "1234");
        ///             Car decryptedObj = CryptographyUtil<Car>.Decrypt(encryptedObj, "1234");
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Encrypt(T obj, string salt)
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(PASSWORD, Encoding.Unicode.GetBytes(salt));
            
            SymmetricAlgorithm algorithm = new TripleDESCryptoServiceProvider();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        string serializedObj = JSON<T>.Serialize(obj);
                        writer.Write(serializedObj);
                    }
                }

                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        public static T Decrypt(string encryptedText, string salt)
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(PASSWORD, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new TripleDESCryptoServiceProvider();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(encryptedText)))
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                    {
                        T deserializedObj = JSON<T>.Deserialize(reader.ReadToEnd());
                        return deserializedObj;
                    }
                }
            }
        }

        #endregion
    }
}

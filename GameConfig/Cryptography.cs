using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GameConfig
{
    /// <summary>
    /// Cryptography Extension using Rijndael
    /// </summary>
    public static class Cryptography
    {
        /// <summary>
        /// While an app specific salt is not the best practice for
        /// password based encryption, it's probably safe enough as long as
        /// it is truly uncommon. Also too much work to alter this answer otherwise.
        /// </summary>
        private static byte[] salt = Encoding.ASCII.GetBytes("6t672zd8j2i##@5677a&/&$/9=H/$/9*'*~3");

        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="encryptionKey">The encryption key to encrypt the Data</param>
        /// <returns>Returns a encrypted string</returns>
        public static string EncryptStringAES(string plainText, string encryptionKey)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException("plainText");
            }

            if (string.IsNullOrEmpty(encryptionKey))
            {
                throw new ArgumentNullException("encryptionKey");
            }

            // Encrypted string to return
            string outStr = null;

            // RijndaelManaged object used to encrypt the data.
            RijndaelManaged aesAlg = null;

            try
            {
                // generate the key from the shared secret and the salt
                using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(encryptionKey, salt))
                {
                    // Create a RijndaelManaged object
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        // prepend the IV
                        msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                // Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                        }

                        outStr = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                {
                    aesAlg.Clear();
                    aesAlg.Dispose();
                }
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="encryptionKey">The encryption key to decrypt the data</param>
        /// <returns>Returns a decrypted string</returns>
        public static string DecryptStringAES(string cipherText, string encryptionKey)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException("cipherText");
            }

            if (string.IsNullOrEmpty(encryptionKey))
            {
                throw new ArgumentNullException("encryptionKey");
            }

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plainText = null;

            try
            {
                // generate the key from the shared secret and the salt
                using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(encryptionKey, salt))
                {
                    // Create the streams used for decryption.                
                    byte[] bytes = Convert.FromBase64String(cipherText);
                    using (MemoryStream msDecrypt = new MemoryStream(bytes))
                    {
                        // Create a RijndaelManaged object
                        // with the specified key and IV.
                        aesAlg = new RijndaelManaged();
                        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                        // Get the initialization vector from the encrypted stream
                        aesAlg.IV = ReadByteArray(msDecrypt);

                        // Create a decrytor to perform the stream transform.
                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plainText = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                {
                    aesAlg.Clear();
                    aesAlg.Dispose();
                }
            }

            return plainText;
        }

        /// <summary>
        /// Read Byte Array
        /// </summary>
        /// <param name="s"><see cref="Stream"/> Stream</param>
        /// <returns>Returns <see cref="byte[]"/></returns>
        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new FormatException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new FormatException("Did not read byte array properly");
            }

            return buffer;
        }
    }
}

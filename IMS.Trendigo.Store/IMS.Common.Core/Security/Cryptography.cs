using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Security
{
    public class RSACryptography
    {
        //public static string privateKey;
        //public static string publicKey;
        //public static RSACryptoServiceProvider rsa;
        private static void GenerateKeyPair()
        {
            X509Store x509Store = new X509Store("TrendigoCertStore", StoreLocation.CurrentUser);

            x509Store.Add(new X509Certificate2());
        }

        private static string GetPublicKey()
        {
            X509Store x509Store = new X509Store("TrendigoCertStore", StoreLocation.CurrentUser);

            x509Store.Open(OpenFlags.ReadOnly);

            return x509Store.Certificates[0].GetPublicKeyString();
        }

        private static X509Certificate2 GetCertFromStore()
        {
            //to access to store we need to specify store name and location    
            X509Store x509Store = new X509Store("TrendigoCertStore", StoreLocation.CurrentUser);

            //obtain read only access to get cert    
            x509Store.Open(OpenFlags.ReadOnly);

            return x509Store.Certificates[0];
        }

        private static byte[] GetDigitalSignature(byte[] hashBytes)
        {
            X509Certificate2 certificate = GetCertFromStore();
            /*use any asymmetric crypto service provider for encryption of hash   
            with private key of cert.   
            */
            RSACryptoServiceProvider rsaCryptoService = (RSACryptoServiceProvider)certificate.PrivateKey;

            /*now lets sign the hash   
            1.spevify hash bytes   
            2. and hash algorithm name to obtain the bytes   
            */
            return rsaCryptoService.SignHash(hashBytes, CryptoConfig.MapNameToOID("SHA1"));
        }

        private static bool VerifyData(byte[] signature, string messageFromAhemd)
        {
            var messageHash = GetDataHash(messageFromAhemd);

            X509Certificate2 certificate = GetCertFromStore();

            RSACryptoServiceProvider cryptoServiceProvider = (RSACryptoServiceProvider)certificate.PublicKey.Key;

            return cryptoServiceProvider.VerifyHash(messageHash, CryptoConfig.MapNameToOID("SHA1"), signature);
        }

        private static byte[] GetDataHash(string sampleData)
        {
            //choose any hash algorithm    
            SHA1Managed managedHash = new SHA1Managed();

            return managedHash.ComputeHash(Encoding.Unicode.GetBytes(sampleData));
        }

        //public static string EncryptData(string data2Encrypt, string salt)
        //{
        //    AssignParameter();

        //    //using (SqlConnection myConn = new SqlConnection(Utilities.ConnectionString))
        //    //{
        //    //    SqlCommand myCmd = myConn.CreateCommand();

        //    //    myCmd.CommandText = "SELECT TOP 1 PublicKey FROM Settings";

        //    //    myConn.Open();

        //    //    using (SqlDataReader sdr = myCmd.ExecuteReader())
        //    //    {
        //    //        if (sdr.HasRows)
        //    //        {
        //    //            DataTable dt = new DataTable();
        //    //            dt.Load(sdr);
        //    //            rsa.FromXmlString(dt.Rows[0]["PublicKey"].ToString());
        //    //        }
        //    //    }
        //    //}

        //    //read plaintext, encrypt it to ciphertext
        //    byte[] plainbytes = Encoding.UTF8.GetBytes(data2Encrypt + salt);
        //    byte[] cipherbytes = rsa.Encrypt(plainbytes, false);
        //    return Convert.ToBase64String(cipherbytes);
        //}
        //public static string DecryptData(string data2Decrypt, string privatekey, string salt)
        //{
        //    AssignParameter();

        //    byte[] rgb = Convert.FromBase64String(data2Decrypt);

        //    string publicPrivateKeyXML = privatekey;
        //    rsa.FromXmlString(publicPrivateKeyXML);

        //    //read ciphertext, decrypt it to plaintext
        //    byte[] plain = rsa.Decrypt(rgb, false);
        //    string dataAndSalt = System.Text.Encoding.UTF8.GetString(plain);
        //    return dataAndSalt.Substring(0, dataAndSalt.Length - salt.Length);
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace UnityLight.Crypts
{
    public class SignInfo
    {
        public string PrivateKey { get; set; }

        public string PublicKey { get; set; }
    }

    public class RSASign
    {
        /// <summary>
        /// 控制台输出测试信息
        /// </summary>
        /// <param name="dataToSign"></param>
        public static void ConsoleDebug(string dataToSign)
        {
            try
            {
                //string dataToSign = @"Data to Sign!Data to Sign!Data to Sign!";
                Console.WriteLine("原文：" + dataToSign);
                Console.WriteLine("长度：" + dataToSign.Length.ToString());
                Console.WriteLine();

                SignInfo info = CreateSignInfo();
                string str_Private_Key = info.PrivateKey;
                string str_Public_Key = info.PublicKey; ;
                Console.WriteLine("公钥：" + str_Public_Key);
                Console.WriteLine();
                Console.WriteLine("私钥：" + str_Private_Key);
                Console.WriteLine();

                string str_SignedData = RSASign.HashAndSign(dataToSign, str_Private_Key);// Hash and sign the data.
                Console.WriteLine("签名数据：" + str_SignedData);
                Console.WriteLine();

                if (RSASign.VerifySignedHash(dataToSign, str_SignedData, str_Public_Key))
                {
                    Console.WriteLine("验证签名OK.");
                }
                else
                {
                    Console.WriteLine("签名不匹配!");
                }

                Console.WriteLine();
            }
            catch (Exception)
            {
                Console.WriteLine("The data was not signed or verified");
            }
        }

        /// <summary>
        /// 生成 Base64 编码的公钥和私钥。
        /// </summary>
        /// <returns></returns>
        public static SignInfo CreateSignInfo()
        {
            SignInfo info = new SignInfo();

            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            info.PrivateKey = Convert.ToBase64String(RSAalg.ExportCspBlob(true));
            info.PublicKey = Convert.ToBase64String(RSAalg.ExportCspBlob(false));

            return info;
        }

        /// <summary>
        /// 对数据签名，签名失败时返回 null。
        /// </summary>
        /// <param name="dataToSign">要签名的字符串</param>
        /// <param name="privateKey">用Base64编码的私钥字符串</param>
        /// <returns></returns>
        public static string HashAndSign(string dataToSign, string privateKey)
        {
            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            byte[] DataToSign = ByteConverter.GetBytes(dataToSign);
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.ImportCspBlob(Convert.FromBase64String(privateKey));
                byte[] signedData = RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
                string str_SignedData = Convert.ToBase64String(signedData);
                return str_SignedData;
            }
            catch// (CryptographicException ex)
            {
                //Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 验证签名。
        /// </summary>
        /// <param name="dataToVerify">未签名的原始字符串</param>
        /// <param name="signedData">签名后的字符串</param>
        /// <param name="public_Key">用Base64编码的公钥字符串</param>
        /// <returns></returns>
        public static bool VerifySignedHash(string dataToVerify, string signedData, string public_Key)
        {
            byte[] SignedData = Convert.FromBase64String(signedData);
            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            byte[] DataToVerify = ByteConverter.GetBytes(dataToVerify);
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.ImportCspBlob(Convert.FromBase64String(public_Key));
                return RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);
            }
            catch// (CryptographicException ex)
            {
                //Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}

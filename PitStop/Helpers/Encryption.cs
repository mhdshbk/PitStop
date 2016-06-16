using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace PitStop.Helpers        
{
    static public class Encryption
    {
        public static string EncryptPassword(string password)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var
                res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }

    }
}

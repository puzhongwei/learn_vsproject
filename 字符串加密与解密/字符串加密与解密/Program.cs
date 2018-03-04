using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace 字符串加密与解密
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string str = "1233424235345";
            Console.WriteLine(EncodeBase64(str));
            Console.WriteLine(DecodeBase64(EncodeBase64(str)));

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Compilerservices;
using System.Text;
using System.Threading.Tasks;

namespace InterceptorsTest
{
    public static class InterceptClass
    {
        [InterceptsLocation(
            filePath: @"C:\Users\zrogers\OneDrive - Universally Speaking Ltd\Documents\.AutomationTesting\Visual Studio\Training\InterceptorsTest\InterceptorsTest\Program.cs",
            line: 6,
            character:22)]
        public static void InterceptMethodPrintValue2 (this InterceptableExample example)
        {
            Console.WriteLine("Interceptor is here!");
        }
    }
}

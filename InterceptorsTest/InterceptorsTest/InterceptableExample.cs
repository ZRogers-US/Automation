using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterceptorsTest
{
    public class InterceptableExample
    {
        //wont be intercepted
        public void PrintValue1()
        {
            Console.WriteLine("Test 1");
        }
        
        //will be intercepted
        public void PrintValue2()
        {
            Console.WriteLine("Test 2");
        }
        
        //wont be intercepted
        public void PrintValue3()
        {
            Console.WriteLine("Test 3");
        }
    }
}

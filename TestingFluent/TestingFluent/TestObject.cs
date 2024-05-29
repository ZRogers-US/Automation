using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingFluent
{
    internal class TestObject
    {
        string _name = "";
        int _age = 0;
        double _height = 0.0;

        public TestObject(string name, int age, double height) 
        { 
            _name = name;
            _age = age;
            _height = height;
        }
    }
}

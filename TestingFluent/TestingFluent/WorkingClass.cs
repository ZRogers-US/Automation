using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingFluent
{
    public delegate void AgeUpdatedHandler();
    public class SecondWorkingClass
    {
        public string Name { get; set; } = "George";
        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                AgeUpdated();
            }
        }

        public event AgeUpdatedHandler AgeUpdatedEvent;

        public void AgeUpdated()
        {
            AgeUpdatedEvent?.Invoke();
        }

    }

    public class WorkingClass
    {
        public string Name { get; set; } = "George";
        public int Age { get; set; } = 5;

        public string GetFirstName(string fullName)
        {
            string firstName = fullName.Split(' ')[0];
            return firstName;
        }

        public string[] SplitFullName(string fullName)
        {
            string[] name = fullName.Split(' ');
            return name;
        }

        public string SetStringToNull(string input)
        {
            input = null;
            return input;
        }

        public bool GenerateBool()
        {
            return false;
        }


        public void ASlowMethod()
        {
            for (short i = 0; i < short.MaxValue; i++)
            {
                string tmp = " ";
                if (!string.IsNullOrEmpty(tmp))
                {
                    tmp += " ";
                }
            }
        }

        public Task TaskMethod()
        {
            Console.WriteLine("Hello");
            return Task.CompletedTask;
        }
    }
}

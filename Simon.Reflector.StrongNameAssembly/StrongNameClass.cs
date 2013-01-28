using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simon.Reflector.StrongNameAssembly
{
    public delegate string GetNameHandler();

    public class StrongNameClass
    {
        public event EventHandler OnChange;

        public string Name { get; set; }
        public string Value { get; private set; }

        public string PublicField;
        private string PrivateField;

        private string[] List = { "a", "b", "c", "d", "e"};

        public StrongNameClass() 
        {
            Console.WriteLine("call structor without parameters");
        }

        public StrongNameClass(string name, string value)
        {
            Console.WriteLine("call structor with parameters");
            this.Name = name;
            this.Value = value;
        }

        private void SetName(string name)
        {
            Console.WriteLine("call private method of instance");
            this.Name = name;
        }

        public string GetName()
        {
            Console.WriteLine("call public method of instance");
            return this.Name;
        }

        public static void CallPublicStaticMethod()
        {
            Console.WriteLine("call public static method");
        }

        private static void CallPrivateStaticMethod()
        {
            Console.WriteLine("call private static method");
        }

        public string this[int index]
        {
            get
            {
                return List[index];
            }
            set
            {
                List[index] = value;
            }
        }

        public void Change()
        {
            if (this.OnChange != null) ;
                OnChange(this, null);
        }
    }
}

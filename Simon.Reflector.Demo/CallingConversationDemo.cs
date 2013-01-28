using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Simon.Reflector.Demo
{
    public class CallingConversationDemo
    {
        public static void Run()
        {
            Type t = typeof(CallingConversationDemo);
            MethodInfo mi = t.GetMethod("MethodA",
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.Any,
                new Type[] { typeof(int), typeof(int) },
                null);
            Console.WriteLine(mi);

            mi = t.GetMethod("MethodA",
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.HasThis,
                new Type[] { typeof(int[]) },
                null);
            Console.WriteLine(mi);

            mi = t.GetMethod("MethodA",
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.HasThis,
                new Type[] { typeof(int).MakeByRefType() },
                null);
            Console.WriteLine(mi);

            mi = t.GetMethod("MethodA",
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.HasThis,
                new Type[] { typeof(int), typeof(int).MakeByRefType() },
                null);
            Console.WriteLine(mi);
        }

        public void MethodA(int a, int b)
        {

        }

        public void MethodA(int[] a)
        {

        }

        public void MethodA(ref int a)
        {

        }

        public void MethodA(int a, out int b)
        {
            b = 100;
        }
    }
}

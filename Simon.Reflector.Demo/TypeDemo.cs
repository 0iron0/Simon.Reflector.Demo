using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Simon.Reflector.StrongNameAssembly;

namespace Simon.Reflector.Demo
{
    /// <summary>
    /// 反射包含的Info对象：
    /// ConstructorInfo
    /// EventInfo
    /// FieldInfo
    /// MemberInfo
    /// MethodInfo
    /// ParametorInfo
    /// PropertyInfo
    /// </summary>
    public class TypeDemo
    {
        public static void Run()
        {
            //CreateInstance();
            //CallContructor();
            //CallProperty();
            //CallField();
            //CallInstanceMethod();
            //CallStaticMethod();

            //CallDelegate();
            CallEvent();
        }

        static void CreateInstance()
        {
            Type t = typeof(StrongNameClass);
            object o = t.InvokeMember("StrongNameClass",
                BindingFlags.CreateInstance,
                null,
                null,
                new object[] { "simon", "30" });
            Console.WriteLine((o as StrongNameClass).Name);

            t = Type.GetType("Simon.Reflector.StrongNameAssembly.StrongNameClass, Simon.Reflector.StrongNameAssembly", true, true);
            o = Activator.CreateInstance(t, "robin", "30");
            Console.WriteLine((o as StrongNameClass).Name);
        }

        static void CallContructor()
        {
            Type t = typeof(StrongNameClass);

            object o = t.InvokeMember("StrongNameClass", 
                BindingFlags.CreateInstance, 
                null, 
                null, 
                null);
            ConstructorInfo ci = t.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public, 
                null, 
                CallingConventions.HasThis, 
                new Type[] { }, 
                null);
            o = ci.Invoke(null);

            o = t.InvokeMember("StrongNameClass", 
                BindingFlags.CreateInstance,
                null, 
                null, 
                new object[] { "simon", "30" });
            ci = t.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public, 
                null, 
                CallingConventions.HasThis, 
                new Type[] { typeof(string), typeof(string) }, 
                null);
            o = ci.Invoke(new object[] { "simon", "30" });
        }

        static void CallProperty()
        {
            Type t = typeof(StrongNameClass);
            object o = t.InvokeMember("StrongNameClass",
                BindingFlags.CreateInstance,
                null,
                null,
                new object[] { "simon", "30" });

            object rs = t.InvokeMember("Name",
                BindingFlags.GetProperty,
                null,
                o,
                new object[] { });
            Console.WriteLine(rs);

            rs = t.InvokeMember("set_Value",
                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                o,
                new object[] { "18" });

            rs = t.InvokeMember("Value",
                BindingFlags.GetProperty,
                null,
                o,
                new object[] { });
            Console.WriteLine(rs);

            rs = t.InvokeMember("Item",
                 BindingFlags.SetProperty,
                 null,
                 o,
                 new object[] { 1, "z" });//index, value
            rs = t.InvokeMember("Item",
                 BindingFlags.GetProperty,
                 null,
                 o,
                 new object[] { 1 });
            Console.WriteLine(rs);
        }

        static void CallField()
        {
            Type t = typeof(StrongNameClass);
            object o = t.InvokeMember("StrongNameClass",
                BindingFlags.CreateInstance,
                null,
                null,
                new object[] { "simon", "30" });

            t.InvokeMember("PublicField",
                 BindingFlags.SetField,
                 null,
                 o,
                 new object[] { "i am a public field" });

            object rs = t.InvokeMember("PublicField",
                 BindingFlags.GetField,
                 null,
                 o,
                 null);

            Console.WriteLine(rs);

            t.InvokeMember("PrivateField",
                 BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Instance,
                 null,
                 o,
                 new object[] { "i am a private field" });

            rs = t.InvokeMember("PrivateField",
                 BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance,
                 null,
                 o,
                 null);
            Console.WriteLine(rs);
        }

        static void CallInstanceMethod()
        {
            Type t = typeof(StrongNameClass);
            object o = t.InvokeMember("StrongNameClass",
                BindingFlags.CreateInstance,
                null,
                null,
                new object[] { "simon", "30" });

            t.InvokeMember("SetName",
                 BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
                 null,
                 o,
                 new object[] { "Robin" });

            object rs = t.InvokeMember("GetName",
                 BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                 null,
                 o,
                 null);
            Console.WriteLine(rs);
        }

        static void CallStaticMethod()
        {
            Type t = typeof(StrongNameClass);

            t.InvokeMember("CallPrivateStaticMethod",
                 BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic,
                 null,
                 null,
                 new object[] { });

            object rs = t.InvokeMember("CallPublicStaticMethod",
                 BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
                 null,
                 null,
                 null);
        }

        static void CallDelegate()
        { 
            Type t = typeof(StrongNameClass);
            object instance = t.InvokeMember("StrongNameClass",
                 BindingFlags.CreateInstance,
                 null,
                 null,
                 new object[]{"Simon", "30"});
            object o = Delegate.CreateDelegate(typeof(GetNameHandler), instance, "GetName");
            GetNameHandler handler = o as GetNameHandler;
            Console.WriteLine(handler());
        }

        static void CallEvent()
        {
            Type t = typeof(StrongNameClass);
            object o = t.InvokeMember("StrongNameClass", 
                BindingFlags.CreateInstance,
                null, 
                null, 
                new object[] { "simon", "30" });

            EventInfo ei = t.GetEvent("OnChange", BindingFlags.Public | BindingFlags.Instance);
            ei.AddEventHandler(o, 
                new EventHandler((sender, e) => 
                { 
                    Console.WriteLine("call event handler"); 
                }));
            //StrongNameClass instance = o as StrongNameClass;
            //instance.Change();

            FieldInfo fi = t.GetField("OnChange",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Delegate d = fi.GetValue(o) as Delegate;
            if (d != null)
            {
                foreach (var item in d.GetInvocationList())
                {
                    if (item != null)
                    {
                        item.DynamicInvoke(new object[]{o, null});
                    }
                }
            }
        }
    }
}

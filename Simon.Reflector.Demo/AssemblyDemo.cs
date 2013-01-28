using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Simon.Reflector.Demo
{
    /// <summary>
    /// CLR 查找dll的策略
    /// 1. 强命名先找GAC
    /// 2. 非强命名或GAC找不到，通过<codebase>中设置的URL查找
    /// 3. 然后CLR会探测特定的文件夹：
    /// Application Directory\AssemblyName.dll
    /// Application Directory\AssemblyName\AssemblyName.dll
    /// 如果配置了<probing>中的privatePath属性，会查找
    /// Application Directory\PrivatePath\AssemblyName.dll
    ///  Application Directory\PrivatePath\AssemblyName\AssemblyName.dll
    ///  LoadFile()和LoadFrom()的区别：
    ///  LoadFile只加载指定的dll，不加载dependence的dll
    ///  LoadFrom在加载指定的dll的同时，也加载dependence的dll
    /// </summary>
    public class AssemblyDemo
    {
        public static void Run()
        {
            Case6();
        }

        static void Case1()
        {
            Assembly assembly = Assembly.Load("Simon.Reflector.Library");
            if (assembly != null)
            {
                Console.WriteLine("{0} assembly is loaded.", assembly.FullName);
            }
        }

        static void Case2()
        {
            //Assembly assembly = Assembly.LoadFile(@"E:\practice\Simon.Reflector.Demo\Simon.Reflector.StrongNameAssembly\bin\Debug\Simon.Reflector.StrongNameAssembly.dll");
            Assembly assembly = Assembly.Load("Simon.Reflector.StrongNameAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=21eaa485d730f262");
            if (assembly != null)
            {
                Console.WriteLine("{0} assembly is loaded.", assembly.FullName);
            }
        }

        static void Case3()
        {
            byte[] dllBinary = File.ReadAllBytes(@"E:\practice\Simon.Reflector.Demo\Simon.Reflector.StrongNameAssembly\bin\Debug\Simon.Reflector.StrongNameAssembly.dll");
            Assembly assembly = Assembly.Load(dllBinary);
            if (assembly != null)
            {
                Console.WriteLine("{0} assembly is loaded.", assembly.FullName);
            }
        }

        /// <summary>
        /// Assembly.ReflectionOnlyLoad()
        /// 只使用程序集的类型，但不加载dependence的程序集
        /// 不能调用任何属性或者方法
        /// </summary>
        static void Case4()
        {
            Assembly assembly = Assembly.ReflectionOnlyLoad("Simon.Reflector.StrongNameAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=21eaa485d730f262");
            if (assembly != null)
            {
                Console.WriteLine("{0} assembly is loaded.", assembly.FullName);
            }
        }

        static void Case5()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine(assembly.FullName);
            Console.WriteLine(assembly.EntryPoint);
            Console.WriteLine(assembly.CodeBase);
            Console.WriteLine(assembly.EscapedCodeBase);
            Console.WriteLine(assembly.Location);
            Console.WriteLine(assembly.GlobalAssemblyCache);
            Console.WriteLine(assembly.HostContext);
            Console.WriteLine(assembly.IsDynamic);
            Console.WriteLine(assembly.IsFullyTrusted);
            Console.WriteLine(assembly.ReflectionOnly);

            Console.WriteLine();

            AssemblyName name = assembly.GetName();
            Console.WriteLine(name.Name);
            Console.WriteLine(name.FullName);
            Console.WriteLine(name.CodeBase);
            Console.WriteLine(name.EscapedCodeBase);
            Console.WriteLine(name.CultureInfo);
            Console.WriteLine(name.ProcessorArchitecture);
            Console.WriteLine(name.Version);
            Console.WriteLine(name.VersionCompatibility);

            //assembly.GetModules();

            //assembly.GetCustomAttributes(true);

            //assembly.GetFiles();

            //assembly.GetReferencedAssemblies();

            //assembly.GetTypes();
        }

        static void Case6()
        {
            Assembly assembly = Assembly.Load("Simon.Reflector.StrongNameAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=21eaa485d730f262");
            if (assembly != null)
            {
                object o = assembly.CreateInstance("Simon.Reflector.StrongNameAssembly.StrongNameAssembly", true);

                Type t = assembly.GetType("Simon.Reflector.StrongNameAssembly.StrongNameAssembly");
                //foreach (MethodInfo mi in t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
                //{
                //    Console.WriteLine(mi.Name);
                //}

                MethodInfo m = t.GetMethod("GetName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                object rs = m.Invoke(o, null);
                Console.WriteLine(rs);
                rs = t.InvokeMember("getname", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase, null, o, null);
                Console.WriteLine(rs);
            }
        }
    }
}

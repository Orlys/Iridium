
namespace Iridium.Callee
{
}

namespace Iridium.Callee
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Reflection;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public class A
    {
        public A()
        {
            CalleeChecker.Allow(typeof(Program));
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            AsyncCall().Wait();

            try
            {
                new A();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task<int> AsyncCall()
        {
            var test = new CallTest();

            test.Call();
            await Task.Delay(1);
            try
            {
                var t = test.GetType().GetMethod("Internal", BindingFlags.NonPublic | BindingFlags.Instance);
                t.Invoke(test, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
            }

            return 0;
        }
    }

    public class CallTest
    {
        public void Call()
        {
            this.Internal();
        }

        private void Internal()
        {
            try
            {
                CalleeChecker.CheckCaller();
                Console.WriteLine("pass");
            }
            catch (AccessDeniedException)
            {
                Console.WriteLine("fail");
            }
        }
    }
}

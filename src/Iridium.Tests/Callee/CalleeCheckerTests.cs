
namespace Iridium.Tests.Callee
{
    using Iridium.Callee;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Diagnostics;
    using System.Reflection;

    [TestClass]
    public class CalleeCheckerTests
    {
        [TestInitialize]
        public void Init()
        {
            this._t = new TargetObject();
        }

        private TargetObject _t;

        [TestMethod]
        public void MaliciousCallPrivateMethod_CheckCaller_Test()
        {
            try
            {
                typeof(TargetObject)
                    .GetMethod("PrivateForCall", BindingFlags.NonPublic | BindingFlags.Instance)
                    .Invoke(this._t, null);

            }
            catch (TargetInvocationException e) when (e.InnerException is AccessDeniedException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void MaliciousCallInternalMethod_CheckCaller_Test()
        {
            try
            {
                this._t.InternalForCall();
            }
            catch (AccessDeniedException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod] 
        public void CallPublicMethod_CheckCaller_Test()
        {
            try
            {
                this._t.PublicForCall();
            }
            catch (NotSupportedException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void CallDisallowOtherClass_Allow_Test()
        {
            try
            {
                this._t.DisallowOtherClassCall(); 
            }
            catch (AccessDeniedException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void CallAllowTestClass_Allow_Test()
        {
            try
            {
                this._t.AllowTestClassCall();
                return;
            }
            catch
            {
                Assert.Fail();
            }
        }
    } 
    
    [DebuggerNonUserCode]
    public class TargetObject
    {
        public void PublicForCall()
        {
            CalleeChecker.CheckCaller();
        }

        private void PrivateForCall()
        {
            CalleeChecker.CheckCaller();
        }

        internal void InternalForCall()
        {
            CalleeChecker.CheckCaller();
        }

        public void DisallowOtherClassCall()
        {
            CalleeChecker.Allow(null);
        }

        public void AllowTestClassCall()
        {
            CalleeChecker.Allow(typeof(CalleeCheckerTests));
        }
    }
}
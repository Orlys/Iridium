

namespace Iridium.Callee
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode]
    public static class CalleeChecker
    {
        const int CALLEE_OFFSET = 1;
        const int CALLER_OFFSET = 2;
        /// <summary>
        /// Allows types member to calling callee.
        /// </summary>
        /// <param name="types"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Allow(params Type[] types)
        {

            var st = new StackTrace();
            var method = st.GetFrame(CALLEE_OFFSET).GetMethod();

            var ownerType = method.DeclaringType;
            var callerType = st.GetFrame(CALLER_OFFSET).GetMethod().DeclaringType;

            if (ownerType == callerType)
                return;

            if (types?.Length > 0 && types.Contains(callerType))
                return;

            throw new AccessDeniedException();
        }

        /// <summary>
        /// Checks the caller equals callee's declaring type, otherwise throw <see cref="AccessDeniedException"/>, this method only supported <see langword="public"/> methods. 
        /// </summary>
        /// <exception cref="NotSupportedException" />
        /// <exception cref="AccessDeniedException" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckCaller()
        {
            var st = new StackTrace();
            var method = st.GetFrame(CALLEE_OFFSET).GetMethod();

            if (method.IsPublic)
            {
                throw new NotSupportedException("method should not be public.");
            }

            var owner = method.DeclaringType;
            var callerType = st.GetFrame(CALLER_OFFSET).GetMethod().DeclaringType;
            if (owner != callerType)
            {
                throw new AccessDeniedException();
            }
        }
    }
}

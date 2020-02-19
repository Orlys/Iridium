
namespace Iridium.Callee
{
    using System;

    [Serializable]
    public sealed class AccessDeniedException : Exception
    {
        public AccessDeniedException() : base(string.Empty, default(Exception))
        {
        }
    }
}

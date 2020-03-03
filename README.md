# Iridium
Lightweight library for runtime method's caller checking.
## Usage
```csharp
using Iridium.Callee;

internal class NeedCheck
{
    internal NeedCheck()
    {
        CalleeChecker.CheckCaller(); // Ensures the caller's type is NeedCheck, otherwise throw AccessDeniedException.
        CalleeChecker.Allow(typeof(OtherClass)); // Allows OtherClass members calling this method, otherwise throw AccessDeniedException.
    }
}

public static class OtherClass
{
    public static NeedCheck CallTest() => new NeedCheck(); // OK
}

public static class NotAllowClass
{
    public static NeedCheck CallTest() => new NeedCheck(); // AccessDeniedException thrown.
}
```

## LISENCE
MIT

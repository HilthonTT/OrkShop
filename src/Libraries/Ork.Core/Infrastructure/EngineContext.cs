using System.Runtime.CompilerServices;

namespace Ork.Core.Infrastructure;

public sealed class EngineContext
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static IEngine Create()
    {
        return Singleton<IEngine>.Instance ??= new OrkEngine();
    }

    public static void Replace(IEngine engine)
    {
        Singleton<IEngine>.Instance = engine;
    }

    public static IEngine Current => Singleton<IEngine>.Instance ?? Create();
}

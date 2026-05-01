using System.Runtime.CompilerServices;

namespace Bllueprint.Core.Domain;

public file interface ITransition<in T>
{
    void Invoke(T target, [CallerMemberName] string callerName = "");
}

public interface ITransition<in T, in TArgs>
{
    void Invoke(T target, TArgs args, [CallerMemberName] string callerName = "");
}

using System.Runtime.CompilerServices;

namespace Bllueprint.Core.Domain;

public interface ITransition<in T, in TArgs>
{
    void Invoke(T target, TArgs args, [CallerMemberName] string callerName = "");
}

namespace Bllueprint.Core.Domain;

internal file sealed record struct GuardStep<T>(Func<T, bool> Guard, string Message) : ITransitionStep<T>;

internal sealed record struct GuardStep<T, TArgs>(Func<T, TArgs, bool> Guard, string Message) : ITransitionStep<T, TArgs>;

internal sealed record struct GuardAllStep<T>(IReadOnlyList<(Func<T, bool> Guard, string Message)> Guards) : ITransitionStep<T>;

internal sealed record struct GuardAllStep<T, TArgs>(IReadOnlyList<(Func<T, TArgs, bool> Guard, string Message)> Guards) : ITransitionStep<T, TArgs>;

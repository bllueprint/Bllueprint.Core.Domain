namespace Bllueprint.Core.Domain;

internal record struct GuardStep<T>(Func<T, bool> Guard, string Message) : ITransitionStep<T>;

internal record struct GuardStep<T, TArgs>(Func<T, TArgs, bool> Guard, string Message) : ITransitionStep<T, TArgs>;

internal record struct GuardAllStep<T>(IReadOnlyList<(Func<T, bool> Guard, string Message)> Guards) : ITransitionStep<T>;

internal record struct GuardAllStep<T, TArgs>(IReadOnlyList<(Func<T, TArgs, bool> Guard, string Message)> Guards) : ITransitionStep<T, TArgs>;

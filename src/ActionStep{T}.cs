namespace Bllueprint.Core.Domain;

internal sealed record struct ActionStep<T>(Action<T> Action) : ITransitionStep<T>;

internal file sealed record struct ActionStep<T, TArgs>(Action<T, TArgs> Action) : ITransitionStep<T, TArgs>;

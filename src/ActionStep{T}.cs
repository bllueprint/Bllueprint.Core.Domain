namespace Bllueprint.Core.Domain;

internal record struct ActionStep<T>(Action<T> Action) : ITransitionStep<T>;

internal record struct ActionStep<T, TArgs>(Action<T, TArgs> Action) : ITransitionStep<T, TArgs>;

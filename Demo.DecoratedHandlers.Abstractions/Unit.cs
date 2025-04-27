namespace Demo.DecoratedHandlers.Abstractions;

/// <summary>
///     A type that represents a void-like response in a generic context.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    public static Unit Value => new();
    public static Task<Unit> CompletedTask { get; } = Task.FromResult(Value);
    
    public override bool Equals(object obj) => obj is Unit;
    public bool Equals(Unit other) => true;
    public override int GetHashCode() => 0;
    public static bool operator ==(Unit left, Unit right) => true;
    public static bool operator !=(Unit left, Unit right) => false;
    public override string ToString() => "()";
}
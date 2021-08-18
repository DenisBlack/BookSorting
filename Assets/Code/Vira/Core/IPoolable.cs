using UnityEngine;
/// <summary>
/// For objects are able to add to the Poll
/// </summary>
public interface IPoolable
{
    /// <summary>
    /// object's type
    /// </summary>
    UnitType Type { get; }

    Transform Transform { get; }

    /// <summary>
    /// Makes the object inaccessible to interaction.
    /// </summary>
    void Off();

    /// <summary>
    /// Returns object to base state.
    /// </summary>
    void Reset();
}

/// <summary>
/// object's types for objects one class
/// </summary>
public enum UnitType
{
    Cell = 0
}
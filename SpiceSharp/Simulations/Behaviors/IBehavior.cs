﻿namespace SpiceSharp.Behaviors
{
    /// <summary>
    /// Contract for a behavior.
    /// </summary>
    public interface IBehavior : IExportPropertySet, IParameterized
    {
        /// <summary>
        /// Gets the name of the behavior.
        /// </summary>
        string Name { get; }
    }
}

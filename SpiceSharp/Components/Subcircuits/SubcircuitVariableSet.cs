﻿using SpiceSharp.Simulations;
using System.Collections;
using System.Collections.Generic;

namespace SpiceSharp.Components.Subcircuits
{
    /// <summary>
    /// A <see cref="VariableSet"/> that will map unknowns in such a way that the subcircuit does not interfere with the rest of the circuit.
    /// </summary>
    /// <seealso cref="SpiceSharp.Simulations.VariableSet" />
    public class SubcircuitVariableSet : IVariableSet
    {
        private IVariableSet _variables;

        /// <summary>
        /// Gets the name of the subcircuit.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the pins.
        /// </summary>
        /// <value>
        /// The pins.
        /// </value>
        public HashSet<string> Pins { get; }

        /// <summary>
        /// Gets the comparer used for variables.
        /// </summary>
        /// <value>
        /// The comparer.
        /// </value>
        public IEqualityComparer<string> Comparer => _variables.Comparer;

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Count => _variables.Count;

        /// <summary>
        /// Gets the <see cref="Variable"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Variable"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public Variable this[int index] => _variables[index];

        /// <summary>
        /// Gets the <see cref="Variable"/> with the specified identifier.
        /// </summary>
        /// <value>
        /// The <see cref="Variable"/>.
        /// </value>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Variable this[string id] => _variables[Map(id)];

        /// <summary>
        /// Initializes a new instance of the <see cref="SubcircuitVariableSet"/> class.
        /// </summary>
        /// <param name="pins">The pins.</param>
        /// <param name="variableSet">The "real" variable set.</param>
        public SubcircuitVariableSet(string[] pins, IVariableSet variableSet)
        {
            _variables = variableSet.ThrowIfNull(nameof(variableSet));
            Pins = new HashSet<string>(variableSet.Comparer);
            for (var i = 0; i < pins.Length; i++)
                Pins.Add(pins[i].ThrowIfNull("pin"));
        }

        /// <summary>
        /// This method maps a variable in the circuit. If a variable with the same identifier already exists, then that variable is returned.
        /// </summary>
        /// <param name="id">The identifier of the variable.</param>
        /// <param name="type">The type of the variable.</param>
        /// <returns>
        /// A new variable with the specified identifier and type, or a previously mapped variable if it already existed.
        /// </returns>
        /// <remarks>
        /// If the variable already exists, the variable type is ignored.
        /// </remarks>
        public Variable MapNode(string id, VariableType type) => _variables.MapNode(Map(id), type);

        /// <summary>
        /// Create a new variable.
        /// </summary>
        /// <param name="id">The identifier of the new variable.</param>
        /// <param name="type">The type of the variable.</param>
        /// <returns>
        /// A new variable.
        /// </returns>
        /// <remarks>
        /// Variables created using this method cannot be found back using the method <see cref="MapNode(string,VariableType)" />.
        /// </remarks>
        public Variable Create(string id, VariableType type) => _variables.Create(id, type);

        /// <summary>
        /// Determines whether the set contains a mapped variable by a specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if the specified set contains the variable; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsNode(string id) => _variables.ContainsNode(Map(id));

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if the set contains the variable; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string id) => _variables.Contains(id);

        /// <summary>
        /// Tries to get a variable.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="node">The found variable.</param>
        /// <returns>
        ///   <c>true</c> if the variable was found; otherwise <c>false</c>.
        /// </returns>
        public bool TryGetNode(string id, out Variable node) => _variables.TryGetNode(Map(id), out node);

        /// <summary>
        /// Clears the set from any variables.
        /// </summary>
        public void Clear() => _variables.Clear();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Variable> GetEnumerator() => _variables.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Maps the specified variable identifier.
        /// </summary>
        /// <param name="identifier">The variable.</param>
        /// <returns></returns>
        protected virtual string Map(string identifier)
        {
            if (!Pins.Contains(identifier))
                identifier = Name.Combine(identifier);
            return identifier;
        }
    }
}
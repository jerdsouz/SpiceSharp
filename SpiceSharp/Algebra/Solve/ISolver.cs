﻿using System;

namespace SpiceSharp.Algebra
{
    /// <summary>
    /// Describes a linear system of equations. It tracks permutations of
    /// the equations and the variables.
    /// </summary>
    /// <remarks>
    /// The solver readily implements <see cref="IElementMatrix{T}"/> and <see cref="IElementVector{T}"/>.
    /// This makes it easier for other objects to contribute to the solver Y-matrix or Rhs-vector without
    /// the need to store a reference to the solver.
    /// </remarks>
    /// <typeparam name="T">The base type.</typeparam>
    /// <seealso cref="IElementMatrix{T}" />
    /// <seealso cref="IElementVector{T}" />
    public interface ISolver<T> : IElementMatrix<T>, IElementVector<T> where T : IFormattable
    {
        /// <summary>
        /// Occurs before the solver uses the decomposition to find the solution.
        /// </summary>
        event EventHandler<SolveEventArgs<T>> BeforeSolve;

        /// <summary>
        /// Occurs after the solver used the decomposition to find a solution.
        /// </summary>
        event EventHandler<SolveEventArgs<T>> AfterSolve;

        /// <summary>
        /// Occurs before the solver uses the transposed decomposition to find the solution.
        /// </summary>
        event EventHandler<SolveEventArgs<T>> BeforeSolveTransposed;

        /// <summary>
        /// Occurs after the solver uses the transposed decomposition to find a solution.
        /// </summary>
        event EventHandler<SolveEventArgs<T>> AfterSolveTransposed;

        /// <summary>
        /// Occurs before the solver is factored.
        /// </summary>
        event EventHandler<EventArgs> BeforeFactor;

        /// <summary>
        /// Occurs after the solver has been factored.
        /// </summary>
        event EventHandler<EventArgs> AfterFactor;

        /// <summary>
        /// Occurs before the solver is ordered and factored.
        /// </summary>
        event EventHandler<EventArgs> BeforeOrderAndFactor;

        /// <summary>
        /// Occurs after the solver has been ordered and factored.
        /// </summary>
        event EventHandler<EventArgs> AfterOrderAndFactor;

        /// <summary>
        /// Gets or sets the order of the system that needs to be solved.
        /// </summary>
        /// <remarks>
        /// This property can be used to limit the number of elimination steps to do
        /// partial elimination. The pivots are also only searched within the top-left
        /// Order x Order submatrix. However, the whole system will be solved.
        /// Specifying a negative number of 0 makes the order relative to the size of
        /// the system of equations. For example, -2 means that the last two equations
        /// are expected to be linearly dependent on the first N-2 equations.
        /// </remarks>
        /// <value>
        /// The order.
        /// </value>
        int Order { get; set; }

        /// <summary>
        /// Preconditions the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        void Precondition(PreconditionMethod<T> method);

        /// <summary>
        /// Solves the equations using the Y-matrix and Rhs-vector.
        /// </summary>
        /// <param name="solution">The solution.</param>
        void Solve(IVector<T> solution);

        /// <summary>
        /// Solves the equations using the transposed Y-matrix.
        /// </summary>
        /// <param name="solution">The solution.</param>
        void SolveTransposed(IVector<T> solution);

        /// <summary>
        /// Factor the Y-matrix and Rhs-vector.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the factoring was successful; otherwise <c>false</c>.
        /// </returns>
        bool Factor();

        /// <summary>
        /// Order and factor the Y-matrix and Rhs-vector.
        /// </summary>
        void OrderAndFactor();

        /// <summary>
        /// Clears all matrix and vector elements.
        /// </summary>
        void Reset();

        /// <summary>
        /// Maps an internal row/column tuple to an external one.
        /// </summary>
        /// <param name="indices">The internal row/column indices.</param>
        /// <returns>The external row/column indices.</returns>
        Tuple<int, int> InternalToExternal(Tuple<int, int> indices);

        /// <summary>
        /// Maps an external row/column tuple to an internal one.
        /// </summary>
        /// <param name="indices">The external row/column indices.</param>
        /// <returns>The internal row/column indices.</returns>
        Tuple<int, int> ExternalToInternal(Tuple<int, int> indices);
    }

    /// <summary>
    /// A method that can be used to precondition a solver.
    /// </summary>
    /// <typeparam name="T">The base type.</typeparam>
    /// <param name="matrix">The (permutated) matrix.</param>
    /// <param name="vector">The (permutated) vector.</param>
    public delegate void PreconditionMethod<T>(IMatrix<T> matrix, IVector<T> vector) where T : IFormattable;
}
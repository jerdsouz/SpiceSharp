﻿using SpiceSharp.Behaviors;
using System;
using System.Threading.Tasks;

namespace SpiceSharp.Circuits.ParallelBehaviors
{
    /// <summary>
    /// A behavior that stores a list of sub-behaviors.
    /// </summary>
    /// <typeparam name="T">The behavior type.</typeparam>
    /// <seealso cref="Behavior" />
    public abstract class ParallelBehavior<T> : Behavior where T : IBehavior
    {
        /// <summary>
        /// Gets the behaviors.
        /// </summary>
        /// <value>
        /// The behaviors.
        /// </value>
        protected BehaviorList<T> Behaviors { get; private set; }

        /// <summary>
        /// Gets the flag that indicates whether the behaviors should be run in parallel.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        protected bool ExecuteInParallel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParallelBehavior{T}"/> class.
        /// </summary>
        /// <param name="name">The identifier of the behavior.</param>
        /// <remarks>
        /// The identifier of the behavior should be the same as that of the entity creating it.
        /// </remarks>
        public ParallelBehavior(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Bind the behavior to a simulation.
        /// </summary>
        /// <param name="context">The binding context.</param>
        public override void Bind(BindingContext context)
        {
            base.Bind(context);
            var pc = (ParallelBindingContext)context;
            Behaviors = pc.Concurrent.GetBehaviorList<T>();

            if (context.Behaviors.Parameters.TryGetValue<BaseParameters>(out var bp))
                ExecuteInParallel = bp.ParallelBehaviors.Contains(typeof(T));
        }

        /// <summary>
        /// Destroy the behavior.
        /// </summary>
        public override void Unbind()
        {
            base.Unbind();

            if (Behaviors != null)
            {
                for (var i = 0; i < Behaviors.Count; i++)
                    Behaviors[i].Unbind();
                Behaviors = null;
            }
        }

        /// <summary>
        /// Run a behavior method in parallel.
        /// </summary>
        /// <param name="method">The method.</param>
        protected void For(Func<T, Action> method)
        {
            if (ExecuteInParallel)
            {
                var tasks = new Task[Behaviors.Count];
                for (var i = 0; i < Behaviors.Count; i++)
                    tasks[i] = Task.Run(method(Behaviors[i]));
                Task.WaitAll(tasks);
            }
            else
            {
                for (var i = 0; i < Behaviors.Count; i++)
                    method(Behaviors[i]).Invoke();
            }
        }

        /// <summary>
        /// Run a behavior method in parallel and do boolean AND on all results.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>
        /// The boolean AND result.
        /// </returns>
        protected bool ForAnd(Func<T, Func<bool>> method)
        {
            if (ExecuteInParallel)
            {
                var tasks = new Task<bool>[Behaviors.Count];
                for (var i = 0; i < Behaviors.Count; i++)
                    tasks[i] = Task.Run(method(Behaviors[i]));
                Task.WaitAll(tasks);

                // Combine all outputs
                var result = true;
                for (var i = 0; i < tasks.Length; i++)
                    result &= tasks[i].Result;
                return result;
            }
            else
            {
                var result = true;
                for (var i = 0; i < Behaviors.Count; i++)
                    result &= method(Behaviors[i]).Invoke();
                return result;
            }
        }
    }
}

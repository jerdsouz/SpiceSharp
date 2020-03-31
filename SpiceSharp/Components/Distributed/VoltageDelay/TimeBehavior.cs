﻿using SpiceSharp.Behaviors;
using SpiceSharp.Components.Distributed;
using SpiceSharp.Algebra;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components.DelayBehaviors
{
    /// <summary>
    /// Time behavior for a <see cref="VoltageDelay"/>.
    /// </summary>
    public class TimeBehavior : BiasingBehavior, ITimeBehavior
    {
        private readonly int _contPosNode, _contNegNode, _branchEq;
        private readonly ElementSet<double> _elements;
        private readonly ITimeSimulationState _time;
        private readonly IBiasingSimulationState _biasing;

        /// <summary>
        /// Gets the delayed signal.
        /// </summary>
        /// <value>
        /// The signal.
        /// </value>
        protected DelayedSignal Signal { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBehavior"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="context">The context.</param>
        public TimeBehavior(string name, ComponentBindingContext context)
            : base(name, context)
        {
            _time = context.GetState<ITimeSimulationState>();
            _biasing = context.GetState<IBiasingSimulationState>();
            _contPosNode = _biasing.Map[_biasing.GetSharedVariable(context.Nodes[2])];
            _contNegNode = _biasing.Map[_biasing.GetSharedVariable(context.Nodes[3])];
            _branchEq = _biasing.Map[Branch];
            _elements = new ElementSet<double>(_biasing.Solver, null, new[] { _branchEq });
            Signal = new DelayedSignal(1, Parameters.Delay);
        }

        /// <summary>
        /// Calculates the state values from the current DC solution.
        /// </summary>
        void ITimeBehavior.InitializeStates()
        {
            var sol = _biasing.Solution;
            var input = sol[_contPosNode] - sol[_contNegNode];
            Signal.SetProbedValues(input);
        }

        /// <summary>
        /// Loads the Y-matrix and Rhs-vector.
        /// </summary>
        void IBiasingBehavior.Load()
        {
            var sol = _biasing.Solution;
            var input = sol[_contPosNode] - sol[_contNegNode];
            Signal.SetProbedValues(input);

            if (_time.UseDc)
                BiasingElements.Add(1, -1, 1, -1, -1, 1);
            else
            {
                BiasingElements.Add(1, -1, 1, -1);
                _elements.Add(Signal.Values[0]);
            }
        }
    }
}

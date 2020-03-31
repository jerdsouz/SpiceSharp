﻿using SpiceSharp.Attributes;
using SpiceSharp.Behaviors;
using SpiceSharp.Simulations;
using SpiceSharp.Algebra;
using SpiceSharp.Components.CommonBehaviors;

namespace SpiceSharp.Components.VoltageSourceBehaviors
{
    /// <summary>
    /// General behavior for <see cref="VoltageSource"/>
    /// </summary>
    public class BiasingBehavior : Behavior, IBiasingBehavior, IBranchedBehavior<double>,
        IParameterized<IndependentSourceParameters>
    {
        private readonly IIntegrationMethod _method;
        private readonly IIterationSimulationState _iteration;
        private readonly OnePort<double> _variables;
        private readonly ElementSet<double> _elements;
        private readonly IBiasingSimulationState _biasing;

        /// <summary>
        /// Gets the base parameters.
        /// </summary>
        /// <value>
        /// The base parameters.
        /// </value>
        public IndependentSourceParameters Parameters { get; }

        /// <summary>
        /// Gets the waveform.
        /// </summary>
        /// <value>
        /// The waveform.
        /// </value>
        protected IWaveform Waveform { get; }

        /// <summary>
        /// Gets the current through the source.
        /// </summary>
        /// <returns></returns>
        [ParameterName("i"), ParameterName("i_r"), ParameterInfo("Voltage source current")]
        public double Current => Branch.Value;

        /// <summary>
        /// Gets the power dissipated by the source.
        /// </summary>
        /// <returns></returns>
        [ParameterName("p"), ParameterName("p_r"), ParameterInfo("Instantaneous power")]
        public double Power => Voltage * -Branch.Value;

        /// <summary>
        /// Gets the voltage applied by the source.
        /// </summary>
        [ParameterName("v"), ParameterName("v_r"), ParameterInfo("Instantaneous voltage")]
        public double Voltage { get; private set; }

        /// <summary>
        /// Gets the branch equation.
        /// </summary>
        public IVariable<double> Branch { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiasingBehavior"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="context">The context.</param>
        public BiasingBehavior(string name, IComponentBindingContext context) : base(name)
        {
            context.ThrowIfNull(nameof(context));

            Parameters = context.GetParameterSet<IndependentSourceParameters>();
            _iteration = context.GetState<IIterationSimulationState>();
            context.TryGetState(out _method);
            Waveform = Parameters.Waveform?.Create(_method);
            if (!Parameters.DcValue.Given)
            {
                // No DC value: either have a transient value or none
                if (Waveform != null)
                {
                    SpiceSharpWarning.Warning(this, 
                        Properties.Resources.IndependentSources_NoDcUseWaveform.FormatString(Name));
                    Parameters.DcValue = new GivenParameter<double>(Waveform.Value, false);
                }
                else
                {
                    SpiceSharpWarning.Warning(this, 
                        Properties.Resources.IndependentSources_NoDc.FormatString(Name));
                }
            }

            // Connections
            _biasing = context.GetState<IBiasingSimulationState>();
            context.TryGetState(out _method);

            _variables = new OnePort<double>(_biasing, context);
            Branch = _biasing.CreatePrivateVariable(Name.Combine("branch"), Units.Ampere);
            var pos = _biasing.Map[_variables.Positive];
            var neg = _biasing.Map[_variables.Negative];
            var br = _biasing.Map[Branch];

            _elements = new ElementSet<double>(_biasing.Solver, new[] {
                new MatrixLocation(pos, br),
                new MatrixLocation(br, pos),
                new MatrixLocation(neg, br),
                new MatrixLocation(br, neg)
            }, new[] { br });
        }

        /// <summary>
        /// Execute behavior
        /// </summary>
        void IBiasingBehavior.Load()
        {
            double value;

            if (_method != null)
            {
                // Use the waveform if possible
                if (Waveform != null)
                    value = Waveform.Value;
                else
                    value = Parameters.DcValue * _iteration.SourceFactor;
            }
            else
            {
                value = Parameters.DcValue * _iteration.SourceFactor;
            }

            Voltage = value;
            _elements.Add(1, 1, -1, -1, value);
        }
    }
}

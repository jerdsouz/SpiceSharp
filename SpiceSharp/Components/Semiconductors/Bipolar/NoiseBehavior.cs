﻿using System;
using SpiceSharp.Behaviors;
using SpiceSharp.Components.NoiseSources;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components.BipolarBehaviors
{
    /// <summary>
    /// Noise behavior for <see cref="BipolarJunctionTransistor"/>
    /// </summary>
    public class NoiseBehavior : BiasingBehavior, INoiseBehavior
    {
        private readonly INoiseSimulationState _noise;

        /// <summary>
        /// Gets the noise parameters.
        /// </summary>
        protected ModelNoiseParameters NoiseParameters { get; private set; }

        /// <summary>
        /// Noise sources by their index
        /// </summary>
        private const int _rcNoise = 0;
        private const int _rbNoise = 1;
        private const int _reNoise = 2;
        private const int _icNoise = 3;
        private const int _ibNoise = 4;
        private const int _flickerNoise = 5;

        /// <summary>
        /// Noise generators
        /// </summary>
        public ComponentNoise BipolarJunctionTransistorNoise { get; } = new ComponentNoise(
            new NoiseThermal("rc", 0, 4),
            new NoiseThermal("rb", 1, 5),
            new NoiseThermal("re", 2, 6),
            new NoiseShot("ic", 4, 6),
            new NoiseShot("ib", 5, 6),
            new NoiseGain("1overf", 5, 6)
            );

        /// <summary>
        /// Initializes a new instance of the <see cref="NoiseBehavior"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="context">The context.</param>
        public NoiseBehavior(string name, ComponentBindingContext context) : base(name, context) 
        {
            NoiseParameters = context.ModelBehaviors.GetParameterSet<ModelNoiseParameters>();
            _noise = context.GetState<INoiseSimulationState>();
            BipolarJunctionTransistorNoise.Bind(context, context.Nodes[0], context.Nodes[1], context.Nodes[2], context.Nodes[3],
                CollectorPrime, BasePrime, EmitterPrime);
        }
        
        /// <summary>
        /// Noise calculations
        /// </summary>
        void INoiseBehavior.Noise()
        {
            var generators = BipolarJunctionTransistorNoise.Generators;

            // Set noise parameters
            generators[_rcNoise].SetCoefficients(ModelTemperature.CollectorConduct * BaseParameters.Area);
            generators[_rbNoise].SetCoefficients(ConductanceX);
            generators[_reNoise].SetCoefficients(ModelTemperature.EmitterConduct * BaseParameters.Area);
            generators[_icNoise].SetCoefficients(CollectorCurrent);
            generators[_ibNoise].SetCoefficients(BaseCurrent);
            generators[_flickerNoise].SetCoefficients(NoiseParameters.FlickerNoiseCoefficient * Math.Exp(NoiseParameters.FlickerNoiseExponent 
                * Math.Log(Math.Max(Math.Abs(BaseCurrent), 1e-38))) / _noise.Frequency);

            // Evaluate all noise sources
            BipolarJunctionTransistorNoise.Evaluate();
        }
    }
}

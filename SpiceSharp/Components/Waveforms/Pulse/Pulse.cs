﻿using SpiceSharp.Attributes;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components
{
    /// <summary>
    /// This class implements a pulse waveform.
    /// </summary>
    /// <seealso cref="ParameterSet" />
    /// <seealso cref="IWaveformDescription" />
    public partial class Pulse : ParameterSet, IWaveformDescription
    {
        /// <summary>
        /// Gets the initial value.
        /// </summary>
        [ParameterName("v1"), ParameterInfo("The initial value")]
        public double InitialValue { get; set; }

        /// <summary>
        /// Gets the pulsed value.
        /// </summary>
        [ParameterName("v2"), ParameterInfo("The peak value")]
        public double PulsedValue { get; set; }

        /// <summary>
        /// Gets the delay of the waveform in seconds.
        /// </summary>
        [ParameterName("td"), ParameterInfo("The initial delay time in seconds")]
        public double Delay { get; set; }

        /// <summary>
        /// Gets the rise time in seconds.
        /// </summary>
        [ParameterName("tr"), ParameterInfo("The rise time in seconds")]
        public double RiseTime { get; set; }

        /// <summary>
        /// Gets the fall time in seconds.
        /// </summary>
        [ParameterName("tf"), ParameterInfo("The fall time in seconds")]
        public double FallTime { get; set; }

        /// <summary>
        /// Gets the width of the pulse in seconds.
        /// </summary>
        [ParameterName("pw"), ParameterInfo("The pulse width in seconds")]
        public double PulseWidth { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// Gets the period in seconds.
        /// </summary>
        [ParameterName("per"), ParameterInfo("The period in seconds")]
        public double Period { get; set; } = double.PositiveInfinity;

        /// <summary>
        /// Sets all the pulse parameters.
        /// </summary>
        /// <param name="parameters">The pulse parameters</param>
        [ParameterName("pulse"), ParameterInfo("A vector of all pulse waveform parameters")]
        public void SetPulse(double[] parameters)
        {
            parameters.ThrowIfNull(nameof(parameters));
            switch (parameters.Length)
            {
                case 7:
                    Period = parameters[6];
                    goto case 6;
                case 6:
                    PulseWidth = parameters[5];
                    goto case 5;
                case 5:
                    FallTime = parameters[4];
                    goto case 4;
                case 4:
                    RiseTime = parameters[3];
                    goto case 3;
                case 3:
                    Delay = parameters[2];
                    goto case 2;
                case 2:
                    PulsedValue = parameters[1];
                    goto case 1;
                case 1:
                    InitialValue = parameters[0];
                    break;
                default:
                    throw new BadParameterException(nameof(parameters));
            }
        }

        /// <summary>
        /// Creates a waveform instance for the specified simulation and entity.
        /// </summary>
        /// <param name="method">The time simulation state.</param>
        /// <returns>
        /// A waveform instance.
        /// </returns>
        public IWaveform Create(IIntegrationMethod method)
        {
            return new Instance(method,
                InitialValue, PulsedValue, Delay, RiseTime, FallTime, PulseWidth, Period);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pulse"/> class.
        /// </summary>
        public Pulse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pulse"/> class.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        /// <param name="pulsedValue">The peak value.</param>
        /// <param name="delay">The initial delay time in seconds.</param>
        /// <param name="riseTime">The rise time in seconds.</param>
        /// <param name="fallTime">The fall time in seconds.</param>
        /// <param name="pulseWidth">The pulse width in seconds.</param>
        /// <param name="period">The period in seconds.</param>
        public Pulse(double initialValue, double pulsedValue, double delay, double riseTime, double fallTime, double pulseWidth, double period)
        {
            InitialValue = initialValue;
            PulsedValue = pulsedValue;
            Delay = delay;
            RiseTime = riseTime;
            FallTime = fallTime;
            PulseWidth = pulseWidth;
            Period = period;
        }
    }
}

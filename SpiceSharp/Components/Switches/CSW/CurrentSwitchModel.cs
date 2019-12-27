﻿using SpiceSharp.Behaviors;
using SpiceSharp.Components.SwitchBehaviors;
using SpiceSharp.Simulations;

namespace SpiceSharp.Components
{
    /// <summary>
    /// A model for a <see cref="CurrentSwitch"/>
    /// </summary>
    public class CurrentSwitchModel : Model,
        IParameterized<CurrentModelParameters>
    {
        /// <summary>
        /// Gets the parameter set.
        /// </summary>
        /// <value>
        /// The parameter set.
        /// </value>
        public CurrentModelParameters Parameters { get; } = new CurrentModelParameters();

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSwitchModel"/> class.
        /// </summary>
        /// <param name="name">The name of the model</param>
        public CurrentSwitchModel(string name) 
            : base(name)
        {
        }

        /// <summary>
        /// Creates the behaviors for the specified simulation and registers them with the simulation.
        /// </summary>
        /// <param name="simulation">The simulation.</param>
        public override void CreateBehaviors(ISimulation simulation)
        {
            CalculateDefaults();
            var container = new BehaviorContainer(Name)
            {
                new ModelBehavior(Name, new ModelBindingContext(this, simulation))
            };
            simulation.EntityBehaviors.Add(container);
        }
    }
}

﻿using SpiceSharp.Behaviors;
using SpiceSharp.Circuits;
using SpiceSharp.Components.MosfetBehaviors.Level2;

namespace SpiceSharp.Components
{
    /// <summary>
    /// A model for a <see cref="Mosfet2"/>
    /// </summary>
    public class Mosfet2Model : Model
    {
        static Mosfet2Model()
        {
            RegisterBehaviorFactory(typeof(Mosfet2Model), new BehaviorFactoryDictionary
            {
                {typeof(ITemperatureBehavior), e => new ModelTemperatureBehavior(e.Name)}
            });
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Mosfet2Model"/> class.
        /// </summary>
        /// <param name="name">The name of the device</param>
        public Mosfet2Model(string name) : base(name)
        {
            Parameters.Add(new ModelBaseParameters());
            Parameters.Add(new MosfetBehaviors.Common.ModelNoiseParameters());
        }
    }
}

﻿using SpiceSharp.Attributes;
using SpiceSharp.Components.MosfetBehaviors.Level1;

namespace SpiceSharp.Components
{
    /// <summary>
    /// A MOS1 Mosfet.
    /// Level 1, Shichman-Hodges.
    /// </summary>
    [PinsAttribute("Drain", "Gate", "Source", "Bulk"), ConnectedAttribute(0, 2, 3)]
    public class MOS1 : Component
    {
        /// <summary>
        /// Set the model for the MOS1 Mosfet
        /// </summary>
        public void SetModel(MOS1Model model) => Model = model;

        /// <summary>
        /// Nodes
        /// </summary>
        [PropertyName("dnode"), PropertyInfo("Number of the drain node ")]
        public int DrainNode { get; protected set; }
        [PropertyName("gnode"), PropertyInfo("Number of the gate node ")]
        public int GateNode { get; protected set; }
        [PropertyName("snode"), PropertyInfo("Number of the source node ")]
        public int SourceNode { get; protected set; }
        [PropertyName("bnode"), PropertyInfo("Number of the node ")]
        public int BulkNode { get; protected set; }

        /// <summary>
        /// Constants
        /// </summary>
        public const int MOS1PinCount = 4;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the device</param>
        public MOS1(Identifier name) : base(name, MOS1PinCount)
        {
            // Add parameters
            Parameters.Add(new BaseParameters());

            // Add factories
            AddFactory(typeof(TemperatureBehavior), () => new TemperatureBehavior(Name));
            AddFactory(typeof(LoadBehavior), () => new LoadBehavior(Name));
            AddFactory(typeof(TransientBehavior), () => new TransientBehavior(Name));
            AddFactory(typeof(FrequencyBehavior), () => new FrequencyBehavior(Name));
            AddFactory(typeof(NoiseBehavior), () => new NoiseBehavior(Name));
        }

        /// <summary>
        /// Setup the device
        /// </summary>
        /// <param name="circuit">The circuit</param>
        public override void Setup(Circuit circuit)
        {
            // Allocate nodes
            var nodes = BindNodes(circuit);
            DrainNode = nodes[0].Index;
            GateNode = nodes[1].Index;
            SourceNode = nodes[2].Index;
            BulkNode = nodes[3].Index;
        }
    }
}

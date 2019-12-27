﻿using NUnit.Framework;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Diagnostics.Validation;
using SpiceSharp.Simulations;
using System;
using System.Numerics;

namespace SpiceSharpTest.Models
{
    [TestFixture]
    public class CurrentSwitchTests : Framework
    {
        private CurrentSwitch CreateCurrentSwitch(string name, string pos, string neg, string contSource, string model)
        {
            var vsw = new CurrentSwitch(name, pos, neg, contSource) {Model = model};
            return vsw;
        }

        private CurrentSwitchModel CreateCurrentSwitchModel(string name, string parameters)
        {
            var model = new CurrentSwitchModel(name);
            ApplyParameters(model, parameters);
            return model;
        }

        [Test]
        public void When_SimpleSwitchDC_Expect_Spice3f5Reference()
        {
            // NOTE: The hysteresis is chosen such that it does not switch on the same point as a sweep. If that happens, then the smallest
            // numerical error can lead to a big output change, causing a very different output than the reference.

            // Build the circuit
            var ckt = new Circuit(
                CreateCurrentSwitch("S1", "out", "0", "V1", "myswitch"),
                CreateCurrentSwitchModel("myswitch", "IT=0.5 RON=1 ROFF=1e3 IH=0.2001"),
                new CurrentSource("I1", "0", "in", 0.0),
                new VoltageSource("V1", "in", "0", 0),
                new VoltageSource("V2", "vdd", "0", 5.0),
                new Resistor("R1", "vdd", "out", 1e3)
                );

            // Create the simulation, exports and references
            var dc = new DC("DC", "I1", -3, 3, 10e-3);
            IExport<double>[] exports = { new RealVoltageExport(dc, "out") };
            double[][] references =
            {
                new[]
                {
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00,
                    2.500000000000000e+00, 2.500000000000000e+00, 2.500000000000000e+00, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03, 4.995004995004996e-03,
                    4.995004995004996e-03
                }
            };
            AnalyzeDC(dc, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_SimpleSwitchSmallSignal_Expect_Reference()
        {
            var ckt = new Circuit(
                new CurrentSource("I1", "0", "in", -1),
                new VoltageSource("V1", "in", "0", 0),
                CreateCurrentSwitch("S1", "out", "0", "V1", "myswitch"),
                new VoltageSource("Vdd", "vdd", "0", 5).SetParameter("acmag", 1.0),
                new Resistor("R1", "out", "vdd", 1e3),
                CreateCurrentSwitchModel("myswitch", "IT=0.5 RON=1 ROFF=1e3 IH=0.2001")
                );

            var ac = new AC("ac", new DecadeSweep(1, 1e6, 2));
            var exports = new IExport<Complex>[] { new ComplexVoltageExport(ac, "out") };
            var reference = new Func<double, Complex>[] { f => 0.5 };
            AnalyzeAC(ac, ckt, exports, reference);
            DestroyExports(exports);
        }

        [Test]
        public void When_SimpleSwitchTransient_Expect_Spice3f5Reference()
        {
            // Build the switch
            var ckt = new Circuit(
                CreateCurrentSwitch("S1", "0", "OUT", "V1", "MYSW"),
                CreateCurrentSwitchModel("MYSW", "Ron=1 Roff=1e6 It=0.5 Ih=-0.4"),
                new CurrentSource("I1", "0", "IN", new Pulse(0, 1, 0.0, 0.4e-3, 0.4e-3, 0.1e-3, 1e-3)),
                new VoltageSource("V1", "IN", "0", 0),
                new VoltageSource("V2", "N001", "0", 3.3),
                new Resistor("R1", "N001", "OUT", 1e3)
                );

            // Build simulation, exports and references
            var tran = new Transient("Tran 1", 0.1e-3, 3e-3);
            IExport<double>[] exports = { new GenericExport<double>(tran, () => tran.GetState<IIntegrationMethod>().Time), new RealVoltageExport(tran, "OUT") };
            double[][] references =
            {
                new[]
                {
                    0.000000000000000e+00, 6.000000000000000e-07, 1.200000000000000e-06, 2.400000000000000e-06,
                    4.800000000000000e-06, 9.600000000000000e-06, 1.920000000000000e-05, 3.840000000000000e-05,
                    7.680000000000000e-05, 1.368000000000000e-04, 1.968000000000000e-04, 2.568000000000000e-04,
                    3.168000000000000e-04, 3.768000000000000e-04, 4.000000000000000e-04, 4.060000000000000e-04,
                    4.180000000000000e-04, 4.420000000000000e-04, 4.900000000000000e-04, 5.000000000000000e-04,
                    5.060000000000000e-04, 5.180000000000000e-04, 5.420000000000001e-04, 5.900000000000000e-04,
                    6.500000000000001e-04, 7.100000000000001e-04, 7.700000000000002e-04, 8.300000000000002e-04,
                    8.900000000000003e-04, 9.000000000000000e-04, 9.060000000000000e-04, 9.180000000000000e-04,
                    9.420000000000000e-04, 9.900000000000000e-04, 1.000000000000000e-03, 1.006000000000000e-03,
                    1.018000000000000e-03, 1.042000000000000e-03, 1.090000000000000e-03, 1.150000000000000e-03,
                    1.210000000000000e-03, 1.270000000000000e-03, 1.330000000000000e-03, 1.390000000000000e-03,
                    1.400000000000000e-03, 1.406000000000000e-03, 1.418000000000000e-03, 1.442000000000000e-03,
                    1.490000000000000e-03, 1.500000000000000e-03, 1.506000000000000e-03, 1.518000000000000e-03,
                    1.542000000000000e-03, 1.590000000000000e-03, 1.650000000000000e-03, 1.710000000000000e-03,
                    1.770000000000000e-03, 1.830000000000000e-03, 1.890000000000000e-03, 1.900000000000000e-03,
                    1.906000000000000e-03, 1.918000000000000e-03, 1.942000000000000e-03, 1.990000000000000e-03,
                    2.000000000000000e-03, 2.006000000000000e-03, 2.018000000000000e-03, 2.042000000000000e-03,
                    2.090000000000000e-03, 2.150000000000000e-03, 2.210000000000000e-03, 2.270000000000000e-03,
                    2.330000000000000e-03, 2.390000000000001e-03, 2.400000000000000e-03, 2.406000000000000e-03,
                    2.418000000000000e-03, 2.442000000000000e-03, 2.490000000000000e-03, 2.500000000000000e-03,
                    2.506000000000000e-03, 2.518000000000000e-03, 2.542000000000000e-03, 2.590000000000000e-03,
                    2.650000000000000e-03, 2.710000000000000e-03, 2.770000000000000e-03, 2.830000000000000e-03,
                    2.890000000000001e-03, 2.900000000000000e-03, 2.906000000000000e-03, 2.918000000000000e-03,
                    2.942000000000000e-03, 2.990000000000000e-03, 3.000000000000000e-03
                },
                new[]
                {
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703298e-03,
                    3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03, 3.296703296703298e-03,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00,
                    3.296703296703297e+00, 3.296703296703297e+00, 3.296703296703297e+00
                }
            };
            AnalyzeTransient(tran, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_BooleanParameter_Expect_DirectAccess()
        {
            // Create voltage source
            var s = new CurrentSwitch("SW 1");
            var p = s.Parameters;

            // Check on
            s.SetParameter("on");
            Assert.AreEqual(true, p.ZeroState);

            // Check off
            s.SetParameter("off");
            Assert.AreEqual(false, p.ZeroState);
        }

        [Test]
        public void When_ShortedValidation_Expect_ShortCircuitComponentException()
        {
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1),
                new CurrentSwitch("S1", "in", "in", "V1"));
            Assert.Throws<ShortCircuitComponentException>(() => ckt.Validate());
        }

        [Test]
        public void When_FloatingOutputValidation_Expect_NoException()
        {
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1),
                new CurrentSwitch("S1", "out", "0", "V1"));
            ckt.Validate();
        }

        [Test]
        public void When_ConnectedOutputValidation_Expect_NoException()
        {
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1),
                new CurrentSwitch("S1", "out", "in", "V1"));
            ckt.Validate();
        }
    }
}

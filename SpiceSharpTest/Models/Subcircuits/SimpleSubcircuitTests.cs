﻿using NUnit.Framework;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Diagnostics.Validation;
using SpiceSharp.Simulations;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SpiceSharpTest.Models
{
    [TestFixture]
    public class SimpleSubcircuitTests : Framework
    {
        [Test]
        public void When_SimpleSubcircuit_Expect_Reference()
        {
            // Define the subcircuit
            var subckt = new SubcircuitDefinition(new Circuit(
                new Resistor("R1", "a", "b", 1e3),
                new Resistor("R2", "b", "0", 1e3)),
                "a", "b");

            // Define the circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 5.0),
                new Subcircuit("X1", subckt).Connect("in", "out"));

            // Simulate the circuit
            var op = new OP("op");
            IExport<double>[] exports = new[] { new RealVoltageExport(op, "out") };
            IEnumerable<double> references = new double[] { 2.5 };
            AnalyzeOp(op, ckt, exports, references);
        }

        [Test]
        public void When_RecursiveSubcircuit_Expect_Reference()
        {
            // Define the subcircuit
            var subckt = new SubcircuitDefinition(new Circuit(
                new Resistor("R1", "a", "b", 1e3),
                new Resistor("R2", "b", "c", 1e3)),
                "a", "c");

            // Define the parent subcircuit
            var subckt2 = new SubcircuitDefinition(new Circuit(
                new Subcircuit("X1", subckt).Connect("x", "y"),
                new Subcircuit("X2", subckt).Connect("y", "z")),
                "x", "y", "z");

            // Define the circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 2.0),
                new Subcircuit("X1", subckt2).Connect("in", "out", "0"));

            // Simulate the circuit
            var op = new OP("op");
            IExport<double>[] exports = new[] { new RealVoltageExport(op, "out") };
            IEnumerable<double> references = new double[] { 1.0 };
            AnalyzeOp(op, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_LocalSolverSubcircuitOp_Expect_Reference()
        {
            // No internal nodes
            var subckt = new SubcircuitDefinition(new Circuit(
                new Resistor("R1", "a", "b", 1e3),
                new Resistor("R2", "b", "0", 1e3)),
                "a", "b");
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1.0),
                new Subcircuit("X1", subckt, "in", "out"));
            ckt["X1"].Parameters.Add(new SpiceSharp.Components.SubcircuitBehaviors.Simple.BiasingParameters() { LocalSolver = true });

            var op = new OP("op");
            IExport<double>[] exports = new[] { new RealVoltageExport(op, "out") };
            IEnumerable<double> references = new double[] { 0.5 };
            AnalyzeOp(op, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_LocalSolverSubcircuitAc_Expect_Reference()
        {
            // No internal nodes
            var subckt = new SubcircuitDefinition(new Circuit(
                new Resistor("R1", "a", "b", 1e3),
                new Resistor("R2", "b", "0", 1e3)),
                "a", "b");
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1.0).SetParameter("acmag", 1.0),
                new Subcircuit("X1", subckt, "in", "out"));
            ckt["X1"].Parameters.Add(new SpiceSharp.Components.SubcircuitBehaviors.Simple.FrequencyParameters() { LocalSolver = true });

            var ac = new AC("ac", new DecadeSweep(1, 100, 3));
            IExport<Complex>[] exports = new[] { new ComplexVoltageExport(ac, "out") };
            IEnumerable<Func<double, Complex>> references = new Func<double, Complex>[] { f => 0.5 };
            AnalyzeAC(ac, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_LocalSolverSubcircuitOp2_Expect_Reference()
        {
            // One internal node
            var subckt = new SubcircuitDefinition(new Circuit(
                new Resistor("R1", "a", "b", 1e3),
                new Resistor("R2", "b", "c", 1e3),
                new Resistor("R3", "b", "0", 1e3)),
                "a", "c");
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1.0),
                new Subcircuit("X1", subckt, "in", "out"));
            ckt["X1"].Parameters.Add(new SpiceSharp.Components.SubcircuitBehaviors.Simple.BiasingParameters() { LocalSolver = true });

            var op = new OP("op");
            IExport<double>[] exports = new[] { new RealVoltageExport(op, "out") };
            IEnumerable<double> references = new double[] { 0.5 };
            AnalyzeOp(op, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_LocalSolverSubcircuitAC2_Expect_Reference()
        {
            // One internal node
            var subckt = new SubcircuitDefinition(new Circuit(
                new Resistor("R1", "a", "b", 1e3),
                new Resistor("R2", "b", "c", 1e3),
                new Resistor("R3", "b", "0", 1e3)),
                "a", "c");
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1.0).SetParameter("acmag", 1.0),
                new Subcircuit("X1", subckt, "in", "out"));
            ckt["X1"].Parameters.Add(new SpiceSharp.Components.SubcircuitBehaviors.Simple.FrequencyParameters() { LocalSolver = true });

            var ac = new AC("ac", new DecadeSweep(1, 100, 3));
            IExport<Complex>[] exports = new[] { new ComplexVoltageExport(ac, "out") };
            IEnumerable<Func<double, Complex>> references = new Func<double, Complex>[] { f => 0.5 };
            AnalyzeAC(ac, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_LocalSolverSubcircuitTransient_Expect_Reference()
        {
            // With internal states
            var subckt = new SubcircuitDefinition(new Circuit(
                new Resistor("R1", "a", "b", 1e3),
                new Capacitor("C1", "b", "0", 1e-6)),
                "a", "b");
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 1.0),
                new Subcircuit("X1", subckt, "in", "out"));
            ckt["X1"].Parameters.Add(new SpiceSharp.Components.SubcircuitBehaviors.Simple.BiasingParameters() { LocalSolver = true });

            var tran = new Transient("transient", 1e-6, 1e-3);
            tran.Configurations.GetValue<TimeConfiguration>().InitialConditions.Add("out", 0.0);
            IExport<double>[] exports = new[] { new RealVoltageExport(tran, "out") };
            IEnumerable<Func<double, double>> references = new Func<double, double>[] { t => 1.0 - Math.Exp(-t * 1e3) };
            AnalyzeTransient(tran, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_LocalSolverSubcircuitOp3_Expect_Reference()
        {
            // Variable that makes an equivalent circuit impossible
            var subckt = new SubcircuitDefinition(new Circuit(
                new VoltageSource("V1", "a", "0", 1.0)), "a");
            var ckt = new Circuit(
                new Resistor("R1", "in", "out", 1e3),
                new Resistor("R2", "out", "0", 1e3),
                new Subcircuit("X1", subckt, "in"));
            ckt["X1"].Parameters.Add(new SpiceSharp.Components.SubcircuitBehaviors.Simple.BiasingParameters { LocalSolver = true });

            var op = new OP("op");
            IExport<double>[] exports = new[] { new RealVoltageExport(op, "out") };
            Assert.Throws<NoEquivalentSubcircuitException>(() => op.Run(ckt));
        }

        [Test]
        public void When_InternalFloatingNodeValidation_Expect_FloatingNodeException()
        {
            var subckt = new SubcircuitDefinition(new Circuit(
                new Capacitor("C1", "in", "out", 1e-6)), "in");
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 0),
                new Subcircuit("X1", subckt).Connect("in"));
            Assert.Throws<FloatingNodeException>(() => ckt.Validate());
        }

        [Test]
        public void When_ExternalFloatingNodeValidation_Expect_FloatingNodeException()
        {
            var subckt = new SubcircuitDefinition(new Circuit(
                new Resistor("R1", "in", "0", 1e3)), "in", "out");
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 0),
                new Subcircuit("X1", subckt, "in", "out"));
            Assert.Throws<FloatingNodeException>(() => ckt.Validate());
        }

        [Test]
        public void When_IndependentSourceValidation_Expect_NoException()
        {
            var subckt = new SubcircuitDefinition(new Circuit(
                new VoltageSource("V1", "out", "0", 0)), "out");
            var ckt = new Circuit(
                new Subcircuit("X1", subckt, "out"),
                new Resistor("R1", "out", "0", 1e3));
            ckt.Validate();
        }

        [Test]
        public void When_VoltageLoopValidation_Expect_VoltageLoopException()
        {
            var subckt = new SubcircuitDefinition(new Circuit(
                new VoltageSource("V1", "out", "0", 0)), "out");
            var ckt = new Circuit(
                new Subcircuit("X1", subckt, "out"),
                new VoltageSource("V1", "out", "0", 1));
            Assert.Throws<VoltageLoopException>(() => ckt.Validate());
        }
    }
}

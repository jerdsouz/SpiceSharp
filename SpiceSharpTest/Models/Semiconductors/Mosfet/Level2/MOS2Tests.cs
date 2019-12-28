﻿using System.Numerics;
using NUnit.Framework;
using SpiceSharp;
using SpiceSharp.Components;
using SpiceSharp.Simulations;

namespace SpiceSharpTest.Models
{
    /// <summary>
    /// Taken from https://ecee.colorado.edu/~bart/book/book/chapter7/ch7_5.htm
    /// .MODEL NFET NMOS (LEVEL=2 L=1u W=1u VTO=-1.44 KP=8.64E-6 NSUB = 1E17 TOX = 20n)
    /// </summary>
    [TestFixture]
    public class MOS2Tests : Framework
    {
        private Mosfet2 CreateMOS2(string name, string d, string g, string s, string b, string model)
        {
            // Create transistor
            var mos = new Mosfet2(name) {Model = model};
            mos.Connect(d, g, s, b);
            return mos;
        }

        private Mosfet2Model CreateMOS2Model(string name, string parameters)
        {
            var model = new Mosfet2Model(name);
            ApplyParameters(model, parameters);
            return model;
        }

        [Test]
        public void When_SimpleDC_Expect_Spice3f5Reference()
        {
            /*
             * MOS2 shunted by voltage sources
             * Current is expected to match the reference. Reference is simulated by Spice 3f5.
             */
            // Create circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 0.0),
                new VoltageSource("V2", "out", "0", 0.0),
                CreateMOS2("M1", "out", "in", "0", "0", "NFET")
                    .SetParameter("l", 6e-6)
                    .SetParameter("w", 1e-6),
                CreateMOS2Model("NFET", "VTO = -1.44 KP = 8.64E-6 NSUB = 1e17 TOX = 20e-9")
            );

            // Create simulation
            var dc = new DC("dc", new[] {
                new SourceSweep("V2", new LinearSweep(0, 3.3, 0.3)),
                new SourceSweep("V1", new LinearSweep(0, 3.3, 0.3))
            });

            // Create exports
            IExport<double>[] exports = { new RealPropertyExport(dc, "V2", "i") };

            // Create references
            var references = new double[1][];
            references[0] = new[]
            {
                0.000000000000000e+00, 0.000000000000000e+00, 0.000000000000000e+00, 0.000000000000000e+00,
                0.000000000000000e+00, 0.000000000000000e+00, 0.000000000000000e+00, 0.000000000000000e+00,
                0.000000000000000e+00, 0.000000000000000e+00, 0.000000000000000e+00, 0.000000000000000e+00,
                -5.306938374762972e-07, -6.622829041640560e-07, -7.937581773977502e-07, -9.251256793208626e-07,
                -1.056391893430447e-06, -1.187563541271432e-06, -1.318647392836713e-06, -1.449650112001313e-06,
                -1.580578136718209e-06, -1.711437591907900e-06, -1.842234231506578e-06, -1.972973405226006e-06,
                -8.638458189766005e-07, -1.127266444634827e-06, -1.390443306657313e-06, -1.653387842188881e-06,
                -1.916112823385289e-06, -2.178631872717426e-06, -2.440959028691271e-06, -2.703108371482166e-06,
                -2.965093715016955e-06, -3.226928367443185e-06, -3.488624957291339e-06, -3.750195318941171e-06,
                -1.006642980508283e-06, -1.402162238528042e-06, -1.797292612754292e-06, -2.192049695953252e-06,
                -2.586451725095741e-06, -2.980518839976069e-06, -3.374272380032708e-06, -3.767734241737990e-06,
                -4.160926317516499e-06, -4.553870030753595e-06, -4.946585972966656e-06, -5.339093640940220e-06,
                -1.013799629686942e-06, -1.492058405858433e-06, -2.019403706665254e-06, -2.546235206750328e-06,
                -3.072559603046376e-06, -3.598402479001807e-06, -4.123791871424482e-06, -4.648757350976099e-06,
                -5.173329167535178e-06, -5.697537495654653e-06, -6.221411805023045e-06, -6.744980367812900e-06,
                -1.014566859581403e-06, -1.493160556051296e-06, -2.067521923824644e-06, -2.719762043312447e-06,
                -3.378280908481190e-06, -4.036155046468613e-06, -4.693417839623045e-06, -5.350105680759867e-06,
                -6.006256834174742e-06, -6.661910361168374e-06, -7.317105161899917e-06, -7.971879170944456e-06,
                -1.015356741659893e-06, -1.494299909660686e-06, -2.069063097873367e-06, -2.740840251690813e-06,
                -3.506586652651430e-06, -4.296774575599588e-06, -5.086176728084939e-06, -5.874834670198922e-06,
                -6.662793553470293e-06, -7.450100756193913e-06, -8.236804578683890e-06, -9.022953068962966e-06,
                -1.016165437476091e-06, -1.495471083771302e-06, -2.070653897845618e-06, -2.742900593322288e-06,
                -3.513262901776043e-06, -4.382683065140698e-06, -5.304470813563661e-06, -6.225375503988383e-06,
                -7.145400341215117e-06, -8.064599592900621e-06, -8.983030137568078e-06, -9.900749917551870e-06,
                -1.016989023485014e-06, -1.496668363213843e-06, -2.072286704485665e-06, -2.745024194610477e-06,
                -3.515924552362666e-06, -4.385920816155342e-06, -5.355855832487864e-06, -6.403691255248455e-06,
                -7.456069595713784e-06, -8.507429881427832e-06, -9.557835816840147e-06, -1.060735413910678e-05,
                -1.017823668891155e-06, -1.497885983840097e-06, -2.073953544717280e-06, -2.747200785959000e-06,
                -3.518664091102590e-06, -4.389267691949250e-06, -5.359844436218669e-06, -6.431152584792028e-06,
                -7.596428673614574e-06, -8.780248653652516e-06, -9.962909998812256e-06, -1.114448614086207e-05,
                -1.018665778845741e-06, -1.499118382335761e-06, -2.075646475814351e-06, -2.749419763651192e-06,
                -3.521468187712419e-06, -4.392707866929063e-06, -5.363962030875167e-06, -6.435978121313036e-06,
                -7.609441818854552e-06, -8.884419035140827e-06, -1.019964577331805e-05, -1.151357106530835e-05,
                -1.019512094513571e-06, -1.500360390171152e-06, -2.077357913157599e-06, -2.751670684538682e-06,
                -3.524323247900116e-06, -4.396224548237776e-06, -5.368188911914150e-06, -6.440953329762988e-06,
                -7.615191814315274e-06, -8.891527316636293e-06, -1.027054150786247e-05, -1.171578914356212e-05
            };

            // Run test
            AnalyzeDC(dc, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_CommonSourceAmplifierSmallSignal_Expect_Spice3f5Reference()
        {
            /*
             * MOS2 amplifier biased as a diode-connected transistor, AC coupled
             * Current is expected to match the reference. Reference is simulated by Spice 3f5.
             */
            // Create circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 0.0)
                    .SetParameter("acmag", 1.0),
                new VoltageSource("V2", "vdd", "0", 5.0),
                new Resistor("R1", "vdd", "out", 10e3),
                new Resistor("R2", "out", "g", 10e3),
                new Capacitor("C1", "in", "g", 1e-6),
                CreateMOS2("M1", "out", "g", "0", "0", "NFET")
                    .SetParameter("l", 6e-6)
                    .SetParameter("w", 1e-6),
                CreateMOS2Model("NFET", "VTO = -1.44 KP = 8.64E-6 NSUB = 1e17 TOX = 20e-9")
                );

            // Create simulation
            var ac = new AC("ac", new DecadeSweep(10, 10e9, 5));

            // Create exports
            IExport<Complex>[] exports = { new ComplexVoltageExport(ac, "out") };

            // Create references
            double[] riref =
            {
                2.701114568959397e-01, 2.297592026993491e-01, 3.614367501477384e-01, 1.939823511070750e-01,
                4.176532699259306e-01, 1.414313855763639e-01, 4.452214249772693e-01, 9.512747415960789e-02,
                4.572366767698547e-01, 6.164118367404970e-02, 4.622024752123699e-01, 3.931535278467341e-02,
                4.642095432402752e-01, 2.491402951340239e-02, 4.650134308270042e-01, 1.574691222820917e-02,
                4.653342396231024e-01, 9.942484429536930e-03, 4.654620791267926e-01, 6.275007008504113e-03,
                4.655129925001772e-01, 3.959694832353351e-03, 4.655332645790356e-01, 2.498507336198679e-03,
                4.655413355303657e-01, 1.576478884956591e-03, 4.655445487118463e-01, 9.946977962705516e-04,
                4.655458279147781e-01, 6.276136046196179e-04, 4.655463371765942e-01, 3.959978465130569e-04,
                4.655465399176849e-01, 2.498578584664746e-04, 4.655466206304160e-01, 1.576496782075489e-04,
                4.655466527627409e-01, 9.947022918549027e-05, 4.655466655548509e-01, 6.276147338624847e-05,
                4.655466706474820e-01, 3.959981301663537e-05, 4.655466726748949e-01, 2.498579297169927e-05,
                4.655466734820225e-01, 1.576496961048729e-05, 4.655466738033459e-01, 9.947023368109509e-06,
                4.655466739312669e-01, 6.276147451549338e-06, 4.655466739821933e-01, 3.959981330028886e-06,
                4.655466740024674e-01, 2.498579304294980e-06, 4.655466740105386e-01, 1.576496962838461e-06,
                4.655466740137518e-01, 9.947023372605108e-07, 4.655466740150311e-01, 6.276147452678582e-07,
                4.655466740155403e-01, 3.959981330312538e-07, 4.655466740157432e-01, 2.498579304366230e-07,
                4.655466740158237e-01, 1.576496962856358e-07, 4.655466740158559e-01, 9.947023372650060e-08,
                4.655466740158686e-01, 6.276147452689869e-08, 4.655466740158738e-01, 3.959981330315371e-08,
                4.655466740158758e-01, 2.498579304366941e-08, 4.655466740158766e-01, 1.576496962856536e-08,
                4.655466740158769e-01, 9.947023372650503e-09, 4.655466740158770e-01, 6.276147452689979e-09,
                4.655466740158772e-01, 3.959981330315399e-09, 4.655466740158772e-01, 2.498579304366946e-09,
                4.655466740158771e-01, 1.576496962856537e-09, 4.655466740158772e-01, 9.947023372650507e-10,
                4.655466740158772e-01, 6.276147452689978e-10, 4.655466740158771e-01, 3.959981330315396e-10
            };
            var references = new Complex[1][];
            references[0] = new Complex[riref.Length / 2];
            for (var i = 0; i < riref.Length; i += 2)
                references[0][i / 2] = new Complex(riref[i], riref[i + 1]);

            // Run test
            AnalyzeAC(ac, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_SwitchTransient_Expect_Spice3f5Reference()
        {
            /*
             * Simple MOS switch
             * The output voltage is expected to match the reference. Reference as simulated by Spice 3f5.
             */
            // Build circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", new Pulse(1, 5, 1e-6, 1e-9, 0.5e-6, 2e-6, 6e-6)),
                new VoltageSource("Vsupply", "vdd", "0", 3.3),
                new Resistor("R1", "out", "vdd", 100e3),
                CreateMOS2("M1", "out", "in", "0", "0", "NFET")
                    .SetParameter("w", 1e-6)
                    .SetParameter("l", 6e-6),
                CreateMOS2Model("NFET", "VTO = -1.44 KP = 8.64E-6 NSUB = 1e17 TOX = 20e-9")
                );

            // Create simulation
            var tran = new Transient("tran", 1e-9, 10e-6);

            // Create exports
            IExport<double>[] exports = { new GenericExport<double>(tran, () => tran.GetState<IIntegrationMethod>().Time), new RealVoltageExport(tran, "out") };

            // Create references
            var references = new double[2][];
            references[0] = new[]
            {
                0.000000000000000e+00, 1.000000000000000e-11, 2.000000000000000e-11, 4.000000000000000e-11,
                8.000000000000001e-11, 1.600000000000000e-10, 3.200000000000000e-10, 6.400000000000001e-10,
                1.280000000000000e-09, 2.560000000000000e-09, 5.120000000000001e-09, 1.024000000000000e-08,
                2.048000000000000e-08, 4.096000000000000e-08, 8.192000000000001e-08, 1.638400000000000e-07,
                3.276800000000000e-07, 5.276800000000000e-07, 7.276800000000000e-07, 9.276800000000000e-07,
                1.000000000000000e-06, 1.000100000000000e-06, 1.000125000000000e-06, 1.000175000000000e-06,
                1.000275000000000e-06, 1.000278125000000e-06, 1.000284375000000e-06, 1.000296875000000e-06,
                1.000300000000000e-06, 1.000306250000000e-06, 1.000318750000000e-06, 1.000343750000000e-06,
                1.000393750000000e-06, 1.000493750000000e-06, 1.000693750000000e-06, 1.001000000000000e-06,
                1.001040000000000e-06, 1.001120000000000e-06, 1.001280000000000e-06, 1.001600000000000e-06,
                1.002040869607452e-06, 1.002630387377420e-06, 1.003503894605074e-06, 1.005250909060382e-06,
                1.008744937970999e-06, 1.015732995792233e-06, 1.029709111434701e-06, 1.057661342719636e-06,
                1.113565805289506e-06, 1.225374730429247e-06, 1.425374730429247e-06, 1.625374730429247e-06,
                1.825374730429247e-06, 2.025374730429247e-06, 2.225374730429247e-06, 2.425374730429247e-06,
                2.625374730429247e-06, 2.825374730429246e-06, 3.001000000000000e-06, 3.021000000000000e-06,
                3.061000000000000e-06, 3.141000000000000e-06, 3.301000000000000e-06, 3.501000000000000e-06,
                3.520999999999999e-06, 3.560999999999999e-06, 3.641000000000000e-06, 3.801000000000000e-06,
                4.000999999999999e-06, 4.200999999999999e-06, 4.400999999999999e-06, 4.600999999999999e-06,
                4.800999999999999e-06, 5.000999999999998e-06, 5.200999999999998e-06, 5.400999999999998e-06,
                5.600999999999998e-06, 5.800999999999997e-06, 6.000999999999997e-06, 6.200999999999997e-06,
                6.400999999999997e-06, 6.600999999999997e-06, 6.800999999999996e-06, 7.000000000000000e-06,
                7.000100000000000e-06, 7.000125000000000e-06, 7.000175000000000e-06, 7.000275000000000e-06,
                7.000278125000001e-06, 7.000284375000000e-06, 7.000296875000000e-06, 7.000300000000001e-06,
                7.000306250000000e-06, 7.000318750000000e-06, 7.000343750000000e-06, 7.000393750000000e-06,
                7.000493750000001e-06, 7.000693750000000e-06, 7.001000000000000e-06, 7.001040000000000e-06,
                7.001119999999999e-06, 7.001280000000000e-06, 7.001599999999999e-06, 7.002040869607450e-06,
                7.002630387377417e-06, 7.003503894605071e-06, 7.005250909060378e-06, 7.008744937970990e-06,
                7.015732995792216e-06, 7.029709111434669e-06, 7.057661342719574e-06, 7.113565805289383e-06,
                7.225374730429002e-06, 7.425374730429002e-06, 7.625374730429001e-06, 7.825374730429001e-06,
                8.025374730429001e-06, 8.225374730429001e-06, 8.425374730429000e-06, 8.625374730429000e-06,
                8.825374730429000e-06, 9.000999999999999e-06, 9.020999999999999e-06, 9.060999999999998e-06,
                9.140999999999998e-06, 9.300999999999998e-06, 9.500999999999998e-06, 9.520999999999997e-06,
                9.560999999999997e-06, 9.640999999999996e-06, 9.800999999999996e-06, 9.999999999999999e-06
            };
            references[1] = new[]
            {
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00,
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00,
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00,
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00,
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00,
                3.000419418896398e+00, 2.890995883050182e+00, 2.860889788871183e+00, 2.797358759613117e+00,
                2.656952623694448e+00, 2.652277000742125e+00, 2.642873246891359e+00, 2.623855584042447e+00,
                2.625531810664886e+00, 2.634981266431170e+00, 2.650057650004957e+00, 2.677324477875249e+00,
                2.727679218653770e+00, 2.807709256115712e+00, 2.895453785010935e+00, 2.890386972407389e+00,
                2.747482253586531e+00, 2.511507415947686e+00, 2.222521734973983e+00, 1.980842807129200e+00,
                1.907033664434709e+00, 1.897814592838963e+00, 1.898216736456195e+00, 1.898102476804415e+00,
                1.898170304423054e+00, 1.898117262851848e+00, 1.898164305207494e+00, 1.898119974323293e+00,
                1.898163015226234e+00, 1.898120603598022e+00, 1.898162692291108e+00, 1.898120833431374e+00,
                1.898162463711884e+00, 1.898121060761244e+00, 1.898162237622255e+00, 1.898121285615139e+00,
                1.898162013995106e+00, 1.898121508020025e+00, 1.898161777543783e+00, 1.924482579560164e+00,
                1.985322321439621e+00, 2.121613755164430e+00, 2.474985118851241e+00, 3.000536619998078e+00,
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00,
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00,
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00,
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00,
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00,
                2.890995883049932e+00, 2.860889788870923e+00, 2.797358759612839e+00, 2.656952623693816e+00,
                2.652277000741173e+00, 2.642873246891039e+00, 2.623855584042123e+00, 2.625531810665268e+00,
                2.634981266430673e+00, 2.650057650004801e+00, 2.677324477875317e+00, 2.727679218654353e+00,
                2.807709256116441e+00, 2.895453785010313e+00, 2.890386972407629e+00, 2.747482253587951e+00,
                2.511507415948750e+00, 2.222521734974616e+00, 1.980842807129419e+00, 1.907033664434766e+00,
                1.897814592838963e+00, 1.898216736456194e+00, 1.898102476804415e+00, 1.898170304423053e+00,
                1.898117262851849e+00, 1.898164305207493e+00, 1.898119974323294e+00, 1.898163015226234e+00,
                1.898120603598022e+00, 1.898162692291108e+00, 1.898120833431375e+00, 1.898162463711884e+00,
                1.898121060761244e+00, 1.898162237622255e+00, 1.898121285615139e+00, 1.898162013995105e+00,
                1.898121508020025e+00, 1.898161777543783e+00, 1.924482579560162e+00, 1.985322321439618e+00,
                2.121613755164425e+00, 2.474985118851233e+00, 3.000536619998073e+00, 3.000419418896398e+00,
                3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00, 3.000419418896398e+00
            };

            // Run test
            AnalyzeTransient(tran, ckt, exports, references);
            DestroyExports(exports);
        }

        [Test]
        public void When_CommonSourceAmplifierNoise_Expect_Spice3f5Reference()
        {
            // Create circuit
            var ckt = new Circuit(
                new VoltageSource("V1", "in", "0", 0.0)
                    .SetParameter("acmag", 1.0),
                new VoltageSource("V2", "vdd", "0", 5.0),
                new Resistor("R1", "vdd", "out", 10e3),
                new Resistor("R2", "out", "g", 10e3),
                new Capacitor("C1", "in", "g", 1e-6),
                CreateMOS2("M1", "out", "g", "0", "0", "NFET")
                    .SetParameter("l", 6e-6)
                    .SetParameter("w", 1e-6),
                CreateMOS2Model("NFET", "VTO = -1.44 KP = 8.64E-6 NSUB = 1e17 TOX = 20e-9 KF = 0.5e-25")
                );

            // Create simulation, exports and references
            var noise = new Noise("noise", "out", new DecadeSweep(10, 10e9, 10));
            IExport<double>[] exports = { new InputNoiseDensityExport(noise), new OutputNoiseDensityExport(noise) };
            var references = new double[2][];
            references[0] = new[]
            {
                2.362277630616173e-06, 1.379945267414784e-06, 8.473007758572772e-07, 5.483251555734933e-07,
                3.730472062832258e-07, 2.649962499739830e-07, 1.947939731585481e-07, 1.468616981231575e-07,
                1.127127241713318e-07, 8.755438258118984e-08, 6.855631329051349e-08, 5.395973684199731e-08,
                4.261291387932214e-08, 3.372393096425626e-08, 2.672536768434284e-08, 2.119738853640898e-08,
                1.682198426685170e-08, 1.335430849281782e-08, 1.060376070211410e-08, 8.420890083617284e-09,
                6.687960230375347e-09, 5.311939245547357e-09, 4.219174575568831e-09, 3.351284863236070e-09,
                2.661957766932026e-09, 2.114436968645719e-09, 1.679541365117180e-09, 1.334099339452888e-09,
                1.059708909993047e-09, 8.417548116877004e-10, 6.686287034417421e-10, 5.311102416204870e-10,
                4.218756922498825e-10, 3.351077295963854e-10, 2.661855491980500e-10, 2.114387464861675e-10,
                1.679518309568926e-10, 1.334089539422986e-10, 1.059705753459854e-10, 8.417549847903336e-11,
                6.686305453153985e-11, 5.311129198609113e-11, 4.218787896666749e-11, 3.351110370990360e-11,
                2.661889619930598e-11, 2.114422120523689e-11, 1.679553229713451e-11, 1.334124592122790e-11,
                1.059740872594686e-11, 8.417901372215624e-12, 6.686657144343612e-12, 5.311480973435567e-12,
                4.219139713410938e-12, 3.351462208743194e-12, 2.662241468212711e-12, 2.114773974082943e-12,
                1.679905085917544e-12, 1.334476449652446e-12, 1.060092730788699e-12, 8.421419957485428e-13,
                6.690175731282215e-13, 5.314999561210558e-13, 4.222658301605114e-13, 3.354980797147467e-13,
                2.665760056722277e-13, 2.118292562645285e-13, 1.683423674506338e-13, 1.337995038254496e-13,
                1.063611319397393e-13, 8.456605843605668e-14, 6.725361617419147e-14, 5.350185447355857e-14,
                4.257844187754605e-14, 3.390166683299062e-14, 2.700945942874924e-14, 2.153478448798461e-14,
                1.718609560659779e-14, 1.373180924408069e-14, 1.098797205551033e-14, 8.808464705142402e-15,
                7.077220478956046e-15, 5.702044308892841e-15, 4.609703049291635e-15, 3.742025544836112e-15,
                3.052804804411985e-15, 2.505337310335527e-15, 2.070468422196848e-15, 1.725039785945140e-15,
                1.450656067088105e-15, 1.232705332051313e-15, 1.059580909432678e-15
            };
            references[1] = new[]
            {
                2.970552105465777e-07, 2.053392001294538e-07, 1.425716401150457e-07, 1.005637052541777e-07,
                7.253421438447867e-08, 5.355847444717597e-08, 4.037521102279383e-08, 3.093870755193646e-08,
                2.399259594639712e-08, 1.876085041593681e-08, 1.475173003376309e-08, 1.164174697973875e-08,
                9.209128460130984e-09, 7.295852862170559e-09, 5.785653492254035e-09, 4.590866976751766e-09,
                3.644227374777685e-09, 2.893495350636724e-09, 2.297774776401085e-09, 1.824881474232857e-09,
                1.449401554896117e-09, 1.151223737306888e-09, 9.144110142143362e-10, 7.263231967747751e-10,
                5.769293633775424e-10, 4.582664502662651e-10, 3.640115671591474e-10, 2.891434647260522e-10,
                2.296742247661058e-10, 1.824364329323833e-10, 1.449142737768106e-10, 1.151094398350513e-10,
                9.143465704637279e-11, 7.262912784315901e-11, 5.769137465933203e-11, 4.582590036912043e-11,
                3.640082154126390e-11, 2.891421652629465e-11, 2.296739538836536e-11, 1.824366775621906e-11,
                1.449147767750264e-11, 1.151100723242943e-11, 9.143535443489147e-12, 7.262985775837507e-12,
                5.769212087651685e-12, 4.582665475664515e-12, 3.640158002365927e-12, 2.891497706098719e-12,
                2.296815695164321e-12, 1.824442983501082e-12, 1.449224001466344e-12, 1.151176969908153e-12,
                9.144297975040658e-13, 7.263748339915787e-13, 5.769974668031978e-13, 4.583428064215170e-13,
                3.640920595011466e-13, 2.892260300796565e-13, 2.297578290890759e-13, 1.825205579743038e-13,
                1.449986597966671e-13, 1.151939566537973e-13, 9.151923941987867e-14, 7.271374307188275e-14,
                5.777600635467486e-14, 4.591054031732387e-14, 3.648546562569634e-14, 2.899886268375259e-14,
                2.305204258479738e-14, 1.832831547337173e-14, 1.457612565563390e-14, 1.159565534135988e-14,
                9.228183617974503e-15, 7.347633983178169e-15, 5.853860311459011e-15, 4.667313707724732e-15,
                3.724806238562389e-15, 2.976145944368217e-15, 2.381463934472801e-15, 1.909091223330287e-15,
                1.533872241556531e-15, 1.235825210129142e-15, 9.990780377906112e-16, 8.110230743109810e-16,
                6.616457071390669e-16, 5.429910467656397e-16, 4.487402998494061e-16, 3.738742704299892e-16,
                3.144060694404477e-16, 2.671687983261964e-16, 2.296469001488209e-16
            };

            // Run test
            AnalyzeNoise(noise, ckt, exports, references);
            DestroyExports(exports);
        }
    }
}

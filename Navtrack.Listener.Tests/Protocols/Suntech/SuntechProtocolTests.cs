using Navtrack.Listener.Protocols.Suntech;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Suntech
{
    public class FifotrackProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new SuntechProtocol(), new SuntechMessageHandler());
        }

        [Test]
        public void DeviceSendsLocationV1_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice(
                "SA200STT;850000;010;20081017;07:41:56;00100;+37.478519;+126.886819;000.012;000.00;9;1;0;15.30;0011 00;1;0072;0;4.5;1");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
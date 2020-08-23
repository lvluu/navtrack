using System;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Fifotrack
{
    [Service(typeof(ICustomMessageHandler<FifotrackProtocol>))]
    public class FifotrackMessageHandler : BaseMessageHandler<FifotrackProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            Location location = new Location
            {
                Device = new Device
                {
                    IMEI = input.DataMessage.CommaSplit[1]
                },
                DateTime = ConvertDate(input.DataMessage.CommaSplit.Get<string>(5)),
                PositionStatus = input.DataMessage.CommaSplit.Get<string>(6) == "A",
                Latitude = input.DataMessage.CommaSplit.Get<decimal>(7),
                Longitude = input.DataMessage.CommaSplit.Get<decimal>(8),
                Speed = input.DataMessage.CommaSplit.Get<decimal?>(9),
                Heading = input.DataMessage.CommaSplit.Get<decimal?>(10),
                Altitude = input.DataMessage.CommaSplit.Get<decimal?>(11),
                Odometer = input.DataMessage.CommaSplit.Get<double?>(12),
            };

            return location;
        }

        private static DateTime ConvertDate(string date) => DateTimeUtil.New(date[..2], date[2..4], date[4..6],
            date[6..8], date[8..10], date[10..12]);
    }
}
using System.Text.RegularExpressions;
using Navtrack.Library.DI;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Helpers.New;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.ManPower
{
    [Service(typeof(ICustomMessageHandler<ManPowerProtocol>))]
    public class ManPowerMessageHandler : BaseMessageHandler<ManPowerProtocol>
    {
        public override Location Parse(MessageInput input)
        {
            Match locationMatch =
                new Regex(
                        "simei:(\\d+)," + // imei
                        "(.*?)" + // ignore
                        "(\\d{12})," + // yy mm dd hh mm ss
                        "(A|V)," + // gps fix
                        "(\\d+.\\d+),(N|S)," + // latitude
                        "(\\d+.\\d+),(E|W)," + // longitude
                        "(\\d+.\\d+),") // speed
                    .Match(input.DataMessage.String);

            if (locationMatch.Success)
            {
                Location location = new Location
                {
                    Device = new Device
                    {
                        DeviceId = locationMatch.Groups[1].Value
                    },
                    DateTime = NewDateTimeUtil.Convert(DateFormat.YYMMDDHHMMSS, locationMatch.Groups[3].Value),
                    PositionStatus = locationMatch.Groups[4].Value == "A",
                    Latitude = GpsUtil.ConvertDmmLatToDecimal(locationMatch.Groups[5].Value, locationMatch.Groups[6].Value),
                    Longitude = GpsUtil.ConvertDmmLongToDecimal(locationMatch.Groups[7].Value, locationMatch.Groups[8].Value),
                    Speed = locationMatch.Groups[9].Get<decimal?>()
                };

                return location;
            }

            return null;
        }
    }
}
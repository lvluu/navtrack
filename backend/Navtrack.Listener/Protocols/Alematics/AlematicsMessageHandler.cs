using System.Text.RegularExpressions;
using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;
using System;
namespace Navtrack.Listener.Protocols.Alematics;

[Service(typeof(ICustomMessageHandler<AlematicsProtocol>))]
public class AlematicsMessageHandler : BaseMessageHandler<AlematicsProtocol>
{
    public override Position Parse(MessageInput input)
    {
        Match locationMatch =
            new Regex("(.*)," + // IMEI
                      "(\\d{4})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})," + // GPS date
                      "(\\d{4})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})," + // Device date
                      "(.*?)," + // Latitude
                      "(.*?)," + // Longitude
                      "(.*?)," + // Speed
                      "(.*?)," + // Heading
                      "(.*?)," + // Altitude
                      "(.*?)," + // HDOP
                      "(.*?)," + // Satellites
                      "(.*?)," + // Input
                      "(.*?)," + // Output
                      "(.*?)," + // ADC
                      "(.*?)," + // Power
                      "(\\d+)") // Odometer
                .Match(input.DataMessage.String);

        if (locationMatch.Success)
        {
            input.ConnectionContext.SetDevice(locationMatch.Groups[1].Value);
            string gpioStatus = locationMatch.Groups[21].Value;

		    Console.WriteLine("This is a match {0} with status {1}", input.DataMessage.String, gpioStatus);
            
                 position = new()
            {
                Device = input.ConnectionContext.Device,
                Date = DateTimeUtil.New(locationMatch.Groups[2].Value, locationMatch.Groups[3].Value,
                    locationMatch.Groups[4].Value, locationMatch.Groups[5].Value, locationMatch.Groups[6].Value,
                    locationMatch.Groups[7].Value, add2000Year: false),
                Latitude = locationMatch.Groups[14].Get<double>(),
                Longitude = locationMatch.Groups[15].Get<double>(),
                Speed = locationMatch.Groups[16].Get<float?>(),
                Heading = locationMatch.Groups[17].Get<float?>(),
                Altitude = locationMatch.Groups[18].Get<float?>(),
                HDOP = locationMatch.Groups[19].Get<float?>(),
                Satellites = locationMatch.Groups[20].Get<short?>(),
                GpioStatus = locationMatch.Groups[21].Get<int?>(),
                Odometer = locationMatch.Groups[25].Get<double?>(),
            };

            return position;
        }

        return null;
    }
}

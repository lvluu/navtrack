using Navtrack.Common.Model;

namespace Navtrack.Listener.Protocols.Meitrack
{
    public interface IMeitrackLocationParser
    {
        Location<MeitrackData> Parse(string input);
    }
}
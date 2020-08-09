using Navtrack.DataAccess.Model.Common;

namespace Navtrack.DataAccess.Model
{
    public class LogEntity : CreatedEntityAudit, IEntity
    {
        public int Id { get; set; }
        public string Data { get; set; }
    }
}
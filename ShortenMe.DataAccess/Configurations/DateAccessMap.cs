using FluentNHibernate.Mapping;
using ShortenMe.DataAccess.DataAccessModels;

namespace ShortenMe.DataAccess.Configurations
{
    public class DateAccessMap : ClassMap<DateAccessDAO>
    {
        public DateAccessMap()
        {
            Id(a => a.ID).GeneratedBy.Assigned();
            Map(a => a.LinkInfoID);
            Map(a => a.DateCreated);
            Table("DateAccess");
        }
    }
}

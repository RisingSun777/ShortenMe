using FluentNHibernate.Mapping;
using ShortenMe.DataAccess.DataAccessModels;

namespace ShortenMe.DataAccess.Configurations
{
    public class LinkInfoMap : ClassMap<LinkInfoDAO>
    {
        public LinkInfoMap()
        {
            Id(a => a.ID).GeneratedBy.Assigned();
            Map(a => a.FullLink);
            Map(a => a.ShortenedLink);
            Table("LinkInfo");
        }
    }
}

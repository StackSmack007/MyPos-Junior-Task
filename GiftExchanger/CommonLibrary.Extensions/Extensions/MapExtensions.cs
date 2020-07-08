namespace CommonLibrary.Extensions
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using AutomapperCFG;
    using System.Linq;

    public static class MapExtensions
    {

        private static MapperConfiguration mapcfg;
        static MapExtensions()
        {

            mapcfg = new MapperConfiguration(mc =>
             {
                 mc.AddProfile(new MapperConfigurator());
             });
        }
        public static IQueryable<TDestination> To<TDestination>(this IQueryable query)
        {
            return query.ProjectTo<TDestination>(mapcfg);
        }

    }
}

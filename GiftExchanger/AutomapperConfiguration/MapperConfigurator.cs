using AutoMapper;
using CommonLibrary.Interfaces;
using Infrastructure.DTOS;
using Infrasturcture.Models;
using System;
using System.Linq;

namespace AutomapperCFG
{
    public class MapperConfigurator : Profile
    {
        public MapperConfigurator()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
            CreateMapToMappings(allTypes);
            CreateMapFromMappings(allTypes);

            CreateMap<UserGE, UserDashboardInfoDTOout>()
               .ForMember(d => d.Sent, opt => opt.MapFrom(s => s.TransactionsSent))
               .ForMember(d => d.Recieved, opt => opt.MapFrom(s => s.TransactionsRecieved));
        }

        private void CreateMapToMappings(System.Collections.Generic.IEnumerable<Type> allTypes)
        {
            Type[] sourseTypes = allTypes.Where(x => x.GetInterfaces()
                                         .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>)))
                                         .ToArray();
            foreach (Type sType in sourseTypes)
            {
                Type[] targetTypes = sType.GetInterfaces()
                                          .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>))
                                          .Select(x => x.GetGenericArguments().First())
                                          .ToArray();

                foreach (Type targetType in targetTypes)
                {
                    CreateMap(sType, targetType);
                }
            }
        }
        private void CreateMapFromMappings(System.Collections.Generic.IEnumerable<Type> allTypes)
        {
            Type[] destTypes = allTypes.Where(x => x.GetInterfaces()
                                       .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                                       .ToArray();

            foreach (Type dType in destTypes)
            {
                Type[] sourceTypes = dType.GetInterfaces()
                                          .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                                          .Select(x => x.GetGenericArguments().First())
                                          .ToArray();

                foreach (Type sType in sourceTypes)
                {
                    CreateMap(sType, dType);
                }
            }
        }
    }
}
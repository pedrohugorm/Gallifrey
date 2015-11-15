using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Gallifrey.SharedKernel.Application.Extension
{
    /// <summary>
    /// Mapping extensions for any object to another
    /// Project mapping in MapEnumerableFromTo method is used to map object in a lazy way. 
    /// So, when mapping, it only does the map operation when the enumeration happens, avoiding many enumerations
    /// </summary>
    public static class MappingExtension
    {
        static void CreateMappingIfNonExistant<TTypeFrom, TTypeTo>()
        {
            if (Mapper.FindTypeMapFor<TTypeFrom, TTypeTo>() == null)
                Mapper.CreateMap<TTypeFrom, TTypeTo>();
        }

        public static IQueryable<TTypeTo> MapEnumerableFromTo<TTypeFrom, TTypeTo>(this IQueryable<TTypeFrom> enumerable)
        {
            CreateMappingIfNonExistant<TTypeFrom, TTypeTo>();

            return enumerable.Project().To<TTypeTo>();
        }

        public static TTypeTo MapTo<TTypeFrom, TTypeTo>(this TTypeFrom value)
        {
            CreateMappingIfNonExistant<TTypeFrom, TTypeTo>();

            return Mapper.Map<TTypeTo>(value);
        }
    }
}
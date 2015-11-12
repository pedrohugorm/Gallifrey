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
        public static IQueryable<TTypeTo> MapEnumerableFromTo<TTypeFrom, TTypeTo>(this IQueryable<TTypeFrom> enumerable)
        {
            return enumerable.Project().To<TTypeTo>();
        }

        public static T MapTo<T>(this object value)
        {
            return Mapper.Map<T>(value);
        }
    }
}
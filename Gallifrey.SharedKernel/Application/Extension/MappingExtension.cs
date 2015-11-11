using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Gallifrey.SharedKernel.Application.Extension
{
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
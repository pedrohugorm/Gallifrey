using System;
using System.Collections.Generic;

namespace Gallifrey.SharedKernel.Application.Configuration
{
    public interface IGallifreyContainer
    {
        /// <summary>
        /// Wrapper for the internal IoC that retrieves all instances of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAllInstances<T>();
        /// <summary>
        /// Wrapper for the internal IoC that retrieves all instances of a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<object> GetAllInstances(Type type);
        /// <summary>
        /// Retrieves an instane of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetInstance<T>();
        /// <summary>
        /// Retrieves an instance of the given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetInstance(Type type);
    }
}
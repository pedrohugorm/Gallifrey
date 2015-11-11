using System;
using StructureMap;

namespace Gallifrey.SharedKernel.Application.Ioc
{
    /// <summary>
    /// Interface that has IoC StructureMap configuration
    /// use Configure method with <see cref="IContainer"/>->Configure method
    /// </summary>
    public interface IConfigureContainer
    {
        Action<ConfigurationExpression> Configure();
    }
}

using System;
using StructureMap;

namespace Gallifrey.SharedKernel.Application.Ioc
{
    public interface IConfigureContainer
    {
        Action<ConfigurationExpression> Configure();
    }
}

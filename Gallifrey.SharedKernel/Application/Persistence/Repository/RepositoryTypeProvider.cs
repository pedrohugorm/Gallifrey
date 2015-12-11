using System;
using System.Linq;
using Gallifrey.SharedKernel.Application.Persistence.Repository.Document;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    /// <summary>
    /// TODO
    /// </summary>
    public class RepositoryTypeProvider
    {
        public Type GetRepository<TModel>()
        {
            var modelType = typeof (TModel);
            var databaseBaseInterface = typeof (IIdentity);
            var documentBaseType = typeof (Microsoft.Azure.Documents.Document);

            Type repoBaseType;
            if (IsImplementsInterface(databaseBaseInterface, modelType) ||
                IsSameOrSubclass(databaseBaseInterface, modelType))
            {
                repoBaseType = typeof (IDatabaseRepository<,>);

                var item =
                    modelType.GetInterfaces()
                        .OrderByDescending(r => r.GenericTypeArguments.Length).First();

                var genericArgument = item.GenericTypeArguments.First();

                return repoBaseType.MakeGenericType(typeof (TModel), genericArgument);
            }

            if (IsSameOrSubclass(documentBaseType, modelType))
            {
                repoBaseType = typeof (IDocumentRepository<>);

                return repoBaseType.MakeGenericType(typeof (TModel));
            }

            throw new NotSupportedException(string.Format("{0} not supported", typeof (TModel).FullName));
        }

        bool IsImplementsInterface(Type interfaceType, Type type)
        {
            return type.GetInterfaces().Contains(interfaceType);
        }

        bool IsSameOrSubclass(Type baseType, Type descendantType)
        {
            return descendantType.IsSubclassOf(baseType)
                   || descendantType == baseType;
        }
    }
}
using Gallifrey.SharedKernel.Application.Configuration;
using Gallifrey.SharedKernel.Application.Persistence;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Validation;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Gallifrey.RestApi.Application.Configuration
{
    public class BaseRegistry : Registry
    {
        public BaseRegistry()
        {
            Scan(s =>
            {
                s.TheCallingAssembly();
                s.RegisterConcreteTypesAgainstTheFirstInterface();
                s.AddAllTypesOf(typeof (IValidationStrategy<>));
                s.AddAllTypesOf(typeof (IPersistItem<,>));
                s.AddAllTypesOf(typeof (IRetrieveItemByIdentity<,>));
                s.AddAllTypesOf(typeof (IRetrieveQueryOfItems<>));
                s.AddAllTypesOf(typeof (IRepository<,>));
                s.AddAllTypesOf(typeof (IDatabaseRepository<,>));
                s.AddAllTypesOf(typeof (IIdentity<>));
                s.AddAllTypesOf(typeof (IHandleEntityChanged<>));
                s.AddAllTypesOf(typeof (IHandleEntityChanging<>));
                s.AddAllTypesOf<IRegisterMapping>();
            });

            For<IPersistenceConfigurationProvider>().UseIfNone<DefaultPersistenceConfiguration>();
        }
    }
}
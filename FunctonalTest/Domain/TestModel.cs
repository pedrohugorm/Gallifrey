using System;
using Gallifrey.SharedKernel.Application.Persistence.Repository;

namespace FunctionalTest.Domain
{
    public class TestModel : IIdentity<Guid>
    {
        public Guid Id { set; get; }
    }
}
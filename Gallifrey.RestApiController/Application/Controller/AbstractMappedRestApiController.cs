using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gallifrey.RestApi.Application.Domain.Model;
using Gallifrey.SharedKernel.Application.Extension;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Validation;

namespace Gallifrey.RestApi.Application.Controller
{
    public abstract class AbstractMappedRestApiController<TViewModel, TIdentityType, TDomainModel> :
        ApiController
        where TViewModel : class
        where TDomainModel : class
    {
        private readonly IDatabaseRepository<TViewModel, TIdentityType> _respository;

        private readonly ValidationStrategyFactory<TDomainModel> _validation;

        protected AbstractMappedRestApiController(IDatabaseRepository<TViewModel, TIdentityType> respository,
            IEnumerable<IValidationStrategy> validationStrategies)
        {
            _respository = respository;
            //Usefull for serialization of REST API
            _respository.DisableProxyAndLazyLoading();

            _validation =
                new ValidationStrategyFactory<TDomainModel>(error => ModelState.AddModelError("Validation", error),
                    validationStrategies);
        }

        private void ValidateWithStrategy(TDomainModel model)
        {
            _validation.Validate(model);
        }

        public virtual IEnumerable<TDomainModel> Get()
        {
            return _respository.GetAllFiltered().MapEnumerableFromTo<TViewModel, TDomainModel>();
        }

        [HttpGet]
        public virtual ResponseContainer<TDomainModel> Get(TIdentityType id)
        {
            return new ResponseContainer<TDomainModel>(_respository.Find(id).MapTo<TDomainModel>());
        }

        [HttpPost]
        public virtual HttpResponseMessage Post([FromBody] TDomainModel value)
        {
            try
            {
                ValidateWithStrategy(value);

                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                _respository.InsertOrUpdate(value.MapTo<TViewModel>());
                _respository.Save();
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public virtual HttpResponseMessage Put(int id, [FromBody] TDomainModel value)
        {
            try
            {
                ValidateWithStrategy(value);

                if (!ModelState.IsValid)
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                _respository.InsertOrUpdate(value.MapTo<TViewModel>());
                _respository.Save();
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public virtual HttpResponseMessage Delete(TIdentityType id)
        {
            _respository.Delete(id);
            _respository.Save();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
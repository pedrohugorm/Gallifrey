using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Validation;

namespace Gallifrey.RestApi.Application.Controller
{
    public abstract class AbstractRestApiController<TModel, TIdentityType, TRepository> : ApiController
        where TRepository : IDatabaseRepository<TModel, TIdentityType>
        where TModel : class
        where TIdentityType : struct
    {
        private readonly TRepository _respository;

        private readonly ValidationStrategyFactory<TModel> _validation;

        protected AbstractRestApiController(TRepository respository,
            IEnumerable<IValidationStrategy<TModel>> validationStrategies)
        {
            _respository = respository;
            //Usefull for serialization of REST API
            _respository.DisableProxyAndLazyLoading();

            _validation = new ValidationStrategyFactory<TModel>(e => ModelState.AddModelError("", e),
                validationStrategies);
        }

        private void Validate(TModel model)
        {
            _validation.Validate(model);
        }

        // GET api/transactionapi
        public virtual IEnumerable<TModel> Get()
        {
            return _respository.GetAllFiltered();
        }

        // GET api/transactionapi/5
        [System.Web.Mvc.HttpGet]
        public virtual TModel Get(TIdentityType id)
        {
            return _respository.Find(id);
        }

        // POST api/transactionapi
        [System.Web.Mvc.HttpPost]
        public virtual HttpResponseMessage Post([FromBody]TModel value)
        {
            try
            {
                Validate(value);

                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                _respository.InsertOrUpdate(value);
                _respository.Save();
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT api/transactionapi/5
        public virtual HttpResponseMessage Put(int id, [FromBody]TModel value)
        {
            try
            {
                Validate(value);

                if (!ModelState.IsValid)
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                _respository.InsertOrUpdate(value);
                _respository.Save();
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // DELETE api/transactionapi/5
        public virtual HttpResponseMessage Delete(TIdentityType id)
        {
            _respository.Delete(id);
            _respository.Save();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FluentValidation;
using Gallifrey.RestApi.Application.Domain.Model;
using Gallifrey.RestApi.Application.Validation;
using Gallifrey.SharedKernel.Application.Extension;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Validation;

namespace Gallifrey.RestApi.Application.Controller
{
    public abstract class AbstractMappedRestApiController<TModel, TIdentityType, TViewModel> :
        ApiController
        where TModel : class
        where TViewModel : class
    {
        private readonly IDatabaseRepository<TModel, TIdentityType> _respository;
        private readonly IValidationHelper<TViewModel> _validationHelper; 
        
        protected AbstractMappedRestApiController(IDatabaseRepository<TModel, TIdentityType> respository,
            IEnumerable<IValidator<TViewModel>> validators)
        {
            _respository = respository;
            
            //Usefull for serialization of REST API
            _respository.DisableProxyAndLazyLoading();

            _validationHelper = new ModelStateValidationHelper<TViewModel>(ModelState, validators);
        }

        public virtual IEnumerable<TViewModel> Get()
        {
            return _respository.GetAllFiltered().MapEnumerableFromTo<TModel, TViewModel>();
        }

        [HttpGet]
        public virtual ResponseContainer<TViewModel> Get(TIdentityType id)
        {
            return new ResponseContainer<TViewModel>(_respository.Find(id).MapTo<TModel, TViewModel>());
        }

        [HttpPost]
        public virtual HttpResponseMessage Post([FromBody] TViewModel value)
        {
            try
            {
                _validationHelper.Validate(value);

                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                _respository.InsertOrUpdate(value.MapTo<TViewModel, TModel>());
                _respository.Save();
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public virtual HttpResponseMessage Put(int id, [FromBody] TViewModel value)
        {
            try
            {
                _validationHelper.Validate(value);

                if (!ModelState.IsValid)
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                _respository.InsertOrUpdate(value.MapTo<TViewModel, TModel>());
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
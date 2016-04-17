using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sambucks.Data;

using Sambucks.Models;

namespace Sambucks.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        ISambucksRepository _repo;
        ModelFactory _modelFactory;

        public BaseApiController(ISambucksRepository repo)
        {
            _repo = repo;
        }

        protected ISambucksRepository Repository
        {
            get
            {
                return _repo;
            }
        }

        protected ModelFactory ModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, Repository);
                }
                return _modelFactory;
            }
        }


    }
}

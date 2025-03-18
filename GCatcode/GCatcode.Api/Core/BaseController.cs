using Microsoft.AspNetCore.Mvc;

namespace GCatcode.Api.Core
{
    public class BaseController : ControllerBase
    {
        protected readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration)
        { _configuration = configuration; }
    }
}
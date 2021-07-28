using Microsoft.AspNetCore.Mvc;

namespace IoT.WebAPI.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {

    }
}

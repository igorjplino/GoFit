using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoFit.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly IMediator Mediator;

    protected ApiControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }
}

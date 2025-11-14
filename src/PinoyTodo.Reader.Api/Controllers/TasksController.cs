using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PinoyCleanArch.Api.Controllers;

namespace PinoyTodo.Reader.Api.Controllers;

[Route("[controller]")]
public class TasksController : BaseController
{
    public TasksController(ISender mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new Application.Tasks.Queries.GetAllTasksQuery();
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);

    }
}
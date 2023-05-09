using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Products.Commands;
using CleanArchitecture.Application.Products.Queries.GetProducts;
using CleanArchitecture.Application.TodoItems.Commands.DeleteTodoItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

[Authorize]
public class ProductController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductBriefDto>>> GetTodoItemsWithPagination([FromQuery] GetProductsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<long>> Create(CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(long id)
    {
        await Mediator.Send(new DeleteTodoItemCommand(id));

        return NoContent();
    }

    [HttpPost("clean")]
    public async Task<ActionResult<long>> Clean(CleanProductsCommand command)
    {
        return await Mediator.Send(command);
    }
}

using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Entities.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodos;


[Authorize]
public record GetProductsQuery : IRequest<ICollection<Product>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ICollection<Product>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ICollection<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products.ToListAsync();
    }
}
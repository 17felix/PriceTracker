using CleanArchitecture.Domain.Entities.Products;

namespace CleanArchitecture.Application.Common.Interfaces;
public interface IDirtyProductService : IService<DirtyProduct>
{
}

public class DirtyProductService : ForwardingServiceBase<DirtyProduct>, IDirtyProductService

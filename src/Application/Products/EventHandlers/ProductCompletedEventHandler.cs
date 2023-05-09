using CleanArchitecture.Application.TodoItems.EventHandlers;
using CleanArchitecture.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Products.EventHandlers;

public class ProductCompletedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<TodoItemCompletedEventHandler> _logger;

    public ProductCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}

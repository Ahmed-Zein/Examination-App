using Core.Interfaces;
using MediatR;

namespace Application.Commands.Notification.Admin.Handlers;

public class CreateAdminNotificationCommandHandler(IAdminNotificationRepository repository)
    : IRequestHandler<CreateAdminNotificationCommand>
{
    public async Task Handle(CreateAdminNotificationCommand request, CancellationToken cancellationToken = default)
    {
        await repository.AddAsync(request.Notification);
    }
}
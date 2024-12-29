using Core.Entities;
using Core.Interfaces;
using MediatR;

namespace Application.Queries.Notification.Admin.Handlers;

public class GetAdminNotificationsQueryHandler(IAdminNotificationRepository repository)
    : IRequestHandler<GetAdminNotificationsQuery, List<AdminNotification>>
{
    public async Task<List<AdminNotification>> Handle(GetAdminNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync();
    }
}
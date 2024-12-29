using Core.Entities;
using MediatR;

namespace Application.Queries.Notification.Admin;

public record GetAdminNotificationsQuery : IRequest<List<AdminNotification>>;
using Core.Entities;
using MediatR;

namespace Application.Commands.Notification.Admin;

public record CreateAdminNotificationCommand(AdminNotification Notification) : IRequest;
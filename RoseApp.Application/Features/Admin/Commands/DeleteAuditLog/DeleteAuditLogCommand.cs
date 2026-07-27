using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Admin.Commands.DeleteAuditLog;

public record DeleteAuditLogCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;
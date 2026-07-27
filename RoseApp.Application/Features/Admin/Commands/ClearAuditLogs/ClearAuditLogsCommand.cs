using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Admin.Commands.ClearAuditLogs;

public record ClearAuditLogsCommand()
    : IRequest<ResponseViewModel<string>>;
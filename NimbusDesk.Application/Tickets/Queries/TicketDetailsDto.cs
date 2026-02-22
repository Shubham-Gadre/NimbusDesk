using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    public sealed record TicketDetailsDto(
    Guid Id,
    string Title,
    string Description,
    string Status,
    string Priority,
    Guid? AssignedToUserId,
    DateTime CreatedAt,
    DateTime? ClosedAt,
    IReadOnlyList<CommentDto> Comments);
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Tickets.Queries
{
    /// <summary>
    /// Represents complete details of a ticket including description and associated comments.
    /// Used for detailed ticket view operations.
    /// </summary>
    /// <param name="Id">The unique identifier of the ticket.</param>
    /// <param name="Title">The title of the ticket.</param>
    /// <param name="Description">The detailed description of the ticket.</param>
    /// <param name="Status">The current status of the ticket.</param>
    /// <param name="Priority">The priority level of the ticket.</param>
    /// <param name="AssignedToUserId">The ID of the user the ticket is assigned to, or null if unassigned.</param>
    /// <param name="CreatedAt">The date and time when the ticket was created.</param>
    /// <param name="ClosedAt">The date and time when the ticket was closed, or null if still open.</param>
    /// <param name="Comments">The collection of comments attached to this ticket.</param>
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Application.Common;
using NimbusDesk.Application.Common.DTO_s;
using NimbusDesk.Application.Tickets.Assign;
using NimbusDesk.Application.Tickets.Close;
using NimbusDesk.Application.Tickets.Comment;
using NimbusDesk.Application.Tickets.Create;
using NimbusDesk.Application.Tickets.Queries;
using NimbusDesk.Application.Tickets.ReOpen;
using NimbusDesk.Application.Tickets.Update;
using NimbusDesk.Domain.ValueObjects;
using NimbusDesk.Infrastructure.Identity;
using NimbusDesk.Infrastructure.Persistence;

namespace NimbusDesk.API.Controllers
{

    /// <summary>
    /// API controller for ticket management operations.
    /// Provides endpoints for creating, retrieving, updating, closing, and reopening tickets.
    /// Also handles user registration and login operations.
    /// </summary>
    [ApiController]
    [Route("api/tickets")]
    [Authorize]  // Default: all endpoints require authorization
    public sealed class TicketsController : ControllerBase
    {
        private readonly CreateTicketHandler _handler;
        private readonly CloseTicketHandler _closeTicketHandler;
        private readonly GetTicketsHandler _getTicketsHandler;
        private readonly GetTicketHistoryHandler _getTicketHistoryHandler;
        private readonly ReopenTicketHandler _reopenTicketHandler;
        private readonly UpdateTicketHandler _updateTicketHandler;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        /// <summary>
        /// Request model for adding a comment to a ticket.
        /// </summary>
        public record AddCommentRequest(Guid UserId, string Content);


        /// <summary>
        /// Initializes a new instance of the <see cref="TicketsController"/> class.
        /// </summary>
        public TicketsController(UserManager<ApplicationUser> userManager, ITokenService tokenService, CreateTicketHandler handler, CloseTicketHandler closeHandler, ITicketRepository repository, GetTicketsHandler getTicketsHandler, GetTicketHistoryHandler getTicketHistoryHandler, ReopenTicketHandler reopenTicketHandler, UpdateTicketHandler updateTicketHandler)
        {
            _handler = handler;
            _closeTicketHandler = closeHandler;
            _getTicketsHandler = getTicketsHandler ?? throw new ArgumentNullException(nameof(getTicketsHandler));
            _getTicketHistoryHandler = getTicketHistoryHandler;
            _reopenTicketHandler = reopenTicketHandler;
            _updateTicketHandler = updateTicketHandler;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Request model for creating a new ticket.
        /// </summary>
        public sealed record CreateTicketRequest
        (
            string Title,
            string Description,
            string Priority
        );

        /// <summary>
        /// Response model for ticket details.
        /// </summary>
        public sealed record TicketDto(
            Guid Id,
            string Title,
            string Description,
            TicketStatus Status,
            string Priority,
            DateTime CreatedAt,
            DateTime? ClosedAt
        );

        /// <summary>
        /// Request model for updating an existing ticket.
        /// </summary>
        public sealed record UpdateTicketRequest(
            string Title,
            string Description,
            string Priority);



        /// <summary>
        /// Registers a new user with the application.
        /// </summary>
        /// <param name="request">The registration request containing user credentials and profile information.</param>
        /// <returns>200 OK if registration is successful; otherwise, 400 Bad Request with error details.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest("Email already registered");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { message = "Registration failed", errors });
            }

            return Ok(new { message = "Registration successful. Please log in." });
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="request">The login request containing email and password.</param>
        /// <returns>200 OK with JWT token and user information if authentication is successful; otherwise, 401 Unauthorized.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized(new { message = "Invalid email or password" });

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateJwtToken(user, roles);

            return Ok(new AuthResponse(token, user.Email!, user.FirstName));
        }


        /// <summary>
        /// Creates a new ticket.
        /// </summary>
        /// <param name="request">The request containing ticket title, description, and priority.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>201 Created with the location of the created ticket.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateTicketRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateTicketCommand(
                request.Title,
                request.Description,
                request.Priority);

            var ticketId = await _handler.Handle(command, cancellationToken);

            return CreatedAtAction(nameof(GetDetails), new { id = ticketId }, null);
        }

        /// <summary>
        /// Retrieves a paginated list of tickets with optional filtering and sorting.
        /// </summary>
        /// <param name="page">The page number (default: 1).</param>
        /// <param name="pageSize">The number of tickets per page (default: 20).</param>
        /// <param name="status">Optional filter by ticket status.</param>
        /// <param name="priority">Optional filter by ticket priority.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>200 OK with a paginated list of ticket summaries.</returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PagedResult<TicketSummaryDto>>> Get(
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 20,
                [FromQuery] string? status = null,
                [FromQuery] string? priority = null,
                CancellationToken cancellationToken = default)
        {
            var query = new GetTicketsQuery(
                page,
                pageSize,
                status,
                priority);

            var result = await _getTicketsHandler
                .Handle(query, cancellationToken);

            return Ok(result);
        }



        /// <summary>
        /// Closes an existing ticket.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket to close.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>204 No Content if the ticket is successfully closed.</returns>
        [HttpPost("{id:guid}/close")]
        public async Task<IActionResult> Close(
                            Guid id,
                            CancellationToken cancellationToken)
        {
            var command = new CloseTicketCommand(id);

            await _closeTicketHandler.Handle(command, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Retrieves the change history for a specific ticket.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>200 OK with the ticket history records.</returns>
        [HttpGet("{id:guid}/history")]
        public async Task<ActionResult<IReadOnlyList<TicketHistoryDto>>> GetHistory(Guid id, CancellationToken cancellationToken)
        {
            var history = await _getTicketHistoryHandler.Handle(id, cancellationToken);

            return Ok(history);
        }

        /// <summary>
        /// Reopens a closed ticket.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket to reopen.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>204 No Content if the ticket is successfully reopened.</returns>
        [HttpPost("{id:guid}/reopen")]
        public async Task<IActionResult> Reopen(Guid id, CancellationToken cancellationToken)
        {
            var command = new ReopenTicketCommand(id);

            await _reopenTicketHandler.Handle(command, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Updates an existing ticket's details.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket to update.</param>
        /// <param name="request">The request containing the new ticket details.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>204 No Content if the ticket is successfully updated.</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateTicketRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTicketCommand(
                id,
                request.Title,
                request.Description,
                request.Priority);

            await _updateTicketHandler.Handle(command, cancellationToken);

            return NoContent();
        }


        /// <summary>
        /// Assigns a ticket to a user.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket to assign.</param>
        /// <param name="userId">The unique identifier of the user to assign the ticket to.</param>
        /// <param name="handler">The handler for the assign ticket operation (injected from DI container).</param>
        /// <returns>204 No Content if the ticket is successfully assigned.</returns>
        [HttpPost("{id:guid}/assign")]
        public async Task<IActionResult> Assign(Guid id, [FromBody] Guid userId, [FromServices] AssignTicketHandler handler)
        {
            await handler.HandleAsync(new AssignTicketCommand(id, userId), HttpContext.RequestAborted);
            return NoContent(); // Status 204
        }

        /// <summary>
        /// Adds a comment to a ticket.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket.</param>
        /// <param name="request">The request containing the user ID and comment content.</param>
        /// <param name="handler">The handler for the add comment operation (injected from DI container).</param>
        /// <returns>204 No Content if the comment is successfully added.</returns>
        [HttpPost("{id:guid}/comments")]
        public async Task<IActionResult> AddComment(Guid id, [FromBody] AddCommentRequest request, [FromServices] AddCommentHandler handler)
        {
            await handler.HandleAsync(new AddCommentCommand(id, request.UserId, request.Content), HttpContext.RequestAborted);
            return NoContent();
        }

        /// <summary>
        /// Retrieves detailed information about a specific ticket, including its comments.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket.</param>
        /// <param name="handler">The handler for the get ticket details operation (injected from DI container).</param>
        /// <returns>200 OK with ticket details if found; otherwise, 404 Not Found.</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TicketDetailsDto>> GetDetails(Guid id, [FromServices] GetTicketDetailsHandler handler)
        {
            var ticket = await handler.HandleAsync(id, HttpContext.RequestAborted);

            if (ticket is null) return NotFound();

            return Ok(ticket);
        }

    }

}

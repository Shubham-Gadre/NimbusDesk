using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Application.Common
{
    /// <summary>
    /// Represents a paginated result set containing a subset of items from a larger collection.
    /// Provides pagination information including current page, page size, and total count.
    /// </summary>
    /// <typeparam name="T">The type of items in the result set.</typeparam>
    public sealed class PagedResult<T>
    {
        /// <summary>Gets the read-only collection of items in the current page.</summary>
        public IReadOnlyList<T> Items { get; }
        /// <summary>Gets the current page number (1-based).</summary>
        public int Page { get; }
        /// <summary>Gets the number of items per page.</summary>
        public int PageSize { get; }
        /// <summary>Gets the total count of items across all pages.</summary>
        public int TotalCount { get; }
        /// <summary>Gets the total number of pages available.</summary>
        public int TotalPages { get; }
        /// <summary>Gets a value indicating whether there is a previous page.</summary>
        public bool HasPreviousPage => Page > 1;
        /// <summary>Gets a value indicating whether there is a next page.</summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
        /// </summary>
        /// <param name="items">The collection of items for the current page.</param>
        /// <param name="page">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalCount">The total count of items across all pages.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when page or pageSize is less than or equal to zero.</exception>
        public PagedResult(
            IReadOnlyList<T> items,
            int page,
            int pageSize,
            int totalCount)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(page);

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}

using BlogPostWebAPI.Models;

namespace BlogPostWebAPI.Repository
{
    /// <summary>
    /// Blog repository to hadle all data operations.
    /// </summary>
    public interface IBlogRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>List of blogs</returns>
        Task<List<BlogPost>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Blog.</returns>
        Task<BlogPost?> GetByIdAsync(int id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="blog">The blog.</param>
        Task AddAsync(BlogPost blog);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="blog">The blog.</param>
        Task UpdateAsync(BlogPost blog);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task DeleteAsync(int id);
    }
}

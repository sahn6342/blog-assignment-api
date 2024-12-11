using BlogPostWebAPI.Models;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;

namespace BlogPostWebAPI.Repository
{
    /// <summary>
    /// Blog repository to hadle all data operations.
    /// </summary>
    /// <seealso cref="BlogPostWebAPI.Repository.IBlogRepository" />
    public class BlogRepository : IBlogRepository
    {
        /// <summary>
        /// The file path
        /// </summary>
        private readonly string _filePath = "blogs.json";

        public BlogRepository()
        {
            // Primary step to create json file if not exists.
            CreateFileIfNotExist();
        }

        /// <summary>
        /// Gets all Blogs asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<BlogPost>> GetAllAsync()
        {

            var jsonData = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<BlogPost>>(jsonData) ?? new List<BlogPost>();
        }

        /// <summary>
        /// Creates the Blogs asynchronous.
        /// </summary>
        /// <param name="post">The post.</param>
        public async Task AddAsync(BlogPost post)
        {
            var posts = await GetAllAsync();
            post.Id = posts?.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1;
            posts.Add(post);

            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(posts));
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task<BlogPost?> GetByIdAsync(int id)
        {
            var blog = (await GetAllAsync()).FirstOrDefault(b => b.Id == id);

            return blog is null ? throw new Exception("Blog not found") : blog;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="blog">The blog.</param>
        /// <exception cref="System.Exception">Blog not found</exception>
        public async Task UpdateAsync(BlogPost blog)
        {
            var blogs = await GetAllAsync();
            var index = blogs.FindIndex(b => b.Id == blog.Id);
            if (index == -1) throw new Exception("Blog not found");

            blogs[index] = blog;
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(blogs));
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task DeleteAsync(int id)
        {
            var blogs = await GetAllAsync();
            var blog = blogs.FirstOrDefault(b => b.Id == id) ?? throw new Exception("Blog not found");
            blogs.RemoveAll(b => b.Id == id);
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(blogs));
        }

        /// <summary>
        /// Creates the file if not exist.
        /// </summary>
        private void CreateFileIfNotExist()
        {
            if (!File.Exists(_filePath))
            {
                using (StreamWriter sw = File.AppendText(_filePath))
                {
                    sw.WriteLine("[]");
                }
            }
        }
    }
}

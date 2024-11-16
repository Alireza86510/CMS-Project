using CMS.Core.Convertors;
using CMS.Core.Services.Interfaces;
using CMS.DataLayer.Context;
using CMS.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace CMS.Core.Services.Implements;

public class PostService : IPostService
{
    private CmsContext _context;

    public PostService(CmsContext context)
    {
        _context = context;
    }

    public List<Post> GetAllPosts()
    {
        return _context.Posts
            .Include(p => p.PostReactions)
            .ToList();
    }

    public Post GetPostById(int postId)
    {
        return _context.Posts
            .Include(p => p.Category)
            .Include(p => p.PostComments)
            .Include(p => p.PostReactions)
            .Include(p => p.User)
            .SingleOrDefault(p => p.PostId == postId);
    }

    public bool AddPost(Post post)
    {
        try
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool UpdatePost(Post post)
    {
        try
        {
            _context.Posts.Update(post);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<PostComment> GetPostCommentsByPostId(int postId)
    {
        return _context.PostComments
            .Where(c => c.PostId == postId)
            .ToList();
    }

    public List<Post> GetPostByAuthorId(int authorId)
    {
        return _context.Posts
              .Where(p => p.UserId == authorId)
              .ToList();
    }

    public int GetPostReactionsByPostId(int postId)
    {
        return _context.PostReactions
            .Where(p => p.PostId == postId)
            .ToList()
            .Count;
    }

    public bool Comment(int userId, int postId, string message)
    {
        try
        {
            _context.PostComments.Add(new PostComment
            {
                CommentText = message,
                CreateDate = DateTime.Now.ToShamsi(),
                PostId = postId,
                UserId = userId
            });
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<Post> GetPopularPosts()
    {
        return _context.Posts
            .Include(p => p.PostReactions)
            .Include(p => p.PostComments)
            .Include(p => p.User)
            .Include(p => p.Category)
            .OrderBy(p => p.PostReactions.Count)
            .Take(3)
            .ToList();
    }

    public List<Post> GetPostByCategory(int categoryId)
    {
        return _context.Posts
            .Include(p => p.PostComments)
            .Include(p => p.User)
            .Include(p => p.Category)
            .Include(p => p.PostReactions)
            .Where(p => p.CategoryId == categoryId)
            .ToList();
    }

    public bool Like(int userId, int postId)
    {
        try
        {
            _context.PostReactions.Add(new PostReaction
            {
                UserId = userId,
                PostId = postId
            });
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<Category> GetAllCategories()
    {
        return _context.Categories.ToList();
    }

    public List<Post> GetPostsByFilter(string filter)
    {
        return _context.Posts
            .Include(p => p.PostComments)
            .Include(p => p.User)
            .Include(p => p.Category)
            .Include(p => p.PostReactions)
            .Where(p => p.Title.Contains(filter) || p.Tags.Contains(filter) || p.Content.Contains(filter))
            .ToList();
    }

    public List<PostComment> GetAllComments()
    {
        return _context.PostComments
            .Include(c => c.Post)
            .Include(c => c.User)
            .ToList();
    }

    public List<PostReaction> GetAllReactions()
    {
        return _context.PostReactions
            .Include(c => c.Post)
            .Include(c => c.User)
            .ToList();
    }

    public bool DeleteComment(int commentId)
    {
        try
        {
            var comment = _context.PostComments
                .SingleOrDefault(c => c.CommentId == commentId);

            if (comment != null)
            {
                _context.PostComments.Remove(comment);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public bool AddCategory(Category category)
    {
        try
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
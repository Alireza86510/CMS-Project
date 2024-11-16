using CMS.DataLayer.Models;

namespace CMS.Core.Services.Interfaces;

public interface IPostService
{
    List<Post> GetAllPosts();
    List<Post> GetPopularPosts();
    List<Post> GetPostsByFilter(string filter);
    List<Category> GetAllCategories();
    List<PostComment> GetAllComments();
    List<PostReaction> GetAllReactions();
    Post GetPostById(int postId);
    List<Post> GetPostByCategory(int categoryId);
    List<Post> GetPostByAuthorId(int authorId);
    List<PostComment> GetPostCommentsByPostId(int postId);
    int GetPostReactionsByPostId(int postId);
    bool Comment(int userId,int postId,string message);
    bool Like(int userId,int postId);
    bool DeleteComment(int commentId);
    bool AddPost(Post post);
    bool AddCategory(Category category);
    bool UpdatePost(Post post);
}
using CMS.Core.Services.Interfaces;
using CMS.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Core.Services.Implements
{
    public class AdminService : IAdminService
    {
        private CmsContext _context;

        public AdminService(CmsContext context)
        {
            _context = context;
        }

        public Tuple<int, int, int, int> GetDashboardData()
        {
            int users = _context.Users.Count();
            int posts = _context.Posts.Count();
            int comments = _context.PostComments.Count();
            int reactions = _context.PostReactions.Count();
            return Tuple.Create(users, posts, comments, reactions);
        }
    }
}

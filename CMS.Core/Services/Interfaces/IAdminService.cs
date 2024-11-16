using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Core.Services.Interfaces
{
    public interface IAdminService
    {
        Tuple<int, int, int, int> GetDashboardData();
    }
}

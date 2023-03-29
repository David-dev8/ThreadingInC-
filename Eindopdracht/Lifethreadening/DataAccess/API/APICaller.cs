using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.API
{
    public abstract class APICaller
    {
        protected APIHelper _apiHandler = new APIHelper();
    }
}

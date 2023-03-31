using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.API
{
    /// <summary>
    /// This class is used to call API's
    /// </summary>
    public abstract class APICaller
    {
        protected APIHelper _apiHandler = new APIHelper();
    }
}

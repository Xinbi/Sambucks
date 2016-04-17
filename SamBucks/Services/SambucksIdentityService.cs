using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sambucks.Services
{
    public class SambucksIdentityService : ISambucksIdentityService
    {
        public string CurrentUser
        {
            get
            {
#if DEBUG
                return "jscheuerman";
#else
        return Thread.CurrentPrincipal.Identity.Name;
#endif
            }
        }
    }
}
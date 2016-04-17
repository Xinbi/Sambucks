using System;
using System.Collections.Generic;
using System.Linq;

namespace Sambucks.Services
{
    public interface ISambucksIdentityService
    {
        string CurrentUser { get; }
    }
}

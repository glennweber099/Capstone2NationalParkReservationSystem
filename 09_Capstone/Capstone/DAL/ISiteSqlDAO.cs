using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISiteSqlDAO
    {
        IList<Site> TopFiveSites(int campgroundId, DateTime arrival, DateTime departure);
    }
}

using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampgroundSqlDAO
    {
        IList<Campground> GetCampgrounds();
        bool CheckAvailability(int campgroundId, DateTime arrival, DateTime departure);

    }
}

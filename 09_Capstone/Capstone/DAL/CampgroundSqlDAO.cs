using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    class CampgroundSqlDAO : ICampgroundSqlDAO
    {
        public bool CheckAvailability(int campgroundId, DateTime arrival, DateTime departure)
        {
            throw new NotImplementedException();
        }

        public IList<Campground> GetCampgrounds()
        {
            throw new NotImplementedException();
        }
    }
}

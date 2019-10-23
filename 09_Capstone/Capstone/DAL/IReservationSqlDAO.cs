using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationSqlDAO
    {
        bool MakeReservation(int siteId, string name);

    }
}

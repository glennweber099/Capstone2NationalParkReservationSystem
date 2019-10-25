using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IParkSqlDAO
    {
        IList<Park> GetParks(); 
        Park GetPark(int parkId);
    }
}

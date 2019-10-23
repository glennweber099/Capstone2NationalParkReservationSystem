using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IParkSqlDAO
    {
        IList<Park> GetParks(); //Possible change to list of strings if we only use it when displaying the names in the menu
        int SelectPark(int parkId);
        Park GetPark(int parkId);
    }
}

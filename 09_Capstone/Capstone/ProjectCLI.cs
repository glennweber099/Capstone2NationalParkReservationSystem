using Capstone.DAL;
using Capstone.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone
{
    public class ProjectCLI
    {
        private IParkSqlDAO parkDAO;
        private ICampgroundSqlDAO campgroundDAO;
        private ISiteSqlDAO siteDAO;
        private IReservationSqlDAO reservationDAO;
        private Dictionary<int, Park> parksDict= new Dictionary<int, Park>();
        

        public ProjectCLI(IParkSqlDAO parkDAO, ICampgroundSqlDAO campgroundDAO, ISiteSqlDAO siteDAO, IReservationSqlDAO reservationDAO)
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        public void RunCLI()
        {
            IList<Park> parks = parkDAO.GetParks();
            for (int i = 0; i < parks.Count; i++)
            {
                parksDict.Add(i + 1, parks[i]);
            }
            ViewParksInterface();
        }

        private void ViewParksInterface()
        {
            while (true)
            {
                Console.WriteLine("Select a park for further details");
                IList<Park> parks = parkDAO.GetParks();
                for (int i = 0; i < parks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {parks[i].Name}");
                }
                Console.WriteLine("Q) quit");

                string selection = Console.ReadLine();
                if (selection.Trim().ToLower() == "q")
                {
                    return;
                }

                bool selectionIsInt = int.TryParse(selection, out int parkSelection);

                if (selectionIsInt && parksDict.ContainsKey(parkSelection))
                {
                    Console.Clear();
                    ParkInformationScreen(parkSelection);
                }
                else
                {
                    Console.WriteLine("Please enter a valid selection.");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
            }
        }

        private void ParkInformationScreen(int selection)
        {
            Console.WriteLine(parksDict[selection].Name);
            Console.WriteLine($"Location: {parksDict[selection].Location}");
            Console.WriteLine($"Established: {parksDict[selection].EstablishDate}");
            Console.WriteLine($"Area: {parksDict[selection].Area}");
            Console.WriteLine($"Annual Visitors: {parksDict[selection].Visitors}");
            Console.WriteLine();
            Console.WriteLine($"{parksDict[selection].Description}");

            SelectACommand(selection);
        }

        private void SelectACommand(int selection)
        {
            while (true)
            {
                Console.WriteLine("1) View Campgrounds");
                Console.WriteLine("2) Return to Previous Screen");
                Console.WriteLine();
                string userSelection = Console.ReadLine().Trim();

                if (userSelection == "1")
                {
                    GetCampgrounds(selection);
                }
                else if (userSelection == "2")
                {
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid selection.");
                    Console.ReadLine();
                    Console.Clear();
                    ParkInformationScreen(selection);
                }
            }
        }

        private void GetCampgrounds(int parkId)
        {

        }
    }
}

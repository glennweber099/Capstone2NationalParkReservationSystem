using Capstone.DAL;
using Capstone.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone
{
    public class MenuCLI
    {
        private IParkSqlDAO parkDAO;
        private ICampgroundSqlDAO campgroundDAO;
        private ISiteSqlDAO siteDAO;
        private IReservationSqlDAO reservationDAO;
        private Dictionary<int, Park> parksDict= new Dictionary<int, Park>();
        public int parkId { get; set; }

        public MenuCLI(IParkSqlDAO parkDAO, ICampgroundSqlDAO campgroundDAO, ISiteSqlDAO siteDAO, IReservationSqlDAO reservationDAO)
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
                    Environment.Exit(0);
                }

                bool selectionIsInt = int.TryParse(selection, out int parkSelection);

                if (selectionIsInt && parksDict.ContainsKey(parkSelection))
                {
                    parkId = parkSelection;
                    Console.Clear();
                    ParkInformationScreen();
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

        private void ParkInformationScreen()
        {
            Console.WriteLine(parksDict[parkId].Name);
            Console.WriteLine($"Location: {parksDict[parkId].Location}");
            Console.WriteLine($"Established: {parksDict[parkId].EstablishDate}");
            Console.WriteLine($"Area: {parksDict[parkId].Area}");
            Console.WriteLine($"Annual Visitors: {parksDict[parkId].Visitors}");
            Console.WriteLine();
            Console.WriteLine($"{parksDict[parkId].Description}");
            Console.WriteLine();
            SelectACommand(1);
        }

        private void SelectACommand(int commandOption)
        {
            while (true)
            {
                if (commandOption == 1)
                {
                    Console.WriteLine("1) View Campgrounds");
                }
                else if (commandOption == 2)
                {
                    Console.WriteLine("1) Search for Available Reservation");
                }
                Console.WriteLine("2) Return to Previous Screen");
                Console.WriteLine();
                string userSelection = Console.ReadLine().Trim();

                if (userSelection == "1" && commandOption == 1)
                {
                    GetCampgrounds(parkId);
                }
                else if (userSelection == "2" && commandOption == 1)
                {
                    Console.Clear();
                    ViewParksInterface();
                }
                else if (commandOption == 1 && userSelection != "1" && userSelection != "2")
                {
                    Console.WriteLine("Please enter a valid selection.");
                    Console.ReadLine();
                    Console.Clear();
                    ParkInformationScreen();
                }

                else if (userSelection == "1" && commandOption == 2)
                {
                    CheckAvailability(commandOption);
                }
                else if (userSelection == "2" && commandOption == 2)
                {
                    Console.Clear();
                    ParkInformationScreen();
                }
                else if (commandOption == 2 && userSelection != "1" && userSelection != "2")
                {
                    Console.WriteLine("Please enter a valid selection.");
                    Console.ReadLine();
                    Console.Clear();
                    ParkInformationScreen();
                }
            }
        }

        private void GetCampgrounds(int parkId)
        {
            Console.Clear();
            Console.WriteLine("Park Campgrounds");
            Console.WriteLine($"{parksDict[parkId].Name} National Park Campgrounds");
            Console.WriteLine();
            Console.WriteLine($"    {"Name", -35}{"Open",-15}{"Close",-15}{"Daily Fee",-15}");
            IList<Campground> camps = campgroundDAO.GetCampgrounds(parkId);
            for (int i = 0; i < camps.Count; i++)
            {
                Console.WriteLine($"#{i + 1} {Convert.ToString(camps[i].Name),-35} {Convert.ToString(camps[i].OpenMonth),-15} {Convert.ToString(camps[i].CloseMonth),-15} {(camps[i].DailyFee).ToString("C")}");
            }
            Console.WriteLine();
            SelectACommand(2);
        }

        private void CheckAvailability(int selection)
        {
            Console.Clear();
            Console.WriteLine("Park Campgrounds");
            Console.WriteLine($"{parksDict[parkId].Name} National Park Campgrounds");
            Console.WriteLine();
            Console.WriteLine($"    {"Name",-35}{"Open",-15}{"Close",-15}{"Daily Fee",-15}");
            IList<Campground> camps = campgroundDAO.GetCampgrounds(parkId);
            for (int i = 0; i < camps.Count; i++)
            {
                Console.WriteLine($"#{i + 1} {Convert.ToString(camps[i].Name),-35} {Convert.ToString(camps[i].OpenMonth),-15} {Convert.ToString(camps[i].CloseMonth),-15} {(camps[i].DailyFee).ToString("C")}");
            }
            Console.WriteLine();

            Console.WriteLine("Which campground (enter 0 to cancel)?_____");
            string campgroundNumber = Console.ReadLine().Trim();
            Console.WriteLine("What is the arrival date? __/__/____");
            string arrivalDate = Console.ReadLine().Trim();
            Console.WriteLine("What is the departure date? __/__/____");
            string departureDate = Console.ReadLine().Trim();
        }
    }
}

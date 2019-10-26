﻿using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Globalization;


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
                    GetCampgrounds(parkId);
                }
            }
        }
        
        private void GetCampgrounds(int parkId)
        {
            Console.Clear();
            Console.WriteLine("Park Campgrounds");
            Console.WriteLine($"{parksDict[parkId].Name} National Park Campgrounds");
            Console.WriteLine();
            Console.WriteLine($"{"",-18}{"Name", -35}{"Open",-16}{"Close",-16}{"Daily Fee",-15}");
            IList<Campground> camps = campgroundDAO.GetCampgrounds(parkId);
            for (int i = 0; i < camps.Count; i++)
            {
                Console.WriteLine($"#{Convert.ToString(camps[i].Id),-15} {Convert.ToString(camps[i].Name),-35} {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(camps[i].OpenMonth),-15} {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(camps[i].CloseMonth),-15} {(camps[i].DailyFee).ToString("C")}");
            }
            Console.WriteLine();
            SelectACommand(2);
        }

        private void CheckAvailability(int selection)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Park Campgrounds");
                Console.WriteLine($"{parksDict[parkId].Name} National Park Campgrounds");
                Console.WriteLine();
                Console.WriteLine($"{"",-18}{"Name",-35}{"Open",-16}{"Close",-16}{"Daily Fee",-15}");
                IList<Campground> camps = campgroundDAO.GetCampgrounds(parkId);
                for (int i = 0; i < camps.Count; i++)
                {
                    Console.WriteLine($"#{Convert.ToString(camps[i].Id), -15} {Convert.ToString(camps[i].Name),-35} {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(camps[i].OpenMonth),-15} {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(camps[i].CloseMonth),-15} {(camps[i].DailyFee).ToString("C")}");
                }
                Console.WriteLine();

                Console.WriteLine("Which campground (enter 0 to cancel)?__");
                int campgroundNumber = -1;
                bool isANumber = int.TryParse(Console.ReadLine(), out campgroundNumber);
                //int campgroundNumber = Convert.ToInt32(Console.ReadLine().Trim());
                if (!isANumber)
                {
                    campgroundNumber = -1;
                }
                if (campgroundNumber == 0)
                {
                    Console.Clear();
                    GetCampgrounds(parkId);
                }
                if ((campgroundNumber != camps[0].Id && campgroundNumber != camps[1].Id && campgroundNumber != camps[2].Id) || campgroundNumber == -1)
                {
                    Console.WriteLine("Please enter a valid option");
                    Console.ReadLine();
                    Console.Clear();
                    GetCampgrounds(parkId);
                }
                // campgroundNumber = camps[campgroundNumber - 1].Id; // this should work
                Console.WriteLine("What is the arrival date? MM/DD/YYYY");
                string arrivalDate = Console.ReadLine().Trim();
                bool isAnArrivalDate = DateTime.TryParse(arrivalDate, out DateTime actualArrivalDate);

                Console.WriteLine("What is the departure date? MM/DD/YYYY");
                string departureDate = Console.ReadLine().Trim();
                bool isADepartureDate = DateTime.TryParse(departureDate, out DateTime actualDepartureDate);

                int totalDays = 0;
                if (isAnArrivalDate && isADepartureDate)
                {
                    totalDays = (actualDepartureDate - actualArrivalDate).Days;
                }
                if (totalDays <= 0)
                {
                    Console.WriteLine("Please enter a valid time frame.");
                    Console.ReadLine();
                    continue;
                }

                Console.Clear();
                TopFiveSites(campgroundNumber, arrivalDate, departureDate, totalDays);
            }
        }

        private void TopFiveSites(int campgroundId, string arrival, string departure, int totalDays)
        {
            while (true)
            {
                Console.WriteLine("Results Matching Your Search Criteria");
                Console.WriteLine($"{"Site No.",-15}{"Max Occup.",-15}{"Accessible?",-15}{"Max RV Length",-15}{"Utility",-15}{"Cost",-15}");
                IList<Site> topFiveSites = siteDAO.TopFiveSites(campgroundId, Convert.ToDateTime(arrival), Convert.ToDateTime(departure));
                for (int i = 0; i < topFiveSites.Count; i++)
                {
                    Console.WriteLine($"{ Convert.ToString(topFiveSites[i].Id),-15}{ Convert.ToString(topFiveSites[i].MaxOccupancy),-15}{((topFiveSites[i].IsAccessible) == false ? "No" : "Yes"),-15}{ Convert.ToString(topFiveSites[i].MaxRVLength),-15}{ ((topFiveSites[i].HasUtilities) == false ? "No" : "Yes"),-15}{(topFiveSites[i].DailyFee * totalDays).ToString("C")}");
                }
                Console.WriteLine();
                Console.WriteLine("Which site should be reserved (enter 0 to cancel)?__");
                bool isANumber = int.TryParse(Console.ReadLine(), out int siteNumber);
                if (!isANumber)
                {
                    Console.WriteLine("Please enter a valid option");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                if (siteNumber == 0)
                {
                    Console.Clear();
                    GetCampgrounds(parkId);
                }
                bool containsId = false;
                foreach (Site site in topFiveSites)
                {
                    if (site.Id == siteNumber)
                    {
                        containsId = true;
                    }
                }
                if (containsId)
                {
                    Console.WriteLine("What name should the reservation be made under?");
                    string name = Console.ReadLine().Trim();
                    Console.Clear();
                    MakeReservation(siteNumber, name, arrival, departure);
                }
                else
                {
                    Console.WriteLine("Please enter a valid option");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
            }
        }  

        private void MakeReservation(int siteNumber, string name, string arrival, string departure)
        {
            Reservation reservation = new Reservation();
            reservation.SiteId = siteNumber;
            reservation.Name = name;
            reservation.FromDate = Convert.ToDateTime(arrival);
            reservation.ToDate = Convert.ToDateTime(departure);
            reservation.Id = reservationDAO.MakeReservation(reservation);
            Console.WriteLine($"The reservation has been made and the confirmation id is: {reservation.Id}");
            Console.ReadLine();
            Console.Clear();
        }
    }
}

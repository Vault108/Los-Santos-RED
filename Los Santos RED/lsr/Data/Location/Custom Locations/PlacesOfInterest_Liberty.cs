﻿using LosSantosRED.lsr.Helper;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public class PlacesOfInterest_Liberty
{
    private PossibleLocations LibertyCityLocations;
    public void DefaultConfig()
    {
        LibertyCityLocations = new PossibleLocations();
        //These are for centered above, remove 200 from height
        DefaultConfig_Other();
        foreach (GameLocation bl in LibertyCityLocations.InteractableLocations())
        {
            bl.AddDistanceOffset(new Vector3(0f, 0f, -200f));
        }
        //These are for regular Centered
        DefaultConfig_PoliceStations();
        DefaultConfig_Hospitals();
        DefaultConfig_Prisons();
        DefaultConfig_GangDens();
        Serialization.SerializeParam(LibertyCityLocations, "Plugins\\LosSantosRED\\AlternateConfigs\\LibertyCity\\Locations_LibertyCity.xml");
        PossibleLocations centeredAbove = LibertyCityLocations.Copy();
        foreach (GameLocation bl in centeredAbove.InteractableLocations())//for centered above we want to add 200 of height
        {
            bl.AddDistanceOffset(new Vector3(0f, 0f, 200f));
        }
        Serialization.SerializeParam(centeredAbove, "Plugins\\LosSantosRED\\AlternateConfigs\\LibertyCity\\Locations_LibertyCityCenteredAbove.xml");
    }

    private void DefaultConfig_Other()
    {
        //ALL THESE NEED -200 to the Z for the centeres
        Bank BOL1 = new Bank(new Vector3(225.23f, 14.52f, 215.37f), 177.45f, "Bank Of Liberty", "Bleeding you dry","BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20};
        LibertyCityLocations.Banks.Add(BOL1);

        Bank BOL2 = new Bank(new Vector3(1388.10f, 259.31f, 223.59f), 0.02f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL2);

        Bank BOL3 = new Bank(new Vector3(1431.68f, 1137.02f, 238.78f), 90.68f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL3);

        Bank BOL4 = new Bank(new Vector3(323.26f, 1012.09f, 213.23f), 270.99f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL4);

        Bank BOL5 = new Bank(new Vector3(83.71f, 318.04f, 214.76f), 358.65f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL5);

        Bank BOL6 = new Bank(new Vector3(251.77f, -138.63f, 215.19f), 129.22f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL6);

        Bank BOL7 = new Bank(new Vector3(-11.50f, 1863.38f, 225.02f), 90.65f, "Bank Of Liberty", "Bleeding you dry", "BOL") { StateID = StaticStrings.LibertyStateID, OpenTime = 6, CloseTime = 20 };
        LibertyCityLocations.Banks.Add(BOL7);

        List<DeadDrop> DeadDropList_4 = new List<DeadDrop>() {
            new DeadDrop() {
                Name = "Dead Drop",
                Description = "the dumpster behind the Pay'n'Spray in Outlook",
                EntrancePosition = new Vector3(1335.14f, 183.48f, 221.42f),
                EntranceHeading = 98.63f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },

            new DeadDrop() {
                Name = "Dead Drop",
                Description = "in between two dumpsters in the alley behind the Modo, in North Holland",
                EntrancePosition = new Vector3(-56.96f,1874.73f,220.24f),
                EntranceHeading = 84.76f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },

            new DeadDrop() {
                Name = "Dead Drop",
                Description = "under one of the big trees in the Colony Island graveyard parking lot",
                EntrancePosition = new Vector3(723.1f,781.87f,208.56f),
                EntranceHeading = 307.34f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },

            new DeadDrop() {
                Name = "Dead Drop",
                Description = "trash can near the globe in Meadows Park",
                EntrancePosition = new Vector3(1698.11f,1023.4f,229.36f),
                EntranceHeading = 324.12f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },

            new DeadDrop() {
                Name = "Dead Drop",
                Description = "the dumpster behind Burger Shot in Industrial Bohan",
                EntrancePosition = new Vector3(1368.7f,2080.5f,216.71f),
                EntranceHeading = 133.86f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },

            new DeadDrop() {
                Name = "Dead Drop",
                Description = "under one of the seats in Feldspar Subway Station",
                EntrancePosition = new Vector3(-86.15f,117.16f,192.01f),
                EntranceHeading = 214.98f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },

            new DeadDrop() {
                Name = "Dead Drop",
                Description = "under the tires in Westdyke's abandoned mansion",
                EntrancePosition = new Vector3(-726.68f,2394.37f,222.51f),
                EntranceHeading = 161.33f,
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = false,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },

        };
        LibertyCityLocations.DeadDrops.AddRange(DeadDropList_4);
        List<ScrapYard> ScrapYardList_5 = new List<ScrapYard>() {
            new ScrapYard() {
                Name = "Rusty Schit Salvage",
                Description = "We'll take it from you",
                EntrancePosition = new Vector3(-231.24f, 2242.77f, 208.75f),
                EntranceHeading = 117.67f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ScrapYard() {
                Name = "Dukes Used Auto Parts",
                Description = "We buy and sell auto junk",
                EntrancePosition = new Vector3(1283.5f, 1019.19f, 224.47f),
                EntranceHeading = 181.02f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ScrapYard() {
                Name = "Hairy Al's Scrap",
                Description = "Cars bought - parts sold",
                EntrancePosition = new Vector3(1676.12f,230.44f,217.19f),
                EntranceHeading = 222.11f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.ScrapYards.AddRange(ScrapYardList_5);
        List<CarCrusher> CarCrusherList_6 = new List<CarCrusher>() {
            new CarCrusher() {
                Name = "Colony Crusher",
                Description = "Dead skunk in the trunk?",
                EntrancePosition = new Vector3(786.45f, 700.37f, 208.73f),
                EntranceHeading = 90.32f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.CarCrushers.AddRange(CarCrusherList_6);
        List<GunStore> GunStoreList_8 = new List<GunStore>() {
            new GunStore() {
                MenuID = "GunShop1",
                Name = "Choppy Shop",
                Description = "GRAH GRAH BOOM",
                EntrancePosition = new Vector3(1295.57f, 580.55f, 234.26f),
                EntranceHeading = 90.56f,
                OpenTime = 6,
                CloseTime = 20,
                IsEnabled = true, 
                ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(1284.30f, 550.90f, 233.12f),181.34f),
                },
                StateID = StaticStrings.LibertyStateID,
            },
            new GunStore() {
                MenuID = "GunShop1",
                Name = "Blicky Bodega",
                Description = "Protect yourself from the opposition",
                EntrancePosition = new Vector3(311.1f, 151.3f, 211.16f),
                EntranceHeading = 86.28f,
                OpenTime = 6,
                CloseTime = 20,
                IsEnabled = true,
                ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(332.38f, 148.26f, 213.98f),177.80f),
                },
                StateID = StaticStrings.LibertyStateID,
            },
            new GunStore() {
                MenuID = "GunShop1",
                Name = "Knocks Central",
                Description = "Essential business",
                EntrancePosition = new Vector3(-1501.22f,846.25f,225.45f),
                EntranceHeading = 234.43f,
                OpenTime = 6,
                CloseTime = 20,
                IsEnabled = true,
                ContactName = StaticStrings.UndergroundGunsContactName,
                ParkingSpaces = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-1499.26f, 840.65f, 225.13f),329.66f),
                },
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.GunStores.AddRange(GunStoreList_8);
        List<Hotel> HotelList_9 = new List<Hotel>() {
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "The Majestic",
                Description = "The best in town, unrivaled",
                EntrancePosition = new Vector3(71.9f, 1102.72f, 215.02f),
                EntranceHeading = 355.64f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "The Star Plaza Hotel",
                Description = "Sleep like a star",
                EntrancePosition = new Vector3(-47.83f,987.4f,214.78f),
                EntranceHeading = 264.06f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Banner Hotel",
                Description = "Luxury frachised",
                EntrancePosition = new Vector3(64.25f,891.57f,214.82f),
                EntranceHeading = 179.24f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "The Nicoise Hotel",
                Description = "5 star hotel for 1 star people",
                EntrancePosition = new Vector3(263.52f,834.14f,214.7f),
                EntranceHeading = 268.48f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Grand Northumbrian",
                Description = "The spot to crash out after being robbed in Mirror Park",
                EntrancePosition = new Vector3(208.61f,1452.78f,214.93f),
                EntranceHeading = 89.19f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Castle Gardens Hotel",
                Description = "Resort experience in the heart of Liberty",
                EntrancePosition = new Vector3(127.6f,-406.53f,205.52f),
                EntranceHeading = 357.78f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "ExpensiveHotelMenu",
                Name = "Hotel Hamilton",
                Description = "Top in the trash",
                EntrancePosition = new Vector3(-75.54f,1900.84f,215.45f),
                EntranceHeading = 0.81f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "CheapHotelMenu",
                Name = "Beach Hotel",
                Description = "Enjoy the smell of the polluted ocean",
                EntrancePosition = new Vector3(1208.62f, -86.64f, 214.31f),
                EntranceHeading = 358.44f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Hotel() {
                MenuID = "CheapHotelMenu",
                Name = "Berchem Hotel",
                Description = "Stay in the heart of Alderney",
                EntrancePosition = new Vector3(-1226.15f, 1160.89f, 220.11f),
                EntranceHeading = 240.01f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },
        };
        LibertyCityLocations.Hotels.AddRange(HotelList_9);
        List<Residence> ResidenceList_10 = new List<Residence>() {
            new Residence() {
                Name = "370 Galveston Ave",
                EntrancePosition = new Vector3( - 201.89f, 1883.25f, 216.45f),
                EntranceHeading = 90.35f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 2000,
                PurchasePrice = 400000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "1666 Valdez St",
                EntrancePosition = new Vector3(910.86f, 2233.65f, 237.76f),
                EntranceHeading = 180.02f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1300,
                PurchasePrice = 260000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "Northern Gardens Apt 248",
                EntrancePosition = new Vector3(1307.97f, 2349.85f, 212.87f),
                EntranceHeading = 176.59f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1200,
                PurchasePrice = 240000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "50 Shinnecock Ave",
                EntrancePosition = new Vector3(1561.38f, -308.4f, 208.42f),
                EntranceHeading = 267.59f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 3750,
                PurchasePrice = 750000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "35 Iroquois Ave",
                EntrancePosition = new Vector3(1087.62f, 221.67f, 216.97f),
                EntranceHeading = 84.59f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1000,
                PurchasePrice = 180000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "216 Stillwater Ave",
                EntrancePosition = new Vector3(1827.85f, 1061.8f, 230.79f),
                EntranceHeading = 86.2f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 3000,
                PurchasePrice = 600000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "Steinway Projects Apt 98",
                EntrancePosition = new Vector3(1453.92f, 1347.01f, 236.52f),
                EntranceHeading = 222.8f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1200,
                PurchasePrice = 240000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "560 Eastwood Apartments",
                EntrancePosition = new Vector3(735.87f, 572.61f, 208.73f),
                EntranceHeading = 269.21f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1500,
                PurchasePrice = 300000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "3250 Bismarck Ave",
                EntrancePosition = new Vector3(266.08f, 1619.3f, 214.66f),
                EntranceHeading = 269.14f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 2500,
                PurchasePrice = 500000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "1090 Borlock Rd",
                EntrancePosition = new Vector3(442.06f, 28.18f, 211.46f),
                EntranceHeading = 269.36f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1750,
                PurchasePrice = 350000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "511 Feldspar St",
                EntrancePosition = new Vector3(83.69f, 271.23f, 213.47f),
                EntranceHeading = 179.24f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1875,
                PurchasePrice = 375000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "130 Columbus Ave Penthouse",
                EntrancePosition = new Vector3(152.75f, 669.86f, 215.47f),
                EntranceHeading = 272.89f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 5000,
                PurchasePrice = 1000000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "32 Panhandle Rd",
                EntrancePosition = new Vector3( - 968.8f, 1634.06f, 216.7f),
                EntranceHeading = 1.64f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1000,
                PurchasePrice = 190000,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Residence() {
                Name = "9 Cariboo Ave",
                EntrancePosition = new Vector3( - 930.59f, 2028.32f, 230.77f),
                EntranceHeading = 178.75f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 2875,
                PurchasePrice = 575000,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Residence() {
                Name = "1396 Red Wing Ave",
                EntrancePosition = new Vector3( - 1760.63f, 470.83f, 207.5f),
                EntranceHeading = 358.88f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1100,
                PurchasePrice = 220000,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Residence() {
                Name = "46 Babbage Dr",
                EntrancePosition = new Vector3( - 1276.63f, 778.13f, 215.98f),
                EntranceHeading = 266.5f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 28,
                RentalFee = 1900,
                PurchasePrice = 380000,
                StateID = StaticStrings.AlderneyStateID,
            },




            new Residence() {
                Name = "Firefly Projects Apt 153",
                EntrancePosition = new Vector3(1475.07f,68.07f,216.66f),
                EntranceHeading = 215.67f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 1100,
                PurchasePrice = 220000,
                SalesPrice = 120000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "Greg Johnson Projects Apt 2235",
                EntrancePosition = new Vector3(117.59f,1993.17f,219.02f),
                EntranceHeading = 220.4f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 1300,
                PurchasePrice = 260000,
                SalesPrice = 160000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "722 Savannah Ave",
                EntrancePosition = new Vector3(1621.41f,1029.14f,233.11f),
                EntranceHeading = 270.25f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 3100,
                PurchasePrice = 620000,
                SalesPrice = 520000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "480 Pyrite St",
                EntrancePosition = new Vector3(-246.27f,1334.89f,211.02f),
                EntranceHeading = 357.57f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 2100,
                PurchasePrice = 420000,
                SalesPrice = 320000,
                StateID = StaticStrings.LibertyStateID,
            },
            new Residence() {
                Name = "85 Mohawk Ave",
                EntrancePosition = new Vector3(1153.67f,675.45f,236.45f),
                EntranceHeading = 267.69f,
                OpenTime = 0,
                CloseTime = 24,
                RentalDays = 14,
                RentalFee = 1800,
                PurchasePrice = 360000,
                SalesPrice = 260000,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Residences.AddRange(ResidenceList_10);
        List<CityHall> CityHallList_11 = new List<CityHall>() {
            new CityHall() {
                Name = "Civic Citadel",
                Description = "City hall",
                EntrancePosition = new Vector3(59.02f, 148.46f, 217.84f),
                EntranceHeading = 175.24f,
                OpenTime = 9,
                CloseTime = 18,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.CityHalls.AddRange(CityHallList_11);
        List<Restaurant> RestaurantList_12 = new List<Restaurant>() {
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                Name = "Superstar Cafe",
                Description = "The hotspot to not get shit done",
                EntrancePosition = new Vector3(3.17f, 533.78f, 214.71f),
                EntranceHeading = 176.29f,
                OpenTime = 6,
                CloseTime = 20,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                Name = "Superstar Cafe",
                Description = "The hotspot to not get shit done",
                EntrancePosition = new Vector3(265.89f, 1474.1f, 214.66f),
                EntranceHeading = 265.13f,
                OpenTime = 6,
                CloseTime = 20,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },

            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(1276.44f,620.02f,232.89f),
                EntranceHeading = 182.21f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(2032.61f,1124.75f,229.01f),
                EntranceHeading = 175.34f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(349.13f,535.55f,214.82f),
                EntranceHeading = 266.82f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(118.18f,981.08f,208.72f),
                EntranceHeading = 177.67f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeanMachineMenu",
                BannerImagePath = "stores\\beanmachine.png",
                Name = "Bean Machine Coffee",
                Description = "Taking over the world one Gunkaccino at a time",
                EntrancePosition = new Vector3(114.61f,535.91f,214.81f),
                EntranceHeading = 89.48f,
                OpenTime = 0,
                CloseTime = 24,
                TypeName = "Coffee Shop",
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "CoffeeMenu",
                Name = "Coffee Shop",
                Description = "Pit-stop after the highway, or before",
                EntrancePosition = new Vector3(-977.69f, 1285.32f, 219.79f),
                EntranceHeading = 177.38f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "69th Street Diner",
                Description = "Who could resist its greasy allure?",
                EntrancePosition = new Vector3(1122.55f, 12.29f, 215.88f),
                EntranceHeading = 7.14f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "Creek St Diner",
                Description = "Even greasier than the one at 69th",
                EntrancePosition = new Vector3(1184.81f, 937.35f, 216.03f),
                EntranceHeading = 348.6f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "WigwamMenu",
                Name = "Wigwam",
                BannerImagePath = "stores\\wigwam.png",
                Description = "No need for reservations",
                EntrancePosition = new Vector3(-1327.56f, 1077.6f, 225.44f),
                EntranceHeading = 2.22f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(52.45f, 781.2f, 214.77f),
                EntranceHeading = 88.05f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(-203.4f, 1687.77f, 213.05f),
                EntranceHeading = 95.42f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(1338.58f, 2088.76f, 216.9f),
                EntranceHeading = 40.91f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(1890.87f, 719.66f, 225.21f),
                EntranceHeading = 269.4f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(680.63f, 2009.66f, 216.27f),
                EntranceHeading = 23.34f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(-379.51f, 640.95f, 204.81f),
                EntranceHeading = 358.48f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BurgerShotMenu",
                Name = "Burger Shot",
                Description = "Kill your hunger! It's bleedin' tasty",
                EntrancePosition = new Vector3(-767.85f, 2107.42f, 224.31f),
                EntranceHeading = 180.48f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "CluckinBellMenu",
                Name = "Cluckin' Bell",
                Description = "Taste the cock",
                EntrancePosition = new Vector3(1429.49f, 868.19f, 225.1f),
                EntranceHeading = 2.2f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "CluckinBellMenu",
                Name = "Cluckin' Bell",
                Description = "Taste the cock",
                EntrancePosition = new Vector3(101.04f, 566.46f, 214.8f),
                EntranceHeading = 103.41f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "60 Diner",
                Description = "Serving quality food since 1960",
                EntrancePosition = new Vector3(351.02f, 1134.75f, 214.84f),
                EntranceHeading = 268.24f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "60 Diner",
                Description = "Serving quality food since 1960",
                EntrancePosition = new Vector3(-151.08f, 737.13f, 215.35f),
                EntranceHeading = 356.24f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "FancyGenericMenu",
                Name = "60 Diner",
                Description = "Serving quality food since 1960",
                EntrancePosition = new Vector3(-151.08f,737.13f,215.35f),
                EntranceHeading = 356.24f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaThisMenu",
                Name = "Pizza This...",
                Description = "Get stuffed",
                BannerImagePath = "stores\\pizzathis.png",
                EntrancePosition = new Vector3(-918.7f, 1896.49f, 224.63f),
                EntranceHeading = 176.4f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaThisMenu",
                Name = "Pizza This...",
                Description = "Get stuffed",
                BannerImagePath = "stores\\pizzathis.png",
                EntrancePosition = new Vector3(2059.47f, 1080.04f, 229.08f),
                EntranceHeading = 86.71f,
                OpenTime = 9,
                CloseTime = 23,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "AlDentesMenu",
                Name = "Al Dente's",
                Description = "Just like mama never used to make it",
                BannerImagePath = "stores\\aldentes.png",
                EntrancePosition = new Vector3(2084.34f, 1185.76f, 226.66f),
                EntranceHeading = 86.71f,
                OpenTime = 9,
                CloseTime = 23,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "AlDentesMenu",
                Name = "Al Dente's",
                Description = "Just like mama never used to make it",
                BannerImagePath = "stores\\aldentes.png",
                EntrancePosition = new Vector3(167.1f, -137.1f, 214.76f),
                EntranceHeading = 219.37f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaMenu",
                Name = "Colony Pizza",
                Description = "Colony Island's finest pizza",
                EntrancePosition = new Vector3(667.39f, 589.23f, 208.73f),
                EntranceHeading = 88.96f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaMenu",
                Name = "Drusilla's",
                Description = "Traditionally italian",
                EntrancePosition = new Vector3(118.01f, 237.25f, 212.69f),
                EntranceHeading = 89.29f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "NoodleMenu",
                Name = "Mr. Fuk's Rice Box",
                Description = "Quality korean food by Mr. Fuk himself",
                EntrancePosition = new Vector3(-1003.43f, 1566.3f, 219.78f),
                EntranceHeading = 137.64f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "FancyFishMenu",
                Name = "Fanny Crab's",
                Description = "You'll love the taste of our Fanny Crabs",
                EntrancePosition = new Vector3(207.47f, 428.7f, 215.2f),
                EntranceHeading = 88.77f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "InternetCafeMenu",
                Name = "TW@ Internet Cafe",
                Description = "nu-media caffeine solutions provider",
                EntrancePosition = new Vector3(-1343.86f, 958.28f, 225.44f),
                EntranceHeading = 63.7f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Restaurant() {
                MenuID = "InternetCafeMenu",
                Name = "TW@ Internet Cafe",
                Description = "nu-media caffeine solutions provider",
                EntrancePosition = new Vector3(-101.45f, 1886.24f, 212.92f),
                EntranceHeading = 88.88f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "InternetCafeMenu",
                Name = "TW@ Internet Cafe",
                Description = "nu-media caffeine solutions provider",
                EntrancePosition = new Vector3(1213.59f, 324.08f, 224.19f),
                EntranceHeading = 280.19f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "PizzaMenu",
                Name = "Drusilla's",
                Description = "Traditionally italian",
                EntrancePosition = new Vector3(118.01f, 237.25f, 212.69f),
                EntranceHeading = 89.29f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "NoodleMenu",
                Name = "Delicious Chinese Food",
                Description = "Speaks for itself",
                EntrancePosition = new Vector3(1361.27f,1161.68f,238.19f),
                EntranceHeading = 88.88f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Restaurant() {
                MenuID = "BeefyBillsMenu",
                Name = "Meat Factory",
                Description = "Put our meat in your mouth",
                EntrancePosition = new Vector3(-227.99f,1854.46f,217.47f),
                EntranceHeading = 268.31f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Restaurants.AddRange(RestaurantList_12);
        List<Pharmacy> PharmacyList_13 = new List<Pharmacy>() {
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(-96.73f, 645.53f, 214.75f),
                EntranceHeading = 177.06f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(2059.35f, 1098.74f, 228.87f),
                EntranceHeading = 86.71f,
                OpenTime = 9,
                CloseTime = 23,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(1576.89f, 2138.1f, 216.85f),
                EntranceHeading = 265.4f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(-1061.58f, 1323.19f, 219.81f),
                EntranceHeading = 87.01f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Pill Pharm",
                Description = "Big Pharma, big prices",
                EntrancePosition = new Vector3(1481.8f,1223.48f,236.75f),
                EntranceHeading = 262.51f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new Pharmacy() {
                MenuID = "PharmacyMenu",
                Name = "Aycehol",
                Description = "Same pills as anywhere else, but cheaper",
                EntrancePosition = new Vector3(-36.82f, 1770.41f, 224.71f),
                EntranceHeading = 226.09f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Pharmacies.AddRange(PharmacyList_13);
        List<HeadShop> HeadShopList_14 = new List<HeadShop>() {
            new HeadShop() {
                MenuID = "HeadShopMenu",
                Name = "Tobacco Shop",
                Description = "Light it up",
                EntrancePosition = new Vector3(740.33f, 2253.64f, 228.46f),
                EntranceHeading = 0.03f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.HeadShops.AddRange(HeadShopList_14);
        List<HardwareStore> HardwareStoreList_15 = new List<HardwareStore>() {
            new HardwareStore() {
                MenuID = "ToolMenu",
                Name = "Cheapo's Hardware",
                Description = "Affordable tools for tools",
                EntrancePosition = new Vector3(-880.98f, 1866.26f, 224.52f),
                EntranceHeading = 2.11f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new HardwareStore() {
                MenuID = "ToolMenu",
                Name = "BJ Hardware",
                Description = "Blowjobs aren't in the menu",
                EntrancePosition = new Vector3(1007.91f, -23.12f, 206.45f),
                EntranceHeading = 111.11f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new HardwareStore() {
                MenuID = "ToolMenu",
                Name = "Kack's Hardware",
                Description = "Vertified by Mr. Kack himself",
                EntrancePosition = new Vector3(845.51f, 1935.02f, 211.68f),
                EntranceHeading = 358.58f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.HardwareStores.AddRange(HardwareStoreList_15);
        List<PawnShop> PawnShopList_16 = new List<PawnShop>() {
            new PawnShop() {
                MenuID = "PawnShopMenuGeneric",
                Name = "Pawnbrokers",
                Description = "Buying and selling since '81",
                EntrancePosition = new Vector3(831.78f, 2133.7f, 230.31f),
                EntranceHeading = 3.1f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new PawnShop() {
                MenuID = "PawnShopMenuGeneric",
                Name = "Jamaican Pawn Shop",
                Description = "We won't ask where you got it from",
                EntrancePosition = new Vector3(1648.62f, 561.35f, 225.97f),
                EntranceHeading = 270.45f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new PawnShop() {
                MenuID = "PawnShopMenuGeneric",
                Name = "Alderney City Pawn",
                Description = "Find your bargain here",
                EntrancePosition = new Vector3(-1297.42f, 1500.52f, 226.85f),
                EntranceHeading = 88.45f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
        };
        LibertyCityLocations.PawnShops.AddRange(PawnShopList_16);
        List<ConvenienceStore> ConvenienceStoreList_17 = new List<ConvenienceStore>() {
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(-106.51f, 736.58f, 215.16f),
                EntranceHeading = 46.57f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(1243.8f, 79.42f, 216.27f),
                EntranceHeading = 38.36f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(450.18f, -11.98f, 209.43f),
                EntranceHeading = 310.69f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(1241.57f, 2347.29f, 220.01f),
                EntranceHeading = 269.46f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(282.81f, 271.54f, 214.76f),
                EntranceHeading = 177.77f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(1313.52f, 542.68f, 234.08f),
                EntranceHeading = 267.1f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "TwentyFourSevenMenu",
                Name = "24/7",
                Description = "Go all night",
                BannerImagePath = "stores\\247.png",
                EntrancePosition = new Vector3(-99.04f, 1926.12f, 212.71f),
                EntranceHeading = 133.27f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "GasStationMenu",
                Name = "Globe Oil",
                Description = "The pumps might not work, but we'll still sell you a drink",
                EntrancePosition = new Vector3(-240.06f, 299.2f, 207.86f),
                EntranceHeading = 179.28f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "ConvenienceStoreMenu",
                Name = "Checkout!",
                Description = "Shopping for busy people",
                EntrancePosition = new Vector3(-1241.3f, 1887.28f, 213.52f),
                EntranceHeading = 271.59f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(-1409.13f, 768.4f, 219.35f),
                EntranceHeading = 174.93f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(967.88f, 2072.88f, 222.26f),
                EntranceHeading = 270.87f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(1646f, 738.7f, 226.26f),
                EntranceHeading = 358.25f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(-1358.5f, 1255.26f, 225.45f),
                EntranceHeading = 267.96f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Superb Deli",
                Description = "Franchising your beloved corner stores",
                EntrancePosition = new Vector3(-202.13f, 1024.75f, 209.94f),
                EntranceHeading = 358.9f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },

            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(1176.29f,535.71f,227.36f),
                EntranceHeading = 153.44f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "Deli",
                Description = "Fresh food and fresh groceries",
                EntrancePosition = new Vector3(1432.97f,1154.37f,238.7f),
                EntranceHeading = 84.36f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new ConvenienceStore() {
                MenuID = "DeliGroceryMenu",
                Name = "DC's Mini Market",
                Description = "Food, groceries, anything you need",
                EntrancePosition = new Vector3(-790.86f,1845.34f,225.87f),
                EntranceHeading = 87.18f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.AlderneyStateID,
            },
        };
        LibertyCityLocations.ConvenienceStores.AddRange(ConvenienceStoreList_17);
        List<LiquorStore> LiquorStoreList_18 = new List<LiquorStore>() {
            new LiquorStore() {
                MenuID = "LiquorStoreMenu",
                Name = "Liquors",
                Description = "Yes, we sell liquor",
                EntrancePosition = new Vector3(1604.62f, 586.71f, 231.25f),
                EntranceHeading = 64.88f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new LiquorStore() {
                MenuID = "LiquorStoreMenu",
                Name = "Liquors",
                Description = "Yes, we sell liquor",
                EntrancePosition = new Vector3(799.21f, 2179.98f, 229.73f),
                EntranceHeading = 268.5f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.LiquorStores.AddRange(LiquorStoreList_18);
        List<GasStation> GasStationList_19 = new List<GasStation>() {
            new GasStation() {
                MenuID = "GasStationMenu",
                Name = "Globe Oil",
                Description = "Changing the Climate, Powering The Future",
                EntrancePosition = new Vector3(-1171.16f, 527.35f, 207.23f),
                EntranceHeading = 230.76f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },
            new GasStation() {
                MenuID = "XeroMenu",
                Name = "Terroil Station",
                Description = "We sell Xero gasoline though",
                BannerImagePath = "stores\\xero.png",
                EntrancePosition = new Vector3(344.17f, 1613.53f, 214.67f),
                EntranceHeading = 355.4f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(1004.35f, 679.51f, 206.16f),
                EntranceHeading = 234.7f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(1379.89f, 134.58f, 219.33f),
                EntranceHeading = 83.12f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(2025.51f, 1326.49f, 216.58f),
                EntranceHeading = 72.49f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(1691.96f, 2221.8f, 216.83f),
                EntranceHeading = 181.2f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(-1075.94f, 2239.04f, 227.9f),
                EntranceHeading = 177.53f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(-194.07f, 490.83f, 209.97f),
                EntranceHeading = 178.63f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new GasStation() {
                MenuID = "RonMenu",
                Name = "Ron Station",
                Description = "Put RON in your tank",
                BannerImagePath = "stores\\ron.png",
                EntrancePosition = new Vector3(1374.29f, 821.24f, 229.79f),
                EntranceHeading = 80.35f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.GasStations.AddRange(GasStationList_19);
        List<Bar> BarList_20 = new List<Bar>() {
            new Bar() {
                MenuID = "BarMenu",
                Name = "Comrades Bar",
                Description = "Party like it's 1924",
                EntrancePosition = new Vector3(1166.28f, 3.65f, 215.49f),
                EntranceHeading = 91.39f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Hallet's Sports Bar",
                Description = "Perfect for a drink after a funeral",
                EntrancePosition = new Vector3(682.95f, 531.97f, 208.86f),
                EntranceHeading = 60.15f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Lucky Winkles",
                Description = "A classic Purgatory dive bar",
                EntrancePosition = new Vector3(-202.67f, 949.66f, 210.39f),
                EntranceHeading = 42.42f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Jerkov's",
                Description = "Even the elite needs to get drunk",
                EntrancePosition = new Vector3(318.09f, 1052.21f, 218.63f),
                EntranceHeading = 178.83f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Steinway Beer Garden",
                Description = "Take shots and shoot darts",
                EntrancePosition = new Vector3(1388.03f, 1229.34f, 235.4f),
                EntranceHeading = 224.33f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Homebrew Cafe",
                Description = "A piece of Jamaica in dukes",
                EntrancePosition = new Vector3(1713.07f, 548.08f, 225.19f),
                EntranceHeading = 267.78f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },

            new Bar() {
                MenuID = "BarMenu",
                Name = "The Triangle Club",
                Description = "Beautiful women and expensive drinks",
                EntrancePosition = new Vector3(1435.8f,2205.05f,217.48f),
                EntranceHeading = 312.4f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
            new Bar() {
                MenuID = "BarMenu",
                Name = "Honkers",
                Description = "Beautiful women and expensive drinks",
                EntrancePosition = new Vector3(-1342.33f,520.23f,210.02f),
                EntranceHeading = 88.81f,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.AlderneyStateID,
            },

        };
        LibertyCityLocations.Bars.AddRange(BarList_20);
        List<Dealership> DealershipList_21 = new List<Dealership>() {
            new Dealership() {
                MenuID = "PremiumDeluxeMenu",
                Name = "Big Paulie Budget Cars",
                Description = "Something affordable for everyone",
                EntrancePosition = new Vector3(-1414.99f, 425.34f, 206.79f),
                EntranceHeading = 176.06f,
                OpenTime = 6,
                CloseTime = 20,
                LicensePlatePreviewText = "PAULIE",
                VehiclePreviewLocation = new SpawnPlace(new Vector3(-1440.71f, 443.88f, 207.56f), 186.5f),
                CameraPosition = new Vector3(-1437.71f, 436.88f, 209.06f),
                CameraDirection = new Vector3(0.3461686f, 0.9154226f, -0.2053503f),
                VehicleDeliveryLocations = new List<SpawnPlace>() {
                    new SpawnPlace(new Vector3(-1417.26f, 496.72f, 209.71f), 132.01f),
                },
                StateID = StaticStrings.AlderneyStateID,
            },
        };
        LibertyCityLocations.CarDealerships.AddRange(DealershipList_21);
        List<VehicleExporter> ExporterList_94 = new List<VehicleExporter>()
        {
            new VehicleExporter(new Vector3(1024.06f,318.96f,206.09f),154.43f,"East Hook Exports","Deliver Me Timbers","SunshineMenu") { 
                ParkingSpaces = new List<SpawnPlace>() { 
                    new SpawnPlace(new Vector3(994.86f, 277.64f, 205.13f),336.13f),
                    new SpawnPlace(new Vector3(1053.45f, 333.03f, 205.46f),106.87f),
                    new SpawnPlace(new Vector3(1040.43f, 330.67f, 205.48f),277.79f),
                    new SpawnPlace(new Vector3(1009.73f, 304.31f, 205.38f),243.25f),
                    new SpawnPlace(new Vector3(1014.42f, 293.04f, 205.35f),296.14f),
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(692.99f,1586.14f,202.71f),175.41f,"Charge Island Exports","The boats are just a front","SunshineMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(716.16f,1578.42f,202.23f),
                Heading = 298.87f,
                },
                new SpawnPlace() {
                Position = new Vector3(721.59f,1592.13f,202.48f),
                Heading = 178.47f,
                },
                new SpawnPlace() {
                Position = new Vector3(734.77f,1576.18f,202.23f),
                Heading = 269.27f,
                },
                new SpawnPlace() {
                Position = new Vector3(899.49f,1484.39f,202.45f),
                Heading = 88.51f,
                },
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(-79.2f,812.87f,214.82f),88.92f,"Star Junction Chop Shop","Hiding in plain sight, they'll never know","NationalMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(-83.11f,816.82f,214.21f),
                Heading = 89.88f,
                },
                new SpawnPlace() {
                Position = new Vector3(-78.72f,807.16f,214.21f),
                Heading = 269.06f,
                },
                new SpawnPlace() {
                Position = new Vector3(-84.68f,832.47f,214.25f),
                Heading = 178.36f,
                },
                new SpawnPlace() {
                Position = new Vector3(-41.23f,838.85f,214.23f),
                Heading = 0.35f,
                },
                new SpawnPlace() {
                Position = new Vector3(-41.85f,797.67f,214.27f),
                Heading = 180.71f,
                },
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(1183.98f,2049.43f,216.76f),133.07f,"Bohan Chop Shop","Part em out, ship em off","NationalMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(1172.04f,2047.07f,216.12f),
                Heading = 7.04f,
                },
                new SpawnPlace() {
                Position = new Vector3(1177.96f,2050.8f,216.12f),
                Heading = 100.94f,
                },
                new SpawnPlace() {
                Position = new Vector3(1158.17f,2049.91f,216f),
                Heading = 133.73f,
                },
                new SpawnPlace() {
                Position = new Vector3(1191.68f,2041.67f,216.17f),
                Heading = 315.94f,
                },
                new SpawnPlace() {
                Position = new Vector3(1164.95f,1980.8f,216.19f),
                Heading = 345.01f,
                },
                },
                StateID = StaticStrings.LibertyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(-954.33f,2359.07f,206.62f),218.55f,"Alderney Chop Shop","Once a casino, now a chop shop","NationalMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(-967.25f,2345.62f,205.79f),
                Heading = 267.58f,
                },
                new SpawnPlace() {
                Position = new Vector3(-944.4f,2350.9f,205.8f),
                Heading = 320.35f,
                },
                new SpawnPlace() {
                Position = new Vector3(-929.33f,2385.42f,205.91f),
                Heading = 176.78f,
                },
                new SpawnPlace() {
                Position = new Vector3(-987f,2329.23f,205.79f),
                Heading = 0.84f,
                },
                new SpawnPlace() {
                Position = new Vector3(-1062.38f,2333.47f,205.79f),
                Heading = 315.62f,
                },
                },
                StateID = StaticStrings.AlderneyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
            new VehicleExporter(new Vector3(-976.66f,515.11f,204.31f),177.89f,"Port Tudor Exports","From Alderney to anywhere in the world","SunshineMenu") {
                ParkingSpaces = new List<SpawnPlace>() {
                new SpawnPlace() {
                Position = new Vector3(-977.07f,508.83f,203.62f),
                Heading = 89.44f,
                },
                new SpawnPlace() {
                Position = new Vector3(-978.59f,496.64f,203.62f),
                Heading = 0.26f,
                },
                new SpawnPlace() {
                Position = new Vector3(-974.75f,522.03f,203.59f),
                Heading = 359.96f,
                },
                new SpawnPlace() {
                Position = new Vector3(-865.64f,527.79f,203.53f),
                Heading = 180.46f,
                },
                new SpawnPlace() {
                Position = new Vector3(-837.83f,494.61f,203.53f),
                Heading = 89.19f,
                },
                new SpawnPlace() {
                Position = new Vector3(-1044.06f,350.99f,203.54f),
                Heading = 270.36f,
                },
                new SpawnPlace() {
                Position = new Vector3(-949.25f,860.91f,203.51f),
                Heading = 359.81f,
                },
                },
                StateID = StaticStrings.AlderneyStateID,
                ContactName = StaticStrings.VehicleExporterContactName,
                OpenTime = 0,
                CloseTime = 24,
            },
        };
        LibertyCityLocations.VehicleExporters.AddRange(ExporterList_94);

        List<Forger> ForgerList_95 = new List<Forger>()
        {
            new Forger(new Vector3(1163.51f,1076.8f,222.27f), 356.1f, "Dukes Forgeries", "More plates than the DMV")
            {
                CleanPlateSalesPrice = 200,
                WantedPlateSalesPrice = 50,
                CustomPlateCost = 550,
                RandomPlateCost = 300,
                OpenTime = 0,
                CloseTime = 24,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Forgers.AddRange(ForgerList_95);



        List<RepairGarage> RepairGarageList_96 = new List<RepairGarage>()
        {
            new RepairGarage(new Vector3(1296.38f,206.73f,220.67f), 0f, "Native Engines", "The best repair shop in the whole Broker")
            {
                //GarageDoors = new List<InteriorDoor>() { new InteriorDoor(3082692265,new Vector3(-1355.819f,-754.4543f,23.49588f)) },
                CameraPosition = new Vector3(1355.78f,218.95f,231.36f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },

            new RepairGarage(new Vector3(-69.7f,2042.61f,220.27f), 0f, "Auto Cowboys", "Servicing Holland since 1979")
            {
                //GarageDoors = new List<InteriorDoor>() { new InteriorDoor(3082692265,new Vector3(-1355.819f,-754.4543f,23.49588f)) },
                CameraPosition = new Vector3(-113.01f,1998.94f,235.47f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new RepairGarage(new Vector3(-275.82f,868.98f,206.63f), 0f, "Auto Limbo", "Where all the Union Drive accidents go")
            {
                //GarageDoors = new List<InteriorDoor>() { new InteriorDoor(3082692265,new Vector3(-1355.819f,-754.4543f,23.49588f)) },
                CameraPosition = new Vector3(-227f,934.29f,222.85f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new RepairGarage(new Vector3(948.62f,1998.77f,214.84f), 0f, "Muscle Mary's", "We service imports too")
            {
                //GarageDoors = new List<InteriorDoor>() { new InteriorDoor(3082692265,new Vector3(-1355.819f,-754.4543f,23.49588f)) },
                CameraPosition = new Vector3(885.52f,1988.27f,220.1f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.LibertyStateID,
            },
            new RepairGarage(new Vector3(-887.16f,1679.61f,217.16f), 0f, "Axel's Pay'n'Spray", "Franchising car repairs")
            {
                //GarageDoors = new List<InteriorDoor>() { new InteriorDoor(3082692265,new Vector3(-1355.819f,-754.4543f,23.49588f)) },
                CameraPosition = new Vector3(-968.8f,1634.06f,255.7f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.AlderneyStateID,
            },
            new RepairGarage(new Vector3(-1063.54f,766.97f,210.8f), 0f, "Axel's Pay'n'Spray", "Franchising car repairs")
            {
                //GarageDoors = new List<InteriorDoor>() { new InteriorDoor(3082692265,new Vector3(-1355.819f,-754.4543f,23.49588f)) },
                CameraPosition = new Vector3(-1034.16f,779.04f,219.08f),
                OpenTime = 0,
                CloseTime = 24,
                CanInteractWhenWanted = true,
                StateID = StaticStrings.AlderneyStateID,
            },

        };
        LibertyCityLocations.RepairGarages.AddRange(RepairGarageList_96);

        List<SportingGoodsStore> SportingGoodsStoreList_22 = new List<SportingGoodsStore>() {
            new SportingGoodsStore() {
                MenuID = "SportingGoodsMenu",
                Name = "Sportsland Sporting",
                Description = "Land of sports, and they're all good",
                EntrancePosition = new Vector3(1132.65f, 196.33f, 218.42f),
                EntranceHeading = 269.42f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new SportingGoodsStore() {
                MenuID = "DepartmentStoreMenu",
                Name = "Spender's",
                Description = "Low quality products with prices jacked up to the stratosphere!",
                EntrancePosition = new Vector3(60.16f, 426.18f, 214.76f),
                EntranceHeading = 86.51f,
                OpenTime = 6,
                CloseTime = 20,
                StateID = StaticStrings.LibertyStateID,
            },
            new SportingGoodsStore() {
                MenuID = "MallMenu",
                Name = "Big Willis Mall",
                Description = "Get your window shopping on",
                EntrancePosition = new Vector3(2060.04f, 1112.64f, 228.91f),
                EntranceHeading = 86.7f,
                OpenTime = 9,
                CloseTime = 23,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.SportingGoodsStores.AddRange(SportingGoodsStoreList_22);
        List<Airport> AirportList_23 = new List<Airport>() {
            new Airport() {
                Name = "Francis Intl.",
                AirportID = "FIA",
                Description = "Great To Visit, Even Better To Leave~n~~n~City: ~y~Dukes, Liberty City~s~~n~State: ~p~Liberty~s~",
                EntrancePosition = new Vector3(2605.07f, 899.04f, 206.08f),
                EntranceHeading = 91.74f,
                OpenTime = 0,
                CloseTime = 24,
                ArrivalPosition = new Vector3(2596.69f, 821.39f, 206.08f),
                ArrivalHeading = 73.94f,
                CommercialFlights = new List<AirportFlight>()
                {
                    new AirportFlight("LDR",StaticStrings.AirHerlerCarrierID,"Relax on one of our state of the art jets and arrive in luxury. ~n~~n~Taxi service to downtown Ludendorff included.", 1500, 5),
                    new AirportFlight("LDR",StaticStrings.CaipiraAirwaysCarrierID,"Only three connections and 12 hours for a 5 hour flight! What else could you ask for? ~n~~n~Taxi service to downtown Ludendorff included.", 550, 12),
                },
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.Airports.AddRange(AirportList_23);
        List<YanktonAiport> YanktonAiportList_24 = new List<YanktonAiport>() {
            new YanktonAiport("LDR",new Vector3(3153.898f, -4840.879f, 111.8725f),354.7703f,"Ludendorff Regional",
            "The best fish boiled in lye in all of North Yankton!" +
            "~n~" +
            "~n~City: ~y~Ludendorff~s~" +
            "~n~State: ~p~North Yankton~s~")
            {
                OpenTime = 0
                ,CloseTime = 24
                ,ArrivalPosition = new Vector3(3153.898f, -4840.879f, 111.8725f)
                ,ArrivalHeading = 354.7703f

                ,AirArrivalPosition = new Vector3(4538.156f, -5345.569f, 230.4282f)
                ,AirArrivalHeading = 43.45281f
                ,CameraPosition = new Vector3(3142.449f, -4831.813f, 118.558f), CameraDirection = new Vector3(0.5385267f, -0.7833802f, -0.3103296f), CameraRotation = new Rotator(-18.0791f, -1.796229E-06f, -145.4938f)
                ,RequestIPLs = new List<string>() {
                            "plg_01",
                            "prologue01",
                            "prologue01_lod",
                            "prologue01c",
                            "prologue01c_lod",
                            "prologue01d",
                            "prologue01d_lod",
                            "prologue01e",
                            "prologue01e_lod",
                            "prologue01f",
                            "prologue01f_lod",
                            "prologue01g",
                            "prologue01h",
                            "prologue01h_lod",
                            "prologue01i",
                            "prologue01i_lod",
                            "prologue01j",
                            "prologue01j_lod",
                            "prologue01k",
                            "prologue01k_lod",
                            "prologue01z",
                            "prologue01z_lod",
                            "plg_02",
                            "prologue02",
                            "prologue02_lod",
                            "plg_03",
                            "prologue03",
                            "prologue03_lod",
                            "prologue03b",
                            "prologue03b_lod",
                            "prologue03_grv_dug",
                            "prologue03_grv_dug_lod",
                            "prologue_grv_torch",
                            "plg_04",
                            "prologue04",
                            "prologue04_lod",
                            "prologue04b",
                            "prologue04b_lod",
                            "prologue04_cover",
                            "des_protree_end",
                            "des_protree_start",
                            "des_protree_start_lod",
                            "plg_05",
                            "prologue05",
                            "prologue05_lod",
                            "prologue05b",
                            "prologue05b_lod",
                            "plg_06",
                            "prologue06",
                            "prologue06_lod",
                            "prologue06b",
                            "prologue06b_lod",
                            "prologue06_int",
                            "prologue06_int_lod",
                            "prologue06_pannel",
                            "prologue06_pannel_lod",
                            "prologue_m2_door",
                            "prologue_m2_door_lod",
                            "plg_occl_00",
                            "prologue_occl",
                            "plg_rd",
                            "prologuerd",
                            "prologuerdb",
                            "prologuerd_lod",
                        }
                ,RemoveIPLs = new List<string>(){
                            "plg_01",
                            "prologue01",
                            "prologue01_lod",
                            "prologue01c",
                            "prologue01c_lod",
                            "prologue01d",
                            "prologue01d_lod",
                            "prologue01e",
                            "prologue01e_lod",
                            "prologue01f",
                            "prologue01f_lod",
                            "prologue01g",
                            "prologue01h",
                            "prologue01h_lod",
                            "prologue01i",
                            "prologue01i_lod",
                            "prologue01j",
                            "prologue01j_lod",
                            "prologue01k",
                            "prologue01k_lod",
                            "prologue01z",
                            "prologue01z_lod",
                            "plg_02",
                            "prologue02",
                            "prologue02_lod",
                            "plg_03",
                            "prologue03",
                            "prologue03_lod",
                            "prologue03b",
                            "prologue03b_lod",
                            "prologue03_grv_cov",
                            "prologue03_grv_cov_lod",
                            "prologue03_grv_dug",
                            "prologue03_grv_dug_lod",
                            "prologue03_grv_fun",
                            "prologue_grv_torch",
                            "plg_04",
                            "prologue04",
                            "prologue04_lod",
                            "prologue04b",
                            "prologue04b_lod",
                            "prologue04_cover",
                            "des_protree_end",
                            "des_protree_start",
                            "des_protree_start_lod",
                            "plg_05",
                            "prologue05",
                            "prologue05_lod",
                            "prologue05b",
                            "prologue05b_lod",
                            "plg_06",
                            "prologue06",
                            "prologue06_lod",
                            "prologue06b",
                            "prologue06b_lod",
                            "prologue06_int",
                            "prologue06_int_lod",
                            "prologue06_pannel",
                            "prologue06_pannel_lod",
                            "prologue_m2_door",
                            "prologue_m2_door_lod",
                            "plg_occl_00",
                            "prologue_occl",
                            "plg_rd",
                            "prologuerd",
                            "prologuerdb",
                            "prologuerd_lod",

                        }
                ,CommercialFlights = new List<AirportFlight>()
                {
                    new AirportFlight("FIA",StaticStrings.CaipiraAirwaysCarrierID,"You'll get there when you get there", 850, 14),
                }
                ,RoadToggels = new HashSet<RoadToggler>()
                {
                    new RoadToggler(new Vector3(5526.24f, -5137.23f, 61.78925f),new Vector3(3679.327f, -4973.879f, 125.0828f),192.0f),
                    new RoadToggler(new Vector3(3691.211f, -4941.24f, 94.59368f),new Vector3(3511.115f, -4689.191f, 126.7621f),16.0f),
                    new RoadToggler(new Vector3(3510.004f, -4865.81f, 94.69557f),new Vector3(3204.424f, -4833.8147f, 126.8152f),16.0f),
                    new RoadToggler(new Vector3(3186.534f, -4832.798f, 109.8148f),new Vector3(3204.187f, -4833.993f, 114.815f),16.0f),
                }
                ,ZonesToEnable = new HashSet<string>() { "PrLog" }
                ,StateID = StaticStrings.NorthYanktonStateID,
            },
        };
        LibertyCityLocations.Airports.AddRange(YanktonAiportList_24);
        List<IllicitMarketplace> IllicitMarketplaceList_25 = new List<IllicitMarketplace>() {
            new IllicitMarketplace() {

                MenuID = "DealerHangoutMenu1",
                Name = "Dealer Hangout",
                Description = "Dealer Hangout",
                EntrancePosition = new Vector3(923.45f, 1265.04f, 202.79f),
                EntranceHeading = 83.74f,
                OpenTime = 11,
                CloseTime = 19,
                VendorPosition = new Vector3(923.45f, 1265.04f, 202.79f),
                VendorHeading = 83.74f,
                StateID = StaticStrings.LibertyStateID,
            },
        };
        LibertyCityLocations.IllicitMarketplaces.AddRange(IllicitMarketplaceList_25);
    }

    private void DefaultConfig_GangDens()
    {
        List<GangDen> LCGangDens = new List<GangDen>()
        {
            new GangDen(new Vector3(117.9776f, 237.2734f, 12.68547f), 91.83325f, "Gambetti Safehouse", "", "GambettiDenMenu", "AMBIENT_GANG_GAMBETTI")
            {
                IsPrimaryGangDen = true,
                CanInteractWhenWanted = true,
                MapIcon = 541,
                BannerImagePath = "gangs\\gambetti.png",
                OpenTime = 0,
                CloseTime = 24,
                IsEnabled = true,
                StateID = StaticStrings.LibertyStateID,
                PossiblePedSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(116.7614f, 239.4333f, 12.68577f), 117.5893f, 55f) { TaskRequirements = TaskRequirements.Guard },//right by thingo
                    new GangConditionalLocation(new Vector3(116.0579f, 223.9395f, 12.72538f), 73.20693f, 55f){ TaskRequirements = TaskRequirements.Guard },
                },
                PossibleVehicleSpawns = new List<ConditionalLocation>()
                {
                    new GangConditionalLocation(new Vector3(111.3983f, 218.4232f, 12.41919f), 1.662297f, 75f),
                }
            },
        };
        LibertyCityLocations.GangDens.AddRange(LCGangDens);
    }
    private void DefaultConfig_Prisons()
    {
        List<Prison> LCPrison = new List<Prison>()
        {
            new Prison(new Vector3(-903.9021f, 118.7461f, 3.080931f), 91.96844f, "Alderney State Correctional Facility","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID },
        };
        LibertyCityLocations.Prisons.AddRange(LCPrison);
    }
    private void DefaultConfig_Hospitals()
    {
        List<Hospital> Hospitals = new List<Hospital>()
        {
            //Broker
            new Hospital(new Vector3(1438.552f, 690.5808f, 33.54961f), 90.19894f, "Schottler Medical Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            //Dukes
            new Hospital(new Vector3(1486.106f, 977.1287f, 29.53525f), 33.23054f, "Cerveza Heights Medical Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            //Bohan
            new Hospital(new Vector3(1220.561f, 2333.059f, 23.89302f), 92.1476f, "Bohan Medical & Dental Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            //Algon
            new Hospital(new Vector3(-186.4241f, 1797.527f, 17.43241f), 85.08374f, "Holland Hospital Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new Hospital(new Vector3(334.043f, 643.3015f, 14.77393f), 178.5839f, "Lancet-Hospital Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new Hospital(new Vector3(51.70538f, -64.34246f, 4.941999f), 89.92042f, "City Hall Hospital","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            //Alderny
            new Hospital(new Vector3(-1080.693f, 1769.972f, 23.37073f), 309.4161f, "Westdyke Memorial Hospital","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID },
            new Hospital(new Vector3(-1277.354f, 890.3488f, 21.63032f), 313.0107f, "North Tudor Medical Center","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID },
        };
        LibertyCityLocations.Hospitals.AddRange(Hospitals);
    }
    private void DefaultConfig_PoliceStations()
    {
        List<PoliceStation> PoliceStations = new List<PoliceStation>()
        {
            //Broker
            new PoliceStation(new Vector3(1468.795f, 404.4823f, 28.02845f), 268.9079f, "South Slopes Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new PoliceStation(new Vector3(1132.495f, 135.204f, 18.18011f), 321.3237f, "Hove Beach Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },


            //Dukes
            new PoliceStation(new Vector3(1471.042f, 1020.193f, 30.82781f), 179.7346f, "East Island City Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new PoliceStation(new Vector3(2404.376f, 942.9011f, 7.022036f), 270.644f, "FIA Police Station","") { OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },

            //Bohan
            new PoliceStation(new Vector3(673.7206f, 2084.079f, 18.22777f), 2.178002f, "Fortside Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new PoliceStation(new Vector3(1220.499f, 2364.085f, 23.89311f), 88.64945f, "Northern Gardens LCPD Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },

            //Algonquin
            new PoliceStation(new Vector3(-146.7365f, 229.4094f, 13.04987f), 270.1865f, "Suffolk Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new PoliceStation(new Vector3(449.9062f, 286.9244f, 10.75325f), 208.419f, "Lower Easton Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new PoliceStation(new Vector3(53.50484f, 593.811f, 14.76916f), 357.1598f, "Star Junction Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new PoliceStation(new Vector3(-168.6235f, 776.5378f, 13.6791f), 178.2846f, "Westminster Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new PoliceStation(new Vector3(290.0559f, 1174.093f, 15.32371f), 85.63059f, "Middle Park East Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new PoliceStation(new Vector3(-178.7366f, 1597.181f, 11.9273f), 176.7212f, "Varsity Heights Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },
            new PoliceStation(new Vector3(323.3206f, 1685.935f, 14.74491f), 180.4846f, "East Holland LCPD Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.LibertyStateID },

            //Alderney
            new PoliceStation(new Vector3(-1480.706f, 771.6721f, 22.91864f), 240.3791f, "West District Mini Precinct","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID },
            new PoliceStation(new Vector3(-986.9969f, 257.8285f, 2.994598f), 311.2633f, "Acter Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID },
            new PoliceStation(new Vector3(-684.4249f, 1763.837f, 25.79581f), 134.1164f, "Leftwood Police Station","") {OpenTime = 0,CloseTime = 24, StateID = StaticStrings.AlderneyStateID },

        };
        LibertyCityLocations.PoliceStations.AddRange(PoliceStations);
    }
   }


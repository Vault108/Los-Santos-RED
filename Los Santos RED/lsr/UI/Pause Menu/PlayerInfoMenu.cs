﻿using ExtensionsMethods;
using iFruitAddon2;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Locations;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerInfoMenu
{
    private TabView tabView;

    private TabItemSimpleList simpleListTab;
    private TabMissionSelectItem missionSelectTab;
    private TabTextItem textTab;
    private TabSubmenuItem VehiclesSubMenu;
    private TabSubmenuItem TextMessagesSubMenu;
    private TabSubmenuItem GangsSubMenu;
    private TabSubmenuItem ContactsSubMenu;
    private IGangRelateable Player;
    private ITimeReportable Time;
    private IPlacesOfInterest PlacesOfInterest;
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    private IZones Zones;
    private IStreets Streets;
    private IInteriors Interiors;
    private IEntityProvideable World;
    public PlayerInfoMenu(IGangRelateable player, ITimeReportable time, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, IInteriors interiors, IEntityProvideable world)
    {
        Player = player;
        Time = time;
        PlacesOfInterest = placesOfInterest;
        Gangs = gangs;
        GangTerritories = gangTerritories;
        Zones = zones;
        Streets = streets;
        Interiors = interiors;
        World = world;
    }
    public void Setup()
    {
        tabView = new TabView("Los Santos ~r~RED~s~ Information");
        tabView.Tabs.Clear();
        tabView.OnMenuClose += TabView_OnMenuClose;
    }

    private void TabView_OnMenuClose(object sender, EventArgs e)
    {

        Game.IsPaused = false;
    }

    public void Update()
    {
        tabView.Update();
        if(tabView.Visible)
        {
            tabView.Money = Time.CurrentTime;
        }
    }
    public void Toggle()
    {
        if (!TabView.IsAnyPauseMenuVisible)
        {
            if(!tabView.Visible)
            {
                UpdateMenu();
                Game.IsPaused = true;
            }
            tabView.Visible = !tabView.Visible;
        }
    }
    private void UpdateMenu()
    {
        tabView.MoneySubtitle = Player.Money.ToString("C0");
        tabView.Name = Player.PlayerName;
        tabView.Money = Time.CurrentTime;

        tabView.Tabs.Clear();
        AddVehicles();
        AddCrimes();
        AddGangItems();
        AddContacts();
        AddPhoneRepliesMessages();
        AddTextMessages();
        AddLocations();
        tabView.RefreshIndex();
    }
    private void AddVehicles()
    {
        List<TabItem> items = new List<TabItem>();
        foreach (VehicleExt car in Player.OwnedVehicles)
        {
            Color carColor = car.VehicleColor();
            string Make = car.MakeName();
            string Model = car.ModelName();
            string PlateText = car.CarPlate?.PlateNumber;
            string VehicleName = "";

            string hexColor = ColorTranslator.ToHtml(Color.FromArgb(carColor.ToArgb()));
            string ColorizedColorName = carColor.Name;
            if (carColor.ToString() != "")
            {
                ColorizedColorName = $"<FONT color='{hexColor}'>" + carColor.Name + "~s~";
                VehicleName += ColorizedColorName;
            }
            string rightText = "";
            if (car.CarPlate != null && car.CarPlate.IsWanted)
            {
                rightText = " ~r~(Wanted)~s~";
            }

            string DescriptionText = $"~n~Color: {ColorizedColorName}";
            DescriptionText += $"~n~Make: {Make}";
            DescriptionText += $"~n~Model: {Model}";
            DescriptionText += $"~n~Plate: {PlateText} {rightText}";

            string ListEntryText = $"{ColorizedColorName} {Make} {Model} ({PlateText})";
            string DescriptionHeaderText = $"{Model}";
            if (car.Vehicle.Exists())
            {
                LocationData myData = new LocationData(car.Vehicle, Streets, Zones, Interiors);
                myData.Update(car.Vehicle);

                string StreetText = "";
                if (myData.CurrentStreet != null)
                {
                    StreetText += $"~y~{myData.CurrentStreet.Name}~s~";
                    if (myData.CurrentCrossStreet != null)
                    {
                        StreetText += $" at ~y~{myData.CurrentCrossStreet.Name}~s~";
                    }
                }
                string ZoneText = "";
                if (myData.CurrentZone != null)
                {
                    ZoneText = $" {(myData.CurrentZone.IsSpecificLocation ? "near" : "in")} ~p~{myData.CurrentZone.FullDisplayName}~s~";
                }
                string LocationText = $"{StreetText} {ZoneText}".Trim();
                LocationText = LocationText.Trim();

                DescriptionText += $"~n~Location: {LocationText}";
            }
            TabItem tItem = new TabTextItem(ListEntryText, DescriptionHeaderText, DescriptionText);
            //tItem.Activated += (s, e) => Game.DisplaySubtitle("Activated Submenu Item #" + submenuTab.Index, 5000);
            items.Add(tItem);
        }
        tabView.AddTab(VehiclesSubMenu = new TabSubmenuItem("Vehicles", items));
    }
    private void AddCrimes()
    {
        List<UIMenuItem> menuItems2 = new List<UIMenuItem>();
        if (Player.IsWanted)
        {
            foreach (CrimeEvent crime in Player.PoliceResponse.CrimesObserved.OrderByDescending(x => x.AssociatedCrime?.ResultingWantedLevel))
            {
                string crimeText = crime.AssociatedCrime.Name;
                crimeText += $" Instances: ({crime.Instances})";
                menuItems2.Add(new UIMenuItem(crimeText, "") { RightLabel = $"Wanted Level: {crime.AssociatedCrime.ResultingWantedLevel}" });
            }
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Current Crimes", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        else if (Player.WantedCrimes != null)
        {
            foreach (Crime crime in Player.WantedCrimes.OrderByDescending(x => x.ResultingWantedLevel))
            {
                menuItems2.Add(new UIMenuItem(crime.Name, "") { RightLabel = $"Wanted Level: {crime.ResultingWantedLevel}" });
            }
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Criminal History", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        else
        {
            TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("Criminal History", menuItems2);
            tabView.AddTab(interactiveListItem2);
        }
        //menuItems2[0].Activated += (m, s) => Game.DisplaySubtitle("Activated first item!");
    }
    private void AddTextMessages()
    {
        List<TabItem> items = new List<TabItem>();
        foreach (iFruitText text in Player.CellPhone.TextList.OrderByDescending(x => x.TimeReceived))
        {
            string TimeReceived = text.HourSent.ToString("00") + ":" + text.MinuteSent.ToString("00");// string.Format("{0:D2}h:{1:D2}m",text.HourSent,text.MinuteSent);

            string DescriptionText = "";
            DescriptionText += $"~n~Received At: {TimeReceived}";  //+ gr.ToStringBare();
            DescriptionText += $"~n~{text.Message}";

            string ListEntryItem = $"{text.Name}{(!text.IsRead ? " *" : "")} {TimeReceived}";
            string DescriptionHeaderText = $"{text.Name}";

            TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);

            tItem.Activated += (s, e) => {
                iFruitText myText = Player.CellPhone.TextList.Where(x => x.Index == VehiclesSubMenu.Index).FirstOrDefault();
                if(myText != null)
                {
                    myText.IsRead = true;
                    EntryPoint.WriteToConsole($"Text Message Marked Read {myText.Name} {myText.Message}");

                    Game.DisplaySubtitle($"Text Message Marked Read {myText.Name} {myText.Message}", 5000);

                }
                };//Game.DisplaySubtitle("Activated Submenu Item #" + submenuTab.Index, 5000);
            items.Add(tItem);
        }
        tabView.AddTab(TextMessagesSubMenu = new TabSubmenuItem("Text Messages", items));
    }
    private void AddGangItems()
    {
        List<TabItem> items = new List<TabItem>();


        foreach (GangReputation gr in Player.GangRelationships.GangReputations.OrderByDescending(x => x.GangRelationship == GangRespect.Hostile).ThenByDescending(x => x.GangRelationship == GangRespect.Friendly).ThenByDescending(x => Math.Abs(x.ReputationLevel)).ThenBy(x => x.Gang.ShortName))
        {
            string DescriptionText = "";

            DescriptionText = "Relationship: " + gr.ToStringBare();
            //GameLocation gangDen = PlacesOfInterest.GetLocations(LocationType.GangDen).Where(x => x.GangID == gr.Gang.ID).FirstOrDefault();
            ////string DenText = "~y~Unknown~s~";
            //if (gangDen != null && gangDen.IsEnabled)
            //{
            //    string StreetNames = Streets.GetStreetNames(gangDen.EntrancePosition);
            //    Zone Zone = Zones.GetZone(gangDen.EntrancePosition);

            //    //DenText = gangDen.IsEnabled ? "~g~Available~s~" : "~o~Unavailable~s~";
            //    //DescriptionText += $"~n~{gr.Gang.DenName}: {DenText}"; //+ gr.ToStringBare();
            //    string locationText = $"{StreetNames} {(Zone.IsSpecificLocation ? "near" : "in")} ~p~{Zone.FullDisplayName}~s~".Trim();

            //    DescriptionText += $"~n~{gr.Gang.DenName}: {locationText}"; //+ gr.ToStringBare();
                
            //}








            GangDen myDen = PlacesOfInterest.PossibleLocations.GangDens.FirstOrDefault(x => x.AssociatedGang?.ID == gr.Gang.ID);
            if(myDen != null && myDen.IsEnabled)
            {
                DescriptionText += $"~n~{gr.Gang.DenName}: {myDen.StreetAddress}"; //+ gr.ToStringBare();
            }

            string TerritoryText = "None";
            List<ZoneJurisdiction> gangTerritory = GangTerritories.GetGangTerritory(gr.Gang.ID);
            if (gangTerritory.Any())
            {
                TerritoryText = "";
                foreach (ZoneJurisdiction zj in gangTerritory)
                {
                    Zone myZone = Zones.GetZone(zj.ZoneInternalGameName);
                    if (myZone != null)
                    {
                        TerritoryText += "~p~" + myZone.DisplayName + "~s~, ";
                    }
                }
            }
            DescriptionText += $"~n~Territory: {TerritoryText.TrimEnd(' ', ',')}"; 
            //EntryPoint.WriteToConsole($"{gr.Gang.ContactName}");
            //EntryPoint.WriteToConsole($"{gr.Gang.ShortName} Player.CurrentCellPhone {Player.CellPhone != null}");
            //EntryPoint.WriteToConsole($"Player.CurrentCellPhone {Player.CellPhone != null}");

            if (Player.CellPhone.IsContactEnabled(gr.Gang.ContactName))
            {
                string ContactText = gr.Gang.ContactName;
                DescriptionText += $"~n~Contacts: {ContactText}";
            }

            if(Player.PlayerTasks.HasTask(gr.Gang.ContactName))
            {
                DescriptionText += $"~n~~g~Has Task~s~";
            }


            if (gr.MembersKilled > 0)
            {
                DescriptionText += $"~n~~r~Members Killed~s~: {gr.MembersKilled}~s~ ({gr.MembersKilledInTerritory})";
            }
            if (gr.MembersHurt > 0)
            {
                DescriptionText += $"~n~~o~Members Hurt~s~: {gr.MembersHurt}~s~ ({gr.MembersHurtInTerritory})";
            }
            if (gr.MembersCarJacked > 0)
            {
                DescriptionText += $"~n~~o~Members CarJacked~s~: {gr.MembersCarJacked}~s~ ({gr.MembersCarJackedInTerritory})";
            }

            TabItem tabItem = new TabTextItem($"{gr.Gang.ShortName} {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);

            //tabItem.Activated += (s, e) => Game.DisplaySubtitle("Activated Submenu Item #" + GangsSubMenu.Index, 5000);
            items.Add(tabItem);

        }
        tabView.AddTab(GangsSubMenu = new TabSubmenuItem("Gangs", items));
    }

    private void AddGPSRoute(GameLocation coolPlace)
    {
        if (Player.CurrentGPSBlip.Exists())
        {
            NativeFunction.Natives.SET_BLIP_ROUTE(Player.CurrentGPSBlip, false);
            Player.CurrentGPSBlip.Delete();
        }
        if (coolPlace != null)
        {
            Blip MyLocationBlip = new Blip(coolPlace.EntrancePosition)
            {
                Name = coolPlace.Name
            };
            if (MyLocationBlip.Exists())
            {
                MyLocationBlip.Color = Color.Yellow;
                NativeFunction.Natives.SET_BLIP_AS_SHORT_RANGE(MyLocationBlip, false);
                NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(coolPlace.Name);
                NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(MyLocationBlip);
                NativeFunction.Natives.SET_BLIP_ROUTE(MyLocationBlip, true);
                Player.CurrentGPSBlip = MyLocationBlip;
                World.AddEntity(MyLocationBlip);
                Game.DisplaySubtitle($"Adding GPS To {coolPlace.Name}");
            }
        }
    }
    private void AddContacts()
    {
        List<TabItem> items = new List<TabItem>();
        foreach (iFruitContact contact in Player.CellPhone.ContactList.OrderBy(x=> x.Name))
        {
            string DescriptionText = "Select to ~o~Call~s~ the contact";
            string Title = contact.Name;
            string SubTitle = contact.Name;
            Gang myGang = Gangs.GetGangByContact(contact.Name);
            if(myGang != null)
            {
               GangReputation gr = Player.GangRelationships.GangReputations.FirstOrDefault(x => x.Gang.ID == myGang.ID);
                if(gr != null)
                {
                    Title = $"{contact.Name} {gr.ToBlip()}~s~";
                    SubTitle = $"{gr.Gang.ColorPrefix}{contact.Name}~s~";
                }
            }
            TabItem tabItem = new TabTextItem(Title, SubTitle, DescriptionText);//TabItem tabItem = new TabTextItem($"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~ {gr.ToBlip()}~s~", $"{gr.Gang.ColorPrefix}{gr.Gang.FullName}~s~", DescriptionText);
            tabItem.Activated += (s, e) =>
            {
                //Game.DisplaySubtitle("Activated Submenu Item #" + ContactsSubMenu.Index + " "+ contact.Name, 5000);
                tabView.Visible = false;
                Game.IsPaused = false;
                Player.CellPhone.ContactAnswered(contact);

            };
            items.Add(tabItem);
        }
        tabView.AddTab(ContactsSubMenu = new TabSubmenuItem("Contacts", items));
    }
    private void AddPhoneRepliesMessages()
    {
        List<TabItem> items = new List<TabItem>();
        foreach (PhoneResponse text in Player.CellPhone.PhoneResponseList.OrderByDescending(x => x.TimeReceived).Take(15))
        {
            string TimeReceived = text.TimeReceived.ToString("HH:mm");// text.HourSent.ToString("00") + ":" + text.MinuteSent.ToString("00");// string.Format("{0:D2}h:{1:D2}m",text.HourSent,text.MinuteSent);
            string DescriptionText = "";
            DescriptionText += $"~n~Received At: {TimeReceived}";  //+ gr.ToStringBare();
            DescriptionText += $"~n~{text.Message}";
            string ListEntryItem = $"{text.ContactName} {TimeReceived}";
            string DescriptionHeaderText = $"{text.ContactName}";
            TabItem tItem = new TabTextItem(ListEntryItem, DescriptionHeaderText, DescriptionText);
            items.Add(tItem);
        }
        tabView.AddTab(TextMessagesSubMenu = new TabSubmenuItem("Replies", items));
    }
    private void AddLocations()
    {
        List<UIMenuItem> menuItems = new List<UIMenuItem>();
        menuItems.Add(new UIMenuItem("Remove GPR Route", "Remove any enabled GPS Blip"));
        foreach (LocationType lt in (LocationType[])Enum.GetValues(typeof(LocationType)))
        {
            if (IsValidLocationType(lt))
            {
                List<GameLocation> LocationPlaces = new List<GameLocation>();
                List<string> LocationNames = new List<string>();
                foreach (GameLocation gl in PlacesOfInterest.GetLocations(lt))
                {
                    Zone placeZone = Zones.GetZone(gl.EntrancePosition);
                    string betweener = "";
                    string zoneString = "";
                    if (placeZone != null)
                    {
                        if (placeZone.IsSpecificLocation)
                        {
                            betweener = $"near";
                        }
                        else
                        {
                            betweener = $"in";
                        }
                        zoneString = $"~p~{placeZone.DisplayName}~s~";
                    }
                    string streetName = Streets.GetStreetNames(gl.EntrancePosition);
                    string streetNumber = "";
                    if (streetName == "")
                    {
                        betweener = "";
                    }
                    else
                    {
                        if (gl.CellY < 0)
                        {
                            streetNumber = Math.Abs(gl.CellY * 100).ToString() + "S";
                        }
                        else
                        {
                            streetNumber = Math.Abs(gl.CellY * 100).ToString() + "N";
                        }
                        if (gl.CellX < 0)
                        {
                            streetNumber += Math.Abs(gl.CellX * 100).ToString() + "W";
                        }
                        else
                        {
                            streetNumber += Math.Abs(gl.CellX * 100).ToString() + "E";
                        }
                    }

                    string LocationName = $"{gl.Name} - {(gl.IsOpen(Time.CurrentHour) ? "~s~Open~s~" : "~m~Closed~s~")} - " + $"{streetNumber} {streetName} {betweener} {zoneString}".Trim();
                    LocationNames.Add(LocationName);
                    gl.FullAddressText = LocationName;
                    LocationPlaces.Add(gl);
                }
                if (LocationPlaces.Any())
                {
                    string LocationName = GetSafeLocationName(lt);
                    menuItems.Add(new UIMenuListScrollerItem<GameLocation>(LocationName, $"List of all {LocationName}", LocationPlaces) { Formatter = v => v.FullAddressText });
                }
            }
        }
        TabInteractiveListItem interactiveListItem = new TabInteractiveListItem("Locations", menuItems);
        interactiveListItem.BackingMenu.OnItemSelect += BackingMenu_OnItemSelect;
        tabView.AddTab(interactiveListItem);
    }
    private void BackingMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.GetType() == typeof(UIMenuListScrollerItem<GameLocation>))
        {
            UIMenuListScrollerItem<GameLocation> myItem = (UIMenuListScrollerItem<GameLocation>)selectedItem;
            if (myItem.SelectedItem != null)
            {
                Player.AddGPSRoute(myItem.SelectedItem.Name, myItem.SelectedItem.EntrancePosition);
            }
        }
        else if (selectedItem.Text == "Remove GPR Route")
        {
            Player.RemoveGPSRoute();
        }
    }
    private bool IsValidLocationType(LocationType lt)
    {
        if (lt == LocationType.BeautyShop || lt == LocationType.GangDen || lt == LocationType.Garage || lt == LocationType.VendingMachine || lt == LocationType.Stadium || lt == LocationType.Grave || lt == LocationType.GunShop || lt == LocationType.BusStop || lt == LocationType.DrugDealer || lt == LocationType.Other)
        {
            return false;
        }
        return true;
    }
    private string GetSafeLocationName(LocationType lt)
    {
        switch (lt)
        {
            case LocationType.BeautyShop:
                return "Beauty Shop";
            case LocationType.CarDealer:
                return "Car Dealership";
            case LocationType.ConvenienceStore:
                return "Convenience Store";
            case LocationType.DriveThru:
                return "Drive-Thru";
            case LocationType.GasStation:
                return "Gas Station";
            case LocationType.HardwareStore:
                return "Hardware Store";
            case LocationType.LiquorStore:
                return "Liquor Store";
            case LocationType.PawnShop:
                return "Pawn Shop";
            case LocationType.ScrapYard:
                return "Scrap Yard";
            case LocationType.StripClub:
                return "Strip Club";
            case LocationType.VendingMachine:
                return "Vending Machine";
            case LocationType.GunShop:
                return "Gun Shop";
            default:
                return lt.ToString();
        }
    }
}


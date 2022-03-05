﻿using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

public class DebugMenu : Menu
{
    private UIMenu DispatcherMenu;
    private UIMenu Debug;
    private UIMenuListItem AutoSetRadioStation;
    private UIMenuItem GiveMoney;
    private UIMenuItem SetMoney;
    private UIMenuItem FillHealth;
    private UIMenuItem FillHealthAndArmor;
    private UIMenuItem ForceSober;
    private UIMenuListScrollerItem<Agency> SpawnAgencyFoot;
    private UIMenuListScrollerItem<Agency> SpawnAgencyVehicle;
    private UIMenuItem GoToReleaseSettings;
    private UIMenuItem StartRandomCrime;
    private UIMenuItem KillPlayer;
    private UIMenuItem LogCameraPositionMenu;
    private UIMenuItem LogInteriorMenu;
    private UIMenuItem LogLocationMenu;
    private UIMenuItem LogLocationSimpleMenu;
    private UIMenuListItem GetRandomWeapon;
    private UIMenuListItem TeleportToPOI;
    private UIMenuItem DefaultGangRep;
    private UIMenuItem RandomGangRep;
    private UIMenuItem SetDateToToday;
    private UIMenuItem Holder1;
    private IActionable Player;
    private RadioStations RadioStations;
    private int RandomWeaponCategory;
    private IWeapons Weapons;
    private IPlacesOfInterest PlacesOfInterest;
    private int PlaceOfInterestSelected;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private Camera FreeCam;
    private float FreeCamScale = 1.0f;
    private UIMenuItem FreeCamMenu;
    private UIMenuItem LoadSPMap;
    private UIMenuItem LoadMPMap;
    private IEntityProvideable World;
    private UIMenuItem HostileGangRep;
    private UIMenuItem FriendlyGangRep;
    private UIMenuItem RandomSingleGangRep;
    private ITaskerable Tasker;
    private MenuPool MenuPool;
    private Dispatcher Dispatcher;
    private IAgencies Agencies;
    private UIMenuListScrollerItem<Gang> SpawnGangFoot;
    private UIMenuListScrollerItem<Gang> SpawnGangVehicle;
    private IGangs Gangs;

    public DebugMenu(MenuPool menuPool, IActionable player, IWeapons weapons, RadioStations radioStations, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, ITimeControllable time, IEntityProvideable world, ITaskerable tasker, Dispatcher dispatcher, IAgencies agencies, IGangs gangs)
    {
        Gangs = gangs;
        Dispatcher = dispatcher;
        Agencies = agencies;
        MenuPool = menuPool;
        Player = player;
        Weapons = weapons;
        RadioStations = radioStations;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Time = time;
        World = world;
        Tasker = tasker;
        Debug = new UIMenu("Debug", "Debug Settings");
        Debug.SetBannerType(EntryPoint.LSRedColor);
        menuPool.Add(Debug);

        
        Debug.OnItemSelect += DebugMenuSelect;
        Debug.OnListChange += OnListChange;
        CreateDebugMenu();
    }
    public override void Hide()
    {
        Debug.Visible = false;
    }
    public override void Show()
    {
        if (!Debug.Visible)
        {
            Debug.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Debug.Visible)
        {
            Debug.Visible = true;
        }
        else
        {
            Debug.Visible = false;
        }
    }
    private void CreateDebugMenu()
    {

        DispatcherMenu = MenuPool.AddSubMenu(Debug, "Dispatcher");
        DispatcherMenu.SetBannerType(EntryPoint.LSRedColor);
        DispatcherMenu.OnItemSelect += DispatcherMenuSelect;

        SpawnAgencyFoot = new UIMenuListScrollerItem<Agency>("Cop Random On-Foot Spawn", "Spawn a random agency ped on foot", Agencies.GetAgencies());
        SpawnAgencyVehicle = new UIMenuListScrollerItem<Agency>("Cop Random Vehicle Spawn", "Spawn a random agency ped with a vehicle", Agencies.GetAgencies());

        SpawnGangFoot = new UIMenuListScrollerItem<Gang>("Gang Random On-Foot Spawn", "Spawn a random gang ped on foot", Gangs.GetAllGangs());
        SpawnGangVehicle = new UIMenuListScrollerItem<Gang>("Gang Random Vehicle Spawn", "Spawn a random gang ped with a vehicle", Gangs.GetAllGangs());


        DispatcherMenu.AddItem(SpawnAgencyFoot);
        DispatcherMenu.AddItem(SpawnAgencyVehicle);


        DispatcherMenu.AddItem(SpawnGangFoot);
        DispatcherMenu.AddItem(SpawnGangVehicle);





        GoToReleaseSettings = new UIMenuItem("Quick Set Release Settings", "Set some release settings quickly.");


        StartRandomCrime = new UIMenuItem("Start Random Crime", "Trigger a random crime around the map.");
        KillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
        GetRandomWeapon = new UIMenuListItem("Get Random Weapon", "Gives the Player a random weapon and ammo.", Enum.GetNames(typeof(WeaponCategory)).ToList());
        GiveMoney = new UIMenuItem("Get Money", "Give you some cash");
        SetMoney = new UIMenuItem("Set Money", "Sets your cash");


        FillHealth = new UIMenuItem("Fill Health", "Refill health only");
        FillHealthAndArmor = new UIMenuItem("Fill Health and Armor", "Get loaded for bear");

        ForceSober = new UIMenuItem("Become Sober", "Froces a sober state on the player (if intoxicated)");

        AutoSetRadioStation = new UIMenuListItem("Auto-Set Station", "Will auto set the station any time the radio is on", RadioStations.RadioStationList);
        LogLocationMenu = new UIMenuItem("Log Game Location", "Location Type, Then Name");
        LogLocationSimpleMenu = new UIMenuItem("Log Game Location (Simple)", "Location Type, Then Name");
        LogInteriorMenu = new UIMenuItem("Log Game Interior", "Interior Name");
        LogCameraPositionMenu = new UIMenuItem("Log Camera Position", "Logs current rendering cam post direction and rotation");
        FreeCamMenu = new UIMenuItem("Free Cam", "Start Free Camera Mode");

        LoadSPMap = new UIMenuItem("Load SP Map", "Loads the SP map if you have the MP map enabled");
        LoadMPMap = new UIMenuItem("Load MP Map", "Load the MP map if you have the SP map enabled");


        TeleportToPOI = new UIMenuListItem("Teleport To POI", "Teleports to A POI on the Map", PlacesOfInterest.GetAllPlaces());


        DefaultGangRep = new UIMenuItem("Set Gang Rep Default", "Sets the player reputation to each gang to the default value");
        RandomGangRep = new UIMenuItem("Set Gang Rep Random", "Sets the player reputation to each gang to a randomized number");
        RandomSingleGangRep = new UIMenuItem("Set Single Gang Rep Random", "Sets the player reputation to random gang to a randomized number");

        HostileGangRep = new UIMenuItem("Set Gang Rep Hostile", "Sets the player reputation to each gang to hostile");
        FriendlyGangRep = new UIMenuItem("Set Gang Rep Friendly", "Sets the player reputation to each gang to friendly");
        SetDateToToday = new UIMenuItem("Set Game Date Current", "Sets the game date the same as system date");



        Holder1 = new UIMenuItem("Placeholder", "Placeholder nullsub");



        Debug.AddItem(KillPlayer);
        Debug.AddItem(GetRandomWeapon);
        Debug.AddItem(GiveMoney);
        Debug.AddItem(SetMoney);
        Debug.AddItem(FillHealth);
        Debug.AddItem(FillHealthAndArmor);
        Debug.AddItem(GoToReleaseSettings);

        Debug.AddItem(ForceSober);

        Debug.AddItem(AutoSetRadioStation);
        Debug.AddItem(StartRandomCrime);
       // Debug.AddItem(TeleportToPOI);
        Debug.AddItem(DefaultGangRep);
        Debug.AddItem(RandomGangRep);
        Debug.AddItem(RandomSingleGangRep);

        Debug.AddItem(HostileGangRep);
        Debug.AddItem(FriendlyGangRep);

        Debug.AddItem(FreeCamMenu);
        Debug.AddItem(LogLocationMenu);
        Debug.AddItem(LogLocationSimpleMenu);
        Debug.AddItem(LogInteriorMenu);
        Debug.AddItem(LogCameraPositionMenu);
        Debug.AddItem(SetDateToToday);
        Debug.AddItem(LoadSPMap);
        Debug.AddItem(LoadMPMap);



        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To ScrapYard", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.ScrapYards));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Hotel", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Hotels));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To GunStores", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.GunStores));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Gang Den", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.GangDens));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Dead Drops", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.DeadDrops));
        Debug.AddItem(new UIMenuListScrollerItem<BasicLocation>($"Teleport To Residence", "Teleports to A POI on the Map", PlacesOfInterest.PossibleLocations.Residences));
        foreach (LocationType lt in (LocationType[])Enum.GetValues(typeof(LocationType)))
        {
            Debug.AddItem(new UIMenuListScrollerItem<GameLocation>($"Teleport To {lt}", "Teleports to A POI on the Map", PlacesOfInterest.GetLocations(lt)));
        }



    }

    private void DispatcherMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == SpawnAgencyFoot)
        {
            EntryPoint.WriteToConsole($"SpawnAgencyFoot SELECTED {SpawnAgencyFoot.SelectedItem.ID}");
            Dispatcher.DebugSpawnCop(SpawnAgencyFoot.SelectedItem.ID, true);
        }
        else if (selectedItem == SpawnAgencyVehicle)
        {
            EntryPoint.WriteToConsole($"SpawnAgencyVehicle SELECTED {SpawnAgencyVehicle.SelectedItem.ID}");
            Dispatcher.DebugSpawnCop(SpawnAgencyVehicle.SelectedItem.ID, false);
        }


        else if (selectedItem == SpawnGangFoot)
        {
            EntryPoint.WriteToConsole($"SpawnGangFoot SELECTED {SpawnGangFoot.SelectedItem.ID}");
            Dispatcher.DebugSpawnGang(SpawnGangFoot.SelectedItem.ID, true);
        }
        else if (selectedItem == SpawnGangVehicle)
        {
            EntryPoint.WriteToConsole($"SpawnGangVehicle SELECTED {SpawnGangVehicle.SelectedItem.ID}");
            Dispatcher.DebugSpawnGang(SpawnGangVehicle.SelectedItem.ID, false);
        }
        sender.Visible = false;
    }

    private void DebugMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == KillPlayer)
        {
            Game.LocalPlayer.Character.Kill();
        }
        else if (selectedItem == GoToReleaseSettings)
        {
            Settings.SettingsManager.GangSettings.ShowSpawnedBlip = false;
            Settings.SettingsManager.PoliceSettings.ShowSpawnedBlips = false;
            Settings.SettingsManager.UISettings.ShowDebug = false;
            Settings.SettingsManager.VehicleSettings.AutoTuneRadioOnEntry = false;
        }
        else if (selectedItem == GetRandomWeapon)
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon((WeaponCategory)RandomWeaponCategory);
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
            }
        }
        else if (selectedItem == TeleportToPOI)
        {
            GameLocation ToTeleportTo = PlacesOfInterest.GetAllPlaces()[PlaceOfInterestSelected];
            if(ToTeleportTo != null)
            {
                Game.LocalPlayer.Character.Position = ToTeleportTo.EntrancePosition;
                Game.LocalPlayer.Character.Heading = ToTeleportTo.EntranceHeading;
            }
        }
        else if (selectedItem == GiveMoney)
        {
            Player.GiveMoney(50000);
        }
        else if (selectedItem == SetMoney)
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int moneyToSet))
            {
                Player.SetMoney(moneyToSet);
            }
        }
        else if (selectedItem == FillHealth)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
        }
        else if (selectedItem == FillHealthAndArmor)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
            Game.LocalPlayer.Character.Armor = 100;
        }
        else if (selectedItem == StartRandomCrime)
        {
            Tasker.CreateCrime();
        }
        else if (selectedItem == ForceSober)
        {
            Player.Intoxication.Dispose();
        }
        else if (selectedItem == LogLocationMenu)
        {
            LogGameLocation();
        }
        else if (selectedItem == LogLocationSimpleMenu)
        {
            LogGameLocationSimple();
        }
        else if (selectedItem == LogCameraPositionMenu)
        {
            LogCameraPosition();
        }
        else if (selectedItem == LogInteriorMenu)
        {
            LogGameInterior();
        }
        else if (selectedItem == SetDateToToday)
        {
            Time.SetDateToToday();
        }
        else if (selectedItem == FreeCamMenu)
        {
            Frecam();
        }
        else if (selectedItem == Holder1)
        {

        }
        else if (selectedItem == LoadMPMap)
        {
            World.LoadMPMap();
        }
        else if (selectedItem == LoadSPMap)
        {
            World.LoadSPMap();
        }
        else if (selectedItem == RandomGangRep)
        {
            Player.GangRelationships.SetRandomReputations();
        }
        else if (selectedItem == RandomSingleGangRep)
        {
            Player.GangRelationships.SetSingleRandomReputation();
        }
        else if (selectedItem == DefaultGangRep)
        {
            Player.GangRelationships.ResetReputations();
        }
        else if (selectedItem == HostileGangRep)
        {
            Player.GangRelationships.SetHostileReputations();
        }
        else if (selectedItem == FriendlyGangRep)
        {
            Player.GangRelationships.SetFriendlyReputations();
        }

        if (selectedItem.GetType() == typeof(UIMenuListScrollerItem<GameLocation>))
        {
            UIMenuListScrollerItem<GameLocation> myItem = (UIMenuListScrollerItem<GameLocation>)selectedItem;
            if (myItem.SelectedItem != null)
            {
                Game.LocalPlayer.Character.Position = myItem.SelectedItem.EntrancePosition;
                Game.LocalPlayer.Character.Heading = myItem.SelectedItem.EntranceHeading;
            }
        }
        if (selectedItem.GetType() == typeof(UIMenuListScrollerItem<BasicLocation>))
        {
            UIMenuListScrollerItem<BasicLocation> myItem = (UIMenuListScrollerItem<BasicLocation>)selectedItem;
            if (myItem.SelectedItem != null)
            {
                Game.LocalPlayer.Character.Position = myItem.SelectedItem.EntrancePosition;
                Game.LocalPlayer.Character.Heading = myItem.SelectedItem.EntranceHeading;
            }
        }

        Debug.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (list == GetRandomWeapon)
        {
            RandomWeaponCategory = index;
        }
        if (list == AutoSetRadioStation)
        {
            Settings.SettingsManager.VehicleSettings.AutoTuneRadioStation = RadioStations.RadioStationList[index].InternalName;
        }
        if (list == TeleportToPOI)
        {
            PlaceOfInterestSelected = index;
        }
    }

    private void Frecam()
    {
        GameFiber.StartNew(delegate
        {
            FreeCam = new Camera(false);
            FreeCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            FreeCam.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            FreeCam.Rotation = new Rotator(r.X, r.Y, r.Z);
            FreeCam.Active = true;
            Game.LocalPlayer.HasControl = false;
            //This is all adapted from https://github.com/CamxxCore/ScriptCamTool/blob/master/GTAV_ScriptCamTool/PositionSelector.cs#L59
            while (!Game.IsKeyDownRightNow(Keys.P))
            {
                if (Game.IsKeyDownRightNow(Keys.W))
                {
                    FreeCam.Position += NativeHelper.GetCameraDirection(FreeCam, FreeCamScale);
                }
                else if (Game.IsKeyDownRightNow(Keys.S))
                {
                    FreeCam.Position -= NativeHelper.GetCameraDirection(FreeCam, FreeCamScale);
                }
                if (Game.IsKeyDownRightNow(Keys.A))
                {
                    FreeCam.Position = NativeHelper.GetOffsetPosition(FreeCam.Position, FreeCam.Rotation.Yaw, -1.0f * FreeCamScale);
                }
                else if (Game.IsKeyDownRightNow(Keys.D))
                {
                    FreeCam.Position = NativeHelper.GetOffsetPosition(FreeCam.Position, FreeCam.Rotation.Yaw, 1.0f * FreeCamScale);
                }
                FreeCam.Rotation += new Rotator(NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, 221) * -4f, 0, NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, 220) * -5f) * FreeCamScale;

                NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(FreeCam.Position.X, FreeCam.Position.Y, FreeCam.Position.Z, 0f, 0f, 0f);

                if (Game.IsKeyDownRightNow(Keys.O))
                {
                    if (FreeCamScale == 1.0f)
                    {
                        FreeCamScale = 0.25f;
                    }
                    else
                    {
                        FreeCamScale = 1.0f;
                    }
                }

                if (Game.IsKeyDownRightNow(Keys.J))
                {
                    Game.LocalPlayer.Character.Position = FreeCam.Position;
                    Game.LocalPlayer.Character.Heading = FreeCam.Heading;
                }

                string FreeCamString = FreeCamScale == 1.0f ? "Regular Scale" : "Slow Scale";
                Game.DisplayHelp($"Press P to Exit~n~Press O To Change Scale Current: {FreeCamString}~n~Press J To Move Player to Position");
                GameFiber.Yield();
            }
            FreeCam.Active = false;
            Game.LocalPlayer.HasControl = true;
            NativeFunction.Natives.CLEAR_FOCUS();
        }, "Run Debug Logic");
    }
    private void LogGameLocation()
    {
        Vector3 pos = Game.LocalPlayer.Character.Position;
        float Heading = Game.LocalPlayer.Character.Heading;
        string text1 = NativeHelper.GetKeyboardInput("LocationType");
        string text2 = NativeHelper.GetKeyboardInput("Name");
        WriteToLogLocations($"new GameLocation(new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f,new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, LocationType.{text1}, \"{text2}\", \"{text2}\"),");
    }
    private void LogGameLocationSimple()
    {
        Vector3 pos = Game.LocalPlayer.Character.Position;
        float Heading = Game.LocalPlayer.Character.Heading;
        string text1 = NativeHelper.GetKeyboardInput("LocationType");
        string text2 = NativeHelper.GetKeyboardInput("Name");
        WriteToLogLocations($"new GameLocation(new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, LocationType.{text1}, \"{text2}\", \"\"),");
    }
    private void LogCameraPosition()
    {

        if (FreeCam.Active)
        {
            Vector3 pos = FreeCam.Position;
            Rotator r = FreeCam.Rotation;
            Vector3 direction = NativeHelper.GetCameraDirection(FreeCam);
            WriteToLogCameraPosition($", CameraPosition = new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), CameraDirection = new Vector3({direction.X}f, {direction.Y}f, {direction.Z}f), CameraRotation = new Rotator({r.Pitch}f, {r.Roll}f, {r.Yaw}f);");
        }
        else
        {
            uint CameraHAndle = NativeFunction.Natives.GET_RENDERING_CAM<uint>();
            Vector3 pos = NativeFunction.Natives.GET_CAM_COORD<Vector3>(CameraHAndle);
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            Vector3 direction = NativeHelper.GetGameplayCameraDirection();
            WriteToLogCameraPosition($", CameraPosition = new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), CameraDirection = new Vector3({direction.X}f, {direction.Y}f, {direction.Z}f), CameraRotation = new Rotator({r.X}f, {r.Y}f, {r.Z}f);");
        }
    }
    private void LogGameInterior()
    {
        string text1 = NativeHelper.GetKeyboardInput("Name");
        string toWrite = $"new Interior({Player.CurrentLocation?.CurrentInterior?.ID}, \"{text1}\"),";
        WriteToLogInteriors(toWrite);
    }
    private void WriteToLogCameraPosition(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "CameraPositions.txt", sb.ToString());
        sb.Clear();
    }
    private void WriteToLogLocations(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "StoredLocations.txt", sb.ToString());
        sb.Clear();
    }
    private void WriteToLogInteriors(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "StoredInteriors.txt", sb.ToString());
        sb.Clear();
    }

}
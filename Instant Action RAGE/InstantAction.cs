﻿using ExtensionsMethods;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class InstantAction
{
    private static Random rnd;
    private static Police.PoliceState HandsUpPreviousPoliceState;
    private static bool PrevPlayerIsGettingIntoVehicle;
    private static bool PrevPlayerInVehicle = false;
    private static bool PrevPlayerAimingInVehicle = false;
    private static bool IsRunning { get; set; } = true;
    public static bool IsDead { get; set; } = false;
    public static bool IsBusted { get; set; } = false;
    public static bool BeingArrested { get; set; } = false;
    public static bool DiedInVehicle { get; set; } = false;
    public static bool PlayerIsConsideredArmed { get; set; } = false;
    public static int TimesDied { get; set; } = 0;
    public static bool HandsAreUp { get; set; } = false;
    public static int MaxWantedLastLife { get; set; }
    public static WeaponHash LastWeapon { get; set; } = 0;
    public static bool PlayerInVehicle { get; set; } = false;
    public static bool PlayerInAutomobile { get; set; } = false;
    public static bool PlayerAimingInVehicle { get; set; } = false;
    public static bool PlayerIsGettingIntoVehicle { get; set; }
    public static int PlayerWantedLevel { get; set; } = 0;
    public static WeaponHash PlayerCurrentWeaponHash { get; set; }
    public static List<GTAVehicle> TrackedVehicles { get; set; } = new List<GTAVehicle>() ;
    public static Vehicle OwnedCar { get; set; } = null;
    public static List<Rage.Object> CreatedObjects { get; set; } = new List<Rage.Object>();
    public static bool IsHardToSeeInWeather
    {
        get
        {
            WeatherType TheWeather = World.Weather;
            if (TheWeather == WeatherType.Blizzard || TheWeather == WeatherType.Foggy || TheWeather == WeatherType.Rain || TheWeather == WeatherType.Snow || TheWeather == WeatherType.Snowlight || TheWeather == WeatherType.Thunder || TheWeather == WeatherType.Xmas)
                return true;
            else
                return false;
        }
    }
    public static bool IsCurrentVehicleTracked
    {
        get
        {
            if(Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                PoolHandle Handle = Game.LocalPlayer.Character.CurrentVehicle.Handle;
                return TrackedVehicles.Any(x => x.VehicleEnt.Handle == Handle);
            }
            else
            {
                return false;
            }    
        }
    }
    static InstantAction()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        while (Game.IsLoading)
            GameFiber.Yield();
        RespawnStopper.Initialize(); //maye some slowness
        LoadInteriors();
        Agencies.Initialize();
        Zones.Initialize();
        WeatherReporting.Initialize();
        Locations.Initialize();
        Police.Initialize();
        PoliceSpawning.Initialize();
        LicensePlateChanging.Initialize();
        Settings.Initialize();
        Menus.Intitialize();//Somewhat the procees each tick is taking frames
        RespawnStopper.Initialize(); //maye some slowness
        PoliceScanning.Initialize();
        DispatchAudio.Initialize();//slow? moved to 500 ms
        PoliceSpeech.Initialize();//slow? moved to 500 ms
        Vehicles.Initialize();
        VehicleEngine.Initialize();
        Smoking.Initialize();
        Tasking.Initialize();

        GTAWeapons.Initialize();
        Speed.Initialize();
        WeaponDropping.Initialize();
        Streets.Initialize();
        Debugging.Initialize();
        PlayerLocation.Initialize();
        TrafficViolations.Initialize();
        SearchModeStopping.Initialize();
        UI.Initialize();
        MainLoop();
    }
    public static void MainLoop()
    {
        Game.LocalPlayer.Character.CanBePulledOutOfVehicles = true;

        var stopwatch = new Stopwatch();
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    stopwatch.Start();
                    UpdatePlayer();
                    StateTick();
                    ControlTick();
                    AudioTick();
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds >= 16)
                        LocalWriteToLog("InstantActionTick", string.Format("Tick took {0} ms", stopwatch.ElapsedMilliseconds));
                    stopwatch.Reset();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
        foreach (Blip myBlip in Police.CreatedBlips)
        {
            if (myBlip.Exists())
                myBlip.Delete();
        }
        LicensePlateChanging.Dispose();
        Settings.Dispose();
        Menus.Dispose();
        RespawnStopper.Dispose(); //maye some slowness
        PoliceScanning.Dispose();
        DispatchAudio.Dispose();
        PoliceSpeech.Dispose();
        Vehicles.Dispose();
        VehicleEngine.Dispose();
        Smoking.Dispose();
        Tasking.Dispose();
        Agencies.Dispose();
        Locations.Dispose();
        GTAWeapons.Dispose();
        Speed.Dispose();
        WeaponDropping.Dispose();
        Streets.Dispose();
        UI.Dispose();
        Debugging.Dispose();
        PlayerLocation.Dispose();
        Police.Dispose();
        PoliceSpawning.Dispose();
        TrafficViolations.Dispose();
        SearchModeStopping.Dispose();
        WeatherReporting.Dispose();
    }

    private static void UpdatePlayer()
    {
        PlayerInVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        if(PlayerInVehicle)
        {
            if (Game.LocalPlayer.Character.IsInAirVehicle || Game.LocalPlayer.Character.IsInSeaVehicle || Game.LocalPlayer.Character.IsOnBike)
                PlayerInAutomobile = false;
            else
                PlayerInAutomobile = true;
        }
        PlayerIsGettingIntoVehicle = Game.LocalPlayer.Character.IsGettingIntoVehicle;
        PlayerWantedLevel = Game.LocalPlayer.WantedLevel;
        PlayerIsConsideredArmed = Game.LocalPlayer.Character.isConsideredArmed();
        PlayerAimingInVehicle = PlayerInVehicle && Game.LocalPlayer.IsFreeAiming;
        WeaponDescriptor PlayerCurrentWeapon = Game.LocalPlayer.Character.Inventory.EquippedWeapon;
        if (PlayerCurrentWeapon != null)
            PlayerCurrentWeaponHash = PlayerCurrentWeapon.Hash;
        else
            PlayerCurrentWeaponHash = 0;

        if (PrevPlayerIsGettingIntoVehicle != PlayerIsGettingIntoVehicle)
            PlayerIsGettingIntoVehicleChanged();

        if (PlayerInVehicle && !IsCurrentVehicleTracked)
            TrackCurrentVehicle();

        if (PlayerCurrentWeaponHash != 0 && PlayerCurrentWeapon.Hash != LastWeapon)
            LastWeapon = PlayerCurrentWeapon.Hash;

        if (PrevPlayerAimingInVehicle != PlayerAimingInVehicle)
            PlayerAimingInVehicleChanged();

        if (PrevPlayerInVehicle != PlayerInVehicle)
            PlayerInVehicleChanged();
    }
    private static void StateTick()
    {
        if (Game.LocalPlayer.Character.IsDead && !IsDead)
            PlayerDeathEvent();

        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 0))
            BeingArrested = true;
        if (NativeFunction.CallByName<bool>("IS_PLAYER_BEING_ARRESTED", 1))
        {
            BeingArrested = true;
            Game.LocalPlayer.Character.Tasks.Clear();
        }

        if (BeingArrested && !IsBusted)
            PlayerBustedEvent();

        if (PlayerWantedLevel > MaxWantedLastLife) // The max wanted level i saw in the last life, not just right before being busted
            MaxWantedLastLife = PlayerWantedLevel;

        if (PedSwapping.JustTakenOver(1000) && PlayerWantedLevel > 0)//Right when you takeover a ped they might become wanted for some weird reason, this stops that
        {
            Police.SetWantedLevel(0,"Resetting wanted just after takeover");
        }
    }
    private static void TrackCurrentVehicle()
    {
        Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
        bool stolen = true;
        if (OwnedCar != null && OwnedCar.Handle == CurrVehicle.Handle)
            stolen = false;

        CurrVehicle.IsStolen = stolen;
        bool AmStealingCarFromPrerson = Police.PlayerIsJacking;
        Ped PreviousOwner;

        if (CurrVehicle.HasDriver && CurrVehicle.Driver.Handle != Game.LocalPlayer.Character.Handle)
            PreviousOwner = CurrVehicle.Driver;
        else
            PreviousOwner = CurrVehicle.GetPreviousPedOnSeat(-1);

        if (PreviousOwner != null && PreviousOwner.DistanceTo2D(Game.LocalPlayer.Character) <= 20f && PreviousOwner.Handle != Game.LocalPlayer.Character.Handle)
        {
            AmStealingCarFromPrerson = true;
        }
        GTALicensePlate MyPlate = new GTALicensePlate(CurrVehicle.LicensePlate, (uint)CurrVehicle.Handle, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", CurrVehicle), false);
        TrackedVehicles.Add(new GTAVehicle(CurrVehicle, Game.GameTime, AmStealingCarFromPrerson, CurrVehicle.IsAlarmSounding, PreviousOwner, !stolen, stolen, MyPlate));
    }
    private static void PlayerBustedEvent()
    {
        DiedInVehicle = PlayerInVehicle; //Game.LocalPlayer.Character.IsInAnyVehicle(false);
        IsBusted = true;
        BeingArrested = true;
        Game.LocalPlayer.Character.Tasks.Clear();
        NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPIn", 0, 0);//_START_SCREEN_EFFECT
        //Game.TimeScale = 0.4f;
        TransitionToSlowMo();
        HandsAreUp = false;
        Surrendering.SetArrestedAnimation(Game.LocalPlayer.Character, false);
        DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectArrested, 5, false));
        GameFiber HandleBusted = GameFiber.StartNew(delegate
        {
            GameFiber.Wait(1000);
            Menus.ShowBustedMenu();
        }, "HandleBusted");
        Debugging.GameFibers.Add(HandleBusted);
    }
    private static void PlayerDeathEvent()
    {
        DiedInVehicle = PlayerInVehicle;//Game.LocalPlayer.Character.IsInAnyVehicle(false);
        IsDead = true;
        NativeFunction.Natives.x2206BF9A37B7F724("DeathFailOut", 0, 0);//_START_SCREEN_EFFECT
        Game.LocalPlayer.Character.Kill();
        Game.LocalPlayer.Character.Health = 0;
        Game.LocalPlayer.Character.IsInvincible = true;
        Police.SetWantedLevel(0,"You died");
        //Game.TimeScale = 0.4f;
        TransitionToSlowMo();
        if (Police.PreviousWantedLevel > 0 || PoliceScanning.CopPeds.Any(x => x.isTasked))
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportSuspectWasted, 5, false));
        GameFiber HandleDeath = GameFiber.StartNew(delegate
        {
            GameFiber.Wait(1000);
            Menus.ShowDeathMenu();
        }, "HandleDeath");
        Debugging.GameFibers.Add(HandleDeath);
    }
    public static void PlayerIsGettingIntoVehicleChanged()
    {
        if (PlayerIsGettingIntoVehicle)
        {
            CarStealing.EnterVehicleEvent();
        }
        PrevPlayerIsGettingIntoVehicle = PlayerIsGettingIntoVehicle;
    }
    private static void PlayerInVehicleChanged()
    {
        if (PlayerInVehicle)
        {
            CarStealing.UpdateStolenStatus();
        }
        PrevPlayerInVehicle = PlayerInVehicle;
        LocalWriteToLog("ValueChecker", String.Format("PlayerInVehicle Changed to: {0}", PlayerInVehicle));
    }
    private static void PlayerAimingInVehicleChanged()
    {
        if (PlayerAimingInVehicle)
        {
             TrafficViolations.SetDriverWindow(true);
        }
        else
        {
            TrafficViolations.SetDriverWindow(false);
        }
        PrevPlayerAimingInVehicle = PlayerAimingInVehicle;
        LocalWriteToLog("ValueChecker", String.Format("PlayerAimingInVehicle Changed to: {0}", PlayerAimingInVehicle));
    }
    private static void ControlTick()
    {
        if (Game.IsKeyDownRightNow(Settings.SurrenderKey) && !Game.LocalPlayer.IsFreeAiming && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed < 2.5f))
        {
            if (!HandsAreUp && !IsBusted)
            {
                SetPedUnarmed(Game.LocalPlayer.Character, false);
                HandsUpPreviousPoliceState = Police.CurrentPoliceState;
                Surrendering.RaiseHands();
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 10f)
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
            }
        }
        else
        {
            if (HandsAreUp && !IsBusted)
            {
                HandsAreUp = false; // You put your hands down
                Police.CurrentPoliceState = HandsUpPreviousPoliceState;
                Game.LocalPlayer.Character.Tasks.Clear();
                if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
            }
        }
    }
    private static void AudioTick()
    {
        if (Settings.DisableAmbientScanner)
            NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
        if (Settings.WantedMusicDisable)
            NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
    }
    public static GTAWeapon GetCurrentWeapon()
    {
        ulong myHash = (ulong)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash;
        GTAWeapon CurrentGun = GTAWeapons.GetWeaponFromHash(myHash);//Weapons.Where(x => (WeaponHash)x.Hash == MyWeapon.Hash).FirstOrDefault();
        if (CurrentGun != null)
            return CurrentGun;
        else
            return null;
    }
    public static void SetPlayerToLastWeapon()
    {
        if (Game.LocalPlayer.Character.Inventory.EquippedWeapon != null && LastWeapon != 0)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeapon, true);
            LocalWriteToLog("SetPlayerToLastWeapon", LastWeapon.ToString());
        }
    }
    public static bool MovePedToCarPosition(Vehicle TargetVehicle, Ped PedToMove, float DesiredHeading, Vector3 PositionToMoveTo, bool StopDriver)
    {
        bool Continue = true;
        bool isPlayer = false;
        if (PedToMove == Game.LocalPlayer.Character)
            isPlayer = true;
        Ped Driver = TargetVehicle.Driver;
        Vector3 CarPosition = TargetVehicle.Position;
        NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);

        while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.15f && PedToMove.Heading.IsWithin(DesiredHeading - 5f, DesiredHeading + 5f)))
        {
            GameFiber.Yield();
            if (isPlayer && Extensions.IsMoveControlPressed())
            {
                Continue = false;
                break;
            }
            if (StopDriver && TargetVehicle.Driver != null)
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
        }
        if (!Continue)
        {
            PedToMove.Tasks.Clear();
            return false;
        }
        return true;
    }
    public static GTAVehicle GetPlayersCurrentTrackedVehicle()
    {
        if (!Game.LocalPlayer.Character.IsInAnyVehicle(false))
            return null;
        else
        {
            Vehicle CurrVehicle = Game.LocalPlayer.Character.CurrentVehicle;
            return TrackedVehicles.Where(x => x.VehicleEnt.Handle == CurrVehicle.Handle).FirstOrDefault();
        }

    }
    public static void SetPedUnarmed(Ped Pedestrian, bool SetCantChange)
    {
        if (!(Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, (uint)2725352035, true); //Unequip weapon so you don't get shot
            if (SetCantChange)
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
        }
    }
    public static WeaponVariation GetWeaponVariation(Ped WeaponOwner, uint WeaponHash)
    {
        int Tint = NativeFunction.CallByName<int>("GET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash);
        GTAWeapon MyGun = GTAWeapons.GetWeaponFromHash(WeaponHash);
        if (MyGun == null)
            return new WeaponVariation(Tint);

        List<WeaponVariation.WeaponComponent> Components = new List<WeaponVariation.WeaponComponent>();
        List<WeaponVariation.WeaponComponent> PossibleComponents = GTAWeapons.GetWeaponVariations(MyGun.Name);

        if (!Components.Any())
            return new WeaponVariation(Tint);

        foreach (WeaponVariation.WeaponComponent PossibleComponent in PossibleComponents)
        {
            if (NativeFunction.CallByName<bool>("HAS_PED_GOT_WEAPON_COMPONENT", WeaponOwner, WeaponHash, PossibleComponent.Hash))
            {
                Components.Add(new WeaponVariation.WeaponComponent(PossibleComponent.Name, PossibleComponent.HashKey, PossibleComponent.Hash, true));
            }

        }
        return new WeaponVariation(Tint, Components);

    }
    public static void ApplyWeaponVariation(Ped WeaponOwner, uint WeaponHash, WeaponVariation _WeaponVariation)
    {
        if (_WeaponVariation == null)
            return;
        NativeFunction.CallByName<bool>("SET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash, _WeaponVariation.Tint);
        GTAWeapon LookupGun = GTAWeapons.GetWeaponFromHash(WeaponHash);//Weapons.Where(x => x.Hash == WeaponHash).FirstOrDefault();
        if (LookupGun == null)
            return;
        List<WeaponVariation.WeaponComponent> PossibleComponents = GTAWeapons.GetWeaponVariations(LookupGun.Name);//WeaponComponentsLookup.Where(x => x.BaseWeapon == LookupGun.Name).ToList();
        foreach (WeaponVariation.WeaponComponent ToRemove in PossibleComponents)
        {
            NativeFunction.CallByName<bool>("REMOVE_WEAPON_COMPONENT_FROM_PED", WeaponOwner, WeaponHash, ToRemove.Hash);
        }


        foreach (WeaponVariation.WeaponComponent ToAdd in _WeaponVariation.Components)
        {
            NativeFunction.CallByName<bool>("GIVE_WEAPON_COMPONENT_TO_PED", WeaponOwner, WeaponHash, ToAdd.Hash);
        }
    }
    public static void RequestAnimationDictionay(String sDict)
    {
        NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
        while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
            GameFiber.Yield();
    }
    public static Rage.Object AttachScrewdriverToPed(Ped Pedestrian)
    {
        Rage.Object Screwdriver = new Rage.Object("prop_tool_screwdvr01", Pedestrian.GetOffsetPositionUp(50f));
        CreatedObjects.Add(Screwdriver);
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005);
        Screwdriver.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
        return Screwdriver;
    }
    public static void LoadInteriors()
    {
        //Pillbox hill hospital?
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Destroyed");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_HospitalInterior");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Default");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Fixed");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "RC12B_Default");//state 1 normal

        //Lifeinvader
        NativeFunction.CallByName<bool>("REQUEST_IPL", "facelobby");  // lifeinvader
        NativeFunction.CallByName<bool>("REMOVE_IPL", "facelobbyfake");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -340230128, -1042.518f, -240.6915f, 38.11796f, true, 0.0f, 0.0f, -1.0f);//_DOOR_CONTROL

        //    FIB Lobby      
        NativeFunction.CallByName<bool>("REQUEST_IPL", "FIBlobby");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "FIBlobbyfake");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1517873911, 106.3793f, -742.6982f, 46.51962f, false, 0.0f, 0.0f, 0.0f);
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -90456267, 105.7607f, -746.646f, 46.18266f, false, 0.0f, 0.0f, 0.0f);

        //Paleto Sheriff Office
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", -444.89068603515625f, 6013.5869140625f, 30.7164f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", -444.89068603515625f, 6013.5869140625f, 30.7164f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_sheriff2");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "cs1_16_sheriff_cap");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1501157055, -444.4985f, 6017.06f, 31.86633f, false, 0.0f, 0.0f, 0.0f);
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1501157055, -442.66f, 6015.222f, 31.86633f, false, 0.0f, 0.0f, 0.0f);

        //Sheriffs Office Sandy Shores
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", 1854.2537841796875f, 3686.738525390625f, 33.2671012878418f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", 1854.2537841796875f, 3686.738525390625f, 33.2671012878418f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_sheriff");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "sheriff_cap");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1765048490, 1855.685f, 3683.93f, 34.59282f, false, 0.0f, 0.0f, 0.0f);

        //    Tequila la       
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", -556.5089111328125f, 286.318115234375f, 81.1763f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", -556.5089111328125f, 286.318115234375f, 81.1763f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_rockclub");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, 993120320, -565.1712f, 276.6259f, 83.28626f, false, 0.0f, 0.0f, 0.0f);// front door
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, 993120320, -561.2866f, 293.5044f, 87.77851f, false, 0.0f, 0.0f, 0.0f);// back door

    }
    private static void LocalWriteToLog(string ProcedureString, string TextToLog)
    {
        if (Settings.GeneralLogging)
            Debugging.WriteToLog(ProcedureString, TextToLog);
    }
    public static void TransitionToSlowMo()
    {
        GameFiber Transition = GameFiber.StartNew(delegate
        {
            int WaitTime = 100;
            while (Game.TimeScale > 0.4f)
            {
                Game.TimeScale = Game.TimeScale - 0.05f;
                GameFiber.Wait(WaitTime);
                if (WaitTime <= 200)
                    WaitTime = WaitTime + 1;
            }

        }, "TransitionIn");
        Debugging.GameFibers.Add(Transition);
    }
    public static void TransitionToRegularSpeed()
    {
        GameFiber Transition = GameFiber.StartNew(delegate
        {
            int WaitTime = 100;
            while (Game.TimeScale < 1f)
            {
                Game.TimeScale = Game.TimeScale + 0.05f;
                GameFiber.Wait(WaitTime);
                if (WaitTime >= 12)
                    WaitTime = WaitTime - 1;
            }

        }, "TransitionOut");
        Debugging.GameFibers.Add(Transition);
    }
}
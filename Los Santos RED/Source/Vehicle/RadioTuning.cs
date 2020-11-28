﻿using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public static class RadioTuning
{
    private static string CurrentRadioStationName;
    public static bool AutoTune { get; private set; }
    public static string AutoTuneStation { get; set; }
    public static bool IsRunning { get; set; }
    public static bool WantedLevelTune { get; set; }
    public static bool CanChangeStation
    {
        get
        {
            if(Game.LocalPlayer.Character.IsInAnyVehicle(false) && !Game.LocalPlayer.Character.IsInHelicopter && !Game.LocalPlayer.Character.IsInPlane && !Game.LocalPlayer.Character.IsInBoat && Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn && !VehicleEngine.IsHotwiring)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public static void Initialize()
    {
        IsRunning = true;
        AutoTune = true;
        AutoTuneStation = "RADIO_19_USER";
        WantedLevelTune = false;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            EnablePoliceCarMusic();
            CheckAutoTuning();
        }
    }
    private static void EnablePoliceCarMusic()
    {
        if (PlayerState.IsInVehicle && VehicleEngine.IsEngineRunning && Game.LocalPlayer.Character.IsInAnyPoliceVehicle)
        {
           NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
        }
        else
        {
            NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
        }
    }
    private static void CheckAutoTuning()
    {
        if (PlayerState.IsInVehicle)
        {
            if (AutoTuneStation.ToUpper() != "NONE")
            {
                unsafe
                {
                    IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_PLAYER_RADIO_STATION_NAME");
                    CurrentRadioStationName = Marshal.PtrToStringAnsi(ptr);
                }
                if (CurrentRadioStationName != AutoTuneStation && Game.LocalPlayer.Character.CurrentVehicle != null)
                {
                    SetRadioStation(AutoTuneStation);
                }
            }
        }
    }
    public static void ChangeStation(string StationName)
    {
        if(CanChangeStation)
        {
            if (!Game.LocalPlayer.Character.IsOnBike)
            {
                ChangeStationAnimation(StationName);
            }
            else
            {
                SetRadioStation(StationName);
            }
        }
    }
    private static void ChangeStationAnimation(string StationName)
    {
        GameFiber.StartNew(delegate
        {
            var sDict = "veh@van@ds@base";
            NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
            while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
                GameFiber.Yield();
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "start_engine", 2.0f, -2.0f, -1, 48, 0, true, false, true);

            bool Cancel = false;
            uint GameTimeStartedAnimation = Game.GameTime;
            while (Game.GameTime - GameTimeStartedAnimation <= 1000)
            {
                if (Game.IsControlJustPressed(0, GameControl.VehicleExit))
                {
                    NativeFunction.CallByName<bool>("STOP_ANIM_TASK", Game.LocalPlayer.Character, sDict, "start_engine", 8.0f);
                    Cancel = true;
                }
                GameFiber.Sleep(200);
            }
            if (!Cancel)
                SetRadioStation(StationName);

        });
    }
    private static void SetRadioStation(string StationName)
    {
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle != null && Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
        {
            Debugging.WriteToLog("RadioTuning", string.Format("Tuned: {0} Desired: {1}", CurrentRadioStationName, StationName));
            NativeFunction.CallByName<bool>("SET_VEH_RADIO_STATION", Game.LocalPlayer.Character.CurrentVehicle, StationName);
        }
    }
}

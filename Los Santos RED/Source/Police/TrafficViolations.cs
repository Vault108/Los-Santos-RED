﻿using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;

public static class TrafficViolations
{
    private static uint GameTimeLastRanRed;
    private static uint GameTimeStartedDrivingOnPavement;
    private static uint GameTimeStartedDrivingAgainstTraffic;
    private static int TimeSincePlayerHitPed;
    private static int TimeSincePlayerHitVehicle;
    private static bool PlayersVehicleIsSuspicious;
    private static List<Vehicle> CloseVehicles;
    private static bool TreatAsCop;
    private static float CurrentSpeed;


    private static bool ShouldCheckViolations
    {
        get
        {
            if (PlayerState.IsInVehicle && Game.LocalPlayer.Character.IsInAnyVehicle(false) && (PlayerState.IsInAutomobile || PlayerState.IsOnMotorcycle) && !PedSwap.RecentlyTakenOver)
                return true;
            else
                return false;
        }
    }
    public static bool IsRunning { get; set; }
    public static bool PlayerIsSpeeding { get; set; }
    public static bool PlayerIsRunningRedLight { get; set; }
    public static bool RecentlyRanRed
    {
        get
        {
            if (GameTimeLastRanRed == 0)
                return false;
            else if (Game.GameTime - GameTimeLastRanRed <= 1000)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyHitPed
    {
        get
        {
            if (TimeSincePlayerHitPed > -1 && TimeSincePlayerHitPed <= 1000)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyHitVehicle
    {
        get
        {
            if (TimeSincePlayerHitVehicle > -1 && TimeSincePlayerHitVehicle <= 1000)
                return true;
            else
                return false;
        }
    }
    public static bool HasBeenDrivingAgainstTraffic
    {
        get
        {
            if (GameTimeStartedDrivingAgainstTraffic == 0)
                return false;
            else if (Game.GameTime - GameTimeStartedDrivingAgainstTraffic >= 1000)
                return true;
            else
                return false;
        }
    }
    public static bool HasBeenDrivingOnPavement
    {
        get
        {
            if (GameTimeStartedDrivingOnPavement == 0)
                return false;
            else if (Game.GameTime - GameTimeStartedDrivingOnPavement >= 1000)
                return true;
            else
                return false;
        }
    }
    public static bool ViolatingTrafficLaws
    {
        get
        {
            if (HasBeenDrivingAgainstTraffic || HasBeenDrivingOnPavement || PlayerIsRunningRedLight || PlayerIsSpeeding || PlayersVehicleIsSuspicious)
                return true;
            else
                return false;
        }
    }
    public static void Initialize()
    {
        GameTimeStartedDrivingOnPavement = 0;
        GameTimeStartedDrivingAgainstTraffic = 0;
        PlayersVehicleIsSuspicious = false;
        CloseVehicles = new List<Vehicle>();
        IsRunning = true;
        PlayerIsSpeeding = false;
        PlayerIsRunningRedLight = false;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            if (!General.MySettings.TrafficViolations.Enabled || PlayerState.IsBusted || PlayerState.IsDead)
            {
                ResetViolations();
                return;
            }

            if (ShouldCheckViolations)
            {
                UpdateTrafficStats();
                CheckViolations();
            }
            else
            {
                ResetViolations();
            }
        }
    }
    private static void UpdateTrafficStats()
    {
        CurrentSpeed = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
        PlayersVehicleIsSuspicious = false;
        TreatAsCop = false;
        PlayerIsSpeeding = false;

        if (!PlayerState.CurrentVehicle.VehicleEnt.IsRoadWorthy() || PlayerState.CurrentVehicle.VehicleEnt.IsDamaged())
            PlayersVehicleIsSuspicious = true;

        if (General.MySettings.TrafficViolations.ExemptCode3 && PlayerState.CurrentVehicle.VehicleEnt != null && PlayerState.CurrentVehicle.VehicleEnt.IsPoliceVehicle && PlayerState.CurrentVehicle != null && !PlayerState.CurrentVehicle.WasReportedStolen)
        {
            if (PlayerState.CurrentVehicle.VehicleEnt.IsSirenOn && !Police.AnyCanRecognizePlayer) //see thru ur disguise if ur too close
            {
                TreatAsCop = true;//Cops dont have to do traffic laws stuff if ur running code3?
            }
        }


        //Streets.ResetStreets();
        PlayerIsRunningRedLight = false;

        foreach (PedExt Civilian in PedList.Civilians.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.DistanceToPlayer))
        {
            Civilian.IsWaitingAtTrafficLight = false;
            Civilian.IsFirstWaitingAtTrafficLight = false;
            Civilian.PlaceCheckingInfront = Vector3.Zero;
            if (Civilian.DistanceToPlayer <= 250f && Civilian.IsInVehicle)
            {
                if (Civilian.Pedestrian.IsInAnyVehicle(false) && Civilian.Pedestrian.CurrentVehicle != null)
                {
                    Vehicle PedCar = Civilian.Pedestrian.CurrentVehicle;
                    if (NativeFunction.CallByName<bool>("IS_VEHICLE_STOPPED_AT_TRAFFIC_LIGHTS", PedCar))
                    {
                        Civilian.IsWaitingAtTrafficLight = true;

                        if (Extensions.FacingSameOrOppositeDirection(Civilian.Pedestrian, Game.LocalPlayer.Character) && Game.LocalPlayer.Character.InFront(Civilian.Pedestrian) && Civilian.DistanceToPlayer <= 10f && Game.LocalPlayer.Character.Speed >= 3f)
                        {
                            GameTimeLastRanRed = Game.GameTime;
                            PlayerIsRunningRedLight = true;
                        }
                    }
                }
            }
        }


        // UI.DebugLine = string.Format("PlayerIsRunningRedLight {0}", PlayerIsRunningRedLight);



        if (Game.LocalPlayer.IsDrivingOnPavement)
        {
            if (GameTimeStartedDrivingOnPavement == 0)
                GameTimeStartedDrivingOnPavement = Game.GameTime;
        }
        else
            GameTimeStartedDrivingOnPavement = 0;

        if (Game.LocalPlayer.IsDrivingAgainstTraffic)
        {
            if (GameTimeStartedDrivingAgainstTraffic == 0)
                GameTimeStartedDrivingAgainstTraffic = Game.GameTime;
        }
        else
            GameTimeStartedDrivingAgainstTraffic = 0;


        TimeSincePlayerHitPed = Game.LocalPlayer.TimeSincePlayerLastHitAnyPed;
        TimeSincePlayerHitVehicle = Game.LocalPlayer.TimeSincePlayerLastHitAnyVehicle;

        float SpeedLimit = 60f;
        if (PlayerLocation.PlayerCurrentStreet != null)
            SpeedLimit = PlayerLocation.PlayerCurrentStreet.SpeedLimit;

        PlayerIsSpeeding = CurrentSpeed > SpeedLimit + General.MySettings.TrafficViolations.SpeedingOverLimitThreshold;
    }
    private static void CheckViolations()
    {
        if (General.MySettings.TrafficViolations.HitPed && RecentlyHitPed && (PedDamage.RecentlyHurtCivilian || PedDamage.RecentlyHurtCop) && (PedList.Civilians.Any(x => x.DistanceToPlayer <= 10f) || PedList.Cops.Any(x => x.DistanceToPlayer <= 10f)))//needed for non humans that are returned from this native
        {
            Crimes.HitPedWithCar.IsCurrentlyViolating = true;
        }
        else
        {
            Crimes.HitPedWithCar.IsCurrentlyViolating = false;
        }
        if (General.MySettings.TrafficViolations.HitVehicle && RecentlyHitVehicle)
        {
            Crimes.HitCarWithCar.IsCurrentlyViolating = true;
        }
        else
        {
            Crimes.HitCarWithCar.IsCurrentlyViolating = false;
        }
        if (!TreatAsCop)
        {
            if (General.MySettings.TrafficViolations.DrivingAgainstTraffic && (HasBeenDrivingAgainstTraffic || (Game.LocalPlayer.IsDrivingAgainstTraffic && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
            {
                Crimes.DrivingAgainstTraffic.IsCurrentlyViolating = true;
            }
            else
            {
                Crimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
            }
            if (General.MySettings.TrafficViolations.DrivingOnPavement && (HasBeenDrivingOnPavement || (Game.LocalPlayer.IsDrivingOnPavement && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 10f)))
            {
                Crimes.DrivingOnPavement.IsCurrentlyViolating = true;
            }
            else
            {
                Crimes.DrivingOnPavement.IsCurrentlyViolating = false;
            }

            if (General.MySettings.TrafficViolations.NotRoadworthy && PlayersVehicleIsSuspicious)
            {
                Crimes.NonRoadworthyVehicle.IsCurrentlyViolating = true;
            }
            else
            {
                Crimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
            }
            if (General.MySettings.TrafficViolations.Speeding && PlayerIsSpeeding)
            {
                Crimes.FelonySpeeding.IsCurrentlyViolating = true;
            }
            else
            {
                Crimes.FelonySpeeding.IsCurrentlyViolating = false;
            }
            if (General.MySettings.TrafficViolations.RunningRedLight && RecentlyRanRed)
            {
                // Crimes.RunningARedLight.IsCurrentlyViolating = true;//turned off for now until i fix it
            }
            else
            {
                Crimes.RunningARedLight.IsCurrentlyViolating = false;
            }
        }

    }
    private static void ResetViolations()
    {
        GameTimeStartedDrivingOnPavement = 0;
        GameTimeStartedDrivingAgainstTraffic = 0;

        TreatAsCop = false;
        PlayerIsSpeeding = false;
        PlayerIsRunningRedLight = false;
        PlayersVehicleIsSuspicious = false;
        CurrentSpeed = 0f;

        Crimes.HitCarWithCar.IsCurrentlyViolating = false;
        Crimes.HitPedWithCar.IsCurrentlyViolating = false;
        Crimes.DrivingOnPavement.IsCurrentlyViolating = false;
        Crimes.DrivingAgainstTraffic.IsCurrentlyViolating = false;
        Crimes.NonRoadworthyVehicle.IsCurrentlyViolating = false;
        Crimes.FelonySpeeding.IsCurrentlyViolating = false;
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerSettings
{
    public uint AlarmedCarTimeToReportStolen { get; set; } = 100000;
    public uint NonAlarmedCarTimeToReportStolen { get; set; } = 600000;
    public bool KeepRadioStationAutoTuned { get; set; } = false;
    public string AutoTuneRadioStation { get; set; } = "NONE";    
    public uint Recognize_BaseTime { get; set; } = 2000;
    public uint Recognize_NightPenalty { get; set; } = 3500;
    public uint Recognize_VehiclePenalty { get; set; } = 750;
    public bool DisableAutoEngineStart { get; set; } = true;
    public bool UseCustomFuelSystem { get; set; } = false;
    public bool Scanner_IsEnabled { get; set; } = true;
    public bool Scanner_EnableAudio { get; set; } = true;
    public int Scanner_AudioVolume { get; set; } = 5;
    public bool Scanner_EnableSubtitles { get; set; } = false;
    public bool Scanner_EnableNotifications { get; set; } = true;
    public int Scanner_DelayMinTime { get; set; } = 1500;
    public int Scanner_DelayMaxTime { get; set; } = 2500;
    public bool Scanner_AllowStatusAnnouncements { get; set; } = true;
    public bool Scanner_UseNearForLocations { get; set; } = false;
    public uint Violations_RecentlyHurtCivilianTime { get; set; } = 5000;
    public uint Violations_RecentlyHurtPoliceTime { get; set; } = 5000;
    public uint Violations_RecentlyKilledCivilianTime { get; set; } = 5000;
    public uint Violations_RecentlyKilledPoliceTime { get; set; } = 5000;
    public float Violations_MurderDistance { get; set; } = 9f;
    public uint Violations_RecentlyDrivingAgainstTraffiTime { get; set; } = 1000;
    public uint Violations_RecentlyDrivingOnPavementTime { get; set; } = 1000;
    public uint Violations_RecentlyHitPedTime { get; set; } = 1500;
    public uint Violations_RecentlyHitVehicleTime { get; set; } = 1500;
    public uint Violations_ResistingArrestFastTriggerTime { get; set; } = 5000;
    public uint Violations_ResistingArrestMediumTriggerTime { get; set; } = 10000;
    public uint Violations_ResistingArrestSlowTriggerTime { get; set; } = 25000;
    public bool Violations_TreatAsCop { get; set; } = false;
    public float Investigation_ActiveDistance { get; set; } = 800f;
    public uint Investigation_TimeLimit { get; set; } = 60000;
    public float Investigation_MaxDistance { get; set; } = 1500f;
    public float Investigation_SuspiciousDistance { get; set; } = 250f;
    public bool Investigation_CreateBlip { get; set; } = true;
    public uint CriminalHistory_MaxTime { get; set; } = 120000;
    public int CriminalHistory_MaxCalendarHours { get; set; } = 24;
    public float CriminalHistory_MinimumSearchRadius { get; set; } = 400f;
    public float CriminalHistory_SearchRadiusIncrement { get; set; } = 400f;
    public bool SearchMode_FakeActiveWanted { get; set; } = true;
    public uint SearchMode_SearchTimeMultiplier { get; set; } = 30000;
    public bool AllowStartRandomScenario { get; set; } = false;
    public bool SetSlowMoOnDeath { get; set; } = true;
    public bool SetSlowMoOnBusted { get; set; } = true;
    public bool AllowConsumeWithoutInventory { get; set; } = false;
    public bool AllowSetEngineState { get; set; } = true;
    public bool ScaleEngineDamage { get; set; } = true;
    public float ScaleEngineDamageMultiplier { get; set; } = 3.0f;
    public bool AllowSetIndicatorState { get; set; } = true;
    public bool AllowWeaponDropping { get; set; } = true;


    public PlayerSettings()
    {
        #if DEBUG
                AutoTuneRadioStation = "RADIO_19_USER";
                KeepRadioStationAutoTuned = true;
        #endif
    }

}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GangSettings : ISettingsDefaultable
{
    public bool ManageTasking { get; set; }
    public float FightPercentage { get; set; }
    public bool CheckCrimes { get; set; }
    public float DrugDealerPercentage { get; set; }
    public bool ShowSpawnedBlip { get; set; }
    public bool RemoveVanillaSpawnedPeds { get; set; }
    public int PercentSpawnOutsideTerritory { get; set; }
    public bool ManageDispatching { get; set; }
    public bool RemoveVanillaSpawnedPedsOutsideTerritory { get; set; }
    public int TimeBetweenSpawn { get; set; }
    public float MaxDistanceToSpawn { get; set; }
    public float MinDistanceToSpawn { get; set; }
    public int TotalSpawnedMembersLimit { get; set; }

    public int MoneyMin { get; set; }
    public int MoneyMax { get; set; }
    public float VehicleSpawnPercentage { get; set; }

    public GangSettings()
    {
        SetDefault();
#if DEBUG
        ShowSpawnedBlip = true;
        //RemoveVanillaGangs = true;
        RemoveVanillaSpawnedPedsOutsideTerritory = false;
#else
               // ShowSpawnedBlips = false;
#endif
    }
    public void SetDefault()
    {
        ManageTasking = true;
        FightPercentage = 70f;
        CheckCrimes = true;
        DrugDealerPercentage = 40f;
        ShowSpawnedBlip = false;
        RemoveVanillaSpawnedPeds = false;
        PercentSpawnOutsideTerritory = 10;
        ManageDispatching = true;
        RemoveVanillaSpawnedPedsOutsideTerritory = false;
        TimeBetweenSpawn = 10000;
        MinDistanceToSpawn = 50f;
        MaxDistanceToSpawn = 150f;
        TotalSpawnedMembersLimit = 8;//5
        MoneyMin = 500;
        MoneyMax = 5000;
        VehicleSpawnPercentage = 40;
    }

}
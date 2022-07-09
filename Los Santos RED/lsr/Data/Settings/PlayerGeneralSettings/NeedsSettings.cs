﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NeedsSettings : ISettingsDefaultable
{
    [Description("Enable or disable the entire needs system")]
    public bool ApplyNeeds { get; set; }
    [Description("Enable or diable the thirst component of the needs system")]
    public bool ApplyThirst { get; set; }
    [Description("Change the intensity of the drain and recovery for thirst. Default 1.0")]
    public float ThirstChangeScalar { get; set; }
    [Description("Enable or diable the hunger component of the needs system")]
    public bool ApplyHunger { get; set; }
    [Description("Change the intensity of the drain and recovery for hunger. Default 1.0")]
    public float HungerChangeScalar { get; set; }
    [Description("Enable or diable the sleep component of the needs system")]
    public bool ApplySleep { get; set; }
    [Description("Change the intensity of the drain and recovery for sleep. Default 1.0")]
    public float SleepChangeScalar { get; set; }

    [Description("Changes the amount of digits seen on the hunger ui")]
    public int HungerDisplayDigits { get; set; }
    [Description("Changes the amount of digits seen on the thirst ui")]
    public int ThirstDisplayDigits { get; set; }
    [Description("Changes the amount of digits seen on the sleep ui")]
    public int SleepDisplayDigits { get; set; }
    public NeedsSettings()
    {
        SetDefault();

    }
    public void SetDefault()
    {
        ApplyNeeds = true;
        ApplyThirst = true;
        ThirstChangeScalar = 1.0f;
        ThirstDisplayDigits = 0;
        ApplyHunger = true;
        HungerChangeScalar = 1.0f;
        HungerDisplayDigits = 0;
        ApplySleep = true;
        SleepChangeScalar = 1.0f;
        SleepDisplayDigits = 0;

#if DEBUG
        ThirstDisplayDigits = 2;
        HungerDisplayDigits = 2;
        SleepDisplayDigits = 2;
#endif
    }

}
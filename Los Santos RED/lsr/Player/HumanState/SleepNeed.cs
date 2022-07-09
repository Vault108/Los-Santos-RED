﻿using LosSantosRED.lsr.Interface;
using System;


public class SleepNeed : HumanNeed
{
    private IHumanStateable Player;
    private float MinChangeValue = -0.002f;
    private DateTime TimeLastUpdatedValue;
    private ITimeReportable Time;
    private float RealTimeScalar;
    private ISettingsProvideable Settings;
    private bool ShouldRecover => Player.IsResting || Player.IsSleeping;
    private bool ShouldChange => Player.IsAlive;
    private bool ShouldSlowDrain => Player.IsResting || Player.IsSitting || Player.IsLayingDown;
    public SleepNeed(string name, float minValue, float maxValue, IHumanStateable humanStateable, ITimeReportable time, ISettingsProvideable settings) : base(name, minValue, maxValue, humanStateable, time, settings.SettingsManager.NeedsSettings.SleepDisplayDigits)
    {
        Player = humanStateable;
        Time = time;
        TimeLastUpdatedValue = Time.CurrentDateTime;
        Settings = settings;
    }

    public override void OnMaximum()
    {

    }

    public override void OnMinimum()
    {

    }

    public override void Update()
    {
        if (NeedsUpdate && Settings.SettingsManager.NeedsSettings.ApplySleep)
        {
            UpdateRealTimeScalar();
            if (ShouldChange)
            {
                if (ShouldRecover)
                {
                    Recover();
                }
                else
                {
                    Drain();
                }
            }
        }
    }
    private void UpdateRealTimeScalar()//move this into the base class?
    {
        RealTimeScalar = 1.0f;
        TimeSpan TimeDifference = Time.CurrentDateTime - TimeLastUpdatedValue;
        RealTimeScalar = (float)TimeDifference.TotalSeconds;
        TimeLastUpdatedValue = Time.CurrentDateTime;
    }
    private void Drain()
    {
        float ChangeAmount = MinChangeValue * RealTimeScalar;
        if (ShouldSlowDrain)
        {
            ChangeAmount *= 0.5f;
        }
        if (!Player.IsInVehicle)
        {
            ChangeAmount *= FootSpeedMultiplier();
        }
        Change(ChangeAmount, false);
    }
    private float FootSpeedMultiplier()
    {
        float Multiplier = 1.0f;
        if (Player.FootSpeed >= 1.0f)
        {
            Multiplier = Player.FootSpeed / 5.0f;
        }
        if (Multiplier <= 1.0f)
        {
            Multiplier = 1.0f;
        }
        return Multiplier;
    }
    private void Recover()
    {
        float ChangeAmount = Math.Abs(MinChangeValue) * RealTimeScalar;
        Change(ChangeAmount, false);
    }
}


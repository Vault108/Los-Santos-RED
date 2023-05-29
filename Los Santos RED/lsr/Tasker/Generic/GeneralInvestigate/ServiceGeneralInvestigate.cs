﻿using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ServiceGeneralInvestigate : GeneralInvestigate
{
    public ServiceGeneralInvestigate(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents,
        IWeaponIssuable weaponIssuable) : base(pedGeneral, ped, player, world, possibleVehicles, placesOfInterest, settings, blockPermanentEvents, weaponIssuable)
    {

    }
    private bool IsRespondingCode3 => Player.Investigation.IsActive;
    protected override bool ShouldInvestigateOnFoot => !Ped.IsInHelicopter;
    protected override bool ForceSetArmed => false;
    protected override void UpdateVehicleState()
    {
        if (!Ped.IsInVehicle || !Ped.Pedestrian.Exists())
        {
            return;
        }
        if (Settings.SettingsManager.PoliceTaskSettings.AllowSettingSirenState && Ped.Pedestrian.Exists() && Ped.Pedestrian.CurrentVehicle.Exists() && Ped.Pedestrian.CurrentVehicle.HasSiren)
        {
            if (IsRespondingCode3)
            {
                if (!Ped.Pedestrian.CurrentVehicle.IsSirenOn)
                {
                    Ped.Pedestrian.CurrentVehicle.IsSirenOn = true;
                    Ped.Pedestrian.CurrentVehicle.IsSirenSilent = false;
                }
            }
            else
            {
                if (Ped.Pedestrian.CurrentVehicle.IsSirenOn)
                {
                    Ped.Pedestrian.CurrentVehicle.IsSirenOn = false;
                    Ped.Pedestrian.CurrentVehicle.IsSirenSilent = true;
                }
            }
        }
    }
    protected override void GetLocations()
    {
        if (Player.Investigation.IsActive)
        {
            PlaceToDriveTo = Player.Investigation.Position;
            PlaceToWalkTo = Player.Investigation.Position;
        }
        else if (Ped.PedAlerts.IsAlerted)
        {
            PlaceToDriveTo = Ped.PedAlerts.AlertedPoint;
            PlaceToWalkTo = Ped.PedAlerts.AlertedPoint;
        }
    }
    public override void OnLocationReached()
    {
        Ped.GameTimeReachedInvestigationPosition = Game.GameTime;
        HasReachedLocatePosition = true;
        EntryPoint.WriteToConsole($"{PedGeneral.Handle} Police Located HasReachedLocatePosition");
    }
}


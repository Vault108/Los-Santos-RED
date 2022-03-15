﻿using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EMTTasker
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    private List<PedExt> PossibleTargets;
    public EMTTasker(Tasker tasker, IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Tasker = tasker;
        PedProvider = pedProvider;
        Player = player;
        Weapons = weapons;
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
    }
    public void Setup()
    {
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@enter");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@base");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@exit");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@idle_a");
    }
    public void Update()
    {
        if (Settings.SettingsManager.EMSSettings.ManageTasking)
        {
            SetPossibleTargets();
            Tasker.ExpireSeatAssignments();

            foreach (EMT emt in PedProvider.Pedestrians.EMTs.Where(x => x.Pedestrian.Exists() && x.HasBeenSpawnedFor >= 2000 && x.CanBeTasked).ToList())
            {
                try
                {
                    if (emt.NeedsTaskAssignmentCheck)
                    {
                        ReAssignTask(emt);//has yields if it does anything
                    }
                    if (emt.CurrentTask != null && emt.CurrentTask.ShouldUpdate)
                    {
                        emt.UpdateTask(PedToGoTo(emt));
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
        }
    }
    private void ReAssignTask(EMT emt)
    {
        if (emt.DistanceToPlayer <= 150f)
        {
            WitnessedCrime HighestPriority = emt.OtherCrimesWitnessed.OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
            bool SeenScaryCrime = emt.PlayerCrimesWitnessed.Any(x => x.ScaresCivilians && x.CanBeReportedByCivilians) || emt.OtherCrimesWitnessed.Any(x => x.Crime.ScaresCivilians && x.Crime.CanBeReportedByCivilians);
            bool SeenAngryCrime = emt.PlayerCrimesWitnessed.Any(x => x.AngersCivilians && x.CanBeReportedByCivilians) || emt.OtherCrimesWitnessed.Any(x => x.Crime.AngersCivilians && x.Crime.CanBeReportedByCivilians);
            bool SeenMundaneCrime = emt.PlayerCrimesWitnessed.Any(x => !x.AngersCivilians && !x.ScaresCivilians && x.CanBeReportedByCivilians) || emt.OtherCrimesWitnessed.Any(x => !x.Crime.AngersCivilians && !x.Crime.ScaresCivilians && x.Crime.CanBeReportedByCivilians);

            PedExt MainTarget = PedToGoTo(emt);

            if (SeenScaryCrime || SeenAngryCrime)
            {
                SetScaredTask(emt, HighestPriority);
            }
            else if (SeenMundaneCrime)
            {
                SetCallInTask(emt);
            }
            else if (MainTarget != null)
            {
                SetTreatTask(emt, MainTarget);
            }
            else if (Player.Investigation.IsActive && Player.Investigation.RequiresEMS)
            {
                SetRespondTask(emt);
            }
            else
            {
                SetIdleTask(emt);
            }
        }
        else if (emt.DistanceToPlayer <= 1200f && Player.Investigation.IsActive && Player.Investigation.RequiresEMS)
        {
            SetRespondTask(emt);
        }
        else
        {
            SetIdleTask(emt);
        }
        emt.GameTimeLastUpdatedTask = Game.GameTime;
    }
    private void SetCallInTask(EMT emt)
    {
        if (emt.CurrentTask?.Name != "CalmCallIn")
        {
            emt.CurrentTask = new CalmCallIn(emt, Player);//oither target not needed, they just call in all crimes
            GameFiber.Yield();//TR Added back 7
            emt.CurrentTask.Start();
        }
    }
    private void SetScaredTask(EMT emt, WitnessedCrime HighestPriority)
    {
        if (emt.CurrentTask?.Name != "ScaredCallIn")
        {
            emt.CurrentTask = new ScaredCallIn(emt, Player) { OtherTarget = HighestPriority?.Perpetrator };
            GameFiber.Yield();//TR Added back 7
            emt.CurrentTask.Start();
        }
    }
    private void SetIdleTask(EMT emt)
    {
        if (emt.CurrentTask?.Name != "EMTIdle")// && Cop.IsIdleTaskable)
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {emt.Pedestrian.Handle} Task Changed from {emt.CurrentTask?.Name} to EMTIdle", 3);
            emt.CurrentTask = new EMTIdle(emt, Player, PedProvider, Tasker, PlacesOfInterest, emt);
            GameFiber.Yield();//TR Added back 4
            emt.CurrentTask.Start();
        }
    }
    private void SetRespondTask(EMT emt)
    {
        if (emt.CurrentTask?.Name != "EMTRespond")// && Cop.IsIdleTaskable)
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {emt.Pedestrian.Handle} Task Changed from {emt.CurrentTask?.Name} to EMTRespond", 3);
            emt.CurrentTask = new EMTRespond(emt, Player);
            GameFiber.Yield();//TR Added back 4
            emt.CurrentTask.Start();
        }
    }
    private void SetTreatTask(EMT emt, PedExt targetPed)
    {
        if (emt.CurrentTask?.Name != "EMTTreat" || (targetPed != null && emt.CurrentTask?.OtherTarget?.Handle != targetPed.Handle))// && Cop.IsIdleTaskable)
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {emt.Pedestrian.Handle} Task Changed from {emt.CurrentTask?.Name} to EMTTreat", 3);
            emt.CurrentTask = new EMTTreat(emt, Player, targetPed);
            GameFiber.Yield();//TR Added back 4
            emt.CurrentTask.Start();
        }
    }
    private PedExt PedToGoTo(EMT emt)
    {
        PedExt MainTarget = null;
        if (emt.Pedestrian.Exists())
        {
            List<EMTTarget> EMTsPossibleTargets = new List<EMTTarget>();
            foreach (PedExt possibleTarget in PossibleTargets)
            {
                int TotalOtherAssignedEMTs = PedProvider.Pedestrians.EMTList.Count(x => x.Handle != emt.Handle && x.CurrentTask != null && x.CurrentTask.OtherTarget != null && x.CurrentTask.OtherTarget.Handle == possibleTarget.Handle);
                if (possibleTarget.Pedestrian.Exists())
                {
                    float distanceTo = possibleTarget.Pedestrian.DistanceTo2D(emt.Pedestrian);
                    if (distanceTo <= 60f)
                    {
                        EMTsPossibleTargets.Add(new EMTTarget(possibleTarget, distanceTo) { TotalAssignedEMTs = TotalOtherAssignedEMTs });
                    }
                }
            }

            int PossibleTargetCount = EMTsPossibleTargets.Count();
            EMTTarget MainPossibleTarget = EMTsPossibleTargets
                .OrderBy(x => x.IsOverloaded)
                .ThenBy(x => x.DistanceToTarget).FirstOrDefault();

            if(MainPossibleTarget != null && (!MainPossibleTarget.IsOverloaded || PossibleTargetCount == 1))
            {
                MainTarget = MainPossibleTarget.Target;
            }
        }       
        return MainTarget;
    }
    private void SetPossibleTargets()
    {
        PossibleTargets = PedProvider.Pedestrians.PedExts.Where(x => x.Pedestrian.Exists() && (x.IsUnconscious || x.IsInWrithe) && !x.HasBeenTreatedByEMTs).ToList();//150f//writhe peds that are still alive
        //PossibleTargets.AddRange(PedProvider.Pedestrians.DeadPeds.Where(x => x.Pedestrian.Exists() && !x.HasBeenTreatedByEMTs));//dead peds go here?
    }
    private class EMTTarget
    {
        public EMTTarget(PedExt target, float distanceToTarget)
        {
            Target = target;
            DistanceToTarget = distanceToTarget;
        }
        public PedExt Target { get; set; }
        public float DistanceToTarget { get; set; } = 999f;
        public int TotalAssignedEMTs { get; set; } = 0;
        public bool IsOverloaded => TotalAssignedEMTs >= 1;
    }
}


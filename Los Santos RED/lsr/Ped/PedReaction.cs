﻿using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PedReaction
{
    private ITargetable Player;
    private PedExt ReactingPed;
    private bool IsOtherExpired;
    public PedReaction(ITargetable player, PedExt reactingPed, PedExt reactingToPed, uint gameTimeLastReacted, ReactionTier reactionTier)
    {
        Player = player;
        ReactingPed = reactingPed;
        ReactingToPed = reactingToPed;
        IsReactingToPlayer = false;
        GameTimeLastReacted = gameTimeLastReacted;
        ReactionTier = reactionTier;
    }

    public PedReaction(ITargetable player, PedExt reactingPed, uint gameTimeLastReacted, ReactionTier reactionTier)
    {
        Player = player;
        ReactingPed = reactingPed;
        IsReactingToPlayer = true;
        GameTimeLastReacted = gameTimeLastReacted;
        ReactionTier = reactionTier;
    }

    public bool IsReactingToPlayer { get; set; }
    public PedExt ReactingToPed { get; set; }
    public uint GameTimeLastReacted { get; set; }
    public ReactionTier ReactionTier { get; set; } = ReactionTier.None;
    public Vector3 PlaceLastReacted { get; set; }
    public bool IsExpired => IsTimedOut || IsDistanceExpired || IsOtherExpired;
    public bool IsTimedOut
    {
        get
        {
            if (ReactionTier == ReactionTier.None)
            {
                return Game.GameTime - GameTimeLastReacted >= 5000;
            }
            else if (ReactionTier == ReactionTier.Mundane)
            {
                return Game.GameTime - GameTimeLastReacted >= 10000;
            }
            else if (ReactionTier == ReactionTier.Alerted)
            {
                return Game.GameTime - GameTimeLastReacted >= 15000;
            }
            else if (ReactionTier == ReactionTier.Intense)
            {
                return Game.GameTime - GameTimeLastReacted >= 20000;
            }
            return false;
        }
    }
    public bool IsDistanceExpired { get; private set; }
    public void Update()
    {
        if(IsReactingToPlayer)
        {
            UpdatePlayer();
        }
        else
        { 
            UpdateNPC();
        }
    }
    private void UpdateNPC()
    {
        if (!ReactingToPed.Pedestrian.Exists())
        {
            IsOtherExpired = true;
            EntryPoint.WriteToConsole($"Ped Reaction {ReactingPed.Handle} NPC Expired - Doesnt Exist");
            return;
        }
        if (ReactingToPed.IsDead)
        {
            IsOtherExpired = true;
            EntryPoint.WriteToConsole($"Ped Reaction {ReactingPed.Handle} to {ReactingToPed.Handle} NPC Expired - Dead");
            return;
        }
        //if(ReactingToPed.WantedLevel >= 3)
        //{
        //    IsOtherExpired = true;
        //    EntryPoint.WriteToConsole($"Ped Reaction {ReactingPed.Handle} to {ReactingToPed.Handle} NPC Expired - wanted level");
        //    return;
        //}
        if(ReactingToPed.IsBusted)
        {
            IsOtherExpired = true;
            EntryPoint.WriteToConsole($"Ped Reaction {ReactingPed.Handle} to {ReactingToPed.Handle} NPC Expired - busted");
            return;
        }
        if(!NativeHelper.IsNearby(ReactingPed.CellX, ReactingPed.CellY,ReactingToPed.CellX,ReactingToPed.CellY,3))
        {
            IsDistanceExpired = true;
            EntryPoint.WriteToConsole($"Ped Reaction {ReactingPed.Handle} to {ReactingToPed.Handle} NPC Expired - distance");
            return;
        }
    }
    private void UpdatePlayer()
    {
        if(!Player.IsAliveAndFree)
        {
            IsOtherExpired = true;
            EntryPoint.WriteToConsole($"Ped Reaction {ReactingPed.Handle} Player Expired General State");
            return;
        }
        if (!NativeHelper.IsNearby(ReactingPed.CellX, ReactingPed.CellY, Player.CellX, Player.CellY, 3))
        {
            IsDistanceExpired = true;
            EntryPoint.WriteToConsole($"Ped Reaction {ReactingPed.Handle} Player Expired Distance");
            return;
        }
    }
}


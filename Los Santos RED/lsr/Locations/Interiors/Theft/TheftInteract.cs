﻿using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Xml.Serialization;
using RAGENativeUI;
using LosSantosRED.lsr;
using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using System.Drawing.Text;
using System.Windows.Forms;

public class TheftInteract : InteriorInteract
{
    protected virtual bool CanInteract => false;

    public string StealPrimptText { get; set; } = "Steal";
    public string EmptyText { get; set; } = "Empty";

    public uint IncrementGameTimeMin { get; set; } = 900;
    public uint IncrementGameTimeMax { get; set; } = 900;
    public int SpawnPercent { get; set; }

   public uint GameTimeBeforeInitialReward { get; set; }

    public string IntroAnimationDictionary { get; set; }
    public string IntroAnimation { get; set; }
    public string LoopAnimationDictionary { get; set; }
    public string LoopAnimation { get; set; }



    public string ViolatingCrimeID { get; set; }

    public TheftInteract()
    {

    }

    public TheftInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    public override void Setup(IModItems modItems)
    {

    }

    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera(false);
        if (!WithWarp)
        {
            if (!MoveToPosition(3.0f))
            {
                Interior.IsMenuInteracting = false;
                Game.DisplayHelp("Interact Failed");
                LocationCamera?.StopImmediately(true);
                return;
            }
        }
        if (!CanInteract)
        {
            Interior.IsMenuInteracting = false;
            Game.DisplaySubtitle(EmptyText);
            LocationCamera?.StopImmediately(true);
            return;
        }
        PerformAnimation();
        Interior.IsMenuInteracting = false;
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);
    }
    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
    public bool StopPerformingAnimation()
    {
        return true;
    }
    private bool PerformAnimation()
    {
        Player.ActivityManager.StopDynamicActivity();
        SetupAnimations();
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, IntroAnimationDictionary, IntroAnimation, 4.0f, -4.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, LoopAnimationDictionary, LoopAnimation, 4.0f, -4.0f, -1, 1, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        bool IsCancelled = false;
        if (!string.IsNullOrEmpty(ViolatingCrimeID))
        {
            Player.Violations.SetContinuouslyViolating(ViolatingCrimeID);
        }
        uint GameTimeStarted = Game.GameTime;
        if(GameTimeBeforeInitialReward > 0)
        {
            while(Game.GameTime <= GameTimeStarted + GameTimeBeforeInitialReward)
            {
                GameFiber.Yield();
            }
        }

        uint GameTimeGotReward = Game.GameTime;
        uint IncrementGameTime = RandomItems.GetRandomNumber(IncrementGameTimeMin,IncrementGameTimeMax);
        while (Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            if (Game.GameTime - GameTimeGotReward >= IncrementGameTime)
            {
                GiveReward();
                GameTimeGotReward = Game.GameTime;
                IncrementGameTime = RandomItems.GetRandomNumber(IncrementGameTimeMin, IncrementGameTimeMax);
            }
            if (!CanInteract)//CashMinAmount <= 0)
            {
                break;
            }
            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                IsCancelled = true;
                break;
            }
            GameFiber.Yield();
        }
        EntryPoint.WriteToConsole($"TheftInteract IsCancelled: {IsCancelled}");
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        if (!string.IsNullOrEmpty(ViolatingCrimeID))
        {
            Player.Violations.StopContinuouslyViolating(ViolatingCrimeID);
        }
        if (IsCancelled)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void SetupAnimations()
    {
        if (string.IsNullOrEmpty(IntroAnimationDictionary))
        {
            IntroAnimationDictionary = "oddjobs@shop_robbery@rob_till";
        }
        if (string.IsNullOrEmpty(IntroAnimation))
        {
            IntroAnimation = "enter";
        }
        if (string.IsNullOrEmpty(LoopAnimationDictionary))
        {
            LoopAnimationDictionary = "oddjobs@shop_robbery@rob_till";
        }
        if (string.IsNullOrEmpty(LoopAnimation))
        {
            LoopAnimation = "loop";
        }
        if (!string.IsNullOrEmpty(IntroAnimationDictionary))
        {
            AnimationDictionary.RequestAnimationDictionay(IntroAnimationDictionary);
        }
        if (!string.IsNullOrEmpty(LoopAnimationDictionary))
        {
            AnimationDictionary.RequestAnimationDictionay(LoopAnimationDictionary);
        }
    }
    protected virtual void GiveReward()
    {

    }
}


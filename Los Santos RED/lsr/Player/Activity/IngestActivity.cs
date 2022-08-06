﻿using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class IngestActivity : DynamicActivity
    {
        private Intoxicant CurrentIntoxicant;
        private EatingData Data;
        private bool hasGainedHP = false;
        private IIntoxicants Intoxicants;
        private bool IsAttachedToHand;
        private bool IsCancelled;
        private Rage.Object Item;
        private IIntoxicatable Player;
        private string PlayingAnim;
        private string PlayingDict;
        private ISettingsProvideable Settings;
        public IngestActivity(IIntoxicatable consumable, ISettingsProvideable settings, ModItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            Intoxicants = intoxicants;
        }
        public override string DebugString => $"";
        public override ModItem ModItem { get; set; }
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Taking Pills";
        public override string CancelPrompt { get; set; } = "Stop Taking Pills";
        public override string ContinuePrompt { get; set; } = "Continue Taking Pills";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
        }
        public override void Continue()
        {
        }
        public override void Pause()
        {
            Cancel();//for now it just cancels
        }
        public override bool IsPaused() => false;
        public override void Start()
        {
            Setup();
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "DrinkingWatcher");
        }
        private void AttachItemToHand()
        {
            CreateItem();
            if (Item.Exists() && !IsAttachedToHand)
            {
                Item.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.HandBoneName), Data.HandOffset, Data.HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp = Item;
            }
        }
        private void CreateItem()
        {
            if (!Item.Exists() && Data.PropModelName != "")
            {
                try
                {
                    Item = new Rage.Object(Data.PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception e)
                {
                    Game.DisplayNotification($"Could Not Spawn Prop {Data.PropModelName}");
                }
                if (Item.Exists())
                {
                    Item.IsGravityDisabled = false;
                }
                else
                {
                    IsCancelled = true;
                }
            }
        }
        private void Enter()
        {
            Player.WeaponEquipment.SetUnarmed();
            AttachItemToHand();
            Player.IsPerformingActivity = true;
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
            Idle();
        }
        private void Exit()
        {
            if (Item.Exists())
            {
                Item.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.IsPerformingActivity = false;
            if (!CurrentIntoxicant.ContinuesWithoutCurrentUse)
            {
                EntryPoint.WriteToConsole("IngestActivity Exit, Stopping ingestion", 5);
                Player.Intoxication.StopIngesting(CurrentIntoxicant);
            }
            GameFiber.Sleep(5000);
            if (Item.Exists())
            {
                Item.Delete();
            }
        }
        private void Idle()
        {
            while (Player.CanPerformActivities && !IsCancelled)
            {
                Player.WeaponEquipment.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (AnimationTime >= 0.25f)
                {
                    if (Item.Exists())
                    {
                        Item.Delete();
                    }
                }
                if (AnimationTime >= 0.35f)
                {
                    NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);//NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    break;
                }
                GameFiber.Yield();
            }
            Exit();
        }
        private void Setup()
        {
            List<string> AnimIdle;
            string AnimEnter = "";
            string AnimEnterDictionary = "";
            string AnimExit = "";
            string AnimExitDictionary = "";
            string AnimIdleDictionary;

            string PropModel = "";

            string HandBoneName = "BONETAG_R_PH_HAND";
            Vector3 HandOffset = Vector3.Zero;
            Rotator HandRotator = Rotator.Zero;

            if (ModItem != null && ModItem.ModelItem != null)
            {
                PropModel = ModItem.ModelItem.ModelName;
                PropAttachment pa = ModItem.ModelItem.Attachments.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
                if (pa != null)
                {
                    HandOffset = pa.Attachment;
                    HandRotator = pa.Rotation;
                    HandBoneName = pa.BoneName;
                }
            }

            AnimIdleDictionary = "mp_suicide";
            AnimIdle = new List<string>() { "pill" };

            if (ModItem != null && ModItem.IsIntoxicating)
            {
                CurrentIntoxicant = Intoxicants.Get(ModItem.IntoxicantName);
                Player.Intoxication.StartIngesting(CurrentIntoxicant);
            }

            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            Data = new EatingData("", "", AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneName, HandOffset, HandRotator, PropModel);
        }
    }
}
﻿using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player.Activity;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Player
{
    public class DrinkingActivity : DynamicActivity
    {
        private Rage.Object Bottle;
        private string PlayingAnim;
        private string PlayingDict;
        private DrinkingData Data;
        private IntoxicatingEffect IntoxicatingEffect;
        private bool IsAttachedToHand;
        private bool IsCancelled;
        private IIntoxicatable Player;
        private ISettingsProvideable Settings;
       // private ModItem ModItem;
        private IIntoxicants Intoxicants;
        private Intoxicant CurrentIntoxicant;
        public DrinkingActivity(IIntoxicatable consumable, ISettingsProvideable settings) : base()
        {
            Player = consumable;
            Settings = settings;
        }
        public DrinkingActivity(IIntoxicatable consumable, ISettingsProvideable settings, ModItem modItem, IIntoxicants intoxicants) : base()
        {
            Player = consumable;
            Settings = settings;
            ModItem = modItem;
            Intoxicants = intoxicants;
        }
        public override ModItem ModItem { get; set; }
        public override string DebugString => $"Intox {Player.IsIntoxicated} Consum: {Player.IsPerformingActivity} I: {Player.IntoxicatedIntensity}";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
            Player.StopIngesting(CurrentIntoxicant);
        }
        public override void Pause()
        {
            Cancel();//for now it just cancels
        }
        public override void Continue()
        {
        }
        public override void Start()
        {
            Setup();
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                Enter();
            }, "DrinkingWatcher");
        }
        private void AttachBottleToHand()
        {
            CreateBottle();
            if (Bottle.Exists() && !IsAttachedToHand)
            {
                Bottle.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, Data.HandBoneID), Data.HandOffset, Data.HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp = Bottle;
            }
        }
        private void CreateBottle()
        {
            if (!Bottle.Exists())
            {
                try 
                {
                    //Vector3 position = Player.Character.GetOffsetPositionUp(50f);
                    //Model modelToCreate = new Model(Game.GetHashKey(Data.PropModelName));
                    //modelToCreate.LoadAndWait();
                    //Bottle = NativeFunction.Natives.CREATE_OBJECT<Rage.Object>(Game.GetHashKey(Data.PropModelName), position.X, position.Y, position.Z, 0f);
                    Bottle = new Rage.Object(Data.PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception e)
                {
                    Game.DisplayNotification($"Could Not Spawn Prop {Data.PropModelName}");
                }
                if (Bottle.Exists())
                {
                    Bottle.IsGravityDisabled = false;
                }
                else
                {
                    IsCancelled = true;
                }
            }
        }
        private void Enter()
        {
            Player.SetUnarmed();
            AttachBottleToHand();
            Player.IsPerformingActivity = true;
            PlayingDict = Data.AnimEnterDictionary;
            PlayingAnim = Data.AnimEnter;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, Data.AnimEnterDictionary, Data.AnimEnter, 1.0f, -1.0f, -1, 50, 0, false, false, false);//-1
            Idle();
        }
        private void Exit()
        {
            if (Bottle.Exists())
            {
                Bottle.Detach();
            }
            //Player.Character.Tasks.Clear();
            //NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.IsPerformingActivity = false;
            Player.StopIngesting(CurrentIntoxicant);
            GameFiber.Sleep(5000);
            if (Bottle.Exists())
            {
                Bottle.Delete();
            }
        }
        private void Idle()
        {
            while (Player.CanPerformActivities && !IsCancelled)
            {
                Player.SetUnarmed();
                if (NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim) >= 1.0f)
                {
                    PlayingDict = Data.AnimIdleDictionary;
                    PlayingAnim = Data.AnimIdle.PickRandom();
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
                    //EntryPoint.WriteToConsole($"New Drinking Idle {PlayingAnim}",5);
                }
                GameFiber.Yield();
            }
            Exit();
        }
        private void Setup()
        {
            List<string> AnimIdle;
            string AnimEnter;
            string AnimEnterDictionary;
            string AnimExit;
            string AnimExitDictionary;
            string AnimIdleDictionary;
            int HandBoneID;
            Vector3 HandOffset;
            Rotator HandRotator;
            string PropModel;
            if (Player.ModelName.ToLower() == "player_zero" || Player.ModelName.ToLower() == "player_one" || Player.ModelName.ToLower() == "player_two" || Player.IsMale)
            {
                AnimEnterDictionary = "amb@world_human_drinking@beer@male@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_drinking@beer@male@exit";
                AnimExit = "exit";
                AnimIdleDictionary = "amb@world_human_drinking@beer@male@idle_a";
                AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
                HandBoneID = 57005;
                HandOffset = new Vector3(0.12f, 0.0f, -0.06f);
                HandRotator = new Rotator(-77.0f, 23.0f, 0.0f);


                AnimEnterDictionary = "amb@world_human_drinking@beer@male@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_drinking@beer@male@exit";
                AnimExit = "exit";
                AnimIdleDictionary = "amb@code_human_wander_drinking@male@idle_a";
                AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };

            }
            else
            {
                AnimEnterDictionary = "amb@world_human_drinking@beer@female@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_drinking@beer@female@exit";
                AnimExit = "exit";
                AnimIdleDictionary = "amb@world_human_drinking@beer@female@idle_a";
                AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
                HandBoneID = 57005;
                HandOffset = new Vector3(0.12f, 0.0f, -0.06f);
                HandRotator = new Rotator(-77.0f, 23.0f, 0.0f);


                AnimEnterDictionary = "amb@world_human_drinking@beer@female@enter";
                AnimEnter = "enter";
                AnimExitDictionary = "amb@world_human_drinking@beer@female@exit";
                AnimExit = "exit";
                AnimIdleDictionary = "amb@code_human_wander_drinking@female@idle_a";
                AnimIdle = new List<string>() { "idle_a", "Idle_b", "Idle_c" };
            }


            if(ModItem != null && ModItem.ModelItem != null)
            {
                PropModel = ModItem.ModelItem.ModelName;
                HandBoneID = ModItem.ModelItem.AttachBoneIndex;
                HandOffset = ModItem.ModelItem.AttachOffset;
                HandRotator = ModItem.ModelItem.AttachRotation;
                //if(ModItem.IsIntoxicating)
                //{
                //    IntoxicatingEffect = new IntoxicatingEffect(Player, Settings.SettingsManager.ActivitySettings.Alcohol_MaxEffectAllowed, Settings.SettingsManager.ActivitySettings.Alcohol_TimeToReachEachIntoxicatedLevel, Settings.SettingsManager.ActivitySettings.Alcohol_TimeToReachEachSoberLevel, Settings.SettingsManager.ActivitySettings.Alcohol_Overlay);
                //    IntoxicatingEffect.Start();
                //}
            }
            else
            {   // assume drinking beer if nothing
                PropModel = Settings.SettingsManager.ActivitySettings.Alcohol_PossibleProps.PickRandom();
                IntoxicatingEffect = new IntoxicatingEffect(Player, Settings.SettingsManager.ActivitySettings.Alcohol_MaxEffectAllowed, Settings.SettingsManager.ActivitySettings.Alcohol_TimeToReachEachIntoxicatedLevel, Settings.SettingsManager.ActivitySettings.Alcohol_TimeToReachEachSoberLevel, Settings.SettingsManager.ActivitySettings.Alcohol_Overlay);
                IntoxicatingEffect.Start();
            }

            if (ModItem != null && ModItem.IsIntoxicating)
            {
                CurrentIntoxicant = Intoxicants.Get(ModItem.IntoxicantName);
                Player.StartIngesting(CurrentIntoxicant);
            }


            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            Data = new DrinkingData(AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneID, HandOffset, HandRotator, PropModel);
        }
    }
}
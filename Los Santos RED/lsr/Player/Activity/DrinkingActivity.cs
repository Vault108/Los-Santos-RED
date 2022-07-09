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
        private bool IsAttachedToHand;
        private bool IsCancelled;
        private IIntoxicatable Player;
        private ISettingsProvideable Settings;
        private IIntoxicants Intoxicants;
        private Intoxicant CurrentIntoxicant;
        private bool hasGainedHP = false;
        private uint GameTimeLastGivenHealth;
        private int HealthGiven;
        private int TimesDrank;

        private uint GameTimeLastCheckedAnimation;
        private float LastAnimationValue;
        private uint GameTimeLastGivenNeeds;
        private float HungerGiven;
        private float ThirstGiven;
        private int SleepGiven;
        private bool GivenFullHealth;
        private bool GivenFullHunger;
        private bool GivenFullThirst;
        private bool GivenFullSleep;
        private float PrevAnimationTime;

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
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Drinking";
        public override string CancelPrompt { get; set; } = "Stop Drinking";
        public override string ContinuePrompt { get; set; } = "Continue Drinking";
        public override void Cancel()
        {
            IsCancelled = true;
            Player.IsPerformingActivity = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
        }
        public override void Pause()
        {
            Cancel();//for now it just cancels
        }
        public override bool IsPaused() => false;
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
                Bottle.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, "BONETAG_L_PH_HAND"), Data.HandOffset, Data.HandRotator);
                IsAttachedToHand = true;
                Player.AttachedProp = Bottle;
            }
        }
        private void CreateBottle()
        {
            if (!Bottle.Exists() && Data.PropModelName != "")
            {
                try 
                {
                    Bottle = new Rage.Object(Data.PropModelName, Player.Character.GetOffsetPositionUp(50f));
                }
                catch (Exception e)
                {
                    Game.DisplayNotification($"Could Not Spawn Prop {Data.PropModelName}");
                }
                if (Bottle.Exists())
                {
                    //Bottle.IsGravityDisabled = false;
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
            StartNewEnterAnimation();
            while (Player.CanPerformActivities && !IsCancelled)
            {
                Player.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (AnimationTime >= 1.0f)
                {
                    break;
                }
                if (!IsAnimationRunning(AnimationTime))
                {
                    IsCancelled = true;
                }
                GameFiber.Yield();
            }
            Idle();
        }
        private void Exit()
        {
            if (Bottle.Exists())
            {
                Bottle.Detach();
            }
            NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
            Player.IsPerformingActivity = false;
            Player.Intoxication.StopIngesting(CurrentIntoxicant);
            if(ModItem?.CleanupItemImmediately == false)
            {
                GameFiber.Sleep(5000);
            }
            if (Bottle.Exists())
            {
                Bottle.Delete();
            }
        }
        private void Idle()
        {
            uint GameTimeBetweenDrinks = RandomItems.GetRandomNumber(2500,4000);
            uint GameTimeLastChangedIdle = Game.GameTime;
            bool IsFinishedWithSip = false;
            StartNewIdleAnimation();
            while (Player.CanPerformActivities && !IsCancelled)
            {
                Player.SetUnarmed();
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
                if (AnimationTime >= 1.0f)
                {
                    if(!IsFinishedWithSip)
                    {
                        StartBaseAnimation();
                        GameTimeLastChangedIdle = Game.GameTime;
                        GameTimeBetweenDrinks = RandomItems.GetRandomNumber(3500, 5500);
                        IsFinishedWithSip = true;
                        EntryPoint.WriteToConsole($"Drinking Sip finished {PlayingAnim} TimesDrank {TimesDrank} HealthGiven {HealthGiven}", 5);
                    }
                    if (TimesDrank >= 5 && GivenFullHealth && GivenFullHunger && GivenFullSleep && GivenFullThirst)// || Player.Character.Health == Player.Character.MaxHealth))
                    {
                        IsCancelled = true;
                    }
                    else if(IsFinishedWithSip && Game.GameTime - GameTimeLastChangedIdle >= GameTimeBetweenDrinks)
                    {
                        TimesDrank++;
                        StartNewIdleAnimation();
                        IsFinishedWithSip = false;
                        EntryPoint.WriteToConsole($"New Drinking Idle {PlayingAnim} TimesDrank {TimesDrank} HealthGiven {HealthGiven}", 5);
                    }
                }
                if (!IsAnimationRunning(AnimationTime))
                {
                    IsCancelled = true;
                }
                UpdateHealthGain();
                UpdateNeeds();
                GameFiber.Yield();
            }
            Exit();
        }
        private bool IsAnimationRunning(float AnimationTime)
        {
            return true;
            if (Game.GameTime - GameTimeLastCheckedAnimation >= 500)
            {
                if (PrevAnimationTime == AnimationTime)
                {
                    EntryPoint.WriteToConsole("Animation Issues Detected, Cancelling");
                    return false;
                }
                PrevAnimationTime = AnimationTime;
                GameTimeLastCheckedAnimation = Game.GameTime;
            }
            return true;
        }

        private void StartNewEnterAnimation()
        {
            GameTimeLastCheckedAnimation = Game.GameTime;
            PrevAnimationTime = 0.0f;
            PlayingDict = Data.AnimEnterDictionary;
            PlayingAnim = Data.AnimEnter;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
        }
        private void StartNewIdleAnimation()
        {
            GameTimeLastCheckedAnimation = Game.GameTime;
            PrevAnimationTime = 0.0f;
            PlayingDict = Data.AnimIdleDictionary;
            PlayingAnim = Data.AnimIdle.PickRandom();
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
        }
        private void StartExitAnimation()
        {
            GameTimeLastCheckedAnimation = Game.GameTime;
            PrevAnimationTime = 0.0f;
            PlayingDict = Data.AnimExitDictionary;
            PlayingAnim = Data.AnimExit;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, -1, 50, 0, false, false, false);
        }

        private void StartBaseAnimation()
        {
            GameTimeLastCheckedAnimation = Game.GameTime;
            PrevAnimationTime = 0.0f;
            PlayingDict = Data.AnimExitDictionary;
            PlayingAnim = Data.AnimExit;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 1.0f, -1.0f, 1.0f, 50, 0, false, false, false);
        }

        private void UpdateHealthGain()
        {
            if (Game.GameTime - GameTimeLastGivenHealth >= 1000)
            {
                if (ModItem.ChangesHealth)
                {
                    if(ModItem.HealthChangeAmount > 0 && HealthGiven < ModItem.HealthChangeAmount)
                    {
                        HealthGiven++;
                        Player.ChangeHealth(1);
                    }
                    else if (ModItem.HealthChangeAmount < 0 && HealthGiven > ModItem.HealthChangeAmount)
                    {
                        HealthGiven--;
                        Player.ChangeHealth(-1);
                    }
                    //Player.HumanState.Thirst.Change(2.0f, true);
                }
                GameTimeLastGivenHealth = Game.GameTime;
            }
        }
        private void UpdateNeeds()
        {
            if (Game.GameTime - GameTimeLastGivenNeeds >= 1000)
            {
                if (ModItem.ChangesNeeds)
                {
                    if (ModItem.ChangesHunger)
                    {
                        if (ModItem.HungerChangeAmount < 0.0f)
                        {
                            if (HungerGiven > ModItem.HungerChangeAmount)
                            {
                                Player.HumanState.Hunger.Change(-1.0f, true);
                                HungerGiven--;
                            }
                            else
                            {
                                GivenFullHunger = true;
                            }
                        }
                        else
                        {
                            if (HungerGiven < ModItem.HungerChangeAmount)
                            {
                                Player.HumanState.Hunger.Change(1.0f, true);
                                HungerGiven++;
                            }
                            else
                            {
                                GivenFullHunger = true;
                            }
                        }
                    }
                    else
                    {
                        GivenFullHunger = true;
                    }
                    if (ModItem.ChangesThirst)
                    {
                        if (ModItem.ThirstChangeAmount < 0.0f)
                        {
                            if (ThirstGiven > ModItem.ThirstChangeAmount)
                            {
                                Player.HumanState.Thirst.Change(-1.0f, true);
                                ThirstGiven--;
                            }
                            else
                            {
                                GivenFullThirst = true;
                            }
                        }
                        else
                        {
                            if (ThirstGiven < ModItem.ThirstChangeAmount)
                            {
                                Player.HumanState.Thirst.Change(1.0f, true);
                                ThirstGiven++;
                            }
                            else
                            {
                                GivenFullThirst = true;
                            }
                        }
                    }
                    else
                    {
                        GivenFullThirst = true;
                    }
                    if (ModItem.ChangesSleep)
                    {
                        if (ModItem.SleepChangeAmount < 0.0f)
                        {
                            if (SleepGiven > ModItem.SleepChangeAmount)
                            {
                                Player.HumanState.Sleep.Change(-1.0f, true);
                                SleepGiven--;
                            }
                            else
                            {
                                GivenFullSleep = true;
                            }
                        }
                        else
                        {
                            if (SleepGiven < ModItem.SleepChangeAmount)
                            {
                                Player.HumanState.Sleep.Change(1.0f, true);
                                SleepGiven++;
                            }
                            else
                            {
                                GivenFullSleep = true;
                            }
                        }
                    }
                    else
                    {
                        GivenFullSleep = true;
                    }
                }
                GameTimeLastGivenNeeds = Game.GameTime;
            }
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
            Vector3 HandOffset = Vector3.Zero;
            Rotator HandRotator = Rotator.Zero;
            string PropModel = "";
            bool isBottle = false;

            if (ModItem != null && ModItem.Name.ToLower().Contains("bottle"))
            {
                isBottle = true;
            }
            EntryPoint.WriteToConsole($"Drinking Start isBottle {isBottle} isMale {Player.IsMale}");
            HandBoneID = 18905;
            //HandOffset = new Vector3(0.12f, -0.07f, 0.07f);
            //HandRotator = new Rotator(-110.0f, 14.0f, 1.0f);
            HandOffset = new Vector3();
            HandRotator = new Rotator();
            if (ModItem != null && ModItem.ModelItem != null)
            {
                PropModel = ModItem.ModelItem.ModelName;
                //HandBoneID = ModItem.ModelItem.AttachBoneIndex;
                HandOffset = ModItem.ModelItem.AttachOffsetOverride;
                HandRotator = ModItem.ModelItem.AttachRotationOverride;
            }
            if (Player.IsInVehicle)
            {
                if (Player.IsDriver)
                {
                    if (isBottle)
                    {
                        AnimEnterDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ds@base";
                        AnimEnter = "enter";
                        AnimExitDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ds@base";
                        AnimExit = "exit";
                        AnimIdleDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ds@base";
                        AnimIdle = new List<string>() { "idle_a" };
                    }
                    else
                    {
                        AnimEnterDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ds@base";
                        AnimEnter = "enter";
                        AnimExitDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ds@base";
                        AnimExit = "exit";
                        AnimIdleDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ds@base";
                        AnimIdle = new List<string>() { "idle_a" };
                    }
                }
                else
                {
                    if (isBottle)
                    {
                        AnimEnterDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ps@base";
                        AnimEnter = "enter";
                        AnimExitDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ps@base";
                        AnimExit = "exit";
                        AnimIdleDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ps@base";
                        AnimIdle = new List<string>() { "idle_a" };
                    }
                    else
                    {
                        AnimEnterDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ps@base";
                        AnimEnter = "enter";
                        AnimExitDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ps@base";
                        AnimExit = "exit";
                        AnimIdleDictionary = "amb@code_human_in_car_mp_actions@drink_bottle@std@ps@base";
                        AnimIdle = new List<string>() { "idle_a" };
                    }
                }
            }
            else
            {
                if (isBottle)
                {
                    AnimEnterDictionary = "mp_player_intdrink";
                    AnimEnter = "intro_bottle";
                    AnimExitDictionary = "mp_player_intdrink";
                    AnimExit = "outro_bottle";
                    AnimIdleDictionary = "mp_player_intdrink";
                    AnimIdle = new List<string>() { "loop_bottle" };
                }
                else
                {
                    AnimEnterDictionary = "mp_player_intdrink";
                    AnimEnter = "intro";
                    AnimExitDictionary = "mp_player_intdrink";
                    AnimExit = "outro";
                    AnimIdleDictionary = "mp_player_intdrink";
                    AnimIdle = new List<string>() { "loop" };
                }
            }

            if (ModItem != null && ModItem.IsIntoxicating)
            {
                CurrentIntoxicant = Intoxicants.Get(ModItem.IntoxicantName);
                Player.Intoxication.StartIngesting(CurrentIntoxicant);
            }
            AnimationDictionary.RequestAnimationDictionay(AnimIdleDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimEnterDictionary);
            AnimationDictionary.RequestAnimationDictionay(AnimExitDictionary);
            Data = new DrinkingData(AnimEnter, AnimEnterDictionary, AnimExit, AnimExitDictionary, AnimIdle, AnimIdleDictionary, HandBoneID, HandOffset, HandRotator, PropModel);
        }
    }
}
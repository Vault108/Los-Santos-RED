﻿using Rage.Native;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using ExtensionsMethods;

public class AdvancedConversation
{
    private IInteractionable Player;
    private IShopMenus ShopMenus;
    private IModItems ModItems;
    private IZones Zones;
    private Conversation_Simple ConversationSimple;
    private MenuPool MenuPool;
    private UIMenu ConversationMenu;
    private IPlacesOfInterest PlacesOfInterest;
    private IGangs Gangs;
    private IGangTerritories GangTerritories;
    public AdvancedConversation(IInteractionable player, Conversation_Simple conversation_Simple, IModItems modItems, IZones zones, IShopMenus shopMenus, IPlacesOfInterest placesOfInterest, IGangs gangs, IGangTerritories gangTerritories)
    {
        Player = player;
        ConversationSimple = conversation_Simple;
        ModItems = modItems;
        Zones = zones;
        ShopMenus = shopMenus;
        PlacesOfInterest = placesOfInterest;
        Gangs = gangs;
        GangTerritories = gangTerritories;
    }
    public void Setup()
    {
        CreateMenu();
    }
    public void Show()
    {
        UpdateMenuItems();
        ConversationMenu.Visible = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                while(EntryPoint.ModController.IsRunning && ConversationMenu.Visible)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                Dispose();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Conversation");

    }
    public void Dispose()
    {
        ConversationMenu.Visible = false;
        ConversationSimple.OnAdvancedConversationStopped();
    }

    private void CreateMenu()
    {
        MenuPool = new MenuPool();
        ConversationMenu = new UIMenu("Conversation", "Select an Option");
        ConversationMenu.RemoveBanner();
        MenuPool.Add(ConversationMenu);
    }
    private void UpdateMenuItems()
    {
        UIMenuItem transactionInteract = new UIMenuItem("Start Transact", "Start a transaction with the current ped.");
        transactionInteract.Activated += (menu, item) =>
        {
            menu.Visible = false;
            StartTransactionWithPed();
        };
        if (ConversationSimple.ConversingPed?.HasMenu == true)
        {
            ConversationMenu.AddItem(transactionInteract);
        }
        AddDrugItemQuestions();
        AddGangItemQuestions();
        UIMenuItem Cancel = new UIMenuItem("Cancel", "Stop asking questions");
        Cancel.Activated += (menu, item) =>
        {
            Dispose();
        };
        ConversationMenu.AddItem(Cancel);
    }
    private void StartTransactionWithPed()
    {
        ConversationSimple?.TransitionToTransaction();
        Player.ActivityManager.StartTransaction(ConversationSimple.ConversingPed);
    }
    private void AddGangItemQuestions()
    {
        UIMenuListScrollerItem<Gang> AskAboutGangDenScroller = new UIMenuListScrollerItem<Gang>("Gang Hangouts", "Ask where to find a specific gang hangout", Gangs.AllGangs);
        AskAboutGangDenScroller.Activated += (menu, item) =>
        {
            AskAboutGangDen(AskAboutGangDenScroller.SelectedItem);
        };
        UIMenuListScrollerItem<Gang> AskAboutGangTerritoryScroller = new UIMenuListScrollerItem<Gang>("Gang Territory", "Ask about the gangs territory", Gangs.AllGangs);
        AskAboutGangTerritoryScroller.Activated += (menu, item) =>
        {
            AskAboutGangTerritory(AskAboutGangTerritoryScroller.SelectedItem);
        };
        ConversationMenu.AddItem(AskAboutGangDenScroller);
        ConversationMenu.AddItem(AskAboutGangTerritoryScroller);      
    }
    private void AskAboutGangDen(Gang gang)
    {
        if (ConversationSimple.ConversingPed == null || !ConversationSimple.ConversingPed.KnowsGangAreas || gang == null)
        {
            ReplyUnknown();
            return;
        }
        GangDen foundDen = PlacesOfInterest.PossibleLocations.GangDens.Where(x => x.AssociatedGang != null && x.AssociatedGang.ID == gang.ID).FirstOrDefault();
        if(foundDen == null)
        {
            ReplyUnknown();
            return;
        }     
        List<string> PossibleReplies = new List<string>() {
            $"Check out ~p~{foundDen.ZoneName}~s~",
            $"I heard its near ~p~{foundDen.ZoneName}~s~",
            $"Go ask around ~p~{foundDen.ZoneName}~s~",
            $"Might get lucky in ~p~{foundDen.ZoneName}~s~",
            $"Go look around ~p~{foundDen.ZoneName}~s~",
            $"Scope out ~p~{foundDen.ZoneName}~s~",
            };
        ConversationSimple.PedReply(PossibleReplies.PickRandom());
    }
    private void AskAboutGangTerritory(Gang gang)
    {
        if (ConversationSimple.ConversingPed == null || !ConversationSimple.ConversingPed.KnowsGangAreas)
        {
            ReplyUnknown();
            return;
        }
        List<ZoneJurisdiction> foundTerritory = GangTerritories.GetGangTerritory(gang.ID);
        if (foundTerritory == null || !foundTerritory.Any())
        {
            ReplyUnknown();
            return;
        }
        List<Zone> FoundZones = new List<Zone>();
        foreach (ZoneJurisdiction zoneJurisdiction in foundTerritory)
        {
            Zone foundZone = Zones.GetZone(zoneJurisdiction.ZoneInternalGameName);
            if(foundZone != null)
            {
                FoundZones.Add(foundZone);
            }
        }
        if(!FoundZones.Any())
        {
            ReplyUnknown();
            return;
        }
        string zoneList = string.Join(", ", FoundZones.Select(x => x.DisplayName).Take(3));
        List<string> PossibleReplies = new List<string>() {
            $"Normally they hang out near ~p~{zoneList}~s~",
            $"I've seen them in ~p~{zoneList}~s~",
            $"You can check out ~p~{zoneList}~s~",
            $"I'd go visit ~p~{zoneList}~s~",
            $"Go look around ~p~{zoneList}~s~",
            $"They should be near ~p~{zoneList}~s~",
            };
        ConversationSimple.PedReply(PossibleReplies.PickRandom());
    }
    private void AddDrugItemQuestions()
    {
        List<ModItem> dealerItems = ModItems.AllItems().Where(x => x.ItemType == ItemType.Drugs && x.ItemSubType == ItemSubType.Narcotic).ToList();
        UIMenuListScrollerItem<ModItem> AskForItemDealer = new UIMenuListScrollerItem<ModItem>("Dealers", "Ask where to find dealers for an item", dealerItems);
        AskForItemDealer.Activated += (menu, item) =>
        {
            AskForItem(AskForItemDealer.SelectedItem, true);
        };
        UIMenuListScrollerItem<ModItem> AskForItemCustomer = new UIMenuListScrollerItem<ModItem>("Customers", "Ask where to find customers for an item", dealerItems);
        AskForItemCustomer.Activated += (menu, item) =>
        {
            AskForItem(AskForItemCustomer.SelectedItem, false);
        };
        if (dealerItems.Any())
        {
            ConversationMenu.AddItem(AskForItemDealer);
            ConversationMenu.AddItem(AskForItemCustomer);
        }
    }
    private void AskForItem(ModItem modItem, bool isPurchase)
    {
        if(ConversationSimple.ConversingPed == null || !ConversationSimple.ConversingPed.KnowsDrugAreas)
        {
            ReplyUnknown();
            return;
        }
        List<Zone> PossibleZones = Zones.GetZoneByItem(modItem, ShopMenus, isPurchase);
        if (PossibleZones == null)
        {
            ReplyUnknown();
        }
        else
        {
            ReplyFound(PossibleZones, isPurchase);
            Dispose();
        }
        
    }
    private void ReplyUnknown()
    {
        List<string> PossibleReplies = new List<string>() { "I don't know", "How the fuck would I know?","I really have no idea", "I just live here man", "Not sure", "Maybe ask someone else?", "Why would I know that?", "Leave me alone" };
        ConversationSimple.PedReply(PossibleReplies.PickRandom());
        //Game.DisplaySubtitle(PossibleReplies.PickRandom());
    }
    private void ReplyFound(List<Zone> PossibleZones, bool isPurchase)
    {
        if(PossibleZones == null)
        {
            return;
        }
        Zone selectedZone = PossibleZones.PickRandom();
        if(selectedZone == null)
        {
            return;
        }
        string ZoneString = selectedZone.DisplayName;
        List<string> PossibleReplies;
        if (isPurchase)
        {
            PossibleReplies = new List<string>() {
            $"Check out ~p~{ZoneString}~s~",
            $"Can probably find something in ~p~{ZoneString}~s~",
            $"Go ask around ~p~{ZoneString}~s~",
            $"I heard you can find some in ~p~{ZoneString}~s~",
            $"Swing by ~p~{ZoneString}~s~",
            $"Scope out ~p~{ZoneString}~s~",
            };
        }
        else
        {
            PossibleReplies = new List<string>() {
            $"Always find interested people in ~p~{ZoneString}~s~",
            $"The people in ~p~{ZoneString}~s~ go wild for that",
            $"A good spot to sell would be ~p~{ZoneString}~s~",
            $"There should be some customers in ~p~{ZoneString}~s~",
            $"The best place to start would be ~p~{ZoneString}~s~",
            $"Should be able to offload some of that in ~p~{ZoneString}~s~",
            };
        }
        ConversationSimple.PedReply(PossibleReplies.PickRandom());
    }


}

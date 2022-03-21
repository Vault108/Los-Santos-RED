﻿using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GangDen : InteractableLocation
{
    private LocationCamera StoreCamera;
    private UIMenuItem dropoffCash;
    private UIMenuItem dropoffItem;

    private IActivityPerformable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private UIMenuItem completeTask;


    private Transaction Transaction;

    public GangDen() : base()
    {

    }
    public override int MapIcon { get; set; } = (int)BlipSprite.Shrink;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
    public string GangID { get; set; }

    //[XmlIgnore]
    //public ShopMenu Menu { get; set; }
    //public string MenuID { get; set; }
    [XmlIgnore]
    public int ExpectedMoney { get; set; }
    [XmlIgnore]
    public ModItem ExpectedItem { get; set; }
    [XmlIgnore]
    public Gang AssociatedGang { get; set; }

    public GangDen(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, string _gangID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        GangID = _gangID;
        MenuID = menuID;
        ButtonPromptText = $"Enter {Name}";
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (CanInteract)
        {
            Player.IsInteractingWithLocation = true;
            CanInteract = false;

            GameFiber.StartNew(delegate
            {
                StoreCamera = new LocationCamera(this, Player);
                StoreCamera.SayGreeting = false;
                StoreCamera.Setup();


                CreateInteractionMenu();



                Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);

                //CreateTransactionMenu(Player, modItems, world, settings, weapons, time);

                PlayerTask pt = Player.PlayerTasks.GetTask(AssociatedGang.ContactName);



                if (ExpectedMoney > 0 && pt.IsReadyForPayment)
                {
                    dropoffCash = new UIMenuItem("Drop Cash", "Drop off the expected amount of cash.") { RightLabel = $"${ExpectedMoney}" };
                    InteractionMenu.AddItem(dropoffCash);
                }
                else if (ExpectedItem != null && pt.IsReadyForPayment)
                {
                    dropoffItem = new UIMenuItem($"Drop off item", $"Drop off the {ExpectedItem.Name}.") { RightLabel = $"{ExpectedItem.Name}" };
                    InteractionMenu.AddItem(dropoffItem);
                }
                else if (pt != null && pt.IsActive && pt.IsReadyForPayment)
                {
                    completeTask = new UIMenuItem($"Collect Money", $"Inform the higher ups that you have completed the assigment and collect your payment.") { RightLabel = $"${pt.PaymentAmountOnCompletion}" };
                    InteractionMenu.AddItem(completeTask);
                }
                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;

                Transaction.ProcessTransactionMenu();


                //ProcessTransactionMenu();

                Transaction.DisposeTransactionMenu();

                //DisposeTransactionMenu();
                DisposeInteractionMenu();

                StoreCamera.Dispose();

                Player.IsInteractingWithLocation = false;
                CanInteract = true;
            }, "GangDenInteract");       
        }
    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Buy")
        {
            Transaction?.SellMenu?.Dispose();
            Transaction?.PurchaseMenu?.Show();
        }
        else if (selectedItem.Text == "Sell")
        {
            Transaction?.PurchaseMenu?.Dispose();
            Transaction?.SellMenu?.Show();
        }
        else if (selectedItem == dropoffCash)
        {
            if(Player.Money >= ExpectedMoney)
            {
                Player.GiveMoney(-1*ExpectedMoney);
                Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~g~Reply", "Thanks for the cash. Here's your cut.");
                ExpectedMoney = 0;
                Player.PlayerTasks.CompleteTask(AssociatedGang.ContactName);
                InteractionMenu.Visible = false;
            }
            else
            {
                Game.DisplayNotification(AssociatedGang.ContactIcon,AssociatedGang.ContactIcon,AssociatedGang.ContactName,"~r~Reply","Come back when you actually have the cash.");
            }
        }
        else if (selectedItem == dropoffItem)
        {
            if(Player.Inventory.HasItem(ExpectedItem.Name))
            {
                Player.Inventory.Remove(ExpectedItem,1);
                Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~g~Reply", $"Thanks for bringing us {ExpectedItem.Name}. Have something for your time.");
                ExpectedItem = null;
                Player.PlayerTasks.CompleteTask(AssociatedGang.ContactName);
                InteractionMenu.Visible = false;
            }
            else
            {
                Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~r~Reply", $"Come back when you actually have the {ExpectedItem.Name}.");
            }

        }
        else if (selectedItem == completeTask)
        {
            Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~g~Reply", "Thanks for taking care of that thing. Here's your share.");
            ExpectedMoney = 0;
            Player.PlayerTasks.CompleteTask(AssociatedGang.ContactName);
            InteractionMenu.Visible = false;
        }
    }

}


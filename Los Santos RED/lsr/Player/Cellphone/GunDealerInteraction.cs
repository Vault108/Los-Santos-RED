﻿using ExtensionsMethods;
using iFruitAddon2;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GunDealerInteraction
{
    private IContactInteractable Player;
    private UIMenu GunDealerMenu;
    private MenuPool MenuPool;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private UIMenuItem RequestLocation;
    private UIMenuItem RequestWork;
    private UIMenu LocationSubMenu;
    private iFruitContact AnsweredContact;
    public GunDealerInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        MenuPool = new MenuPool();
    }
    public void Start(iFruitContact contact)
    {
        AnsweredContact = contact;
        GunDealerMenu = new UIMenu("", "Select an Option");
        GunDealerMenu.RemoveBanner();
        MenuPool.Add(GunDealerMenu);
        GunDealerMenu.OnItemSelect += OnTopItemSelect;

        // RequestLocation = new UIMenuItem("Payoff", "Payoff the gang to return to a neutral relationship") { RightLabel = 0.ToString("C0") };
        RequestWork = new UIMenuItem("Request Work", "Ask for some work from the gun dealers, better be strapped");
        LocationSubMenu = MenuPool.AddSubMenu(GunDealerMenu, "Request Store Address");

        GunDealerMenu.AddItem(RequestWork);

        LocationSubMenu.RemoveBanner();
        //GunDealerMenu.AddItem(RequestLocation);

        foreach (GunStore gl in PlacesOfInterest.PossibleLocations.GunStores)
        {
            if (gl.IsIllegalShop && gl.IsEnabled)
            {
                LocationSubMenu.AddItem(new UIMenuItem(gl.StreetAddress, "Get GPS Coordinates to this Store") { RightLabel = gl.Description });
            }
        }

        LocationSubMenu.OnItemSelect += OnGangItemSelect;
        GunDealerMenu.Visible = true;
        GameFiber.StartNew(delegate
        {
            while (GunDealerMenu?.Visible == true || LocationSubMenu?.Visible == true)
            {
                GameFiber.Yield();
            }
            Player.CellPhone.Close(2000);
        }, "CellPhone");
    }

    private void OnTopItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == RequestWork)
        {
            List<string> Replies = new List<string>() {
                "Nothing yet, I'll let you know",
                "I've got nothing for you yet",
                "Give me a few days",
                "Not a lot to be done right now",
                "We will let you know when you can do something for us",
                "Check back later.",
                };
            Player.CellPhone.AddPhoneResponse(AnsweredContact.Name, AnsweredContact.IconName, Replies.PickRandom());
            sender.Visible = false;
        }
    }

    public void Update()
    {
        MenuPool.ProcessMenus();
    }

    private void OnGangItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        RequestLocations(selectedItem.Text);
        sender.Visible = false;
    }
    private void RequestLocations(string AddressText)
    {
        GunStore gunStore = PlacesOfInterest.PossibleLocations.GunStores.FirstOrDefault(x => x.StreetAddress == AddressText);
        if (gunStore != null)
        {
            Player.AddGPSRoute(gunStore.Name, gunStore.EntrancePosition);
            List<string> Replies = new List<string>() {
                    $"Our shop is located on {gunStore.StreetAddress} come see us.",
                    $"Come check out our shop on {gunStore.StreetAddress}.",
                    $"You can find our shop on {gunStore.StreetAddress}.",
                    $"{gunStore.StreetAddress}.",
                    $"It's on {gunStore.StreetAddress} come see us.",
                    $"The shop? It's on {gunStore.StreetAddress}.",

                    };
            Player.CellPhone.AddPhoneResponse(AnsweredContact.Name, AnsweredContact.IconName, Replies.PickRandom());
        }
    }
}


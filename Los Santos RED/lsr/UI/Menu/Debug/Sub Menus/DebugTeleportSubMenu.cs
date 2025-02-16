﻿using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugTeleportSubMenu : DebugSubMenu
{
    private IPlacesOfInterest PlacesOfInterest;
    private IEntityProvideable World;
    private IInteractionable Interactionable;
    private UIMenu LocationItemsMenu;
    private bool isMPMapLoaded;
    public DebugTeleportSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IPlacesOfInterest placesOfInterest, IEntityProvideable world, IInteractionable interactionable) : base(debug, menuPool, player)
    {
        PlacesOfInterest = placesOfInterest;
        World = world;
        Interactionable = interactionable;
    }
    public override void AddItems()
    {
        LocationItemsMenu = MenuPool.AddSubMenu(Debug, "Teleport Menu");
        LocationItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Teleport to various locations";
        LocationItemsMenu.Width = 0.6f;
        CreateMenu();
    }
    public override void Update()
    {
        //CreateMenu();

        if(isMPMapLoaded != World.IsMPMapLoaded)
        {
            EntryPoint.WriteToConsole("LOADED MAP HAS CHANGED, RELOADING TELEPORT MENU");
            CreateMenu();
        }

    }
    private void CreateMenu()
    {
        isMPMapLoaded = World.IsMPMapLoaded;


        LocationItemsMenu.Clear();
        UIMenuItem teleportToMarker = new UIMenuItem("Teleport To Marker", "Teleport to the current marker.");
        teleportToMarker.Activated += (sender, selectedItem) =>
        {
            Player.GPSManager.TeleportToMarker();
            sender.Visible = false;
        };
        LocationItemsMenu.AddItem(teleportToMarker);


        UIMenu InteriorsSubMenu = MenuPool.AddSubMenu(LocationItemsMenu, "Interiors");
        InteriorsSubMenu.SetBannerType(EntryPoint.LSRedColor);
        LocationItemsMenu.MenuItems[LocationItemsMenu.MenuItems.Count() - 1].Description = "Teleport to various interior locations";
        InteriorsSubMenu.Width = 0.6f;

        List<GameLocation> InteriorLocations = PlacesOfInterest.AllLocations().Where(x => x.HasInterior && x.Interior != null && x.Interior.IsTeleportEntry && x.IsCorrectMap(World.IsMPMapLoaded)).ToList();
        foreach (string typeName in InteriorLocations.OrderBy(x => x.TypeName).Select(x => x.TypeName).Distinct())
        {
            UIMenuListScrollerItem<GameLocation> myLocationType = new UIMenuListScrollerItem<GameLocation>($"{typeName} - Int", "Teleports to an Interior Location on the Map", InteriorLocations.Where(x => x.TypeName == typeName));
            myLocationType.Activated += (menu, item) =>
            {
                GameLocation toTele = myLocationType.SelectedItem;
                if (toTele != null && toTele.Interior != null)
                {
                    //toTele.Interior.Load(true);
                    toTele.Interior.Teleport(Interactionable, toTele, null);
                }
            };
            InteriorsSubMenu.AddItem(myLocationType);
        }

        List<GameLocation> AllLocations = PlacesOfInterest.AllLocations().Where(x => x.IsCorrectMap(World.IsMPMapLoaded)).ToList();
        foreach (string typeName in AllLocations.OrderBy(x => x.TypeName).Select(x => x.TypeName).Distinct())
        {
            UIMenuListScrollerItem<GameLocation> myLocationType = new UIMenuListScrollerItem<GameLocation>($"{typeName}", "Teleports to a POI on the Map", AllLocations.Where(x => x.TypeName == typeName));
            myLocationType.Activated += (menu, item) =>
            {
                GameLocation toTele = myLocationType.SelectedItem;
                if (toTele != null)
                {
                    Game.LocalPlayer.Character.Position = toTele.EntrancePosition;
                    Game.LocalPlayer.Character.Heading = toTele.EntranceHeading;
                }
            };
            LocationItemsMenu.AddItem(myLocationType);
        }
    }
}


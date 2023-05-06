﻿using LSR.Vehicles;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;

public class VehicleInteractionMenu
{
    public bool IsShowingMenu { get; private set; } = false;
    private VehicleExt VehicleExt;
    private MenuPool MenuPool;
    private UIMenu VehicleInteractMenu;
    private VehicleDoorSeatData VehicleDoorSeatData;
    private IInteractionable Player;
    public VehicleInteractionMenu(VehicleExt vehicleExt)
    {
        VehicleExt = vehicleExt;
    }
    public void ShowInteractionMenu(IInteractionable player, IWeapons weapons, IModItems modItems, VehicleDoorSeatData vehicleDoorSeatData, IVehicleSeatAndDoorLookup vehicleSeatDoorData, IEntityProvideable world, ISettingsProvideable settings)
    {
        VehicleDoorSeatData = vehicleDoorSeatData;
        Player = player;
        CreateInteractionMenu();
        if (!player.IsInVehicle)
        {
            VehicleExt.VehicleBodyManager.CreateInteractionMenu(MenuPool, VehicleInteractMenu, vehicleSeatDoorData, world);
        }
        UIMenu InventoryWeaponHeaderMenu = MenuPool.AddSubMenu(VehicleInteractMenu, "Inventory and Weapons");
        VehicleInteractMenu.MenuItems[VehicleInteractMenu.MenuItems.Count() - 1].Description = "Manage Stored Inventory and Weapons. Place items or weapons within storage, or retreive them for use.";
        InventoryWeaponHeaderMenu.SetBannerType(EntryPoint.LSRedColor);
        InventoryWeaponHeaderMenu.OnMenuOpen += (sender) =>
        {
            vehicleDoorSeatData = VehicleExt.GetClosestPedStorageBone(player, 5.0f, vehicleSeatDoorData);
            if(vehicleDoorSeatData== null || player.IsInVehicle)
            {
                return;
            }
            player.ActivityManager.SetDoor(vehicleDoorSeatData.DoorID, true, false);
        };
        InventoryWeaponHeaderMenu.OnMenuClose += (sender) =>
        {
            if (vehicleDoorSeatData == null || player.IsInVehicle)
            {
                return;
            }
            player.ActivityManager.SetDoor(vehicleDoorSeatData.DoorID, false, false);
        };




        VehicleExt.HandleRandomItems(modItems);
        VehicleExt.HandleRandomWeapons(modItems, weapons);
        VehicleExt.SimpleInventory.CreateInteractionMenu(player, MenuPool, InventoryWeaponHeaderMenu, !player.IsInVehicle);
        VehicleExt.WeaponStorage.CreateInteractionMenu(player, MenuPool, InventoryWeaponHeaderMenu, weapons, modItems, !player.IsInVehicle);
        VehicleInteractMenu.Visible = true;
        IsShowingMenu = true;
        Player.ButtonPrompts.RemovePrompts("VehicleInteract");
        ProcessMenu();
    }




    private void CreateInteractionMenu()
    {
        MenuPool = new MenuPool();
        VehicleInteractMenu = new UIMenu("Vehicle", "Select an Option");
        VehicleInteractMenu.SetBannerType(EntryPoint.LSRedColor);
        MenuPool.Add(VehicleInteractMenu);     
    }
    private void ProcessMenu()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (EntryPoint.ModController.IsRunning && Player.IsAliveAndFree && MenuPool.IsAnyMenuOpen() && VehicleExt.Vehicle.Exists() && VehicleExt.Vehicle.Speed <= 0.5f && VehicleExt.Vehicle.DistanceTo2D(Game.LocalPlayer.Character) <= 7f)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                //if(VehicleDoorSeatData != null)
                //{
                //    Player.ActivityManager.SetDoor(VehicleDoorSeatData.DoorID, true, false);
                //}
                IsShowingMenu = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "VehicleInteraction");
    }
}

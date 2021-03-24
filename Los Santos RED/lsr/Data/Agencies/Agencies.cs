﻿using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Agencies : IAgencies
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\Agencies.xml";
    private bool UseVanillaConfig = true;
    private List<Agency> AgenciesList;
    private Agency DefaultAgency;
    public Agencies()
    {

    }
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            AgenciesList = Serialization.DeserializeParams<Agency>(ConfigFileName);
        }
        else
        {
            if (UseVanillaConfig)
            {
                DefaultConfig();
            }
            else
            {
                CustomConfig();
            }
            Serialization.SerializeParams(AgenciesList, ConfigFileName);
        }
    }
    public Agency GetAgency(string AgencyInitials)
    {
        return AgenciesList.Where(x => x.ID.ToLower() == AgencyInitials.ToLower()).FirstOrDefault();
    }
    public Agency GetRandomMilitaryAgency()
    {
        return AgenciesList.Where(x => x.Classification == Classification.Military).FirstOrDefault();
    }
    public List<Agency> GetAgencies(Ped ped)
    {
        return AgenciesList.Where(x => x.Personnel != null && x.Personnel.Any(b => b.ModelName.ToLower() == ped.Model.Name.ToLower())).ToList();
    }
    public List<Agency> GetAgencies(Vehicle vehicle)
    {
        return AgenciesList.Where(x => x.Vehicles != null && x.Vehicles.Any(b => b.ModelName.ToLower() == vehicle.Model.Name.ToLower())).ToList();
    }
    public List<Agency> GetSpawnableAgencies(int WantedLevel)
    {
        return AgenciesList.Where(x => x.CanSpawnAnywhere && x.CanSpawn(WantedLevel)).ToList();
    }
    public List<Agency> GetSpawnableHighwayAgencies(int WantedLevel)
    {
        return AgenciesList.Where(x => x.SpawnsOnHighway && x.CanSpawn(WantedLevel)).ToList();
    }
    public List<Agency> GetSpawnableAgencies(int WantedLevel, ResponseType responseType)
    {
        return AgenciesList.Where(x => x.CanSpawnAnywhere && x.CanSpawn(WantedLevel) && x.ResponseType == responseType).ToList();
    }
    public List<Agency> GetSpawnableHighwayAgencies(int WantedLevel, ResponseType responseType)
    {
        return AgenciesList.Where(x => x.SpawnsOnHighway && x.CanSpawn(WantedLevel) && x.ResponseType == responseType).ToList();
    }
    private void DefaultConfig()
    {
        //Peds
        List<DispatchablePerson> StandardCops = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_cop_01",85,85),
            new DispatchablePerson("s_f_y_cop_01",15,15) };
        List<DispatchablePerson> ExtendedStandardCops = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_cop_01",85,85),
            new DispatchablePerson("s_f_y_cop_01",10,10),
            new DispatchablePerson("ig_trafficwarden",5,5) };
        List<DispatchablePerson> ParkRangers = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_ranger_01",75,75),
            new DispatchablePerson("s_f_y_ranger_01",25,25) };
        List<DispatchablePerson> SheriffPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_sheriff_01",75,75),
            new DispatchablePerson("s_f_y_sheriff_01",25,25) };
        List<DispatchablePerson> SWAT = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_swat_01", 100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<DispatchablePerson> PoliceAndSwat = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_cop_01",70,0),
            new DispatchablePerson("s_f_y_cop_01",30,0),
            new DispatchablePerson("s_m_y_swat_01", 0,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<DispatchablePerson> SheriffAndSwat = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_sheriff_01", 75, 0),
            new DispatchablePerson("s_f_y_sheriff_01", 25, 0),
            new DispatchablePerson("s_m_y_swat_01", 0, 100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<DispatchablePerson> DOAPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("u_m_m_doa_01",100,100) };
        List<DispatchablePerson> IAAPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_fibsec_01",100,100) };
        List<DispatchablePerson> SAHPPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_hwaycop_01",100,100) };
        List<DispatchablePerson> MilitaryPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_armymech_01",25,0),
            new DispatchablePerson("s_m_m_marine_01",50,0),
            new DispatchablePerson("s_m_m_marine_02",0,0),
            new DispatchablePerson("s_m_y_marine_01",25,0),
            new DispatchablePerson("s_m_y_marine_02",0,0),
            new DispatchablePerson("s_m_y_marine_03",100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(2, 1, 0, 0),new PedComponent(8, 0, 0, 0) },new List<PedPropComponent>() { new PedPropComponent(3, 1, 0) }) },
            new DispatchablePerson("s_m_m_pilot_02",0,0),
            new DispatchablePerson("s_m_y_pilot_01",0,0) };
        List<DispatchablePerson> FIBPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_fibsec_01",55,70){MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_01",15,0){MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_m_fiboffice_02",15,0){MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("u_m_m_fibarchitect",10,0) {MaxWantedLevelSpawn = 3 },
            new DispatchablePerson("s_m_y_swat_01", 5,30) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4, RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 1,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<DispatchablePerson> PrisonPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_prisguard_01",100,100) };
        List<DispatchablePerson> SecurityPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_security_01",100,100) };
        List<DispatchablePerson> CoastGuardPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_uscg_01",100,100) };
        List<DispatchablePerson> NOOSEPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_swat_01", 100,100) { RequiredVariation = new PedVariation(new List<PedComponent>() { new PedComponent(10, 0, 0,0) },new List<PedPropComponent>() { new PedPropComponent(0, 0, 0) }) } };
        List<DispatchablePerson> NYSPPeds = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_snowcop_01",100,100) };

        List<DispatchablePerson> Firefighters = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_y_fireman_01",100,100) };
        List<DispatchablePerson> EMTs = new List<DispatchablePerson>() {
            new DispatchablePerson("s_m_m_paramedic_01",100,100) };
        

        //Vehicles
        List<DispatchableVehicle> UnmarkedVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police4", 100, 100)};
        List<DispatchableVehicle> AllUnmarkedVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 33, 33),
            new DispatchableVehicle("fbi2", 33, 33),
            new DispatchableVehicle("police4", 33, 33)};
        List<DispatchableVehicle> CoastGuardVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("predator", 75, 50),
            new DispatchableVehicle("dinghy", 0, 25),
            new DispatchableVehicle("seashark2", 25, 25) { MaxOccupants = 1 },};
        List<DispatchableVehicle> SecurityVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("dilettante2", 100, 100) {MaxOccupants = 1 } };
        List<DispatchableVehicle> ParkRangerVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("pranger", 100, 100) };
        List<DispatchableVehicle> FIBVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 30) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, RequiredPassengerModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 4, MaxOccupants = 6 },
        };
        List<DispatchableVehicle> NOOSEVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("fbi", 70, 70){ MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 30, 30) { MinWantedLevelSpawn = 0 , MaxWantedLevelSpawn = 3 },
            new DispatchableVehicle("fbi2", 0, 30) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, RequiredPassengerModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 4, MaxOccupants = 6 },
            new DispatchableVehicle("riot", 0, 70) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, RequiredPassengerModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 2, MaxOccupants = 3 },
            new DispatchableVehicle("annihilator", 0, 100) { MinWantedLevelSpawn = 4 ,MaxWantedLevelSpawn = 4, RequiredPassengerModels = new List<string>() { "s_m_y_swat_01" },MinOccupants = 3,MaxOccupants = 4 }};
        List<DispatchableVehicle> HighwayPatrolVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policeb", 70, 70) { MaxOccupants = 1 },
            new DispatchableVehicle("police4", 30, 30) };
        List<DispatchableVehicle> PrisonVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policet", 70, 70),
            new DispatchableVehicle("police4", 30, 30) };
        List<DispatchableVehicle> LSPDVehiclesVanilla = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 48,35) { RequiredLiveries = new List<int>() { 0,1,2,3,4,5 } },
            new DispatchableVehicle("police2", 25, 20) { RequiredLiveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new DispatchableVehicle("police3", 25, 20) { RequiredLiveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new DispatchableVehicle("police4", 1,1),
            new DispatchableVehicle("fbi2", 1,1),
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> LSSDVehiclesVanilla = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 50, 50){ RequiredLiveries = new List<int> { 0, 1, 2, 3 } },
            new DispatchableVehicle("sheriff2", 50, 50) { RequiredLiveries = new List<int> { 0, 1, 2, 3 } } };
        List<DispatchableVehicle> LSPDVehicles = LSPDVehiclesVanilla;
        List<DispatchableVehicle> SAHPVehicles = HighwayPatrolVehicles;
        List<DispatchableVehicle> LSSDVehicles = LSSDVehiclesVanilla;
        List<DispatchableVehicle> BCSOVehicles = LSSDVehiclesVanilla;
        List<DispatchableVehicle> VWHillsLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff2", 100, 100) { RequiredLiveries = new List<int> { 0, 1, 2, 3 } } };
        List<DispatchableVehicle> ChumashLSSDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff2", 100, 100) { RequiredLiveries = new List<int> { 0, 1, 2, 3 } } };
        List<DispatchableVehicle> LSSDDavisVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("sheriff", 100, 100){ RequiredLiveries = new List<int> { 0, 1, 2, 3 } } };
        List<DispatchableVehicle> RHPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police2", 100, 75) { RequiredLiveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> DPPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police3", 100, 75) { RequiredLiveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> ChumashLSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police3", 100, 75) { RequiredLiveries = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 } },
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> EastLSPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 100,75) { RequiredLiveries = new List<int>() { 0,1,2,3,4,5 } },
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> VWPDVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("police", 100,75) { RequiredLiveries = new List<int>() { 0,1,2,3,4,5 } },
            new DispatchableVehicle("policet", 0, 25) { MinWantedLevelSpawn = 3} };
        List<DispatchableVehicle> PoliceHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("polmav", 0,100) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 } };
        List<DispatchableVehicle> SheriffHeliVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("buzzard2", 0,25) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 },
            new DispatchableVehicle("polmav", 0,75) { RequiredLiveries = new List<int>() { 0 }, MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4,MinOccupants = 3,MaxOccupants = 3 } };
        List<DispatchableVehicle> ArmyVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("crusader", 75,50) { RequiredLiveries = new List<int>() { 0 },MinOccupants = 1,MaxOccupants = 2,MaxWantedLevelSpawn = 4 },
            new DispatchableVehicle("barracks", 25,50) { RequiredLiveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 5,MinWantedLevelSpawn = 4 },
            new DispatchableVehicle("rhino", 0,10) { RequiredLiveries = new List<int>() { 0 },MinOccupants = 1,MaxOccupants = 2,MinWantedLevelSpawn = 5 },
            new DispatchableVehicle("valkyrie", 0,50) { RequiredLiveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 3,MinWantedLevelSpawn = 4 },
            new DispatchableVehicle("valkyrie2", 0,50) { RequiredLiveries = new List<int>() { 0 },MinOccupants = 3,MaxOccupants = 3,MinWantedLevelSpawn = 4 },
        };
        List<DispatchableVehicle> OldSnowyVehicles = new List<DispatchableVehicle>() {
            new DispatchableVehicle("policeold1", 50, 50),
            new DispatchableVehicle("policeold2", 50, 50) };


        List<DispatchableVehicle> Firetrucks = new List<DispatchableVehicle>() {
            new DispatchableVehicle("firetruk", 100, 100) };

        List<DispatchableVehicle> Amublance1 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 0 } } };

        List<DispatchableVehicle> Amublance2 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 1 } } };

        List<DispatchableVehicle> Amublance3 = new List<DispatchableVehicle>() {
            new DispatchableVehicle("ambulance", 100, 100) { RequiredLiveries = new List<int>() { 2 } } };

        //Weapon
        List<IssuableWeapon> AllSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation()),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_combatpistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Etched Wood Grip Finish" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
        };
        List<IssuableWeapon> AllLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight" )})),
            new IssuableWeapon("weapon_pumpshotgun_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Holographic Sight" )})),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation()),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_carbinerifle", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation()),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
        };
        List<IssuableWeapon> BestSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip") })),
        };
        List<IssuableWeapon> BestLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight")})),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Holographic Sight"), new WeaponComponent("Grip"), new WeaponComponent("Extended Clip") })),
            new IssuableWeapon("weapon_carbinerifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Grip"), new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip" )})),
        };
        List<IssuableWeapon> HeliSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Extended Clip" )})),
            new IssuableWeapon("weapon_pistol_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight"), new WeaponComponent("Extended Clip")})),
        };
        List<IssuableWeapon> HeliLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_marksmanrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Suppressor"), new WeaponComponent("Tracer Rounds" )})),
            new IssuableWeapon("weapon_marksmanrifle_mk2", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Large Scope"), new WeaponComponent("Tracer Rounds") })),
        };
        List<IssuableWeapon> LimitedSidearms = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation()),
            new IssuableWeapon("weapon_revolver", new WeaponVariation()),
            new IssuableWeapon("weapon_heavypistol", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
        };
        List<IssuableWeapon> LimitedLongGuns = new List<IssuableWeapon>()
        {
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation()),
            new IssuableWeapon("weapon_pumpshotgun", new WeaponVariation(new List<WeaponComponent> {  new WeaponComponent("Flashlight" )})),
        };
        DefaultAgency = new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, StandardCops, LSPDVehicles, "LS ", AllSidearms, AllLongGuns);
        AgenciesList = new List<Agency>
        {
            new Agency("~b~", "LSPD", "Los Santos Police Department", "Blue", Classification.Police, StandardCops, LSPDVehicles, "LS ",AllSidearms,AllLongGuns) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-VW", "Los Santos Police - Vinewood Division", "Blue", Classification.Police, ExtendedStandardCops, VWPDVehicles, "LSV ",LimitedSidearms,LimitedLongGuns) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-ELS", "Los Santos Police - East Los Santos Division", "Blue", Classification.Police, ExtendedStandardCops, EastLSPDVehicles, "LSE ",LimitedSidearms,LimitedLongGuns) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-DP", "Los Santos Police - Del Pierro Division", "Blue", Classification.Police, StandardCops, DPPDVehicles, "VP ",AllSidearms,AllLongGuns) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-RH", "Los Santos Police - Rockford Hills Division", "Blue", Classification.Police, StandardCops, RHPDVehicles, "RH ",AllSidearms,AllLongGuns) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD", "Los Santos County Sheriff", "Red", Classification.Sheriff, SheriffPeds, LSSDVehicles, "LSCS ",LimitedSidearms,LimitedLongGuns) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-VW", "Los Santos Sheriff - Vinewood Division", "Red", Classification.Sheriff, SheriffPeds, VWHillsLSSDVehicles, "LSCS ",LimitedSidearms,LimitedLongGuns) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-CH", "Los Santos Sheriff - Chumash Division", "Red", Classification.Sheriff, SheriffPeds, ChumashLSSDVehicles, "LSCS ",LimitedSidearms,LimitedLongGuns) { MaxWantedLevelSpawn = 3 },
            new Agency("~r~", "LSSD-BC", "Los Santos Sheriff - Blaine County Division", "Red", Classification.Sheriff, SheriffPeds, BCSOVehicles, "BCS ",LimitedSidearms,LimitedLongGuns) { MaxWantedLevelSpawn = 3 },
            new Agency("~b~", "LSPD-ASD", "Los Santos Police Department - Air Support Division", "Blue", Classification.Police, PoliceAndSwat, PoliceHeliVehicles, "ASD ",HeliSidearms,HeliLongGuns) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, SpawnLimit = 3 },
            new Agency("~r~", "LSSD-ASD", "Los Santos Sheriffs Department - Air Support Division", "Red", Classification.Sheriff, SheriffAndSwat, SheriffHeliVehicles, "ASD ",HeliSidearms,HeliLongGuns) { MinWantedLevelSpawn = 3,MaxWantedLevelSpawn = 4, SpawnLimit = 3 },
            new Agency("~r~", "NOOSE", "National Office of Security Enforcement", "DarkSlateGray", Classification.Federal, NOOSEPeds, NOOSEVehicles, "",BestSidearms,BestLongGuns) { MinWantedLevelSpawn = 4, MaxWantedLevelSpawn = 4,CanSpawnAnywhere = true},
            new Agency("~p~", "FIB", "Federal Investigation Bureau", "Purple", Classification.Federal, FIBPeds, FIBVehicles, "FIB ",BestSidearms,BestLongGuns) {MaxWantedLevelSpawn = 4, SpawnLimit = 6,CanSpawnAnywhere = true },
            new Agency("~p~", "DOA", "Drug Observation Agency", "Purple", Classification.Federal, DOAPeds, UnmarkedVehicles, "DOA ",AllSidearms,AllLongGuns)  {MaxWantedLevelSpawn = 3, SpawnLimit = 4,CanSpawnAnywhere = true },
            new Agency("~y~", "SAHP", "San Andreas Highway Patrol", "Yellow", Classification.State, SAHPPeds, SAHPVehicles, "HP ",LimitedSidearms,LimitedLongGuns) { MaxWantedLevelSpawn = 3, SpawnsOnHighway = true },
            new Agency("~o~", "SASPA", "San Andreas State Prison Authority", "Orange", Classification.State, PrisonPeds, PrisonVehicles, "SASPA ",AllSidearms,AllLongGuns) { MaxWantedLevelSpawn = 3, SpawnLimit = 2 },
            new Agency("~g~", "SAPR", "San Andreas Park Ranger", "Green", Classification.State, ParkRangers, ParkRangerVehicles, "",AllSidearms,AllLongGuns) { MaxWantedLevelSpawn = 3, SpawnLimit = 3 },
            new Agency("~o~", "SACG", "San Andreas Coast Guard", "DarkOrange", Classification.State, CoastGuardPeds, CoastGuardVehicles, "SACG ",LimitedSidearms,LimitedLongGuns){ MaxWantedLevelSpawn = 3,SpawnLimit = 3 },
            new Agency("~p~", "LSPA", "Port Authority of Los Santos", "LightGray", Classification.Police, SecurityPeds, UnmarkedVehicles, "LSPA ",LimitedSidearms,LimitedLongGuns) {MaxWantedLevelSpawn = 3, SpawnLimit = 3 },
            new Agency("~p~", "LSIAPD", "Los Santos International Airport Police Department", "LightBlue", Classification.Police, StandardCops, LSPDVehicles, "LSA ",AllSidearms,AllLongGuns) { MaxWantedLevelSpawn = 3, SpawnLimit = 3 },
            new Agency("~u~", "ARMY", "Army", "Black", Classification.Military, MilitaryPeds, ArmyVehicles, "",BestSidearms,BestLongGuns) { MinWantedLevelSpawn = 5,CanSpawnAnywhere = true },       
            new Agency("~b~", "APD", "Acadia Police Department", "Blue", Classification.Police, StandardCops, AllUnmarkedVehicles, "APD ", AllSidearms,AllLongGuns) { MaxWantedLevelSpawn = 5 },
            new Agency("~b~", "APD-ASD", "Acadia Police Department - Air Support Division", "Blue", Classification.Police, PoliceAndSwat, PoliceHeliVehicles, "ASD ", HeliSidearms,HeliLongGuns) { MinWantedLevelSpawn = 3, MaxWantedLevelSpawn = 4, SpawnLimit = 3 },
            new Agency("~b~", "NYSP", "North Yankton State Police", "Blue", Classification.Police, NYSPPeds, OldSnowyVehicles, "NYSP ", LimitedSidearms,LimitedLongGuns) { MaxWantedLevelSpawn = 5 },
            new Agency("~g~", "VCPD", "Vice City Police Department", "Green", Classification.Police, StandardCops, AllUnmarkedVehicles, "VCPD ", AllSidearms,AllLongGuns) { MaxWantedLevelSpawn = 5 },
            new Agency("~b~", "LCPD", "Liberty City Police Department", "Blue", Classification.Police, StandardCops, AllUnmarkedVehicles, "LC ",AllSidearms, AllLongGuns) { MaxWantedLevelSpawn = 5 },
            new Agency("~b~", "LCPD", "Liberty City Police Department", "Blue", Classification.Police, StandardCops, AllUnmarkedVehicles, "LC ",AllSidearms, AllLongGuns) { MaxWantedLevelSpawn = 5 },


            new Agency("~r~", "LSFD", "Los Santos Fire Department", "Red", Classification.Fire, Firefighters, Firetrucks, "LSFD ",null, null) { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true },
            new Agency("~w~", "LSMC", "Los Santos Medical Center", "White", Classification.EMS, EMTs, Amublance1, "LSMC ",null, null) { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true },
            new Agency("~w~", "MRH", "Mission Row Hospital", "White", Classification.EMS, EMTs, Amublance2, "MRH ",null, null) { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true },
            new Agency("~w~", "LSFD", "Los Santos Fire Department", "White", Classification.EMS, EMTs, Amublance3, "LSFD ",null, null) { MaxWantedLevelSpawn = 0, CanSpawnAnywhere = true },

            new Agency("~s~", "UNK", "Unknown Agency", "White", Classification.Other, null, null, "",null,null) { MaxWantedLevelSpawn = 0 },
        };  
    }
    private void CustomConfig()
    {

    }
}
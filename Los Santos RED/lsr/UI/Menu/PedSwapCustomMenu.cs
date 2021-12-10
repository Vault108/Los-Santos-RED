﻿using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedSwapCustomMenu : Menu
{
    private UIMenu PedSwapCustomUIMenu;
    private UIMenuListItem TakeoverRandomPed;
    private UIMenuItem BecomeRandomPed;
    private UIMenuItem BecomeRandomCop;
    private IPedSwap PedSwap;
    private List<DistanceSelect> Distances;
    MenuPool MenuPool;
    private UIMenu ComponentsUIMenu;
    private UIMenu PropsUIMenu;
    private UIMenuItem ClearProps;
    private string CurrentSelectedComponent = "-1";
    private string DrawableSelected;
    private int TextureSelected;

    private int CurrentComponent = -1;
    private int CurrentDrawable = -1;
    private int CurrentTexture = -1;
    private UIMenuItem ChangeModel;
    private string ModelSelected = "S_M_M_GENTRANSPORT";
    private Camera CharCam;
    private Ped PedModel;
    private Vector3 PreviousPos;
    private float PreviousHeading;
    private UIMenuItem BecomeModel;
    private UIMenuListItem SelectModel;
    private UIMenuItem Exit;
    private bool IsDisposed = false;
    private UIMenuItem RandomizeVariation;
    private List<FashionComponent> ComponentLookup;
    private List<FashionProp> PropLookup;
    private List<ColorLookup> ColorList;
    private UIMenuItem ChangeMoney;
    private UIMenuItem ChangeName;
    private UIMenuItem RandomizeName;
    private string CurrentSelectedName = "John Doe";
    private int CurrentSelectedMoney = 5000;
    private UIMenuItem RandomizeHead;
    private UIMenuNumericScrollerItem<int> Parent1IDMenu;
    private UIMenuNumericScrollerItem<int> Parent2IDMenu;
    private UIMenuNumericScrollerItem<float> Parent1MixMenu;
    private UIMenuNumericScrollerItem<float> Parent2MixMenu;
    private UIMenuItem DefaultVariation;
    private UIMenuListScrollerItem<ColorLookup> HairPrimaryColorMenu;
    private UIMenuListScrollerItem<ColorLookup> HairSecondaryColorMenu;
    private int CurrentSelectedPrimaryHairColor = 0;
    private UIMenu CustomizeHeadMenu;
    private UIMenu DemographicsSubMenu;
    private INameProvideable Names;
    private int CurrentSelectedSecondaryHairColor;
    private HeadBlendData CurrentHeadblend = new HeadBlendData();
    private UIMenuItem RandomizeHair;

    private bool PedModelIsFreeMode => PedModel.Model.Name.ToLower() == "mp_f_freemode_01" || PedModel.Model.Name.ToLower() == "mp_m_freemode_01";

    public PedSwapCustomMenu(MenuPool menuPool, Ped pedModel, IPedSwap pedSwap, INameProvideable names)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        PedModel = pedModel;
        Names = names;
        PedSwapCustomUIMenu = new UIMenu("Customize Ped","Select an Option");
        PedSwapCustomUIMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        menuPool.Add(PedSwapCustomUIMenu);
        PedSwapCustomUIMenu.OnScrollerChange += OnScrollerChange;
        PedSwapCustomUIMenu.OnItemSelect += OnItemSelect;
        PedSwapCustomUIMenu.OnListChange += OnListChange;    
    }



    public void Setup()
    {
        Game.FadeScreenOut(1500, true);
        SetupPlayer();
        SetupCamera();
        SetupPedModel();
        SetupMenu();
        Game.FadeScreenIn(1500, true);
    }
    private void SetupPlayer()
    {

        PreviousPos = Game.LocalPlayer.Character.Position;
        PreviousHeading = Game.LocalPlayer.Character.Heading;
        Game.LocalPlayer.Character.Position = new Vector3(402.5164f, -1002.847f, -99.2587f);
    }
    private void SetupCamera()
    {
        CharCam = new Camera(false);
        CharCam.Position = new Vector3(402.8473f, -998.3224f, -98.00025f);
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        CharCam.Rotation = new Rotator(r.X, r.Y, r.Z);
        Vector3 ToLookAt = new Vector3(402.8473f, -996.3224f, -99.00025f);
        Vector3 _direction = (ToLookAt - CharCam.Position).ToNormalized();
        CharCam.Direction = _direction;
        CharCam.Active = true;
    }
    private void SetupPedModel()
    {
        if (PedModel.Exists())
        {
            PedModel.Delete();
        }
        PedModel = new Ped(ModelSelected, new Vector3(402.8473f, -996.3224f, -99.00025f), 182.7549f);
        GameFiber.Yield();
        if (PedModel.Exists())
        {
            PedModel.IsPersistent = true;
            PedModel.IsVisible = true;
            PedModel.BlockPermanentEvents = true;
        }
    }
    private void SetupMenu()
    {

        ComponentLookup = new List<FashionComponent>() {
            new FashionComponent(0,"Face"),
            new FashionComponent(1, "Mask/Beard"),
            new FashionComponent(2, "Hair"),
            new FashionComponent(3, "Torso"),
            new FashionComponent(4, "Lower"),
            new FashionComponent(5, "Bags"),
            new FashionComponent(6, "Foot"),
            new FashionComponent(7, "Accessories"),
            new FashionComponent(8, "Undershirt"),
            new FashionComponent(9, "Body Armor"),
            new FashionComponent(10, "Decals"),
            new FashionComponent(11, "Tops"), };

        PropLookup = new List<FashionProp>() {
            new FashionProp(0,"Hats"),
            new FashionProp(1, "Glasses"),
            new FashionProp(2, "Ear"),};

        ColorList = new List<ColorLookup>()
        { 
        new ColorLookup(0,"Black 1"),
        new ColorLookup(1,"Black 2"),
        new ColorLookup(2,"Black 3"),
        new ColorLookup(3,"Brown 1"),
        new ColorLookup(4,"Brown 2"),
        new ColorLookup(5,"Brown 3"),
        new ColorLookup(6,"Brown 4"),
        new ColorLookup(7,"Brown 5"),
        new ColorLookup(8,"Brown 6"),
        new ColorLookup(9,"Blonde 1"),
        new ColorLookup(10,"Blonde 2"),
        new ColorLookup(11,"Blonde 3"),
        new ColorLookup(12,"Blonde 4"),
        new ColorLookup(13,"Blonde 5"),
        new ColorLookup(14,"Blonde 6"),
        new ColorLookup(15,"Blonde 7"),
        new ColorLookup(16,"Blonde 8"),
        new ColorLookup(17,"Blonde 9"),
        new ColorLookup(18,"Redhead 1"),
        new ColorLookup(19,"Redhead 2"),
        new ColorLookup(20,"Redhead 3"),
        new ColorLookup(21,"Redhead 4"),
        new ColorLookup(22,"Orange 1"),
        new ColorLookup(23,"Orange 2"),
        new ColorLookup(24,"Orange 3"),
        new ColorLookup(25,"Orange 4"),
        new ColorLookup(26,"Grey 1"),
        new ColorLookup(27,"Grey 2"),
        new ColorLookup(28,"White 1"),
        new ColorLookup(29,"White 2"),
        new ColorLookup(30,"Purple 1"),
        new ColorLookup(31,"Purple 2"),
        new ColorLookup(32,"Purple 3"),
        new ColorLookup(33,"Pink 1"),
        new ColorLookup(34,"Pink 2"),
        new ColorLookup(35,"Pink 3"),
        new ColorLookup(36,"Blue 4"),
        new ColorLookup(37,"Blue 5"),
        new ColorLookup(38,"Blue 6"),
        new ColorLookup(39,"Green 1"),
        new ColorLookup(40,"Green 2"),
        new ColorLookup(41,"Green 3"),
        new ColorLookup(42,"Green 4"),
        new ColorLookup(43,"Green 5"),
        new ColorLookup(44,"Green 6"),
        new ColorLookup(45,"Yellow 1"),
        new ColorLookup(46,"Yellow 2"),
        new ColorLookup(47,"Yellow 3"),
        new ColorLookup(48,"Orange 5"),
        new ColorLookup(49,"Orange 6"),
        new ColorLookup(50,"Orange 7"),
        new ColorLookup(51,"Orange 8"),
        new ColorLookup(52,"Red 1"),
        new ColorLookup(53,"Red 1"),
        new ColorLookup(54,"Red 1"),
        new ColorLookup(55,"Brown 7"),
        new ColorLookup(56,"Brown 8"),
        new ColorLookup(57,"Brown 9"),
        new ColorLookup(58,"Brown 10"),
        new ColorLookup(59,"Brown 11"),
        new ColorLookup(60,"Brown 12"),
        new ColorLookup(61,"Black 4"),
        new ColorLookup(62,"Unk 1"),
        new ColorLookup(63,"Unk 2"),
        };


        PedSwapCustomUIMenu.Clear();

        DemographicsSubMenu = MenuPool.AddSubMenu(PedSwapCustomUIMenu, "Set Demographics");
        DemographicsSubMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        DemographicsSubMenu.OnItemSelect += OnItemSelect;
        DemographicsSubMenu.OnIndexChange += OnIndexChange;

        ChangeName = new UIMenuItem("Change Name", "Current: " + CurrentSelectedName);
        RandomizeName = new UIMenuItem("Randomize Name", "Current: " + CurrentSelectedName);
        ChangeMoney = new UIMenuItem("Set Money", "Amount: " + CurrentSelectedMoney.ToString("C0"));

        DemographicsSubMenu.AddItem(ChangeName);
        DemographicsSubMenu.AddItem(RandomizeName);
        DemographicsSubMenu.AddItem(ChangeMoney);

        ChangeModel = new UIMenuItem("Input Model","Enter Model Name");
        SelectModel = new UIMenuListItem("Select Model", "Select From Available", Rage.Model.PedModels.Select(x=> x.Name));
        RandomizeVariation = new UIMenuItem("Randomize Variation", "Set Random Variation");
        DefaultVariation = new UIMenuItem("Default Variation", "Set Default Variation");
        PedSwapCustomUIMenu.AddItem(ChangeModel);
        PedSwapCustomUIMenu.AddItem(SelectModel);
        PedSwapCustomUIMenu.AddItem(RandomizeVariation);
        PedSwapCustomUIMenu.AddItem(DefaultVariation);



        //Submenu
        CustomizeHeadMenu = MenuPool.AddSubMenu(PedSwapCustomUIMenu, "Set Head");
        CustomizeHeadMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        CustomizeHeadMenu.OnItemSelect += OnItemSelect;
        CustomizeHeadMenu.OnIndexChange += OnIndexChange;
        CustomizeHeadMenu.OnScrollerChange += OnScrollerChange;

        RandomizeHead = new UIMenuItem("Randomize Head","Set Random Head Data");
        Parent1IDMenu = new UIMenuNumericScrollerItem<int>("Set Parent 1", "Select Parent ID 1", 0, 45, 1);
        Parent2IDMenu = new UIMenuNumericScrollerItem<int>("Set Parent 2", "Select Parent ID 2", 0, 45, 1);
        Parent1MixMenu = new UIMenuNumericScrollerItem<float>("Set Parent 1 Mix", "Select Percent Of Parent ID 1 To Use", 0.0f, 1.0f, 0.1f);
        Parent2MixMenu = new UIMenuNumericScrollerItem<float>("Set Parent 2 Mix", "Select Percent Of Parent ID 2 To Use", 0.0f, 1.0f, 0.1f);

        RandomizeHair = new UIMenuItem("Randomize Hair", "Set Randome Hair (Use Components to Select Manually)");

        HairPrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Primary Hair Color", "Select Primary Hair Color (Requires Head Data)", ColorList);
        HairSecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Secondary Hair Color", "Select Secondary Hair Color (Requires Head Data)", ColorList);

        CustomizeHeadMenu.AddItem(RandomizeHead);
        CustomizeHeadMenu.AddItem(Parent1IDMenu);
        CustomizeHeadMenu.AddItem(Parent2IDMenu);
        CustomizeHeadMenu.AddItem(Parent1MixMenu);
        CustomizeHeadMenu.AddItem(Parent2MixMenu);
        CustomizeHeadMenu.AddItem(RandomizeHair);


        CustomizeHeadMenu.AddItem(HairPrimaryColorMenu);
        CustomizeHeadMenu.AddItem(HairSecondaryColorMenu);

        //Submenu
        ComponentsUIMenu = MenuPool.AddSubMenu(PedSwapCustomUIMenu, "Customize Components");
        ComponentsUIMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        ComponentsUIMenu.OnItemSelect += OnItemSelect;
        ComponentsUIMenu.OnIndexChange += OnIndexChange;

        PropsUIMenu = MenuPool.AddSubMenu(PedSwapCustomUIMenu, "Customize Props");
        PropsUIMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        PropsUIMenu.OnItemSelect += OnItemSelect;
        RefreshMenuList();

        ClearProps = new UIMenuItem("Clear Props","Removes All Props");
        BecomeModel = new UIMenuItem("Become Character","Return To Gameplay As Character");
        Exit = new UIMenuItem("Exit","Return To Gameplay");

        BecomeModel.RightBadge = UIMenuItem.BadgeStyle.Clothes;
        Exit.RightBadge = UIMenuItem.BadgeStyle.Alert;
        PedSwapCustomUIMenu.AddItem(ClearProps);
        PedSwapCustomUIMenu.AddItem(BecomeModel);  
        PedSwapCustomUIMenu.AddItem(Exit);
    }
    private void RefreshMenuList()
    {
        ComponentsUIMenu.Clear();
        PropsUIMenu.Clear();
        if (PedModel.Exists())
        {
            if(PedModelIsFreeMode)
            {
                RandomizeHead.Enabled = true;
                Parent1IDMenu.Enabled = true;
                Parent2IDMenu.Enabled = true;
                Parent1MixMenu.Enabled = true;
                Parent2MixMenu.Enabled = true;
                RandomizeHair.Enabled = true;
                HairPrimaryColorMenu.Enabled = true;
                HairSecondaryColorMenu.Enabled = true;
            }
            else
            {
                RandomizeHead.Enabled = false;
                Parent1IDMenu.Enabled = false;
                Parent2IDMenu.Enabled = false;
                Parent1MixMenu.Enabled = false;
                Parent2MixMenu.Enabled = false;
                RandomizeHair.Enabled = false;
                HairPrimaryColorMenu.Enabled = false;
                HairSecondaryColorMenu.Enabled = false;
            }
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(PedModel, ComponentNumber);
                string description = $"Component: {ComponentNumber}";
                FashionComponent fashionComponent =  ComponentLookup.FirstOrDefault(x => x.ComponentID == ComponentNumber);
                if (fashionComponent != null)
                {
                    description = fashionComponent.ComponentName;
                }
                UIMenu ComponentSubMenu = MenuPool.AddSubMenu(ComponentsUIMenu, description);
                for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
                {
                    int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(PedModel, ComponentNumber, DrawableNumber) - 1;
                    UIMenuNumericScrollerItem<int> Test = new UIMenuNumericScrollerItem<int>($"Drawable: {DrawableNumber}", "Arrow to Change Texture, Select to Reset Texture", 0, NumberOfTextureVariations, 1);
                    ComponentSubMenu.AddItem(Test);
                }
                ComponentSubMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
                ComponentSubMenu.OnItemSelect += OnComponentItemSelect;
                ComponentSubMenu.OnScrollerChange += OnComponentScollerChange;
                ComponentSubMenu.OnIndexChange += OnComponentIndexChange;
            }
            for (int PropsNumber = 0; PropsNumber < 3; PropsNumber++)
            {
                int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS<int>(Game.LocalPlayer.Character, PropsNumber);
                string description = $"Prop: {PropsNumber}";
                FashionProp fashionProp = PropLookup.FirstOrDefault(x => x.PropID == PropsNumber);
                if (fashionProp != null)
                {
                    description = fashionProp.PropName;
                }
                UIMenu PropSubMenu = MenuPool.AddSubMenu(PropsUIMenu, description);
                for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
                {
                    int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS<int>(PedModel, PropsNumber, DrawableNumber);
                    UIMenuNumericScrollerItem<int> Test = new UIMenuNumericScrollerItem<int>($"Drawable: {DrawableNumber}", "", 0, NumberOfTextureVariations, 1);
                    PropSubMenu.AddItem(Test);
                }
                PropSubMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
                PropSubMenu.OnScrollerChange += OnPropsScollerChange;
                PropSubMenu.OnIndexChange += OnPropsIndexChange;
            }
        }
    }
    public override void Hide()
    {
        PedSwapCustomUIMenu.Visible = false;
    }
    public override void Show()
    {
        PedSwapCustomUIMenu.Visible = true;
    }
    public override void Toggle()
    {

        if (!PedSwapCustomUIMenu.Visible)
        {
            PedSwapCustomUIMenu.Visible = true;
        }
        else
        {
            PedSwapCustomUIMenu.Visible = false;
        }
    }
    public void Update()
    {
        MenuPool.ProcessMenus();
    }
    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            if (!Game.IsScreenFadedOut && !Game.IsScreenFadingOut)
            {
                Game.FadeScreenOut(1500, true);
            }
            if (PedModel.Exists() && PedModel.Handle != Game.LocalPlayer.Character.Handle)
            {
                PedModel.Delete();
            }
            MenuPool.CloseAllMenus();
            CharCam.Active = false;
            Game.LocalPlayer.Character.Position = PreviousPos;
            Game.LocalPlayer.Character.Heading = PreviousHeading;
            Game.FadeScreenIn(1500, true);
        }
    }


    private void SetHeadblendData()
    {
        if(CurrentHeadblend != null)
        {
            NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(PedModel, CurrentHeadblend.shapeFirst, CurrentHeadblend.shapeSecond, CurrentHeadblend.shapeThird, CurrentHeadblend.skinFirst, CurrentHeadblend.skinSecond, CurrentHeadblend.skinThird, CurrentHeadblend.shapeMix, CurrentHeadblend.skinMix, CurrentHeadblend.thirdMix, false);
        }
        NativeFunction.Natives.x4CFFC65454C93A49(PedModel, CurrentSelectedPrimaryHairColor, CurrentSelectedPrimaryHairColor);
    }


    private void OnIndexChange(UIMenu sender, int newIndex)
    {
        if (sender == ComponentsUIMenu)
        {
            CurrentComponent = newIndex;
        }
        EntryPoint.WriteToConsole($"OnIndexChange {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (sender == PedSwapCustomUIMenu)
        {
            if (selectedItem == ChangeModel)
            {
                ModelSelected = NativeHelper.GetKeyboardInput("player_zero");
                if(new Rage.Model(ModelSelected).IsValid)
                {
                    SetupPedModel();
                    RefreshMenuList();
                }
            }
            if (selectedItem == SelectModel)
            {
                if (new Rage.Model(ModelSelected).IsValid)
                {
                    SetupPedModel();
                    RefreshMenuList();
                }

            }
            if (selectedItem == BecomeModel)
            {
                if (PedModel.Exists())
                {
                    Game.FadeScreenOut(1500, true);
                    PedSwap.BecomeExistingPed(PedModel, CurrentSelectedName, CurrentSelectedMoney, PedModelIsFreeMode ? CurrentHeadblend : null, CurrentSelectedPrimaryHairColor, CurrentSelectedSecondaryHairColor);
                    Dispose();
                }
            }
            if (selectedItem == ClearProps)
            {
                if (PedModel.Exists())
                {
                    NativeFunction.Natives.CLEAR_ALL_PED_PROPS(PedModel);
                }
            }
            if (selectedItem == RandomizeVariation)
            {
                if (PedModel.Exists())
                {
                    NativeFunction.Natives.CLEAR_ALL_PED_PROPS(PedModel);
                    NativeFunction.Natives.SET_PED_RANDOM_COMPONENT_VARIATION(PedModel);
                    NativeFunction.Natives.SET_PED_RANDOM_PROPS(PedModel);
                }
            }
            if (selectedItem == DefaultVariation)
            {
                if (PedModel.Exists())
                {
                    NativeFunction.Natives.CLEAR_ALL_PED_PROPS(PedModel);
                    NativeFunction.Natives.SET_PED_DEFAULT_COMPONENT_VARIATION(PedModel);
                }
            }
            if (selectedItem == Exit)
            {
                Dispose();
            }
        }
        else if (sender == CustomizeHeadMenu)
        {
            if (selectedItem == RandomizeHead)
            {
                if (PedModel.Exists() && PedModelIsFreeMode)
                {
                    int MotherID = 0;
                    int FatherID = 0;
                    float FatherSide = 0f;
                    float MotherSide = 0f;
                    MotherID = RandomItems.GetRandomNumberInt(0, 45);
                    FatherID = RandomItems.GetRandomNumberInt(0, 45);
                    if (PedModel.IsMale)
                    {
                        FatherSide = RandomItems.GetRandomNumber(0.75f, 1.0f);
                        MotherSide = 1.0f - FatherSide;
                    }
                    else
                    {
                        MotherSide = RandomItems.GetRandomNumber(0.75f, 1.0f);
                        FatherSide = 1.0f - MotherSide;
                    }


                    Parent1IDMenu.Value = MotherID;
                    Parent2IDMenu.Value = FatherID;
                    Parent1MixMenu.Value = MotherSide;
                    Parent2MixMenu.Value = FatherSide;
                    CurrentHeadblend = new HeadBlendData(MotherID, FatherID, 0, MotherID, FatherID, 0, MotherSide, FatherSide, 0.0f);
                    SetHeadblendData();
                    GameFiber.Yield();
                }
            }
            if (selectedItem == RandomizeHair)
            {
                if (PedModel.Exists() && PedModelIsFreeMode)
                {
                    SetHeadblendData();
                    int DrawableID = RandomItems.GetRandomNumberInt(0, NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(PedModel, 2));
                    int TextureID  = RandomItems.GetRandomNumberInt(0, NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(PedModel, 2, DrawableID) - 1);
                    NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(PedModel, 2, DrawableID, TextureID, 0);
                    CurrentSelectedPrimaryHairColor = RandomItems.GetRandomNumberInt(0, ColorList.Count());
                    CurrentSelectedSecondaryHairColor = RandomItems.GetRandomNumberInt(0, ColorList.Count());
                    HairPrimaryColorMenu.Index = CurrentSelectedPrimaryHairColor;
                    HairSecondaryColorMenu.Index = CurrentSelectedSecondaryHairColor;
                    SetHeadblendData();
                    GameFiber.Yield();
                }
            }
        }
        else if (sender == DemographicsSubMenu)
        {
            if (selectedItem == ChangeName)
            {
                CurrentSelectedName = NativeHelper.GetKeyboardInput(CurrentSelectedName);
                ChangeName.Description = "Current: " + CurrentSelectedName;
                RandomizeName.Description = "Current: " + CurrentSelectedName;
            }
            if (selectedItem == RandomizeName)
            {
                string Name = "John Doe";
                if(PedModel.Exists())
                {
                    Name = Names.GetRandomName(PedModel.IsMale);
                }
                else
                {
                    Name = Names.GetRandomName(false);
                }
                CurrentSelectedName = Name;
                ChangeName.Description = "Current: " + CurrentSelectedName;
                RandomizeName.Description = "Current: " + CurrentSelectedName;
            }
            if (selectedItem == ChangeMoney)
            {
                if (int.TryParse(NativeHelper.GetKeyboardInput(CurrentSelectedMoney.ToString()), out int BribeAmount))
                {
                    CurrentSelectedMoney = BribeAmount;
                    ChangeMoney.Description = "Current: " + CurrentSelectedMoney.ToString("C0");
                }
            }
        }
        else if (sender == ComponentsUIMenu)
        {
            CurrentSelectedComponent = selectedItem.Text;
            CurrentComponent = index;
        }
        else if (sender == PropsUIMenu)
        {
            CurrentSelectedComponent = selectedItem.Text;
            CurrentComponent = index;
        }
        EntryPoint.WriteToConsole($"OnItemSelect {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if(list == SelectModel)
        {
            ModelSelected = list.SelectedValue.ToString();//  list.Items[index].Value;

        }
        EntryPoint.WriteToConsole($"OnListChange index {index} {ModelSelected} {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnScrollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        if(item == HairPrimaryColorMenu)
        {
            CurrentSelectedPrimaryHairColor = newIndex;
            if (PedModelIsFreeMode)
            { 
                NativeFunction.Natives.x4CFFC65454C93A49(PedModel, CurrentSelectedPrimaryHairColor, CurrentSelectedSecondaryHairColor);
                EntryPoint.WriteToConsole($"PedSwapCustomeMenu Hair Color Changed {CurrentSelectedPrimaryHairColor} {CurrentSelectedSecondaryHairColor}", 5);
            }
        }
        else if(item == HairSecondaryColorMenu)
        {
            CurrentSelectedSecondaryHairColor = newIndex;
            if (PedModelIsFreeMode)
            {
                NativeFunction.Natives.x4CFFC65454C93A49(PedModel, CurrentSelectedPrimaryHairColor, CurrentSelectedSecondaryHairColor);
                EntryPoint.WriteToConsole($"PedSwapCustomeMenu Hair Color Changed {CurrentSelectedPrimaryHairColor} {CurrentSelectedSecondaryHairColor}", 5);
            }
        }
        else if (item == Parent1IDMenu)
        {
            CurrentHeadblend.skinFirst = newIndex;
            CurrentHeadblend.shapeFirst = newIndex;
            SetHeadblendData();
        }
        else if (item == Parent2IDMenu)
        {
            CurrentHeadblend.skinSecond = newIndex;
            CurrentHeadblend.shapeSecond = newIndex;
            SetHeadblendData();
        }
        else if (item == Parent1MixMenu)
        {
            if(float.TryParse(item.OptionText, out float newMix))
            {
                CurrentHeadblend.shapeMix = newMix;
                CurrentHeadblend.skinMix = 1.0f - newMix;



                Parent2MixMenu.Value = 1.0f - newMix;
                SetHeadblendData();
            }
        }
        else if (item == Parent2MixMenu)
        {
            if (float.TryParse(item.OptionText, out float newMix))
            {
                CurrentHeadblend.skinMix = newMix;
                CurrentHeadblend.shapeMix = 1.0f - newMix;

                Parent1MixMenu.Value = 1.0f - newMix;

                SetHeadblendData();
            }
        }
    }


    private void OnComponentItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        CurrentDrawable = sender.CurrentSelection;//???????
        TextureSelected = index;
        CurrentTexture = index;

        TextureSelected = 0;
        CurrentTexture = 0;

        bool IsValid = false;
        if (PedModel.Exists())
        {
            IsValid = NativeFunction.Natives.IS_PED_COMPONENT_VARIATION_VALID<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture);
            if (IsValid || !IsValid)
            {
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture, 0);
            }
        }
        EntryPoint.WriteToConsole($"OnComponentItemSelect IsValid {IsValid} {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnComponentIndexChange(UIMenu sender, int newIndex)
    {
        CurrentDrawable = newIndex;
        EntryPoint.WriteToConsole($"OnComponentIndexChange {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnComponentScollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        CurrentDrawable = sender.CurrentSelection;//???????
        TextureSelected = newIndex;
        CurrentTexture = newIndex;
        bool IsValid = false;
        if (PedModel.Exists())
        {
            IsValid = NativeFunction.Natives.IS_PED_COMPONENT_VARIATION_VALID<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture);
            if (IsValid || !IsValid)
            {
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture, 0);
            }
        }
        EntryPoint.WriteToConsole($"OnComponentScollerChange IsValid {IsValid} {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }




    private void OnPropItemSelect(UIMenu sender, UIMenuItem selectedItem, int newIndex)
    {
        CurrentDrawable = sender.CurrentSelection;//???????

        TextureSelected = newIndex;
        CurrentTexture = newIndex;
        int propID = -1;
        if (PedModel.Exists())
        {
            //bool IsValid = NativeFunction.Natives.IS_PED_COMPONENT_VARIATION_VALID<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture);
            //if (IsValid || !IsValid)
            //{
            propID = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(PedModel, CurrentComponent);
            NativeFunction.Natives.CLEAR_PED_PROP(PedModel, propID);
            NativeFunction.Natives.SET_PED_PROP_INDEX<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture, true);
            //}
        }
        EntryPoint.WriteToConsole($"OnPropsScollerChange propID {propID} {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnPropsIndexChange(UIMenu sender, int newIndex)
    {
        CurrentDrawable = newIndex;
        EntryPoint.WriteToConsole($"OnComponentIndexChange {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnPropsScollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        CurrentDrawable = sender.CurrentSelection;//???????
        TextureSelected = newIndex;
        CurrentTexture = newIndex;
        int propID = -1;
        if (PedModel.Exists())
        {
            //bool IsValid = NativeFunction.Natives.IS_PED_COMPONENT_VARIATION_VALID<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture);
            //if (IsValid || !IsValid)
            //{
            propID = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(PedModel, CurrentComponent);
            NativeFunction.Natives.CLEAR_PED_PROP(PedModel, propID);
            NativeFunction.Natives.SET_PED_PROP_INDEX<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture, true);
            //}
        }
        EntryPoint.WriteToConsole($"OnPropsScollerChange propID {propID} {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }




    private class FashionComponent
    {
        public FashionComponent()
        {

        }
        public FashionComponent(int componentID, string componentName)
        {
            ComponentID = componentID;
            ComponentName = componentName;
        }

        public int ComponentID { get; set; }
        public string ComponentName { get; set; }
    }
    private class FashionProp
    {
        public FashionProp()
        {

        }
        public FashionProp(int propID, string propName)
        {
            PropID = propID;
            PropName = propName;
        }

        public int PropID { get; set; }
        public string PropName { get; set; }
    }
    private class ColorLookup
    {
        public ColorLookup()
        {

        }
        public ColorLookup(int colorID, string colorName)
        {
            ColorID = colorID;
            ColorName = colorName;
        }

        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public override string ToString()
        {
            return ColorName;
        }
    }
}
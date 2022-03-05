﻿using System;

[Serializable()]
public class ModItem
{
    public ModItem()
    {

    }
    public ModItem(string name)
    {
        Name = name;
    }
    public ModItem(string name, bool requiresDLC)
    {
        Name = name;
        RequiresDLC = requiresDLC;
    }
    public ModItem(string name, string description)
    {
        Name = name;
        Description = description;
    }
    public ModItem(string name, string description, bool requiresDLC)
    {
        Name = name;
        Description = description;
        RequiresDLC = requiresDLC;
    }
    public ModItem(string name, eConsumableType type)
    {
        Name = name;
        Type = type;
    }
    public ModItem(string name, string description, eConsumableType type)
    {
        Name = name;
        Description = description;
        Type = type;
    }
    public PhysicalItem ModelItem { get; set; }
    public PhysicalItem PackageItem { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = "";


    public string MeasurementName { get; set; } = "Item";


    public int AmountPerPackage { get; set; } = 1;



    public bool CanConsume => Type == eConsumableType.Drink || Type == eConsumableType.Eat || Type == eConsumableType.Smoke || Type == eConsumableType.Ingest || Type == eConsumableType.AltSmoke || Type == eConsumableType.Snort || Type == eConsumableType.Inject;
    public eConsumableType Type { get; set; } = eConsumableType.None;
    public string IntoxicantName { get; set; } = "";
    public bool IsIntoxicating => IntoxicantName != "";
    public bool ChangesHealth => HealthChangeAmount != 0;
    public int HealthChangeAmount { get; set; } = 0;
    public string HealthChangeDescription => HealthChangeAmount > 0 ? $"~g~+{HealthChangeAmount} ~s~HP" : $"~r~{HealthChangeAmount} ~s~HP";
    public bool ConsumeOnPurchase { get; set; } = false;



    public bool RequiresDLC { get; set; } = false;


    public bool IsTool => ToolType != ToolTypes.None;
    public ToolTypes ToolType { get; set; } = ToolTypes.None;
    public bool RequiresTool => RequiredToolType != ToolTypes.None;
    public ToolTypes RequiredToolType { get; set; } = ToolTypes.None;
    public float PercentLostOnUse { get; set; } = 0.0f;



    public string FormattedItemType
    {
        get
        {
            if(IsTool)
            {
                return "Tool - " + ToolType.ToString();
            }
            if(Type == eConsumableType.Drink)
            {
                return "Drinkable";
            }
            else if (Type == eConsumableType.Eat)
            {
                return "Edible";
            }
            else if (Type == eConsumableType.Smoke)
            {
                return "Smokable";
            }
            else if (Type == eConsumableType.AltSmoke)
            {
                return "Smokable";
            }
            else if (Type == eConsumableType.Ingest || Type == eConsumableType.Snort)
            {
                return "Ingestable";
            }
            else if (Type == eConsumableType.Inject)
            {
                return "Injectable";
            }
            else if (Type == eConsumableType.Service)
            {
                return "Service";
            }
            else if (Type == eConsumableType.None)
            {
                return "Other";
            }
            else
            {
                return Type.ToString();
            }
        }
    }
}


﻿using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SpawnLocation
{
    public SpawnLocation()
    {

    }
    public SpawnLocation(Vector3 initialPosition)
    {
        InitialPosition = initialPosition;
    }
    public bool HasSpawns => InitialPosition != Vector3.Zero && StreetPosition != Vector3.Zero;
    public float Heading { get; set; }
    public Vector3 InitialPosition { get; set; } = Vector3.Zero;
    public Vector3 StreetPosition { get; set; } = Vector3.Zero;
    public float GetWaterHeight()
    {
        if (NativeFunction.Natives.GET_WATER_HEIGHT<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out float height))
        {
            return height;
        }
        return height;
    }
    public void GetClosestStreet()
    {
        NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out Vector3 streetPosition, out float streetHeading, 0, 3, 0);
        StreetPosition = streetPosition;
        Heading = streetHeading;
    }
}

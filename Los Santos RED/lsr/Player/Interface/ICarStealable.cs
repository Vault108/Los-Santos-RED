﻿using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ICarStealable
    {
        bool IsLockPicking { get; set; }
        bool IsConsideredArmed { get; }
        bool IsBusted { get; }
        bool IsDead { get; }
        void SetPlayerToLastWeapon();
        bool IsCarJacking { get; set; }
        void SetUnarmed();
        void ShootAt(Vector3 targetCoordinate);
    }
}

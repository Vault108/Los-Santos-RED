﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ISettingsProvideable
    {
        SettingsManager SettingsManager { get; }

        void SerializeAllSettings();
    }
}
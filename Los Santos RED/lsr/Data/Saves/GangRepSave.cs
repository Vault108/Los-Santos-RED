﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangRepSave
{
    public GangRepSave()
    {
    }

    public GangRepSave(string gangID, int reputation, int membersHurt, int membersKilled, int membersCarJacked, int membersHurtInTerritory, int membersKilledInTerritory, int membersCarJackedInTerritory)
    {
        GangID = gangID;
        Reputation = reputation;
        MembersHurt = membersHurt;
        MembersKilled = membersKilled;
        MembersCarJacked = membersCarJacked;
        MembersHurtInTerritory = membersHurtInTerritory;
        MembersKilledInTerritory = membersKilledInTerritory;
        MembersCarJackedInTerritory = membersCarJackedInTerritory;
    }

    public string GangID { get; set; }
    public int Reputation { get; set; }
    public int MembersHurt { get; set; }
    public int MembersKilled { get; set; }
    public int MembersCarJacked { get; set; }

    public int MembersHurtInTerritory { get; set; }
    public int MembersKilledInTerritory { get; set; }
    public int MembersCarJackedInTerritory { get; set; }
}

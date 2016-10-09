﻿using System.Collections;
using System.Threading;

namespace _8th_Circle_Server
{
    public class ComRest : CommandClass
    {
        public ComRest(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "";

            if (mob.GetFlagList().Contains(MobFlags.FLAG_INCOMBAT))
                clientString = "you can't rest while in combat!\n";
            else if (!mob.GetFlagList().Contains(MobFlags.FLAG_RESTING))
            {
                clientString = "you sit down and rest...\n";
                mob.GetFlagList().Add(MobFlags.FLAG_RESTING);
                Thread restThread = new Thread(() => restTask(((CombatMob)mob)));
                restThread.Start();
            }
            else
                clientString = "you are already resting\n";

            return clientString;
        }// execute

        public static void restTask(CombatMob mob)
        {
            int maxHp = mob[STAT.BASEMAXHP] + mob[STAT.MAXHPMOD];
            int maxMana = mob[STAT.BASEMAXMANA] + mob[STAT.MAXMANAMOD];
            double hpRegen = (maxHp / 20) + 1;
            double manaRegen = (maxMana / 20) + 1;
            Thread.Sleep(4000);

            while (mob.GetFlagList().Contains(MobFlags.FLAG_RESTING))
            {
                mob[STAT.CURRENTHP] = mob[STAT.CURRENTHP] + (int)hpRegen;
                mob[STAT.CURRENTMANA] = mob[STAT.CURRENTMANA] + (int)manaRegen;

                if (mob[STAT.CURRENTHP] > (maxHp))
                    mob[STAT.CURRENTHP] = maxHp;
                if (mob[STAT.CURRENTMANA] > (maxMana))
                    mob[STAT.CURRENTMANA] = maxMana;
                if (mob.GetResType() == ResType.PLAYER)
                    mob.safeWrite(mob.playerString());

                Thread.Sleep(4000);
            }// while
        }// restTask

    }// class ComRest

}// namespace _8th_Circle_Server

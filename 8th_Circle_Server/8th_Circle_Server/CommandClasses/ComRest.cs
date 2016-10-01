using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            if (mob.mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
                clientString = "you can't rest while in combat!\n";
            else if (!mob.mFlagList.Contains(MobFlags.FLAG_RESTING))
            {
                clientString = "you sit down and rest...\n";
                mob.mFlagList.Add(MobFlags.FLAG_RESTING);
                Thread restThread = new Thread(() => restTask(((CombatMob)mob)));
                restThread.Start();
            }
            else
                clientString = "you are already resting\n";

            return clientString;
        }// execute

        public static void restTask(CombatMob mob)
        {
            int maxHp = mob.mStats.mBaseMaxHp + mob.mStats.mMaxHpMod;
            int maxMana = mob.mStats.mBaseMaxMana + mob.mStats.mMaxManaMod;
            double hpRegen = (maxHp / 20) + 1;
            double manaRegen = (maxMana / 20) + 1;
            Thread.Sleep(4000);

            while (mob.mFlagList.Contains(MobFlags.FLAG_RESTING))
            {
                mob.mStats.mCurrentHp += (int)hpRegen;
                mob.mStats.mCurrentMana += (int)manaRegen;

                if (mob.mStats.mCurrentHp > (maxHp))
                    mob.mStats.mCurrentHp = maxHp;
                if (mob.mStats.mCurrentMana > (maxMana))
                    mob.mStats.mCurrentMana = maxMana;
                if (mob.mResType == ResType.PLAYER)
                    mob.mClientHandler.safeWrite(mob.playerString());

                Thread.Sleep(4000);
            }// while
        }// restTask

    }// class ComRest

}// namespace _8th_Circle_Server

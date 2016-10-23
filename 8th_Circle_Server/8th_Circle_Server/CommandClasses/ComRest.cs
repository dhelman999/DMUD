using System;
using System.Collections;
using System.Threading;

namespace _8th_Circle_Server
{
    public class ComRest : CommandClass
    {
        public ComRest(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            mPostCmdOps.Clear();
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (!mob.HasFlag(MobFlags.RESTING))
            {
                clientString = "you sit down and rest...\n";
                Utils.SetFlag(ref mob.mFlags, MobFlags.RESTING);
                Thread restThread = new Thread(() => restTask(((CombatMob)mob)));
                restThread.Start();
                eCode = errorCode.E_OK;
            }
            else
                clientString = "you are already resting\n";

            return eCode;
        }// execute

        public static void restTask(CombatMob mob)
        {
            int maxHp = mob[STAT.BASEMAXHP] + mob[STAT.MAXHPMOD];
            int maxMana = mob[STAT.BASEMAXMANA] + mob[STAT.MAXMANAMOD];
            double hpRegen = (maxHp / 20) + 1;
            double manaRegen = (maxMana / 20) + 1;
            Thread.Sleep(4000);

            while (mob.HasFlag(MobFlags.RESTING))
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

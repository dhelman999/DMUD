using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class ComSpawn : CommandClass
    {
        public ComSpawn(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       GrammarType[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "";
            int commandIndex = 0;

            try
            {
                // TODO
                // Needs to be made more generic and cleaned up
                int mobID = Int32.Parse((string)commandQueue[++commandIndex]);
                List<Mob> fma = ((CombatMob)mob).mClientHandler.mWorld.mFullMobList;

                if (mobID < 0 || mobID > fma.Count)
                    clientString = "MobID is outside the valid range";
                else
                {
                    if (fma[mobID] is Container)
                    {
                        Container cont = new Container();
                        cont = (Container)fma[mobID];
                        ((CombatMob)mob).mClientHandler.mWorld.addRes(cont);
                        cont.mCurrentArea = ((CombatMob)mob).mClientHandler.mPlayer.mCurrentArea;
                        cont.mCurrentRoom = ((CombatMob)mob).mClientHandler.mPlayer.mCurrentRoom;
                        cont.mCurrentArea.addRes(cont);
                        cont.mCurrentRoom.addRes(cont);
                        clientString = "spawning " + cont.mName;
                    }// if
                    else if (fma[mobID] is Equipment)
                    {
                        Equipment eq = new Equipment();
                        eq = (Equipment)fma[mobID];
                        ((CombatMob)mob).mClientHandler.mWorld.addRes(eq);
                        eq.mCurrentArea = ((CombatMob)mob).mClientHandler.mPlayer.mCurrentArea;
                        eq.mCurrentRoom = ((CombatMob)mob).mClientHandler.mPlayer.mCurrentRoom;
                        eq.mCurrentArea.addRes(eq);
                        eq.mCurrentRoom.addRes(eq);
                        clientString = "spawning " + eq.mName;
                    }// if
                    else if (fma[mobID] is CombatMob)
                    {
                        CombatMob CombatMob = new CombatMob((CombatMob)fma[mobID]);
                        CombatMob.mWorld.addRes(CombatMob);
                        CombatMob.mCurrentArea = ((CombatMob)mob).mClientHandler.mPlayer.mCurrentArea;
                        CombatMob.mCurrentRoom = ((CombatMob)mob).mClientHandler.mPlayer.mCurrentRoom;
                        CombatMob.mCurrentArea.addRes(CombatMob);
                        CombatMob.mCurrentRoom.addRes(CombatMob);
                        clientString = "spawning " + CombatMob.mName;
                    }// if
                    else if (fma[mobID] is Mob)
                    {
                        Mob mob2 = new Mob();
                        mob2 = (Mob)fma[mobID];
                        mob2.mCurrentArea = ((CombatMob)mob).mClientHandler.mPlayer.mCurrentArea;
                        mob2.mCurrentRoom = ((CombatMob)mob).mClientHandler.mPlayer.mCurrentRoom;
                        mob2.mCurrentArea.addRes(mob2);
                        mob2.mCurrentRoom.addRes(mob2);
                        clientString = "spawning " + mob2.mName;
                    }// else if
                    else
                        clientString = "Something went wrong";
                }// else
            }// try
            catch
            {
                clientString = "That is not a valid MobID";
            }

            return clientString;
        }// execute

    }// class ComSpawn

}// namespace _8th_Circle_Server

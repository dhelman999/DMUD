﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace _8th_Circle_Server
{
    public class ComSearch : CommandClass
    {
        public ComSearch(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            mPreCmdOps.Add(new Tuple<MobFlags, CmdOps>(MobFlags.SEARCHING, CmdOps.SET));
        }

        // Kicks off a new thread to search the room
        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            clientString = "you start searching...\n";
            Thread searchThread = new Thread(() => searchTask(mob));
            searchThread.Start();
            
            return errorCode.E_OK;
        }// execute

        public static void searchTask(Mob searcher)
        {
            String clientString = String.Empty;
            Room currentRoom = searcher.GetCurrentRoom();
            Thread.Sleep(4000);  

            List<List<Mob>> targetLists = new List<List<Mob>>();
            targetLists.Add(currentRoom.getRes(ResType.OBJECT));
            targetLists.Add(currentRoom.getRes(ResType.PLAYER));
            targetLists.Add(currentRoom.getRes(ResType.NPC));
            targetLists.Add(currentRoom.getRes(ResType.DOORWAY));
            bool found = false;

            foreach (List<Mob> targetList in targetLists)
            {
                foreach (Mob target in targetList)
                {
                    if (target != null && target.HasFlag(MobFlags.HIDDEN))
                    {
                        clientString += "you discover a " + target.GetName();
                        Utils.UnsetFlag(ref target.mFlags, MobFlags.HIDDEN);
                        found = true;
                    }
                }
            }

            if (!found)
                clientString = "you find nothing";

            clientString += "\n";

            Utils.UnsetFlag(ref searcher.mFlags, MobFlags.SEARCHING);

            searcher.safeWrite(clientString);
        }// searchTask

    }// class ComSearch

}// namespace _8th_Circle_Server

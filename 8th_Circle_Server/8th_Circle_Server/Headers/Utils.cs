using System;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    class Utils
    {
        // There are some dictionaries that have more than one key.  This function creates a combination of objects as a single key.
        // The syntax for doing this in C# is really awkward, this function helps to make it slightly more readable even though it
        // is still pretty bad.  This is mainly used to get command classes out of and into dictionaries.
        static public Tuple<CommandName, int> createTuple(CommandName cmdName, int maxTokens)
        {
            return new Tuple<CommandName, int>(cmdName, maxTokens);
        }// createTuple

        // A generic implementation to set a bit in an enum bitmap.  This takes any enum with the [Flags] property
        // and sets the bit corresponding to any other enum type.
        public static void SetFlag<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue | flagValue);
        }// SetFlag

        // A generic implementation to unset a bit in an enum bitmap.  This takes any enum with the [Flags] property
        // and sets the bit corresponding to any other enum type.
        public static void UnsetFlag<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue & (~flagValue));
        }// UnsetFlag

        // Broadcasts a message to the broadcaster and a different message to everyone else
        public static void broadcast<T>(T location, Mob broadcaster, string receiversMessage, string broadcasterMessage = null) where T : ResourceHandler
        {
            foreach(CombatMob player in location.getRes(ResType.PLAYER))
            {
                if (player == broadcaster)
                    broadcaster.safeWrite(broadcasterMessage);
                else
                    player.safeWrite(receiversMessage);
            }      
        }// broadcast

        // Returns a string of all resources of a specific type in a location
        public static string printResources<T>(T location, ResType resType) where T : ResourceHandler
        {
            string returnString = string.Empty;

            List<Mob> locResources = location.getRes(resType);

            for(int index = 0; index < locResources.Count; ++index)
            {
                Mob currentMob = locResources[index];

                if (currentMob == null || currentMob.mFlags.HasFlag(MobFlags.HIDDEN))
                    continue;

                if(resType == ResType.DOORWAY)
                {
                    Direction dir = (Direction)index;
                    returnString += dir.ToString().ToLower() + " " + currentMob.GetName() + "\n";
                }
                else
                    returnString += currentMob.GetName() + "\n";
            }

            return returnString;
        }// printResources

    }// Utils

}// _8th_Circle_Server

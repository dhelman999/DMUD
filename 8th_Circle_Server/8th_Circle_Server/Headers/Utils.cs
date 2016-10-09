using System;

namespace _8th_Circle_Server
{
    class Utils
    {
        static public Tuple<commandName, int> createTuple(commandName cmdName, int maxTokens)
        {
            return new Tuple<commandName, int>(cmdName, maxTokens);
        }// createTuple

        public static void SetFlag<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue | flagValue);
        }// SetFlag

        public static void UnsetFlag<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue & (~flagValue));
        }// UnsetFlag

    }// Utils

}// _8th_Circle_Server

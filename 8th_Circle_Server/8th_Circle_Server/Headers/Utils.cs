using System;

namespace _8th_Circle_Server
{
    class Utils
    {
        static public Tuple<commandName, int> createTuple(commandName cmdName, int maxTokens)
        {
            return new Tuple<commandName, int>(cmdName, maxTokens);
        }// createTuple

    }// Utils

}// _8th_Circle_Server

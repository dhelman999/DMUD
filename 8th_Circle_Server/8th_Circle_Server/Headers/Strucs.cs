namespace _8th_Circle_Server
{
    public struct commandData
    {
        public string command;
        public CombatMob mob;

        public commandData(string command, CombatMob mob)
        {
            this.command = command;
            this.mob = mob;
        }// Constructor
    }// commandData

    public struct Preposition
    {
        public string name;
        public PrepositionType prepType;

        public Preposition(string name, PrepositionType prepType)
        {
            this.name = name;
            this.prepType = prepType;
        }// Constructor
    }// Preposition

}// _8th_Circle_Server
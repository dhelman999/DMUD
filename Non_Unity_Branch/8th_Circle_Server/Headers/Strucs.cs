using System;

namespace _8th_Circle_Server
{
    // Easy encapsulation for a command and the mob who is trying to execute it
    public struct CommandData
    {
        public String command;
        public CombatMob mob;

        public CommandData(String command, CombatMob mob)
        {
            this.command = command;
            this.mob = mob;
        }// Constructor
    }// CommandData

    // Valid prepisitions and their string representation
    public struct Preposition
    {
        public String name;
        public PrepositionType prepType;

        public Preposition(String name, PrepositionType prepType)
        {
            this.name = name;
            this.prepType = prepType;
        }// Constructor

    }// Preposition

    // All data needed to process events, this should probably be a class.
    public struct EventData
    {
        public ValidityType validity;
        private EventID EventID;
        private Mob trigger;
        private Mob eventObject;
        private Room eventRoom;
        private CommandName CommandName;
        private PrepositionType prepType;
        private Object data;

        public EventData(EventID EventID,
                         CommandName CommandName,
                         Object data = null)
        {
            this.EventID = EventID;
            this.trigger = null;
            this.eventObject = null;
            this.eventRoom = null;
            this.validity = ValidityType.NONE;
            this.CommandName = CommandName;
            this.prepType = PrepositionType.PREP_START;
            this.data = data;
        }// Constructor

        // Accessors
        public EventID GetEvent() { return EventID; }
        public void SetEvent(EventID flag) { EventID = flag; }
        public CommandName GetCommand() { return CommandName; }
        public void SetCommand(CommandName command) { CommandName = command; }
        public void SetPrep(PrepositionType prep) { prepType = prep; }
        public Object GetData() { return data; }
        public void SetData(Object dat) { data = dat; }
        public Mob GetTrigger() { return trigger; }
        public void SetTrigger(Mob trig) { trigger = trig; }
        public Mob GetEventObject() { return eventObject; }
        public void SetEventObject(Mob obj) { eventObject = obj; }
        public void SetRoom(Room room) { eventRoom = room; }
        public PrepositionType GetPrepType() { return prepType; }

    }// EventData

}// _8th_Circle_Server

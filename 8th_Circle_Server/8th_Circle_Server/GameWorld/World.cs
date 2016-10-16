using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public partial class World : ResourceHandler
    {
        // TODO
        // Each area should have its own offsets, and not belong to a global one
        // Spacings to seperate the areas
        internal const int HOUSE_OFFSET = 10000;
        internal const int BAO = 1000;
        internal const int PROTO_OFFSET = 100;

        // Constants
        const int MAXXSIZE = 3;
        const int MAXYSIZE = 3;
        const int MAXZSIZE = 3;
        
        // Member Variables
        private CommandHandler mCommandHandler;
        private EventHandler mEventHandler;
        private AreaHandler mAreaHandler;
        private CombatHandler mCombatHandler;
        private List<Area> mAreaList; 
        private Room[, ,] mWorldGrid;

        public World() : base()
        {
            mCommandHandler = new CommandHandler(this);
            mEventHandler = new EventHandler(this);
            mAreaHandler = new AreaHandler(this);
            mCombatHandler = new CombatHandler(this);
            mAreaList = new List<Area>();
            mWorldGrid = new Room[MAXXSIZE, MAXYSIZE, MAXZSIZE];

            // Add global prototypes and areas
            registerGlobalMobs();
            addAreas();

            // Start all the handlers
            mCommandHandler.start();
            mEventHandler.start();
            mAreaHandler.start();
            mCombatHandler.start();
        }// Constructor

        private void registerGlobalMobs()
        {
            Mob key = new Mob();
            key.SetDesc("An old brass key, what does it unlock?");
            Utils.SetFlag(ref key.mFlags, MobFlags.GETTABLE);
            Utils.SetFlag(ref key.mFlags, MobFlags.INSPECTABLE);
            Utils.SetFlag(ref key.mFlags, MobFlags.STORABLE);
            Utils.SetFlag(ref key.mFlags, MobFlags.DROPPABLE);
            key.SetName("brass key");
            key.SetKeyID((int)MobList.BASIC_KEY);
            key.GetInv().Capacity = 0;
            key.SetWorld(this);
            key.SetMobID(MobList.BASIC_KEY);
            PrototypeManager.registerFullGameMob(MobList.BASIC_KEY, key);

            Mob strange_lump = new Mob();
            strange_lump.SetDesc("A strange lump, made of... something");
            Utils.SetFlag(ref strange_lump.mFlags, MobFlags.GETTABLE);
            Utils.SetFlag(ref strange_lump.mFlags, MobFlags.INSPECTABLE);
            Utils.SetFlag(ref strange_lump.mFlags, MobFlags.STORABLE);
            Utils.SetFlag(ref strange_lump.mFlags, MobFlags.DROPPABLE);
            strange_lump.SetName("strange lump");
            strange_lump.GetInv().Capacity = 0;
            strange_lump.SetWorld(this);
            strange_lump.SetMobID(MobList.STRANGE_LUMP);
            PrototypeManager.registerFullGameMob(MobList.STRANGE_LUMP, strange_lump);

            Container chest = new Container();
            chest.SetDesc("A sturdy, wooden chest.  It makes you wonder what is inside...");
            Utils.SetFlag(ref chest.mFlags, MobFlags.OPENABLE);
            Utils.SetFlag(ref chest.mFlags, MobFlags.CLOSEABLE);
            Utils.SetFlag(ref chest.mFlags, MobFlags.LOCKED);
            Utils.SetFlag(ref chest.mFlags, MobFlags.LOCKABLE);
            Utils.SetFlag(ref chest.mFlags, MobFlags.UNLOCKABLE);
            chest.SetName("chest1");
            chest.GetInv().Capacity = 20;
            chest.SetWorld(this);
            EventData eventData = new EventData(EventFlag.EVENT_TELL_PLAYER, CommandName.COMMAND_LOOK, "A voice speaks to you from within the chest");
            eventData.SetPrep(PrepositionType.PREP_IN);
            chest.GetEventList().Add(eventData);
            chest.SetMobID(MobList.EVENT_CHEST1);
            chest.SetKeyID((int)MobList.BASIC_KEY);
            chest.SetStartingRespawnTime(30);
            chest.SetCurrentRespawnTime(30);
            PrototypeManager.registerFullGameMob(MobList.EVENT_CHEST1, chest);

            Container chest2 = new Container();
            chest2.SetDesc("A sturdy, wooden chest.  It makes you wonder what is inside...");
            Utils.SetFlag(ref chest2.mFlags, MobFlags.OPENABLE);
            Utils.SetFlag(ref chest2.mFlags, MobFlags.CLOSEABLE);
            Utils.SetFlag(ref chest2.mFlags, MobFlags.LOCKED);
            Utils.SetFlag(ref chest2.mFlags, MobFlags.LOCKABLE);
            Utils.SetFlag(ref chest2.mFlags, MobFlags.UNLOCKABLE);
            chest2.SetName("large wooden chest2");
            chest2.GetInv().Capacity = 20;
            chest2.SetWorld(this);
            eventData = new EventData(EventFlag.EVENT_TELL_PLAYER, CommandName.COMMAND_LOOK);
            eventData.SetPrep(PrepositionType.PREP_AT);
            eventData.SetData("The " + chest.GetName() + " says \"hello!\"");
            chest2.GetEventList().Add(eventData);
            chest2.SetMobID(MobList.EVENT_CHEST2);
            chest2.SetKeyID((int)MobList.BASIC_KEY);
            chest2.SetStartingRespawnTime(30);
            chest2.SetCurrentRespawnTime(30);
            PrototypeManager.registerFullGameMob(MobList.EVENT_CHEST2, chest2);

            Mob first_circle = new Mob();
            first_circle.SetDesc("The first of eight ancient golden circles");
            Utils.SetFlag(ref first_circle.mFlags, MobFlags.GETTABLE);
            Utils.SetFlag(ref first_circle.mFlags, MobFlags.INSPECTABLE);
            Utils.SetFlag(ref first_circle.mFlags, MobFlags.STORABLE);
            Utils.SetFlag(ref first_circle.mFlags, MobFlags.DUPLICATABLE);
            Utils.SetFlag(ref first_circle.mFlags, MobFlags.USEABLE);
            first_circle.SetName("1st Circle");
            eventData = new EventData(EventFlag.EVENT_TELEPORT, CommandName.COMMAND_GET);
            eventData.SetData(RoomID.GPG_PLAYER_START);
            EventData eventData2 = new EventData(EventFlag.EVENT_TELEPORT, CommandName.COMMAND_GETALL, RoomID.GPG_PLAYER_START);
            first_circle.GetEventList().Add(eventData);
            first_circle.GetEventList().Add(eventData2);
            first_circle.GetInv().Capacity = 0;
            first_circle.SetWorld(this);
            first_circle.SetMobID(MobList.FIRST_CIRCLE);
            PrototypeManager.registerFullGameMob(MobList.FIRST_CIRCLE, first_circle);

            Container no_event_chest = new Container();
            no_event_chest.SetDesc("A sturdy, wooden chest.  It makes you wonder what is inside...");
            Utils.SetFlag(ref no_event_chest.mFlags, MobFlags.OPENABLE);
            Utils.SetFlag(ref no_event_chest.mFlags, MobFlags.CLOSEABLE);
            Utils.SetFlag(ref no_event_chest.mFlags, MobFlags.LOCKED);
            Utils.SetFlag(ref no_event_chest.mFlags, MobFlags.LOCKABLE);
            Utils.SetFlag(ref no_event_chest.mFlags, MobFlags.UNLOCKABLE);
            no_event_chest.SetName("wooden chest");
            no_event_chest.GetInv().Capacity = 20;
            no_event_chest.SetWorld(this);
            no_event_chest.SetMobID(MobList.BASIC_CHEST);
            no_event_chest.SetKeyID((int)MobList.BASIC_KEY);
            no_event_chest.SetStartingRespawnTime(30);
            no_event_chest.SetCurrentRespawnTime(30);
            PrototypeManager.registerFullGameMob(MobList.BASIC_CHEST, no_event_chest);

            Mob basic_switch = new Mob();
            basic_switch.SetDesc("A switch, it must trigger something...");
            Utils.SetFlag(ref basic_switch.mFlags, MobFlags.USEABLE);
            basic_switch.SetName("switch");
            basic_switch.SetWorld(this);
            basic_switch.SetMobID(MobList.SWITCH);
            basic_switch.SetStartingRespawnTime(30);
            basic_switch.SetCurrentRespawnTime(30);
            PrototypeManager.registerFullGameMob(MobList.SWITCH, basic_switch);

            Equipment basic_sword = new Equipment();
            basic_sword.SetDesc("A rusty old sword, it is barely passable as a weapon");
            Utils.SetFlag(ref basic_sword.mFlags, MobFlags.GETTABLE);
            Utils.SetFlag(ref basic_sword.mFlags, MobFlags.DROPPABLE);
            Utils.SetFlag(ref basic_sword.mFlags, MobFlags.INSPECTABLE);
            Utils.SetFlag(ref basic_sword.mFlags, MobFlags.STORABLE);
            Utils.SetFlag(ref basic_sword.mFlags, MobFlags.WEARABLE);
            basic_sword.SetName("Rusty Sword");
            basic_sword.SetWorld(this);
            basic_sword.SetMobID(MobList.BASIC_SWORD);
            basic_sword.SetStartingRespawnTime(30);
            basic_sword.SetCurrentRespawnTime(30);
            basic_sword.SetMinDam(2);
            basic_sword.SetMaxDam(11);
            basic_sword.SetHitMod(10);
            basic_sword.SetType(EQType.WEAPON);
            basic_sword.SetSlot(EQSlot.PRIMARY);
            PrototypeManager.registerFullGameMob(MobList.BASIC_SWORD, basic_sword);

            CombatMob goblin_runt = new CombatMob();
            goblin_runt.SetDesc("A runt of the goblin litter, truely a wretched creature.");
            goblin_runt.SetName("Goblin Runt");
            goblin_runt.GetInv().Capacity = 0;
            goblin_runt.SetWorld(this);
            goblin_runt.SetMobID(MobList.GOBLIN_RUNT);
            goblin_runt.SetStartingRespawnTime(30);
            goblin_runt.SetCurrentRespawnTime(30);
            goblin_runt[STAT.BASEMAXDAM] = 5;
            goblin_runt[STAT.CURRENTHP] = 50;
            goblin_runt[STAT.BASEMAXHP] = 50;
            goblin_runt[STAT.BASEHIT] = 50;
            PrototypeManager.registerFullGameMob(MobList.GOBLIN_RUNT, goblin_runt);

            CombatMob max = new CombatMob();
            max.SetDesc("A super big fluffy cute black and white kitty cat... you just want to hug him");
            max.SetName("Max the MaineCoon");
            max.GetInv().Capacity = 0;
            max.SetWorld(this);
            max.SetMobID(MobList.MAX);
            max.SetStartingRespawnTime(10);
            max.SetCurrentRespawnTime(10);
            PrototypeManager.registerFullGameMob(MobList.MAX, max);
        }// addMobs

        private void addAreas()
        {
            addProtoArea();
            addGeraldineArea();
            addNewbieArea();
        }// addAreas

        private void addProtoArea()
        {
            Area protoArea = new Area(this, "Proto Area", AreaID.AID_PROTOAREA);
            protoArea.SetDescription("The testing area for the 8th Circle");
            protoArea.SetAreaOffset(PROTO_OFFSET);

            Room pa1 = new Room("Room 0,0,0", PROTO_OFFSET, PROTO_OFFSET, PROTO_OFFSET, RoomID.PROTO_1, protoArea);
            Room pa2 = new Room("Room 1,0,0", PROTO_OFFSET + 1, PROTO_OFFSET, PROTO_OFFSET, RoomID.PROTO_2, protoArea);
            Room pa3 = new Room("Room 2,0,0", PROTO_OFFSET + 2, PROTO_OFFSET, PROTO_OFFSET, RoomID.PROTO_3, protoArea);
            Room pa4 = new Room("Room 0,1,0", PROTO_OFFSET, PROTO_OFFSET + 1, PROTO_OFFSET, RoomID.PROTO_4, protoArea);
            Room pa5 = new Room("Room 1,1,0", PROTO_OFFSET + 1, PROTO_OFFSET + 1, PROTO_OFFSET, RoomID.PROTO_5, protoArea);
            Room pa6 = new Room("Room 2,1,0", PROTO_OFFSET + 2, PROTO_OFFSET + 1, PROTO_OFFSET, RoomID.PROTO_6, protoArea);
            Room pa7 = new Room("Room 0,2,0", PROTO_OFFSET, PROTO_OFFSET + 2, PROTO_OFFSET, RoomID.PROTO_7, protoArea);
            Room pa8 = new Room("Room 1,2,0", PROTO_OFFSET + 1, PROTO_OFFSET + 2, PROTO_OFFSET, RoomID.PROTO_8, protoArea);
            Room pa9 = new Room("Room 2,2,0", PROTO_OFFSET + 2, PROTO_OFFSET + 2, PROTO_OFFSET, RoomID.PROTO_9, protoArea);
            Room pa10 = new Room("Room 0,0,1", PROTO_OFFSET, PROTO_OFFSET, PROTO_OFFSET + 1, RoomID.PROTO_10, protoArea);
            Room pa11 = new Room("Room 1,0,1", PROTO_OFFSET + 1, PROTO_OFFSET, PROTO_OFFSET + 1, RoomID.PROTO_11, protoArea);
            Room pa12 = new Room("Room 2,0,1", PROTO_OFFSET + 2, PROTO_OFFSET, PROTO_OFFSET + 1, RoomID.PROTO_12, protoArea);
            Room pa13 = new Room("Room 0,1,1", PROTO_OFFSET, PROTO_OFFSET + 1, PROTO_OFFSET + 1, RoomID.PROTO_13, protoArea);
            Room pa14 = new Room("Room 1,1,1", PROTO_OFFSET + 1, PROTO_OFFSET + 1, PROTO_OFFSET + 1, RoomID.PROTO_14, protoArea);
            Room pa15 = new Room("Room 2,1,1", PROTO_OFFSET + 2, PROTO_OFFSET + 1, PROTO_OFFSET + 1, RoomID.PROTO_15, protoArea);
            Room pa16 = new Room("Room 0,2,1", PROTO_OFFSET, PROTO_OFFSET + 2, PROTO_OFFSET + 1, RoomID.PROTO_16, protoArea);
            Room pa17 = new Room("Room 1,2,1", PROTO_OFFSET + 1, PROTO_OFFSET + 2, PROTO_OFFSET + 1, RoomID.PROTO_17, protoArea);
            Room pa18 = new Room("Room 2,2,1", PROTO_OFFSET + 2, PROTO_OFFSET + 2, PROTO_OFFSET + 1, RoomID.PROTO_18, protoArea);
            Room pa19 = new Room("Room 0,0,2", PROTO_OFFSET, PROTO_OFFSET, PROTO_OFFSET + 2, RoomID.PROTO_19, protoArea);
            Room pa20 = new Room("Room 1,0,2", PROTO_OFFSET + 1, PROTO_OFFSET, PROTO_OFFSET + 2, RoomID.PROTO_20, protoArea);
            Room pa21 = new Room("Room 2,0,2", PROTO_OFFSET + 2, PROTO_OFFSET, PROTO_OFFSET + 2, RoomID.PROTO_21, protoArea);
            Room pa22 = new Room("Room 0,1,2", PROTO_OFFSET, PROTO_OFFSET + 1, PROTO_OFFSET + 2, RoomID.PROTO_22, protoArea);
            Room pa23 = new Room("Room 1,1,2", PROTO_OFFSET + 1, PROTO_OFFSET + 1, PROTO_OFFSET + 2, RoomID.PROTO_23, protoArea);
            Room pa24 = new Room("Room 2,1,2", PROTO_OFFSET + 2, PROTO_OFFSET + 1, PROTO_OFFSET + 2, RoomID.PROTO_24, protoArea);
            Room pa25 = new Room("Room 0,2,2", PROTO_OFFSET, PROTO_OFFSET + 2, PROTO_OFFSET + 2, RoomID.PROTO_25, protoArea);
            Room pa26 = new Room("Room 1,2,2", PROTO_OFFSET + 1, PROTO_OFFSET + 2, PROTO_OFFSET + 2, RoomID.PROTO_26, protoArea);
            Room pa27 = new Room("Room 2,2,2", PROTO_OFFSET + 2, PROTO_OFFSET + 2, PROTO_OFFSET + 2, RoomID.PROTO_27, protoArea);

            // Add links
            foreach (KeyValuePair<RoomID, Room> keyValPair in protoArea.GetRooms())
            {
                Room currentRoom = keyValPair.Value;

                Room nwRoom = getRoom(currentRoom.GetAreaLoc()[0] - 1, currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room nRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room neRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room wRoom = getRoom(currentRoom.GetAreaLoc()[0] - 1, currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room eRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room swRoom = getRoom(currentRoom.GetAreaLoc()[0] - 1, currentRoom.GetAreaLoc()[1] - 1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room sRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1] - 1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room seRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1] - 1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room uRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2] + 1, AreaID.AID_PROTOAREA);
                Room dRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2] - 1, AreaID.AID_PROTOAREA);

                if (nwRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.NORTHWEST] = nwRoom;
                    nwRoom.GetRoomLinks()[(int)Direction.SOUTHEAST] = currentRoom;
                }
                if (nRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.NORTH] = nRoom;
                    nRoom.GetRoomLinks()[(int)Direction.SOUTH] = currentRoom;
                }
                if (neRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.NORTHEAST] = neRoom;
                    neRoom.GetRoomLinks()[(int)Direction.SOUTHWEST] = currentRoom;
                }
                if (wRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.WEST] = wRoom;
                    wRoom.GetRoomLinks()[(int)Direction.EAST] = currentRoom;
                }
                if (eRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.EAST] = eRoom;
                    eRoom.GetRoomLinks()[(int)Direction.WEST] = currentRoom;
                }
                if (swRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.SOUTHWEST] = swRoom;
                    swRoom.GetRoomLinks()[(int)Direction.NORTHEAST] = currentRoom;
                }
                if (sRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.SOUTH] = sRoom;
                    sRoom.GetRoomLinks()[(int)Direction.NORTH] = currentRoom;
                }
                if (seRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.SOUTHEAST] = seRoom;
                    seRoom.GetRoomLinks()[(int)Direction.NORTHWEST] = currentRoom;
                }
                if (uRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.UP] = uRoom;
                    uRoom.GetRoomLinks()[(int)Direction.DOWN] = currentRoom;
                }
                if (dRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.DOWN] = dRoom;
                    dRoom.GetRoomLinks()[(int)Direction.UP] = currentRoom;
                }
            }// foreach

            addProtoMobs(protoArea);
            mAreaHandler.registerArea(protoArea);
        }// addProtoArea

        private void addProtoMobs(Area protoArea)
        {
            Mob lump1 = PrototypeManager.getFullGameRegisteredMob(MobList.STRANGE_LUMP).Clone();
            Mob lump2 = PrototypeManager.getFullGameRegisteredMob(MobList.STRANGE_LUMP).Clone();

            Mob event_chest1 = PrototypeManager.getFullGameRegisteredMob(MobList.EVENT_CHEST1).Clone();
            event_chest1.SetKeyID(3);
            event_chest1.GetInv().Add(lump1);
            event_chest1.GetInv().Add(lump2);
            protoArea.cloneMob(MobList.EVENT_CHEST1, protoArea[RoomID.PROTO_14], "protochest", event_chest1);

            Mob basic_key = PrototypeManager.getFullGameRegisteredMob(MobList.BASIC_KEY).Clone();
            basic_key.SetKeyID(3);
            protoArea.cloneMob(MobList.BASIC_KEY, protoArea[RoomID.PROTO_27], "proto key", basic_key);
        }

        private void addGeraldineArea()
        {
            Area geraldineArea = new Area(this, "Geraldine Estate", AreaID.AID_GERALDINEMANOR);
            geraldineArea.SetDescription("The residence of the esteemed Renee and David");
            geraldineArea.SetAreaOffset(HOUSE_OFFSET);

            Room house1stentranceway = new Room("The entrance to the Geraldine Manor, there are stairs " + "leading up.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET, RoomID.GERALD_1ST_ENT, geraldineArea);

            Room house1stHallway = new Room("The west hallway is empty besides a few pictures",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET, RoomID.GERALD_1ST_HALLWAY, geraldineArea);

            Room house1stKitchen = new Room("The kitchen has a nice view of the outside to the west; " +
                "there are also stairs leading down and a doorway to the north",
                HOUSE_OFFSET - 2, HOUSE_OFFSET, HOUSE_OFFSET, RoomID.GERALD_1ST_KITCHEN, geraldineArea);

            Room house1stDiningRoom = new Room("The dining room is blue with various pictures; one is " +
                "particularly interesting, featuring a type of chicken",
                HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, HOUSE_OFFSET, RoomID.GERALD_1ST_DININGROOM, geraldineArea);

            Room house1stLivingRoom = new Room("The living room is grey with a nice flatscreen tv along " + "the north wall",
                HOUSE_OFFSET, HOUSE_OFFSET - 1, HOUSE_OFFSET, RoomID.GERALD_1ST_LIVINGROOM, geraldineArea);

            Room house1stBathroom = new Room("The powder room is a nice small comfortable bathroom with " + "a sink and toilet",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET, RoomID.GERALD_1ST_BATHROOM, geraldineArea);

            Room house2ndHallway = new Room("The hallway to the 2nd floor.  This is a long corridor\n with " +
                "many rooms attached to it with stairs leading down at the base.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_HALLWAY, geraldineArea);

            Room house2ndKittyroom = new Room("The kittyroom, there is not much here besides some litterboxes.",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_KITTYROOM, geraldineArea);

            Room house2ndKittyCloset = new Room("The closet of the kittyroom holds various appliances such as " +
                "vaccuums and other cleaning supplies",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_KITTYCLOSET, geraldineArea);

            Room house2ndBathroom = new Room("A small master bathroom has a sink, shower and toilet",
                HOUSE_OFFSET + 1, HOUSE_OFFSET, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_BATHROOM, geraldineArea);

            Room house2ndBedroom = new Room("The master bedroom is huge with two sliding door closets\n and " +
                "windows on the north and northwest sides",
                HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_BEDROOM, geraldineArea);

            Room house2ndBlueroom = new Room("The blueroom has a large bookshelf, a sliding door closet and " + "a loveseat",
                HOUSE_OFFSET, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_BLUEROOM, geraldineArea);

            Room houseBaseentrance = new Room("The bottom of the stairs leads to the basement.\n This " +
                " is a large basement that spans to the south with \nrooms attached on both sides.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART1, geraldineArea);

            Room houseBasepart2 = new Room("There is a piano here along the wall with light grey\n " +
                "carpet with the walls being a darker grey",
                HOUSE_OFFSET, HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART2, geraldineArea);

            Room houseBasepart3 = new Room("There isn't much to this piece of the basement besides\n " +
                "some pictures on both the west and east walls.",
                HOUSE_OFFSET, HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART3, geraldineArea);

            Room houseBasepart4 = new Room("You have reached the southern corner of the basement.\n " +
                "There is a computer desk here with a glowing PC and monitor.  There are all\n " +
                "sorts of figurines of wonderous power sitting on the desk along with pictures\n " +
                "depicting awesome scenes of wonder and adventure.  Something about this room\n " +
                "seems filled with some sort of power.",
                HOUSE_OFFSET, HOUSE_OFFSET - 3, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART4, geraldineArea);

            Room houseBasepart5 = new Room("The southwest most edge of the basement, there is a\n " +
                "couch on the south end of the wall facing a TV in the corner beside a doorway",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 3, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART5, geraldineArea);

            Room houseBaseBathroom = new Room("The bathroom has a standing shower as well as a long \n " +
                "vanity with an accompanying toilet",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_BATHROOM, geraldineArea);

            Room houseBaseCloset = new Room("This is a closet that has large holding shelves with\n " +
                "board games from top to bottom.  This is an impressive collection indeed!",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_CLOSET, geraldineArea);

            Room houseBaseLaundryRoom = new Room("The laundry room has no carpet and has many shelves\n " +
                "with various pieces of hardware and tools",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_LAUNDRYROOM, geraldineArea);

            Room houseBaseSumpRoom = new Room("The sump pump room is bare concrete with a few shelves\n " + "for storage",
                HOUSE_OFFSET, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, RoomID.GERALD_BASE_SUMPROOM, geraldineArea);

            house1stentranceway.GetRoomLinks()[(int)Direction.NORTH] = house1stHallway;
            house1stentranceway.GetRoomLinks()[(int)Direction.WEST] = house1stLivingRoom;
            house1stHallway.GetRoomLinks()[(int)Direction.SOUTH] = house1stentranceway;
            house1stHallway.GetRoomLinks()[(int)Direction.WEST] = house1stBathroom;
            house1stHallway.GetRoomLinks()[(int)Direction.NORTH] = house1stKitchen;
            house1stKitchen.GetRoomLinks()[(int)Direction.WEST] = house1stDiningRoom;
            house1stKitchen.GetRoomLinks()[(int)Direction.SOUTH] = house1stHallway;
            house1stDiningRoom.GetRoomLinks()[(int)Direction.EAST] = house1stKitchen;
            house1stDiningRoom.GetRoomLinks()[(int)Direction.SOUTH] = house1stLivingRoom;
            house1stLivingRoom.GetRoomLinks()[(int)Direction.EAST] = house1stentranceway;
            house1stLivingRoom.GetRoomLinks()[(int)Direction.NORTH] = house1stDiningRoom;
            house1stBathroom.GetRoomLinks()[(int)Direction.EAST] = house1stHallway;

            Doorway newDoor = new Doorway("door");
            house1stBathroom.addDoor(newDoor, Direction.EAST);
            house1stHallway.addDoor(newDoor, Direction.WEST);
            newDoor.CreateMemento();

            newDoor = new Doorway("door");
            house1stKitchen.addDoor(newDoor, Direction.DOWN);
            houseBaseentrance.addDoor(newDoor, Direction.UP);
            newDoor.CreateMemento();

            house1stentranceway.GetRoomLinks()[(int)Direction.UP] = house2ndHallway;
            house2ndHallway.GetRoomLinks()[(int)Direction.DOWN] = house1stentranceway;
            house2ndHallway.GetRoomLinks()[(int)Direction.EAST] = house2ndKittyroom;
            house2ndHallway.GetRoomLinks()[(int)Direction.WEST] = house2ndBathroom;
            house2ndHallway.GetRoomLinks()[(int)Direction.SOUTH] = house2ndBlueroom;
            house2ndHallway.GetRoomLinks()[(int)Direction.SOUTHWEST] = house2ndBedroom;
            house2ndKittyroom.GetRoomLinks()[(int)Direction.NORTH] = house2ndKittyCloset;
            house2ndKittyroom.GetRoomLinks()[(int)Direction.WEST] = house2ndHallway;
            house2ndKittyCloset.GetRoomLinks()[(int)Direction.SOUTH] = house2ndKittyroom;
            house2ndBathroom.GetRoomLinks()[(int)Direction.EAST] = house2ndHallway;
            house2ndBathroom.GetRoomLinks()[(int)Direction.SOUTH] = house2ndBedroom;
            house2ndBedroom.GetRoomLinks()[(int)Direction.NORTHEAST] = house2ndHallway;
            house2ndBedroom.GetRoomLinks()[(int)Direction.NORTH] = house2ndBathroom;
            house2ndBlueroom.GetRoomLinks()[(int)Direction.NORTH] = house2ndHallway;

            newDoor = new Doorway("door");
            house2ndHallway.addDoor(newDoor, Direction.WEST);
            house2ndBathroom.addDoor(newDoor, Direction.EAST);
            newDoor.CreateMemento();

            newDoor = new Doorway("door");
            house2ndHallway.addDoor(newDoor, Direction.EAST);
            house2ndKittyroom.addDoor(newDoor, Direction.WEST);
            newDoor.CreateMemento();

            newDoor = new Doorway("door");
            house2ndHallway.addDoor(newDoor, Direction.SOUTH);
            house2ndBlueroom.addDoor(newDoor, Direction.NORTH);
            newDoor.CreateMemento();

            newDoor = new Doorway("door");
            house2ndHallway.addDoor(newDoor, Direction.SOUTHWEST);
            house2ndBedroom.addDoor(newDoor, Direction.NORTHEAST);
            newDoor.CreateMemento();

            newDoor = new Doorway("door");
            house2ndBathroom.addDoor(newDoor, Direction.SOUTH);
            house2ndBedroom.addDoor(newDoor, Direction.NORTH);
            newDoor.CreateMemento();

            houseBaseentrance.GetRoomLinks()[(int)Direction.UP] = house1stKitchen;
            house1stKitchen.GetRoomLinks()[(int)Direction.DOWN] = houseBaseentrance;
            houseBaseentrance.GetRoomLinks()[(int)Direction.SOUTH] = houseBasepart2;
            houseBaseentrance.GetRoomLinks()[(int)Direction.WEST] = houseBaseLaundryRoom;
            houseBaseLaundryRoom.GetRoomLinks()[(int)Direction.EAST] = houseBaseentrance;
            houseBasepart2.GetRoomLinks()[(int)Direction.NORTH] = houseBaseentrance;
            houseBasepart2.GetRoomLinks()[(int)Direction.WEST] = houseBaseCloset;
            houseBasepart2.GetRoomLinks()[(int)Direction.EAST] = houseBaseSumpRoom;
            houseBasepart2.GetRoomLinks()[(int)Direction.SOUTH] = houseBasepart3;
            houseBaseSumpRoom.GetRoomLinks()[(int)Direction.WEST] = houseBasepart2;
            houseBaseCloset.GetRoomLinks()[(int)Direction.EAST] = houseBasepart2;
            houseBasepart3.GetRoomLinks()[(int)Direction.NORTH] = houseBasepart2;
            houseBasepart3.GetRoomLinks()[(int)Direction.SOUTH] = houseBasepart4;
            houseBasepart4.GetRoomLinks()[(int)Direction.NORTH] = houseBasepart3;
            houseBasepart4.GetRoomLinks()[(int)Direction.WEST] = houseBasepart5;
            houseBasepart5.GetRoomLinks()[(int)Direction.EAST] = houseBasepart4;
            houseBasepart5.GetRoomLinks()[(int)Direction.NORTH] = houseBaseBathroom;
            houseBaseBathroom.GetRoomLinks()[(int)Direction.SOUTH] = houseBasepart5;

            newDoor = new Doorway("door");
            houseBaseentrance.addDoor(newDoor, Direction.WEST);
            houseBaseLaundryRoom.addDoor(newDoor, Direction.EAST);
            newDoor.CreateMemento();

            newDoor = new Doorway("door");
            houseBasepart2.addDoor(newDoor, Direction.WEST);
            houseBaseCloset.addDoor(newDoor, Direction.EAST);
            newDoor.CreateMemento();

            newDoor = new Doorway("door");
            houseBasepart2.addDoor(newDoor, Direction.EAST);
            houseBaseSumpRoom.addDoor(newDoor, Direction.WEST);
            newDoor.CreateMemento();

            newDoor = new Doorway("door");
            houseBaseBathroom.addDoor(newDoor, Direction.SOUTH);
            houseBasepart5.addDoor(newDoor, Direction.NORTH);
            newDoor.CreateMemento();

            Area protoArea = getArea(AreaID.AID_PROTOAREA);
            getRoom(protoArea.GetAreaOffset(), protoArea.GetAreaOffset(), protoArea.GetAreaOffset(), AreaID.AID_PROTOAREA).GetRoomLinks()[(int)Direction.WEST] = house1stentranceway;

            addGeraldineNpcs(geraldineArea);
            mAreaHandler.registerArea(geraldineArea);
        }// geraldineArea

        public void addGeraldineNpcs(Area area)
        {
            area.cloneMob(MobList.MAX, area[RoomID.GERALD_1ST_LIVINGROOM]);
            area.cloneMob(MobList.FIRST_CIRCLE, area[RoomID.GERALD_BASE_PART4]);
        }// addGeraldineNpcs

        public void addNewbieArea()
        {
            Area newbieArea = new Area(this, "Goblin Prooving Grounds", AreaID.AID_NEWBIEAREA);

            newbieArea.SetDescription("This area is the leftover pits where unworthy goblins were abandoned " +
               "and forgotten.  The runts here were unworthy to serve any useful purpose in goblin society " +
               "and were cast out until they prooved themselves");

            string common_gpg_north_field = "The plains end to the north along a rock wall.  There is some light\n" +
               "a little further, the walls to the north rise as high as you can see.\n" +
               "The plains still span in other directions.  All around, you can hear\n" +
               "goblins lurking about and you can see dark mud splattered about.\n";

            string common_gpg_south_field = "The plains end to the south along a rock wall.  There is some light\n" +
               "a little further, the walls to the south rise as high as you can see.\n" +
               "The plains still span in other directions.  All around, you can hear\n" +
               "goblins lurking about and you can see dark mud splattered about.\n";

            string common_gpg_east_field = "The plains end to the east along a rock wall.  There is some light\n" +
               "a little further, the walls to the east rise as high as you can see.\n" +
               "The plains still span in other directions.  All around, you can hear\n" +
               "goblins lurking about and you can see dark mud splattered about.\n";

            string common_gpg_open_field = "The plains continue on in every direction.  All around, you\n" +
               "can hear goblins lurking about and you can see dark mud splattered about.\n";

            Room gpg_playerStart = new Room("You find yourself in some sort of dark plains.  It spans in\n" +
               "every direction, although you see some walls to your west.  You hear grunting.\n" +
               "It smells like... goblins.\n", BAO, BAO, BAO, RoomID.GPG_PLAYER_START, newbieArea);

            Room gpg_1 = new Room("GPG 1", BAO - 1, BAO + 3, BAO, RoomID.GPG_ROOM_1, newbieArea);
            Room gpg_2 = new Room(common_gpg_north_field, BAO, BAO + 3, BAO, RoomID.GPG_ROOM_2, newbieArea);
            Room gpg_3 = new Room(common_gpg_north_field, BAO + 1, BAO + 3, BAO, RoomID.GPG_ROOM_3, newbieArea);
            Room gpg_4 = new Room(common_gpg_north_field, BAO + 2, BAO + 3, BAO, RoomID.GPG_ROOM_4, newbieArea);
            Room gpg_5 = new Room(common_gpg_north_field, BAO + 3, BAO + 3, BAO, RoomID.GPG_ROOM_5, newbieArea);
            Room gpg_6 = new Room(common_gpg_east_field, BAO + 4, BAO + 3, BAO, RoomID.GPG_ROOM_6, newbieArea);
            Room gpg_7 = new Room("GPG 7", BAO - 1, BAO + 2, BAO, RoomID.GPG_ROOM_7, newbieArea);
            Room gpg_8 = new Room(common_gpg_open_field, BAO, BAO + 2, BAO, RoomID.GPG_ROOM_8, newbieArea);
            Room gpg_9 = new Room(common_gpg_open_field, BAO + 1, BAO + 2, BAO, RoomID.GPG_ROOM_9, newbieArea);
            Room gpg_10 = new Room(common_gpg_open_field, BAO + 2, BAO + 2, BAO, RoomID.GPG_ROOM_10, newbieArea);
            Room gpg_11 = new Room(common_gpg_open_field, BAO + 3, BAO + 2, BAO, RoomID.GPG_ROOM_11, newbieArea);
            Room gpg_12 = new Room(common_gpg_east_field, BAO + 4, BAO + 2, BAO, RoomID.GPG_ROOM_12, newbieArea);
            Room gpg_13 = new Room("GPG 13", BAO - 1, BAO + 1, BAO, RoomID.GPG_ROOM_13, newbieArea);
            Room gpg_14 = new Room(common_gpg_open_field, BAO, BAO + 1, BAO, RoomID.GPG_ROOM_14, newbieArea);
            Room gpg_15 = new Room(common_gpg_open_field, BAO + 1, BAO + 1, BAO, RoomID.GPG_ROOM_15, newbieArea);
            Room gpg_16 = new Room(common_gpg_open_field, BAO + 2, BAO + 1, BAO, RoomID.GPG_ROOM_16, newbieArea);
            Room gpg_17 = new Room(common_gpg_open_field, BAO + 3, BAO + 1, BAO, RoomID.GPG_ROOM_17, newbieArea);
            Room gpg_18 = new Room(common_gpg_east_field, BAO + 4, BAO + 1, BAO, RoomID.GPG_ROOM_18, newbieArea);
            Room gpg_19 = new Room("GPG 19", BAO - 1, BAO, BAO, RoomID.GPG_ROOM_19, newbieArea);
            Room gpg_21 = new Room(common_gpg_open_field, BAO + 1, BAO, BAO, RoomID.GPG_ROOM_21, newbieArea);
            Room gpg_22 = new Room(common_gpg_open_field, BAO + 2, BAO, BAO, RoomID.GPG_ROOM_22, newbieArea);
            Room gpg_23 = new Room(common_gpg_open_field, BAO + 3, BAO, BAO, RoomID.GPG_ROOM_23, newbieArea);
            Room gpg_24 = new Room(common_gpg_east_field, BAO + 4, BAO, BAO, RoomID.GPG_ROOM_24, newbieArea);
            Room gpg_25 = new Room("GPG 25", BAO - 1, BAO - 1, BAO, RoomID.GPG_ROOM_25, newbieArea);
            Room gpg_26 = new Room(common_gpg_open_field, BAO, BAO - 1, BAO, RoomID.GPG_ROOM_26, newbieArea);
            Room gpg_27 = new Room(common_gpg_open_field, BAO + 1, BAO - 1, BAO, RoomID.GPG_ROOM_27, newbieArea);
            Room gpg_28 = new Room(common_gpg_open_field, BAO + 2, BAO - 1, BAO, RoomID.GPG_ROOM_28, newbieArea);
            Room gpg_29 = new Room(common_gpg_open_field + "there is a small switch built into the side of a rock.\n" +
               "You can't help but wonder what would happen if you used it...\n", BAO + 3, BAO - 1, BAO, RoomID.GPG_ROOM_29, newbieArea);
            Room gpg_30 = new Room(common_gpg_east_field, BAO + 4, BAO - 1, BAO, RoomID.GPG_ROOM_30, newbieArea);
            Room gpg_31 = new Room("GPG 31", BAO - 1, BAO - 2, BAO, RoomID.GPG_ROOM_31, newbieArea);
            Room gpg_32 = new Room(common_gpg_south_field, BAO, BAO - 2, BAO, RoomID.GPG_ROOM_32, newbieArea);
            Room gpg_33 = new Room(common_gpg_south_field, BAO + 1, BAO - 2, BAO, RoomID.GPG_ROOM_33, newbieArea);
            Room gpg_34 = new Room(common_gpg_south_field, BAO + 2, BAO - 2, BAO, RoomID.GPG_ROOM_34, newbieArea);
            Room gpg_35 = new Room(common_gpg_south_field, BAO + 3, BAO - 2, BAO, RoomID.GPG_ROOM_35, newbieArea);
            Room gpg_36 = new Room(common_gpg_east_field, BAO + 4, BAO - 2, BAO, RoomID.GPG_ROOM_36, newbieArea);
            Room gpg_37 = new Room("GPG 37", BAO - 8, BAO + 3, BAO, RoomID.GPG_ROOM_37, newbieArea);
            Room gpg_38 = new Room("GPG 38", BAO - 7, BAO + 3, BAO, RoomID.GPG_ROOM_38, newbieArea);
            Room gpg_39 = new Room("GPG 39", BAO - 6, BAO + 3, BAO, RoomID.GPG_ROOM_39, newbieArea);
            Room gpg_40 = new Room("GPG 40", BAO - 5, BAO + 3, BAO, RoomID.GPG_ROOM_40, newbieArea);
            Room gpg_41 = new Room("GPG 41", BAO - 4, BAO + 3, BAO, RoomID.GPG_ROOM_41, newbieArea);
            Room gpg_42 = new Room("GPG 42", BAO - 3, BAO + 3, BAO, RoomID.GPG_ROOM_42, newbieArea);
            Room gpg_43 = new Room("GPG 43", BAO - 2, BAO + 3, BAO, RoomID.GPG_ROOM_43, newbieArea);
            Room gpg_44 = new Room("GPG 44", BAO - 8, BAO + 2, BAO, RoomID.GPG_ROOM_44, newbieArea);
            Room gpg_45 = new Room("GPG 45", BAO - 7, BAO + 2, BAO, RoomID.GPG_ROOM_45, newbieArea);
            Room gpg_46 = new Room("GPG 46", BAO - 6, BAO + 2, BAO, RoomID.GPG_ROOM_46, newbieArea);
            Room gpg_47 = new Room("GPG 47", BAO - 5, BAO + 2, BAO, RoomID.GPG_ROOM_47, newbieArea);
            Room gpg_48 = new Room("GPG 48", BAO - 4, BAO + 2, BAO, RoomID.GPG_ROOM_48, newbieArea);
            Room gpg_49 = new Room("GPG 49", BAO - 3, BAO + 2, BAO, RoomID.GPG_ROOM_49, newbieArea);
            Room gpg_50 = new Room("GPG 50", BAO - 2, BAO + 2, BAO, RoomID.GPG_ROOM_50, newbieArea);
            Room gpg_51 = new Room("GPG 51", BAO - 8, BAO + 1, BAO, RoomID.GPG_ROOM_51, newbieArea);
            Room gpg_52 = new Room("GPG 52", BAO - 7, BAO + 1, BAO, RoomID.GPG_ROOM_52, newbieArea);
            Room gpg_53 = new Room("GPG 53", BAO - 6, BAO + 1, BAO, RoomID.GPG_ROOM_53, newbieArea);
            Room gpg_54 = new Room("GPG 54", BAO - 5, BAO + 1, BAO, RoomID.GPG_ROOM_54, newbieArea);
            Room gpg_55 = new Room("GPG 55", BAO - 4, BAO + 1, BAO, RoomID.GPG_ROOM_55, newbieArea);
            Room gpg_56 = new Room("GPG 56", BAO - 3, BAO + 1, BAO, RoomID.GPG_ROOM_56, newbieArea);
            Room gpg_57 = new Room("GPG 57", BAO - 2, BAO + 1, BAO, RoomID.GPG_ROOM_57, newbieArea);
            Room gpg_58 = new Room("GPG 58", BAO - 8, BAO, BAO, RoomID.GPG_ROOM_58, newbieArea);
            Room gpg_59 = new Room("GPG 59", BAO - 7, BAO, BAO, RoomID.GPG_ROOM_59, newbieArea);
            Room gpg_60 = new Room("GPG 60", BAO - 6, BAO, BAO, RoomID.GPG_ROOM_60, newbieArea);
            Room gpg_61 = new Room("GPG 61", BAO - 5, BAO, BAO, RoomID.GPG_ROOM_61, newbieArea);
            Room gpg_62 = new Room("GPG 62", BAO - 4, BAO, BAO, RoomID.GPG_ROOM_62, newbieArea);
            Room gpg_63 = new Room("GPG 63", BAO - 3, BAO, BAO, RoomID.GPG_ROOM_63, newbieArea);
            Room gpg_64 = new Room("GPG 64", BAO - 2, BAO, BAO, RoomID.GPG_ROOM_64, newbieArea);
            Room gpg_65 = new Room("GPG 65", BAO - 8, BAO - 1, BAO, RoomID.GPG_ROOM_65, newbieArea);
            Room gpg_66 = new Room("GPG 66", BAO - 7, BAO - 1, BAO, RoomID.GPG_ROOM_66, newbieArea);
            Room gpg_67 = new Room("GPG 67", BAO - 6, BAO - 1, BAO, RoomID.GPG_ROOM_67, newbieArea);
            Room gpg_68 = new Room("GPG 68", BAO - 5, BAO - 1, BAO, RoomID.GPG_ROOM_68, newbieArea);
            Room gpg_69 = new Room("GPG 69", BAO - 4, BAO - 1, BAO, RoomID.GPG_ROOM_69, newbieArea);
            Room gpg_70 = new Room("GPG 70", BAO - 3, BAO - 1, BAO, RoomID.GPG_ROOM_70, newbieArea);
            Room gpg_71 = new Room("GPG 71", BAO - 2, BAO - 1, BAO, RoomID.GPG_ROOM_71, newbieArea);
            Room gpg_72 = new Room("GPG 72", BAO - 8, BAO - 2, BAO, RoomID.GPG_ROOM_72, newbieArea);
            Room gpg_73 = new Room("GPG 73", BAO - 7, BAO - 2, BAO, RoomID.GPG_ROOM_73, newbieArea);
            Room gpg_74 = new Room("GPG 74", BAO - 6, BAO - 2, BAO, RoomID.GPG_ROOM_74, newbieArea);
            Room gpg_75 = new Room("GPG 75", BAO - 5, BAO - 2, BAO, RoomID.GPG_ROOM_75, newbieArea);
            Room gpg_76 = new Room("GPG 76", BAO - 4, BAO - 2, BAO, RoomID.GPG_ROOM_76, newbieArea);
            Room gpg_77 = new Room("GPG 77", BAO - 3, BAO - 2, BAO, RoomID.GPG_ROOM_77, newbieArea);
            Room gpg_78 = new Room("GPG 78", BAO - 2, BAO - 2, BAO, RoomID.GPG_ROOM_78, newbieArea);

            foreach (KeyValuePair<RoomID, Room> keyValPair in newbieArea.GetRooms())
            {
                Room currentRoom = keyValPair.Value;

                Room nwRoom = getRoom(currentRoom.GetAreaLoc()[0] - 1, currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_NEWBIEAREA);
                Room nRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_NEWBIEAREA);
                Room neRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_NEWBIEAREA);
                Room wRoom = getRoom(currentRoom.GetAreaLoc()[0] - 1, currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2], AreaID.AID_NEWBIEAREA);
                Room eRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2], AreaID.AID_NEWBIEAREA);
                Room swRoom = getRoom(currentRoom.GetAreaLoc()[0] - 1, currentRoom.GetAreaLoc()[1] - 1, currentRoom.GetAreaLoc()[2], AreaID.AID_NEWBIEAREA);
                Room sRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1] - 1, currentRoom.GetAreaLoc()[2], AreaID.AID_NEWBIEAREA);
                Room seRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1] - 1, currentRoom.GetAreaLoc()[2], AreaID.AID_NEWBIEAREA);
                Room uRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2] + 1, AreaID.AID_NEWBIEAREA);
                Room dRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2] - 1, AreaID.AID_NEWBIEAREA);

                if (nwRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.NORTHWEST] = nwRoom;
                    nwRoom.GetRoomLinks()[(int)Direction.SOUTHEAST] = currentRoom;
                }
                if (nRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.NORTH] = nRoom;
                    nRoom.GetRoomLinks()[(int)Direction.SOUTH] = currentRoom;
                }
                if (neRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.NORTHEAST] = neRoom;
                    neRoom.GetRoomLinks()[(int)Direction.SOUTHWEST] = currentRoom;
                }
                if (wRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.WEST] = wRoom;
                    wRoom.GetRoomLinks()[(int)Direction.EAST] = currentRoom;
                }
                if (eRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.EAST] = eRoom;
                    eRoom.GetRoomLinks()[(int)Direction.WEST] = currentRoom;
                }
                if (swRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.SOUTHWEST] = swRoom;
                    swRoom.GetRoomLinks()[(int)Direction.NORTHEAST] = currentRoom;
                }
                if (sRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.SOUTH] = sRoom;
                    sRoom.GetRoomLinks()[(int)Direction.NORTH] = currentRoom;
                }
                if (seRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.SOUTHEAST] = seRoom;
                    seRoom.GetRoomLinks()[(int)Direction.NORTHWEST] = currentRoom;
                }
                if (uRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.UP] = uRoom;
                    uRoom.GetRoomLinks()[(int)Direction.DOWN] = currentRoom;
                }
                if (dRoom != null)
                {
                    currentRoom.GetRoomLinks()[(int)Direction.DOWN] = dRoom;
                    dRoom.GetRoomLinks()[(int)Direction.UP] = currentRoom;
                }
            }// foreach

            // links have been added, now remove targetted links
            gpg_19.removeDualLinks(Direction.NORTHWEST);
            gpg_19.removeDualLinks(Direction.SOUTHWEST);
            gpg_31.removeDualLinks(Direction.NORTHWEST);
            gpg_37.removeDualLinks(Direction.SOUTH);
            gpg_37.removeDualLinks(Direction.SOUTHEAST);
            gpg_38.removeDualLinks(Direction.SOUTH);
            gpg_38.removeDualLinks(Direction.SOUTHWEST);
            gpg_38.removeDualLinks(Direction.SOUTHEAST);
            gpg_39.removeDualLinks(Direction.SOUTH);
            gpg_39.removeDualLinks(Direction.SOUTHWEST);
            gpg_40.removeDualLinks(Direction.SOUTHWEST);
            gpg_46.removeDualLinks(Direction.SOUTHEAST);
            gpg_47.removeDualLinks(Direction.WEST);
            gpg_49.removeDualLinks(Direction.SOUTH);
            gpg_52.removeDualLinks(Direction.NORTHEAST);
            gpg_53.removeDualLinks(Direction.NORTH);
            gpg_53.removeDualLinks(Direction.NORTHWEST);
            gpg_53.removeDualLinks(Direction.WEST);
            gpg_53.removeDualLinks(Direction.SOUTHWEST);
            gpg_55.removeDualLinks(Direction.EAST);
            gpg_55.removeDualLinks(Direction.SOUTHEAST);
            gpg_55.removeDualLinks(Direction.NORTHEAST);
            gpg_56.removeDualLinks(Direction.NORTHWEST);
            gpg_56.removeDualLinks(Direction.NORTHEAST);
            gpg_57.removeDualLinks(Direction.WEST);
            gpg_57.removeDualLinks(Direction.SOUTHWEST);
            gpg_57.removeDualLinks(Direction.SOUTH);
            gpg_60.removeDualLinks(Direction.NORTHWEST);
            gpg_60.removeDualLinks(Direction.WEST);
            gpg_60.removeDualLinks(Direction.SOUTHWEST);
            gpg_60.removeDualLinks(Direction.SOUTH);
            gpg_61.removeDualLinks(Direction.SOUTHWEST);
            gpg_62.removeDualLinks(Direction.EAST);
            gpg_62.removeDualLinks(Direction.NORTHEAST);
            gpg_62.removeDualLinks(Direction.SOUTHEAST);
            gpg_63.removeDualLinks(Direction.SOUTHEAST);
            gpg_63.removeDualLinks(Direction.SOUTH);
            gpg_64.removeDualLinks(Direction.SOUTH);
            gpg_64.removeDualLinks(Direction.SOUTHWEST);
            gpg_64.removeDualLinks(Direction.SOUTHEAST);
            gpg_64.removeDualLinks(Direction.NORTHWEST);
            gpg_64.removeDualLinks(Direction.NORTHEAST);
            gpg_67.removeDualLinks(Direction.NORTHWEST);
            gpg_68.removeDualLinks(Direction.WEST);
            gpg_68.removeDualLinks(Direction.SOUTHWEST);
            gpg_69.removeDualLinks(Direction.EAST);
            gpg_69.removeDualLinks(Direction.NORTHEAST);
            gpg_77.removeDualLinks(Direction.NORTH);
            gpg_71.removeDualLinks(Direction.WEST);
            gpg_71.removeDualLinks(Direction.EAST);
            gpg_71.removeDualLinks(Direction.SOUTHWEST);
            gpg_75.removeDualLinks(Direction.NORTHWEST);
            gpg_76.removeDualLinks(Direction.NORTHEAST);
            gpg_76.removeDualLinks(Direction.NORTHWEST);
            gpg_78.removeDualLinks(Direction.NORTHWEST);
            gpg_78.removeDualLinks(Direction.NORTHEAST);
            gpg_78.removeDualLinks(Direction.EAST);
            gpg_41.removeTripleLinks(Direction.WEST);
            gpg_48.removeTripleLinks(Direction.WEST);
            gpg_55.removeTripleLinks(Direction.WEST);
            gpg_62.removeTripleLinks(Direction.WEST);
            gpg_69.removeTripleLinks(Direction.WEST);
            gpg_76.removeTripleLinks(Direction.WEST);

            MobFlags flags = MobFlags.HIDDEN;
            Doorway newDoor = new Doorway("door", flags);
            gpg_71.addDoor(newDoor, Direction.SOUTH);
            ((gpg_71.GetRoomLinks()[(int)Direction.SOUTH])).addDoor(newDoor, Direction.NORTH);
            newDoor.CreateMemento();

            EventData ed = new EventData(EventFlag.EVENT_GPG_WALL_ADD, CommandName.COMMAND_USE, AreaID.AID_NEWBIEAREA);
            newbieArea.GetRevertEvents().Add(ed);

            addNewbieAreaMobs(newbieArea);
            mAreaHandler.registerArea(newbieArea);
        }// addNewbieArea

        private void addNewbieAreaMobs(Area newbieArea)
        {
            Mob wooden_chest = PrototypeManager.getFullGameRegisteredMob(MobList.BASIC_CHEST).Clone();
            wooden_chest.SetKeyID(4);
            newbieArea.cloneMob(MobList.BASIC_CHEST, newbieArea[RoomID.GPG_ROOM_56], "wooden chest", wooden_chest);

            Mob basic_key = PrototypeManager.getFullGameRegisteredMob(MobList.BASIC_KEY).Clone();
            basic_key.SetKeyID(4);
            newbieArea.cloneMob(MobList.BASIC_KEY, newbieArea[RoomID.GPG_ROOM_70], "brass key", basic_key);

            Mob small_metal_cage = PrototypeManager.getFullGameRegisteredMob(MobList.BASIC_CHEST).Clone();
            small_metal_cage.SetDesc("a small metal cage, I wonder what is inside?");
            small_metal_cage.SetKeyID(5);
            small_metal_cage.GetInv().Add(basic_key);
            newbieArea.cloneMob(MobList.BASIC_CHEST, newbieArea[RoomID.GPG_ROOM_71], "small metal cage", small_metal_cage);

            Mob steel_key = PrototypeManager.getFullGameRegisteredMob(MobList.BASIC_KEY).Clone();
            Utils.SetFlag(ref steel_key.mFlags, MobFlags.HIDDEN);
            Utils.SetFlag(ref steel_key.mFlags, MobFlags.LOCKED);
            steel_key.SetKeyID(5);
            steel_key.SetDesc("a small steel key, I wonder what it opens?");
            newbieArea.cloneMob(MobList.BASIC_KEY, newbieArea[RoomID.GPG_ROOM_46], "small steel key", steel_key);

            Mob basic_switch = PrototypeManager.getFullGameRegisteredMob(MobList.SWITCH).Clone();
            EventData ed = new EventData(EventFlag.EVENT_GPG_WALL_REMOVE, CommandName.COMMAND_USE, AreaID.AID_NEWBIEAREA);
            basic_switch.GetEventList().Add(ed);
            newbieArea.cloneMob(MobList.SWITCH, newbieArea[RoomID.GPG_ROOM_29], "", basic_switch);

            basic_switch = PrototypeManager.getFullGameRegisteredMob(MobList.SWITCH).Clone();
            ed = new EventData(EventFlag.EVENT_GPG_WALL_REMOVE, CommandName.COMMAND_USE, AreaID.AID_NEWBIEAREA);
            basic_switch.GetEventList().Add(ed);
            newbieArea.cloneMob(MobList.SWITCH, newbieArea[RoomID.GPG_ROOM_37], "", basic_switch);

            newbieArea.cloneMob(MobList.BASIC_SWORD, newbieArea[RoomID.GPG_PLAYER_START]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_6]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_10]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_17]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_21]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_25]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_29]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_32]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_42]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_47]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_62]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_77]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_60]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_37]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_46]);
            newbieArea.cloneMob(MobList.GOBLIN_RUNT, newbieArea[RoomID.GPG_ROOM_58]);
        }

        public Area getArea(AreaID areaID)
        {
            foreach (Area area in mAreaList)
            {
                if (area.GetAreaID() == areaID)
                    return area;
            }

            return null;
        }// getArea

        public Room getRoom(int x, int y, int z, AreaID areaID)
        {
            Area area = getArea(areaID);

            foreach (KeyValuePair<RoomID, Room> keyValPair in area.GetRooms())
            {
                Room currentRoom = keyValPair.Value;

                if (currentRoom.GetAreaLoc()[0] == x &&
                    currentRoom.GetAreaLoc()[1] == y &&
                    currentRoom.GetAreaLoc()[2] == z)
                {
                    return currentRoom;
                }
            }

            return null;
        }// getRoom

        public Area getArea(string areaName)
        {
            foreach (Area area in mAreaList)
            {
                if(area.GetName().Equals(areaName))
                    return area;
            }// foreach

            return null;
        }// getArea

        public void totallyRemoveRes(Mob resource)
        {
            Room currentRoom = resource.GetCurrentRoom();
            Area currentArea = resource.GetCurrentArea();

            if (currentRoom != null)
                currentRoom.removeRes(resource);
            if (currentArea != null)
                currentArea.removeRes(resource);

            removeRes(resource);
        }// totallyRemoveRes

        // Accessors
        public CommandHandler GetCommandHandler() { return mCommandHandler; }
        public CombatHandler GetCombatHandler() { return mCombatHandler; }
        public EventHandler GetEventHandler() { return mEventHandler; }
        public List<Area> GetAreas() { return mAreaList; }

    }// Class World

}// Namespace _8th_Circle_Server

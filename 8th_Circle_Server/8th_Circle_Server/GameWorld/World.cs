using System;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    // The game world, it is subdivided into areas and further subdivided into rooms.
    // The world holds a reference to all resources, which include players, npcs, objects, etc.
    // The game world also has handlers that take responsibility for events, respawning areas, facilitating
    // combat and executing commands.
    public partial class World : ResourceHandler
    {   
        private CommandHandler mCommandHandler;
        private EventHandler mEventHandler;
        private AreaHandler mAreaHandler;
        private CombatHandler mCombatHandler;
        private List<Area> mAreaList; 

        public World() : base()
        {
            mCommandHandler = new CommandHandler(this);
            mEventHandler = new EventHandler(this);
            mAreaHandler = new AreaHandler(this);
            mCombatHandler = new CombatHandler(this);
            mAreaList = new List<Area>();

            // Add global prototypes and areas
            registerGlobalMobs();
            addAreas();

            // Start all the handlers
            mCommandHandler.start();
            mEventHandler.start();
            mAreaHandler.start();
            mCombatHandler.start();
        }// Constructor

        // Registers all mobs in the game wide prototype dictionary.
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
            EventData eventData = new EventData(EventID.EVENT_TELL_PLAYER, CommandName.COMMAND_LOOK, "A voice speaks to you from within the chest");
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
            eventData = new EventData(EventID.EVENT_TELL_PLAYER, CommandName.COMMAND_LOOK);
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
            eventData = new EventData(EventID.EVENT_TELEPORT, CommandName.COMMAND_GET);
            eventData.SetData(RoomID.GPG_PLAYER_START);
            EventData eventData2 = new EventData(EventID.EVENT_TELEPORT, CommandName.COMMAND_GETALL, RoomID.GPG_PLAYER_START);
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
            goblin_runt[STAT.BASEMAXDAM] = 6;
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

        // Adds all areas to the world
        private void addAreas()
        {
            addProtoArea();
            addGeraldineArea();
            addGPGArea();
        }// addAreas

        // Adds the prototype testing area
        private void addProtoArea()
        {
            Area protoArea = new Area(this, "Proto Area", AreaID.AID_PROTOAREA);
            protoArea.SetDescription("The testing area for the 8th Circle");

            Room pa1 = new Room("Room 0,0,0",0,0,0, RoomID.PROTO_1, protoArea);
            Room pa2 = new Room("Room 1,0,0",1,0,0, RoomID.PROTO_2, protoArea);
            Room pa3 = new Room("Room 2,0,0",2,0,0, RoomID.PROTO_3, protoArea);
            Room pa4 = new Room("Room 0,1,0",0,1,0, RoomID.PROTO_4, protoArea);
            Room pa5 = new Room("Room 1,1,0",1,1,0, RoomID.PROTO_5, protoArea);
            Room pa6 = new Room("Room 2,1,0",2,1,0, RoomID.PROTO_6, protoArea);
            Room pa7 = new Room("Room 0,2,0",0,2,0, RoomID.PROTO_7, protoArea);
            Room pa8 = new Room("Room 1,2,0",1,2,0, RoomID.PROTO_8, protoArea);
            Room pa9 = new Room("Room 2,2,0",2,2,0, RoomID.PROTO_9, protoArea);
            Room pa10 = new Room("Room 0,0,1",0,0,1, RoomID.PROTO_10, protoArea);
            Room pa11 = new Room("Room 1,0,1",1,0,1, RoomID.PROTO_11, protoArea);
            Room pa12 = new Room("Room 2,0,1",2,0,1, RoomID.PROTO_12, protoArea);
            Room pa13 = new Room("Room 0,1,1",0,1,1, RoomID.PROTO_13, protoArea);
            Room pa14 = new Room("Room 1,1,1",1,1,1, RoomID.PROTO_14, protoArea);
            Room pa15 = new Room("Room 2,1,1",2,1,1, RoomID.PROTO_15, protoArea);
            Room pa16 = new Room("Room 0,2,1",0,2,1, RoomID.PROTO_16, protoArea);
            Room pa17 = new Room("Room 1,2,1",1,2,1, RoomID.PROTO_17, protoArea);
            Room pa18 = new Room("Room 2,2,1",2,2,1, RoomID.PROTO_18, protoArea);
            Room pa19 = new Room("Room 0,0,2",0,0,2, RoomID.PROTO_19, protoArea);
            Room pa20 = new Room("Room 1,0,2",1,0,2, RoomID.PROTO_20, protoArea);
            Room pa21 = new Room("Room 2,0,2",2,0,2, RoomID.PROTO_21, protoArea);
            Room pa22 = new Room("Room 0,1,2",0,1,2, RoomID.PROTO_22, protoArea);
            Room pa23 = new Room("Room 1,1,2",1,1,2, RoomID.PROTO_23, protoArea);
            Room pa24 = new Room("Room 2,1,2",2,1,2, RoomID.PROTO_24, protoArea);
            Room pa25 = new Room("Room 0,2,2",0,2,2, RoomID.PROTO_25, protoArea);
            Room pa26 = new Room("Room 1,2,2",1,2,2, RoomID.PROTO_26, protoArea);
            Room pa27 = new Room("Room 2,2,2",2,2,2, RoomID.PROTO_27, protoArea);

            // Add links
            foreach (KeyValuePair<RoomID, Room> keyValPair in protoArea.GetRooms())
            {
                Room currentRoom = keyValPair.Value;
                
                Room nwRoom = getRoom(currentRoom.GetAreaLoc()[0] -1, currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room nRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room neRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room wRoom = getRoom(currentRoom.GetAreaLoc()[0] -1, currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room eRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room swRoom = getRoom(currentRoom.GetAreaLoc()[0] -1, currentRoom.GetAreaLoc()[1] -1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room sRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1] -1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room seRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1] -1, currentRoom.GetAreaLoc()[2], AreaID.AID_PROTOAREA);
                Room uRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2] + 1, AreaID.AID_PROTOAREA);
                Room dRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2] -1, AreaID.AID_PROTOAREA);

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

        // Adds prototype area mobs
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
        }// addProtoMobs

        // Adds the Geraldine Estate area
        private void addGeraldineArea()
        {
            Area geraldineArea = new Area(this, "Geraldine Estate", AreaID.AID_GERALDINEMANOR);
            geraldineArea.SetDescription("The residence of the esteemed Renee and David");

            Room house1stentranceway = new Room("The entrance to the Geraldine Manor, there are stairs " + "leading up.",
                0, 0, 0, RoomID.GERALD_1ST_ENT, geraldineArea);

            Room house1stHallway = new Room("The west hallway is empty besides a few pictures",
                -1, 0, 0, RoomID.GERALD_1ST_HALLWAY, geraldineArea);

            Room house1stKitchen = new Room("The kitchen has a nice view of the outside to the west; " +
                "there are also stairs leading down and a doorway to the north",
                -2, 0, 0, RoomID.GERALD_1ST_KITCHEN, geraldineArea);

            Room house1stDiningRoom = new Room("The dining room is blue with various pictures; one is " +
                "particularly interesting, featuring a type of chicken",
                -2, -1, 0, RoomID.GERALD_1ST_DININGROOM, geraldineArea);

            Room house1stLivingRoom = new Room("The living room is grey with a nice flatscreen tv along " + "the north wall",
                0, -1, 0, RoomID.GERALD_1ST_LIVINGROOM, geraldineArea);

            Room house1stBathroom = new Room("The powder room is a nice small comfortable bathroom with " + "a sink and toilet",
                -1, -1, 0, RoomID.GERALD_1ST_BATHROOM, geraldineArea);

            Room house2ndHallway = new Room("The hallway to the 2nd floor.  This is a long corridor\n with " +
                "many rooms attached to it with stairs leading down at the base.",
                0, 0,  1, RoomID.GERALD_2ND_HALLWAY, geraldineArea);

            Room house2ndKittyroom = new Room("The kittyroom, there is not much here besides some litterboxes.",
                -1, 0,  1, RoomID.GERALD_2ND_KITTYROOM, geraldineArea);

            Room house2ndKittyCloset = new Room("The closet of the kittyroom holds various appliances such as " +
                "vaccuums and other cleaning supplies",
                -1, -1,  1, RoomID.GERALD_2ND_KITTYCLOSET, geraldineArea);

            Room house2ndBathroom = new Room("A small master bathroom has a sink, shower and toilet",
                 1, 0,  1, RoomID.GERALD_2ND_BATHROOM, geraldineArea);

            Room house2ndBedroom = new Room("The master bedroom is huge with two sliding door closets\n and " +
                "windows on the north and northwest sides",
                 1,  1,  1, RoomID.GERALD_2ND_BEDROOM, geraldineArea);

            Room house2ndBlueroom = new Room("The blueroom has a large bookshelf, a sliding door closet and " + "a loveseat",
                0,  1,  1, RoomID.GERALD_2ND_BLUEROOM, geraldineArea);

            Room houseBaseentrance = new Room("The bottom of the stairs leads to the basement.\n This " +
                " is a large basement that spans to the south with \nrooms attached on both sides.",
                0, 0, -1, RoomID.GERALD_BASE_PART1, geraldineArea);

            Room houseBasepart2 = new Room("There is a piano here along the wall with light grey\n " +
                "carpet with the walls being a darker grey",
                0, -1, -1, RoomID.GERALD_BASE_PART2, geraldineArea);

            Room houseBasepart3 = new Room("There isn't much to this piece of the basement besides\n " +
                "some pictures on both the west and east walls.",
                0, -2, -1, RoomID.GERALD_BASE_PART3, geraldineArea);

            Room houseBasepart4 = new Room("You have reached the southern corner of the basement.\n " +
                "There is a computer desk here with a glowing PC and monitor.  There are all\n " +
                "sorts of figurines of wonderous power sitting on the desk along with pictures\n " +
                "depicting awesome scenes of wonder and adventure.  Something about this room\n " +
                "seems filled with some sort of power.",
                0, -3, -1, RoomID.GERALD_BASE_PART4, geraldineArea);

            Room houseBasepart5 = new Room("The southwest most edge of the basement, there is a\n " +
                "couch on the south end of the wall facing a TV in the corner beside a doorway",
                -1, -3, -1, RoomID.GERALD_BASE_PART5, geraldineArea);

            Room houseBaseBathroom = new Room("The bathroom has a standing shower as well as a long \n " +
                "vanity with an accompanying toilet",
                -1, -2, -1, RoomID.GERALD_BASE_BATHROOM, geraldineArea);

            Room houseBaseCloset = new Room("This is a closet that has large holding shelves with\n " +
                "board games from top to bottom.  This is an impressive collection indeed!",
                -1, -1, -1, RoomID.GERALD_BASE_CLOSET, geraldineArea);

            Room houseBaseLaundryRoom = new Room("The laundry room has no carpet and has many shelves\n " +
                "with various pieces of hardware and tools",
                -1, 0, -1, RoomID.GERALD_BASE_LAUNDRYROOM, geraldineArea);

            Room houseBaseSumpRoom = new Room("The sump pump room is bare concrete with a few shelves\n " + "for storage",
                0,  1,  1, RoomID.GERALD_BASE_SUMPROOM, geraldineArea);

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
            getRoom(0,0,0, AreaID.AID_PROTOAREA).GetRoomLinks()[(int)Direction.WEST] = house1stentranceway;

            addGeraldineMobs(geraldineArea);
            mAreaHandler.registerArea(geraldineArea);
        }// geraldineArea

        // Add all Geraldine Esate mobs
        public void addGeraldineMobs(Area area)
        {
            area.cloneMob(MobList.MAX, area[RoomID.GERALD_1ST_LIVINGROOM]);
            area.cloneMob(MobList.FIRST_CIRCLE, area[RoomID.GERALD_BASE_PART4]);
        }// addGeraldineNpcs

        // Add the Goblin Prooving grounds
        public void addGPGArea()
        {
            Area gpgArea = new Area(this, "Goblin Prooving Grounds", AreaID.AID_gpgArea);

            gpgArea.SetDescription("This area is the leftover pits where unworthy goblins were abandoned " +
                                   "and forgotten. The runts here were unworthy to serve any useful\n" +
                                   "purpose in society and were cast out until they prooved themselves");

            String common_gpg_north_field = "The plains end to the north along a rock wall. There is some light\n" +
                                            "a little further, the walls to the north rise as high as you can see.\n" +
                                            "The plains still span in other directions.  All around, you can hear\n" +
                                            "goblins lurking about and you can see dark mud splattered about.";

            String common_gpg_south_field = "The plains end to the south along a rock wall. There is some light\n" +
                                            "a little further, the walls to the south rise as high as you can see.\n" +
                                            "The plains still span in other directions. All around, you can hear\n" +
                                            "goblins lurking about and you can see dark mud splattered about.";

            String common_gpg_east_field = "The plains end to the east along a rock wall. There is some light\n" +
                                           "a little further, the walls to the east rise as high as you can see.\n" +
                                           "The plains still span in other directions. All around, you can hear\n" +
                                           "goblins lurking about and you can see dark mud splattered about.";

            String common_gpg_open_field = "The plains continue on in every direction.  All around, you can hear\n" +
                                           "goblins lurking about and you can see dark mud splattered about.";

            Room gpg_playerStart = new Room("You find yourself in some sort of dark plains. It spans in every\n" +
                                            "direction, although you see some walls to your west. You hear\n" +
                                            "grunting. It smells like... goblins.", 0, 0, 0, RoomID.GPG_PLAYER_START, gpgArea);

            Room gpg_1 = new Room("The plains are dieing down here. Up ahead to the west, you see a path that goes\n" +
                                  "up towards a rocky mountain.  Down to the south, you see more plains, but some\n" +
                                  "caves in the distance", -1, 3, 0, RoomID.GPG_ROOM_1, gpgArea);

            Room gpg_2 = new Room(common_gpg_north_field, 0, 3, 0, RoomID.GPG_ROOM_2, gpgArea);
            Room gpg_3 = new Room(common_gpg_north_field, 1, 3, 0, RoomID.GPG_ROOM_3, gpgArea);
            Room gpg_4 = new Room(common_gpg_north_field, 2, 3, 0, RoomID.GPG_ROOM_4, gpgArea);
            Room gpg_5 = new Room(common_gpg_north_field, 3, 3, 0, RoomID.GPG_ROOM_5, gpgArea);
            Room gpg_6 = new Room(common_gpg_east_field, 4, 3, 0, RoomID.GPG_ROOM_6, gpgArea);

            Room gpg_7 = new Room("Some blood-covered brush litter the plains here.  You see the giant north wall\n" +
                                  "To the west and south, more plains, and some caves.",-1, 2, 0, RoomID.GPG_ROOM_7, gpgArea);

            Room gpg_8 = new Room(common_gpg_open_field, 0, 2, 0, RoomID.GPG_ROOM_8, gpgArea);
            Room gpg_9 = new Room(common_gpg_open_field, 1, 2, 0, RoomID.GPG_ROOM_9, gpgArea);
            Room gpg_10 = new Room(common_gpg_open_field, 2, 2, 0, RoomID.GPG_ROOM_10, gpgArea);
            Room gpg_11 = new Room(common_gpg_open_field, 3, 2, 0, RoomID.GPG_ROOM_11, gpgArea);
            Room gpg_12 = new Room(common_gpg_east_field, 4, 2, 0, RoomID.GPG_ROOM_12, gpgArea);

            Room gpg_13 = new Room("The plains are approaching some caves to the south here.  The cave walls extend\n" +
                                   "up and wrap around to the west. There is a trial following the cave wall.\n" +
                                   "You shutter nervously as you think you are being watched...\n" +
                                   "yes, you are definately being watched...",-1, 1, 0, RoomID.GPG_ROOM_13, gpgArea);

            Room gpg_14 = new Room(common_gpg_open_field, 0, 1, 0, RoomID.GPG_ROOM_14, gpgArea);
            Room gpg_15 = new Room(common_gpg_open_field, 1, 1, 0, RoomID.GPG_ROOM_15, gpgArea);
            Room gpg_16 = new Room(common_gpg_open_field, 2, 1, 0, RoomID.GPG_ROOM_16, gpgArea);
            Room gpg_17 = new Room(common_gpg_open_field, 3, 1, 0, RoomID.GPG_ROOM_17, gpgArea);
            Room gpg_18 = new Room(common_gpg_east_field, 4, 1, 0, RoomID.GPG_ROOM_18, gpgArea);

            Room infront_of_cave_ent = new Room("The mouth of the cave to the west.  Bones and spikes decorate the entrance.\n" +
                                               "The tips of some spikes have dried blood on them.  It seems to get darker\n" +
                                               "the longer you linger at the cave. You can't help but wonder, would could\n" +
                                               "be inside?",-1, 0, 0, RoomID.GPG_ROOM_19, gpgArea);

            Room gpg_21 = new Room(common_gpg_open_field, 1, 0, 0, RoomID.GPG_ROOM_21, gpgArea);
            Room gpg_22 = new Room(common_gpg_open_field, 2, 0, 0, RoomID.GPG_ROOM_22, gpgArea);
            Room gpg_23 = new Room(common_gpg_open_field, 3, 0, 0, RoomID.GPG_ROOM_23, gpgArea);
            Room gpg_24 = new Room(common_gpg_east_field, 4, 0, 0, RoomID.GPG_ROOM_24, gpgArea);

            Room below_cave_ent = new Room("There is a cave entrance to the north of here. The cave walls extend to the\n" +
                                           "west and to the south-west. There was some goblin noises nearby... but you\n" +
                                           "can't tell from which direction, maybe everywhere?",-1,-1, 0, RoomID.GPG_ROOM_25, gpgArea);

            Room gpg_26 = new Room(common_gpg_open_field, 0,-1, 0, RoomID.GPG_ROOM_26, gpgArea);
            Room gpg_27 = new Room(common_gpg_open_field, 1,-1, 0, RoomID.GPG_ROOM_27, gpgArea);
            Room gpg_28 = new Room(common_gpg_open_field, 2,-1, 0, RoomID.GPG_ROOM_28, gpgArea);

            Room main_switch_room = new Room(common_gpg_open_field + "\n" +
                                             "there is a small switch built into the side of a rock.\n" +
                                             "You can't help but wonder what would happen if you used it...", 3,-1, 0, RoomID.GPG_ROOM_29, gpgArea);
            Room gpg_30 = new Room(common_gpg_east_field, 4,-1, 0, RoomID.GPG_ROOM_30, gpgArea);

            Room gpg_31 = new Room("Cave walls extend all throughout the west.  The high walls from the south die\n" +
                                   "down here and open up to an overlook.  There is a steep dropoff down to a river\n" + 
                                   "running rapidly through a gorge.",-1,-2, 0, RoomID.GPG_ROOM_31, gpgArea);

            Room gpg_32 = new Room(common_gpg_south_field, 0,-2, 0, RoomID.GPG_ROOM_32, gpgArea);
            Room gpg_33 = new Room(common_gpg_south_field, 1,-2, 0, RoomID.GPG_ROOM_33, gpgArea);
            Room gpg_34 = new Room(common_gpg_south_field, 2,-2, 0, RoomID.GPG_ROOM_34, gpgArea);
            Room gpg_35 = new Room(common_gpg_south_field, 3,-2, 0, RoomID.GPG_ROOM_35, gpgArea);
            Room gpg_36 = new Room(common_gpg_east_field, 4,-2, 0, RoomID.GPG_ROOM_36, gpgArea);
            Room gpg_37 = new Room("GPG 37",-8, 3, 0, RoomID.GPG_ROOM_37, gpgArea);
            Room gpg_38 = new Room("GPG 38",-7, 3, 0, RoomID.GPG_ROOM_38, gpgArea);
            Room gpg_39 = new Room("GPG 39",-6, 3, 0, RoomID.GPG_ROOM_39, gpgArea);

            Room switch_tunnel_ne = new Room("GPG 40",-5, 3, 0, RoomID.GPG_ROOM_40, gpgArea);

            Room end_of_north_path1 = new Room("The path comes to an end here.  To the north, the dropoff is as steep as\n" +
                                               "ever, and to the west, giant mountain walls extend upwards.  There is a\n" +
                                               "large gate that you can see in the distance to the south.",-4, 3, 0, RoomID.GPG_ROOM_41, gpgArea);

            Room middle_of_north_path1 = new Room("The path continues on to the west. Shrubs are turning into rocks as the\n" +
                                                  "surroundings become more and more mountainous. There is still room to\n" +
                                                  "the south of the path, but it is becoming narrower. Ahead to the west\n" +
                                                  "you can make out some larger cliffs.",-3, 3, 0, RoomID.GPG_ROOM_42, gpgArea);

            Room start_of_north_path1 = new Room("The path is wide as it starts the trek upwards around the side of a mountain\n" +
                                                 "There is more room to the south. To the north, there is a dropoff down the\n" +
                                                 "mountain. You better watch where you are going...",-2, 3, 0, RoomID.GPG_ROOM_43, gpgArea);

            Room gpg_44 = new Room("GPG 44",-8, 2, 0, RoomID.GPG_ROOM_44, gpgArea);
            Room gpg_45 = new Room("GPG 45",-7, 2, 0, RoomID.GPG_ROOM_45, gpgArea);
            Room gpg_46 = new Room("GPG 46",-6, 2, 0, RoomID.GPG_ROOM_46, gpgArea);

            Room switch_tunnel_ent = new Room("The opening leads to a narrow tunnel leading north. Dim torchlight is the\n" +
                                              "only thing noticeable as there are no decorations on the plain stone walls.",
                                              -5, 2, 0, RoomID.GPG_ROOM_47, gpgArea);

            Room end_of_north_path2 = new Room("The path comes to an end here.  To the north, you see the start of the\n" +
                                               "dropoff and directly to the south, a large cracked and broken iron gate.\n" +
                                               "You would normally think this is the entrance of some great fortress but\n" +
                                               "the broken pieces of metal and glass suggest it was seiged long ago.\n" +
                                               "After encountering so many goblins, you wonder if they did the seiging,\n" +
                                               "or if they simply took up residence afterwards.  In either case, you still\n" +
                                               "smell their stench everywhere.",-4, 2, 0, RoomID.GPG_ROOM_48, gpgArea);

            Room middle_of_north_path2 = new Room("The path continues on to the west. Shrubs are turning into rocks as the\n" +
                                                  "surroundings become more and more mountainous. There is still room to\n" +
                                                  "the south of the path, but it is becoming narrower. Ahead to the west\n" +
                                                  "you can make out some larger cliffs.", -3, 2, 0, RoomID.GPG_ROOM_49, gpgArea);

            Room start_of_north_path2 = new Room("The path is wide as it starts a trek upwards around the side of a mountain.\n" + 
                                                 "There is more room to the north as the mountain gets darker.",-2, 2, 0, RoomID.GPG_ROOM_50, gpgArea);

            Room gpg_51 = new Room("GPG 51",-8, 1, 0, RoomID.GPG_ROOM_51, gpgArea);
            Room gpg_52 = new Room("GPG 52",-7, 1, 0, RoomID.GPG_ROOM_52, gpgArea);

            Room barracks_nw = new Room("The barracks continue here, every part of the room looks the same with torches\n" +
                                        "and beds lining it in neat rows.", -6, 1, 0, RoomID.GPG_ROOM_53, gpgArea);

            Room barracks_ne = new Room("The barracks continue here, every part of the room looks the same with torches\n" +
                                        "and beds lining it in neat rows. There is a small wall opening to the north here.",
                                        -5, 1, 0, RoomID.GPG_ROOM_54, gpgArea);

            Room gate_entrance = new Room("You step inside the great iron gate. There is a large walkway here.\n" +
                                          "Remanants of broken pike's, shields, swords and other debris litter\n" +
                                          "the area. You can still see gashes in the rock wall and imprints in\n" +
                                          "the ground. This area was intentionally designed to be a narrow\n" +
                                          "stranglehold to try to be a defensible position. There must have\n" +
                                          "been a great battle here.",-4, 1, 0, RoomID.GPG_ROOM_55, gpgArea);

            Room cave_treasure = new Room("The end of the cave, ah the gleaming you saw earlier was a golden chest!\n" +
                                          "You can't help but think what riches could be inside, finally, this forsaken\n" +
                                          "place must have some redeeming quality... treasure!",-3, 1, 0, RoomID.GPG_ROOM_56, gpgArea);

            Room stone_golem = new Room("The walls end here.  To the north you see the start of a path going upwards.\n" +
                                        "There is something dead here, at first you thought it was some rocks but it\n" +
                                        "looks like it is some sort of stone golem.",-2, 1, 0, RoomID.GPG_ROOM_57, gpgArea);

            Room gpg_58 = new Room("GPG 58",-8, 0, 0, RoomID.GPG_ROOM_58, gpgArea);
            Room gpg_59 = new Room("GPG 59",-7, 0, 0, RoomID.GPG_ROOM_59, gpgArea);

            Room barracks_sw = new Room("The barracks continue here, every part of the room looks the same with torches\n" +
                                        "and beds lining it in neat rows.",-6, 0, 0, RoomID.GPG_ROOM_60, gpgArea);

            Room barracks_ent = new Room("It looks like this is the barracks. Probably close to the entrance so soldiers\n" +
                                         "can quickly mobilize to defend the hideout. It is a large room that spans to\n" +
                                         "the north and west. Torches and beds line the room in nice rows. Other than\n" +
                                         "the essentials, there is not much else in this room.",-5, 0, 0, RoomID.GPG_ROOM_61, gpgArea);

            Room gate_middle = new Room("The middle of the chokepoint from the gate entrance.  There are some destroyed\n" +
                                        "stairs and several crossbow stations that are also half broken off.  The stairs\n" +
                                        "can almost be climbed, but it doesn't look like there is a great viewpoint in\n" +
                                        "these half towers near the side of the mountainface.",-4, 0, 0, RoomID.GPG_ROOM_62, gpgArea);

            Room cave_west = new Room("The tunnel narrows further and winds up to the north.  You see a faint light.\n" +
                                      "There is definately something to the north, maybe even... gleaming?  The\n" +
                                      "roughness of the cave seems to smooth out a bit from the start, maybe this is\n" +
                                      "some sort of dwelling...",-3, 0, 0, RoomID.GPG_ROOM_63, gpgArea);

            Room cave_east = new Room("You shudder as soon as you enter the cave, there is something wrong here,\n" +
                                      "very wrong. You look back, you thought you saw something, maybe it was just\n" +
                                      "nerves. The cave narrows into a tunnel that continues to the west. It is hard\n" +
                                      "to see, but you can make out stalagtites and stlagmites around the tunnel.\n" +
                                      "If you weren't so nervous, you would think this place is rather majestic", 
                                      -2, 0, 0, RoomID.GPG_ROOM_64, gpgArea);

            Room gpg_65 = new Room("GPG 65",-8,-1, 0, RoomID.GPG_ROOM_65, gpgArea);
            Room gpg_66 = new Room("GPG 66",-7,-1, 0, RoomID.GPG_ROOM_66, gpgArea);
            Room gpg_67 = new Room("GPG 67",-6,-1, 0, RoomID.GPG_ROOM_67, gpgArea);

            Room hideout_ent = new Room("The entrance to the goblin hideout. There are guard stations on both sides\n" +
                                        "as you enter. There are a few broken off arrows stuck in the wall. This is\n" +
                                        "an eloborately made cave inside of the mountain. The walls are higher than\n" +
                                        "the first glances imply. It has been well maintained, sortof. It still has\n" +
                                        "some clutter from the goblins but the overall the walls are in good condition."
                                        ,-5,-1, 0, RoomID.GPG_ROOM_68, gpgArea);

            Room gate_back = new Room("The back of the tunnel shows more signs of battle, some of the fiercest\n" +
                                      "fighting must have occured here as there are suits of armor, pikes and\n" +
                                      "even the remnants of a battering ram that has long decayed. The ram is\n" +
                                      "facing the wall to the west and there are scrapes all along its side.\n" +
                                      "You see daylight coming from an opening to the south.",-4,-1, 0, RoomID.GPG_ROOM_69, gpgArea);

            Room gpg_70 = new Room("The room that you aren't supposed to get to...",-3,-1, 0, RoomID.GPG_ROOM_70, gpgArea);

            Room secret_cage_room = new Room("This room was well hidden, the entrance blends in with the\n" +
                                             "rock face. The room was carefully carved into an oval shape.\n" +
                                             "In the center of the room, there is a small stone pedastal.\n" +
                                             "Highlighted on the pedastal is a small metal cage with\n" +
                                             "something gleaming inside...",-2,-1, 0, RoomID.GPG_ROOM_71, gpgArea);

            Room gpg_72 = new Room("GPG 72",-8,-2, 0, RoomID.GPG_ROOM_72, gpgArea);
            Room gpg_73 = new Room("GPG 73",-7,-2, 0, RoomID.GPG_ROOM_73, gpgArea);
            Room gpg_74 = new Room("GPG 74",-6,-2, 0, RoomID.GPG_ROOM_74, gpgArea);
            Room gpg_75 = new Room("GPG 75",-5,-2, 0, RoomID.GPG_ROOM_75, gpgArea);

            Room cliff_ent = new Room("Outside the enclosed pathway that showed most of the fighting, out here\n" +
                                      "is much more peaceful. There is a cliff to the south overlooking a ravine\n" +
                                      "with a small stream flowing through it. There is even a decent amount of\n" +
                                      "grass out here.",-4,-2, 0, RoomID.GPG_ROOM_76, gpgArea);

            Room cliff_middle = new Room("The backside of the overlook. Not much changes here. The stream continues\n" +
                                       "through the ravine east to west as far as you can see. The greenery here is\n" +
                                       "rather calming and a nice smell compared to the stench of the goblins\n" +
                                       "everywhere else.",  -3,-2, 0, RoomID.GPG_ROOM_77, gpgArea);

            Room cliff_back = new Room("The end of the overlook. The cliffs wrap around to the east here and the cave\n" +
                                       "walls span to the north. This place has some nice decor around the overlook\n" +
                                       "that is only partially destroyed. There are some gargoyal statues to the east\n" +
                                       "that seem to be visible to the prooving grounds. Maybe they are there to serve\n" +
                                       "as a warning.",-2,-2, 0, RoomID.GPG_ROOM_78, gpgArea);

            foreach (KeyValuePair<RoomID, Room> keyValPair in gpgArea.GetRooms())
            {
                Room currentRoom = keyValPair.Value;

                Room nwRoom = getRoom(currentRoom.GetAreaLoc()[0] -1, currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_gpgArea);
                Room nRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_gpgArea);
                Room neRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1] + 1, currentRoom.GetAreaLoc()[2], AreaID.AID_gpgArea);
                Room wRoom = getRoom(currentRoom.GetAreaLoc()[0] -1, currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2], AreaID.AID_gpgArea);
                Room eRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2], AreaID.AID_gpgArea);
                Room swRoom = getRoom(currentRoom.GetAreaLoc()[0] -1, currentRoom.GetAreaLoc()[1] -1, currentRoom.GetAreaLoc()[2], AreaID.AID_gpgArea);
                Room sRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1] -1, currentRoom.GetAreaLoc()[2], AreaID.AID_gpgArea);
                Room seRoom = getRoom(currentRoom.GetAreaLoc()[0] + 1, currentRoom.GetAreaLoc()[1] -1, currentRoom.GetAreaLoc()[2], AreaID.AID_gpgArea);
                Room uRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2] + 1, AreaID.AID_gpgArea);
                Room dRoom = getRoom(currentRoom.GetAreaLoc()[0], currentRoom.GetAreaLoc()[1], currentRoom.GetAreaLoc()[2] -1, AreaID.AID_gpgArea);

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
            infront_of_cave_ent.removeDualLinks(Direction.NORTHWEST);
            infront_of_cave_ent.removeDualLinks(Direction.SOUTHWEST);
            gpg_31.removeDualLinks(Direction.NORTHWEST);
            gpg_37.removeDualLinks(Direction.SOUTH);
            gpg_37.removeDualLinks(Direction.SOUTHEAST);
            gpg_38.removeDualLinks(Direction.SOUTH);
            gpg_38.removeDualLinks(Direction.SOUTHWEST);
            gpg_38.removeDualLinks(Direction.SOUTHEAST);
            gpg_39.removeDualLinks(Direction.SOUTH);
            gpg_39.removeDualLinks(Direction.SOUTHWEST);
            switch_tunnel_ne.removeDualLinks(Direction.SOUTHWEST);
            gpg_46.removeDualLinks(Direction.SOUTHEAST);
            switch_tunnel_ent.removeDualLinks(Direction.WEST);
            middle_of_north_path2.removeDualLinks(Direction.SOUTH);
            middle_of_north_path2.removeDualLinks(Direction.SOUTHEAST);
            gpg_52.removeDualLinks(Direction.NORTHEAST);
            barracks_nw.removeDualLinks(Direction.NORTH);
            barracks_nw.removeDualLinks(Direction.NORTHWEST);
            barracks_nw.removeDualLinks(Direction.WEST);
            barracks_nw.removeDualLinks(Direction.SOUTHWEST);
            gate_entrance.removeDualLinks(Direction.EAST);
            gate_entrance.removeDualLinks(Direction.SOUTHEAST);
            gate_entrance.removeDualLinks(Direction.NORTHEAST);
            cave_treasure.removeDualLinks(Direction.NORTHWEST);
            cave_treasure.removeDualLinks(Direction.NORTHEAST);
            cave_treasure.removeDualLinks(Direction.SOUTHWEST);
            stone_golem.removeDualLinks(Direction.WEST);
            stone_golem.removeDualLinks(Direction.SOUTHWEST);
            stone_golem.removeDualLinks(Direction.SOUTH);
            barracks_sw.removeDualLinks(Direction.NORTHWEST);
            barracks_sw.removeDualLinks(Direction.WEST);
            barracks_sw.removeDualLinks(Direction.SOUTHWEST);
            barracks_sw.removeDualLinks(Direction.SOUTH);
            barracks_ent.removeDualLinks(Direction.SOUTHWEST);
            gate_back.removeTripleLinks(Direction.EAST);
            gate_middle.removeDualLinks(Direction.SOUTHEAST);
            gate_middle.removeDualLinks(Direction.SOUTHWEST);
            cave_west.removeTripleLinks(Direction.WEST);
            cave_west.removeDualLinks(Direction.SOUTH);
            cave_east.removeDualLinks(Direction.SOUTH);
            cave_east.removeDualLinks(Direction.SOUTHWEST);
            cave_east.removeDualLinks(Direction.SOUTHEAST);
            cave_east.removeDualLinks(Direction.NORTHWEST);
            cave_east.removeDualLinks(Direction.NORTHEAST);
            gpg_67.removeDualLinks(Direction.NORTHWEST);
            hideout_ent.removeDualLinks(Direction.WEST);
            hideout_ent.removeDualLinks(Direction.SOUTHWEST);
            hideout_ent.removeDualLinks(Direction.NORTHWEST);
            gate_back.removeTripleLinks(Direction.EAST);
            cliff_middle.removeDualLinks(Direction.NORTH);
            secret_cage_room.removeDualLinks(Direction.NORTHWEST);
            secret_cage_room.removeDualLinks(Direction.WEST);
            secret_cage_room.removeDualLinks(Direction.EAST);
            secret_cage_room.removeDualLinks(Direction.SOUTHWEST);
            gpg_75.removeDualLinks(Direction.NORTHWEST);
            cliff_ent.removeDualLinks(Direction.NORTHEAST);
            cliff_ent.removeDualLinks(Direction.NORTHWEST);
            cliff_back.removeDualLinks(Direction.NORTHWEST);
            cliff_back.removeDualLinks(Direction.NORTHEAST);
            cliff_back.removeDualLinks(Direction.EAST);
            end_of_north_path1.removeTripleLinks(Direction.WEST);
            end_of_north_path2.removeTripleLinks(Direction.WEST);
            gate_entrance.removeTripleLinks(Direction.WEST);
            gate_middle.removeTripleLinks(Direction.WEST);
            gate_back.removeTripleLinks(Direction.WEST);
            cliff_ent.removeTripleLinks(Direction.WEST);
            switch_tunnel_ent.removeDualLinks(Direction.NORTHWEST);
            switch_tunnel_ent.removeDualLinks(Direction.SOUTHWEST);

            MobFlags flags = MobFlags.HIDDEN;
            Doorway newDoor = new Doorway("door", flags);
            secret_cage_room.addDoor(newDoor, Direction.SOUTH);
            ((secret_cage_room.GetRoomLinks()[(int)Direction.SOUTH])).addDoor(newDoor, Direction.NORTH);
            newDoor.CreateMemento();

            EventData ed = new EventData(EventID.EVENT_GPG_WALL_ADD, CommandName.COMMAND_USE, AreaID.AID_gpgArea);
            gpgArea.GetRevertEvents().Add(ed);

            addGPGMobs(gpgArea);
            mAreaHandler.registerArea(gpgArea);
        }// addgpgArea

        private void addGPGMobs(Area gpgArea)
        {
            Mob wooden_chest = PrototypeManager.getFullGameRegisteredMob(MobList.BASIC_CHEST).Clone();
            wooden_chest.SetKeyID(4);
            wooden_chest.SetDesc("A shiny golden chest, there must be treasure inside!");
            gpgArea.cloneMob(MobList.BASIC_CHEST, gpgArea[RoomID.GPG_ROOM_56], "golden chest", wooden_chest);

            Mob brass_key = PrototypeManager.getFullGameRegisteredMob(MobList.BASIC_KEY).Clone();
            brass_key.SetKeyID(4);
            gpgArea.cloneMob(MobList.BASIC_KEY, gpgArea[RoomID.GPG_ROOM_70], "brass key", brass_key);

            Mob small_metal_cage = PrototypeManager.getFullGameRegisteredMob(MobList.BASIC_CHEST).Clone();
            small_metal_cage.SetDesc("a small metal cage, I wonder what is inside?");
            small_metal_cage.SetKeyID(5);
            small_metal_cage.GetInv().Add(brass_key);
            Mob parentCage = gpgArea.cloneMob(MobList.BASIC_CHEST, gpgArea[RoomID.GPG_ROOM_71], "small metal cage", small_metal_cage);

            Mob steel_key = PrototypeManager.getFullGameRegisteredMob(MobList.BASIC_KEY).Clone();
            Utils.SetFlag(ref steel_key.mFlags, MobFlags.HIDDEN);
            Utils.SetFlag(ref steel_key.mFlags, MobFlags.LOCKED);
            steel_key.SetKeyID(5);
            steel_key.SetDesc("a small steel key, I wonder what it opens?");
            gpgArea.cloneMob(MobList.BASIC_KEY, gpgArea[RoomID.GPG_ROOM_46], "small steel key", steel_key);

            Mob basic_switch = PrototypeManager.getFullGameRegisteredMob(MobList.SWITCH).Clone();
            EventData ed = new EventData(EventID.EVENT_GPG_WALL_REMOVE, CommandName.COMMAND_USE, AreaID.AID_gpgArea);
            basic_switch.GetEventList().Add(ed);
            gpgArea.cloneMob(MobList.SWITCH, gpgArea[RoomID.GPG_ROOM_29], "", basic_switch);

            basic_switch = PrototypeManager.getFullGameRegisteredMob(MobList.SWITCH).Clone();
            ed = new EventData(EventID.EVENT_GPG_WALL_REMOVE, CommandName.COMMAND_USE, AreaID.AID_gpgArea);
            basic_switch.GetEventList().Add(ed);
            gpgArea.cloneMob(MobList.SWITCH, gpgArea[RoomID.GPG_ROOM_37], "", basic_switch);

            gpgArea.cloneMob(MobList.BASIC_SWORD, gpgArea[RoomID.GPG_PLAYER_START]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_6]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_10]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_17]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_21]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_25]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_29]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_32]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_42]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_47]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_62]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_77]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_60]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_37]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_46]);
            gpgArea.cloneMob(MobList.GOBLIN_RUNT, gpgArea[RoomID.GPG_ROOM_58]);
        }

        // Easy access to areas
        public Area getArea(AreaID areaID)
        {
            foreach (Area area in mAreaList)
            {
                if (area.GetAreaID() == areaID)
                    return area;
            }

            return null;
        }// getArea

        // Access for rooms
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

        // Removes a mob resource from the world, its area, and its room
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

        // Adds a mob resource to the world, its area, and its room
        public void totallyAddRes(Mob resource)
        {
            Room currentRoom = resource.GetCurrentRoom();
            Area currentArea = resource.GetCurrentArea();

            if (currentRoom != null)
                currentRoom.addRes(resource);
            if (currentArea != null)
                currentArea.addRes(resource);

            addRes(resource);
        }// totallyAddRes

        // Accessors
        public CommandHandler GetCommandHandler() { return mCommandHandler; }
        public CombatHandler GetCombatHandler() { return mCombatHandler; }
        public EventHandler GetEventHandler() { return mEventHandler; }
        public List<Area> GetAreas() { return mAreaList; }

    }// Class World

}// Namespace _8th_Circle_Server

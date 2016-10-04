
namespace _8th_Circle_Server
{
    partial class World
    {
        private void addAreas()
        {
            addGeraldineArea();
            addNewbieArea();
        }// addAreas

        private void addGeraldineArea()
        {
            Area geraldineArea = new Area(this);
            geraldineArea.mName = "Geraldine Estate";
            geraldineArea.mDescription = "The residence of the esteemed Renee and David";

            Room house1stentranceway = new Room("The entrance to the Geraldine Manor, there are stairs " +
                "leading up.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET, RoomID.GERALD_1ST_ENT, geraldineArea);
            house1stentranceway.mCurrentArea = geraldineArea;
            Room house1stHallway = new Room("The west hallway is empty besides a few pictures",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET, RoomID.GERALD_1ST_HALLWAY, geraldineArea);
            house1stHallway.mCurrentArea = geraldineArea;
            Room house1stKitchen = new Room("The kitchen has a nice view of the outside to the west; " +
                "there are also stairs leading down and a doorway to the north",
                HOUSE_OFFSET - 2, HOUSE_OFFSET, HOUSE_OFFSET, RoomID.GERALD_1ST_KITCHEN, geraldineArea);
            house1stKitchen.mCurrentArea = geraldineArea;
            Room house1stDiningRoom = new Room("The dining room is blue with various pictures; one is " +
                "particularly interesting, featuring a type of chicken",
                HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, HOUSE_OFFSET, RoomID.GERALD_1ST_DININGROOM, geraldineArea);
            house1stDiningRoom.mCurrentArea = geraldineArea;
            Room house1stLivingRoom = new Room("The living room is grey with a nice flatscreen tv along " +
                "the north wall",
                HOUSE_OFFSET, HOUSE_OFFSET - 1, HOUSE_OFFSET, RoomID.GERALD_1ST_LIVINGROOM, geraldineArea);
            house1stLivingRoom.mCurrentArea = geraldineArea;
            Room house1stBathroom = new Room("The powder room is a nice small comfortable bathroom with " +
                "a sink and toilet",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET, RoomID.GERALD_2ND_BATHROOM, geraldineArea);
            house1stBathroom.mCurrentArea = geraldineArea;

            Room house2ndHallway = new Room("The hallway to the 2nd floor.  This is a long corridor\n with " +
                "many rooms attached to it with stairs leading down at the base.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_HALLWAY, geraldineArea);
            house2ndHallway.mCurrentArea = geraldineArea;
            Room house2ndKittyroom = new Room("The kittyroom, there is not much here besides some litterboxes.",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_KITTYROOM, geraldineArea);
            house2ndKittyroom.mCurrentArea = geraldineArea;
            Room house2ndKittyCloset = new Room("The closet of the kittyroom holds various appliances such as " +
                "vaccuums and other cleaning supplies",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_KITTYCLOSET, geraldineArea);
            house2ndKittyCloset.mCurrentArea = geraldineArea;
            Room house2ndBathroom = new Room("A small master bathroom has a sink, shower and toilet",
                HOUSE_OFFSET + 1, HOUSE_OFFSET, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_BATHROOM, geraldineArea);
            house2ndBathroom.mCurrentArea = geraldineArea;
            Room house2ndBedroom = new Room("The master bedroom is huge with two sliding door closets\n and " +
                "windows on the north and northwest sides",
                HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_BEDROOM, geraldineArea);
            house2ndBedroom.mCurrentArea = geraldineArea;
            Room house2ndBlueroom = new Room("The blueroom has a large bookshelf, a sliding door closet and " +
                "a loveseat",
                HOUSE_OFFSET, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_BLUEROOM, geraldineArea);
            house2ndBlueroom.mCurrentArea = geraldineArea;

            Room houseBaseentrance = new Room("The bottom of the stairs leads to the basement.\n This " +
                " is a large basement that spans to the south with \nrooms attached on both sides.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART1, geraldineArea);
            houseBaseentrance.mCurrentArea = geraldineArea;
            Room houseBasepart2 = new Room("There is a piano here along the wall with light grey\n " +
                "carpet with the walls being a darker grey",
                HOUSE_OFFSET, HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART2, geraldineArea);
            houseBasepart2.mCurrentArea = geraldineArea;
            Room houseBasepart3 = new Room("There isn't much to this piece of the basement besides\n " +
                "some pictures on both the west and east walls.",
                HOUSE_OFFSET, HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART3, geraldineArea);
            houseBasepart3.mCurrentArea = geraldineArea;
            Room houseBasepart4 = new Room("You have reached the southern corner of the basement.\n " +
                "There is a computer desk here with a glowing PC and monitor.  There are all\n " +
                "sorts of figurines of wonderous power sitting on the desk along with pictures\n " +
                "depicting awesome scenes of wonder and adventure.  Something about this room\n " +
                "seems filled with some sort of power.",
                HOUSE_OFFSET, HOUSE_OFFSET - 3, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART4, geraldineArea);
            houseBasepart4.mCurrentArea = geraldineArea;
            Room houseBasepart5 = new Room("The southwest most edge of the basement, there is a\n " +
                "couch on the south end of the wall facing a TV in the corner beside a doorway",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 3, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART5, geraldineArea);
            houseBasepart5.mCurrentArea = geraldineArea;
            Room houseBaseBathroom = new Room("The bathroom has a standing shower as well as a long \n " +
                "vanity with an accompanying toilet",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_BATHROOM, geraldineArea);
            houseBaseBathroom.mCurrentArea = geraldineArea;
            Room houseBaseCloset = new Room("This is a closet that has large holding shelves with\n " +
                "board games from top to bottom.  This is an impressive collection indeed!",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_CLOSET, geraldineArea);
            houseBaseCloset.mCurrentArea = geraldineArea;
            Room houseBaseLaundryRoom = new Room("The laundry room has no carpet and has many shelves\n " +
                "with various pieces of hardware and tools",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_LAUNDRYROOM, geraldineArea);
            houseBaseLaundryRoom.mCurrentArea = geraldineArea;
            Room houseBaseSumpRoom = new Room("The sump pump room is bare concrete with a few shelves\n " +
                "for storage",
                HOUSE_OFFSET, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, RoomID.GERALD_BASE_SUMPROOM, geraldineArea);

            houseBaseSumpRoom.mCurrentArea = geraldineArea;
            house2ndHallway.mCurrentArea = geraldineArea;
            house1stentranceway.mRoomLinks[(int)Direction.NORTH] = house1stHallway;
            house1stentranceway.mRoomLinks[(int)Direction.WEST] = house1stLivingRoom;
            house1stHallway.mRoomLinks[(int)Direction.SOUTH] = house1stentranceway;
            house1stHallway.mRoomLinks[(int)Direction.WEST] = house1stBathroom;
            house1stHallway.mRoomLinks[(int)Direction.NORTH] = house1stKitchen;
            house1stKitchen.mRoomLinks[(int)Direction.WEST] = house1stDiningRoom;
            house1stKitchen.mRoomLinks[(int)Direction.SOUTH] = house1stHallway;
            house1stDiningRoom.mRoomLinks[(int)Direction.EAST] = house1stKitchen;
            house1stDiningRoom.mRoomLinks[(int)Direction.SOUTH] = house1stLivingRoom;
            house1stLivingRoom.mRoomLinks[(int)Direction.EAST] = house1stentranceway;
            house1stLivingRoom.mRoomLinks[(int)Direction.NORTH] = house1stDiningRoom;
            house1stBathroom.mRoomLinks[(int)Direction.EAST] = house1stHallway;

            Doorway newDoor = new Doorway("door");
            house1stBathroom.addDoor(newDoor, Direction.EAST);
            house1stHallway.addDoor(newDoor, Direction.WEST);

            newDoor = new Doorway("door");
            house1stKitchen.addDoor(newDoor, Direction.DOWN);
            houseBaseentrance.addDoor(newDoor, Direction.UP);

            house1stentranceway.mRoomLinks[(int)Direction.UP] = house2ndHallway;
            house2ndHallway.mRoomLinks[(int)Direction.DOWN] = house1stentranceway;
            house2ndHallway.mRoomLinks[(int)Direction.EAST] = house2ndKittyroom;
            house2ndHallway.mRoomLinks[(int)Direction.WEST] = house2ndBathroom;
            house2ndHallway.mRoomLinks[(int)Direction.SOUTH] = house2ndBlueroom;
            house2ndHallway.mRoomLinks[(int)Direction.SOUTHWEST] = house2ndBedroom;
            house2ndKittyroom.mRoomLinks[(int)Direction.NORTH] = house2ndKittyCloset;
            house2ndKittyroom.mRoomLinks[(int)Direction.WEST] = house2ndHallway;
            house2ndKittyCloset.mRoomLinks[(int)Direction.SOUTH] = house2ndKittyroom;
            house2ndBathroom.mRoomLinks[(int)Direction.EAST] = house2ndHallway;
            house2ndBathroom.mRoomLinks[(int)Direction.SOUTH] = house2ndBedroom;
            house2ndBedroom.mRoomLinks[(int)Direction.NORTHEAST] = house2ndHallway;
            house2ndBedroom.mRoomLinks[(int)Direction.NORTH] = house2ndBathroom;
            house2ndBlueroom.mRoomLinks[(int)Direction.NORTH] = house2ndHallway;

            newDoor = new Doorway("door");
            house2ndHallway.addDoor(newDoor, Direction.WEST);
            house2ndBathroom.addDoor(newDoor, Direction.EAST);

            newDoor = new Doorway("door");
            house2ndHallway.addDoor(newDoor, Direction.EAST);
            house2ndKittyroom.addDoor(newDoor, Direction.WEST);

            newDoor = new Doorway("door");
            house2ndHallway.addDoor(newDoor, Direction.SOUTH);
            house2ndBlueroom.addDoor(newDoor, Direction.NORTH);

            newDoor = new Doorway("door");
            house2ndHallway.addDoor(newDoor, Direction.SOUTHWEST);
            house2ndBedroom.addDoor(newDoor, Direction.NORTHEAST);

            newDoor = new Doorway("door");
            house2ndBathroom.addDoor(newDoor, Direction.SOUTH);
            house2ndBedroom.addDoor(newDoor, Direction.NORTH);

            houseBaseentrance.mRoomLinks[(int)Direction.UP] = house1stKitchen;
            house1stKitchen.mRoomLinks[(int)Direction.DOWN] = houseBaseentrance;
            houseBaseentrance.mRoomLinks[(int)Direction.SOUTH] = houseBasepart2;
            houseBaseentrance.mRoomLinks[(int)Direction.WEST] = houseBaseLaundryRoom;
            houseBaseLaundryRoom.mRoomLinks[(int)Direction.EAST] = houseBaseentrance;
            houseBasepart2.mRoomLinks[(int)Direction.NORTH] = houseBaseentrance;
            houseBasepart2.mRoomLinks[(int)Direction.WEST] = houseBaseCloset;
            houseBasepart2.mRoomLinks[(int)Direction.EAST] = houseBaseSumpRoom;
            houseBasepart2.mRoomLinks[(int)Direction.SOUTH] = houseBasepart3;
            houseBaseSumpRoom.mRoomLinks[(int)Direction.WEST] = houseBasepart2;
            houseBaseCloset.mRoomLinks[(int)Direction.EAST] = houseBasepart2;
            houseBasepart3.mRoomLinks[(int)Direction.NORTH] = houseBasepart2;
            houseBasepart3.mRoomLinks[(int)Direction.SOUTH] = houseBasepart4;
            houseBasepart4.mRoomLinks[(int)Direction.NORTH] = houseBasepart3;
            houseBasepart4.mRoomLinks[(int)Direction.WEST] = houseBasepart5;
            houseBasepart5.mRoomLinks[(int)Direction.EAST] = houseBasepart4;
            houseBasepart5.mRoomLinks[(int)Direction.NORTH] = houseBaseBathroom;
            houseBaseBathroom.mRoomLinks[(int)Direction.SOUTH] = houseBasepart5;

            newDoor = new Doorway("door");
            houseBaseentrance.addDoor(newDoor, Direction.WEST);
            houseBaseLaundryRoom.addDoor(newDoor, Direction.EAST);

            newDoor = new Doorway("door");
            houseBasepart2.addDoor(newDoor, Direction.WEST);
            houseBaseCloset.addDoor(newDoor, Direction.EAST);

            newDoor = new Doorway("door");
            houseBasepart2.addDoor(newDoor, Direction.EAST);
            houseBaseSumpRoom.addDoor(newDoor, Direction.WEST);

            newDoor = new Doorway("door");
            houseBaseBathroom.addDoor(newDoor, Direction.SOUTH);
            houseBasepart5.addDoor(newDoor, Direction.NORTH);

            geraldineArea.mRoomList.Add(house1stentranceway);
            geraldineArea.mRoomList.Add(house1stHallway);
            geraldineArea.mRoomList.Add(house1stKitchen);
            geraldineArea.mRoomList.Add(house1stDiningRoom);
            geraldineArea.mRoomList.Add(house1stLivingRoom);
            geraldineArea.mRoomList.Add(house1stBathroom);
            geraldineArea.mRoomList.Add(house2ndHallway);
            geraldineArea.mRoomList.Add(house2ndBathroom);
            geraldineArea.mRoomList.Add(house2ndBedroom);
            geraldineArea.mRoomList.Add(house2ndBlueroom);
            geraldineArea.mRoomList.Add(house2ndKittyCloset);
            geraldineArea.mRoomList.Add(house2ndKittyroom);
            geraldineArea.mRoomList.Add(houseBaseBathroom);
            geraldineArea.mRoomList.Add(houseBaseCloset);
            geraldineArea.mRoomList.Add(houseBaseentrance);
            geraldineArea.mRoomList.Add(houseBaseLaundryRoom);
            geraldineArea.mRoomList.Add(houseBasepart2);
            geraldineArea.mRoomList.Add(houseBasepart3);
            geraldineArea.mRoomList.Add(houseBasepart4);
            geraldineArea.mRoomList.Add(houseBasepart5);
            geraldineArea.mRoomList.Add(houseBaseSumpRoom);

            addGeraldineNpcs(geraldineArea);
            geraldineArea.mAreaID = AreaID.AID_GERALDINEMANOR;
            geraldineArea.mAreaOffset = HOUSE_OFFSET;
            mAreaList.Add(geraldineArea);
            Area protoArea = getArea(AreaID.AID_PROTOAREA);
            getRoom(protoArea.mAreaOffset, protoArea.mAreaOffset, 
                protoArea.mAreaOffset, AreaID.AID_PROTOAREA).mRoomLinks[(int)Direction.WEST] = house1stentranceway;

            mAreaHandler.registerArea(geraldineArea);
        }// geraldineArea

        public void addGeraldineNpcs(Area area)
        {
            addNewMob(MOBLIST.MAX, area.getRoom(RoomID.GERALD_1ST_LIVINGROOM), area);
            addNewMob(MOBLIST.FIRST_CIRCLE, area.getRoom(RoomID.GERALD_BASE_PART4), area);
        }// addGeraldineNpcs

        public void addNewbieArea()
        {
            Area newbieArea = new Area(this);
            newbieArea.mName = "Goblin Prooving Grounds";
            newbieArea.mDescription = "This area is the leftover pits where unworthy goblins were abandoned " +
               "and forgotten.  The runts here were unworthy to serve any useful purpose in goblin society " +
               "and were cast out until they prooved themselves";
            newbieArea.mAreaID = AreaID.AID_NEWBIEAREA;

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

            Room gpg_1 = new Room("GPG 1",
               BAO-1, BAO+3, BAO, RoomID.GPG_ROOM_1, newbieArea);
            Room gpg_2 = new Room(common_gpg_north_field, BAO, BAO+3, BAO, RoomID.GPG_ROOM_2, newbieArea);
            Room gpg_3 = new Room(common_gpg_north_field, BAO+1, BAO+3, BAO, RoomID.GPG_ROOM_3, newbieArea);
            Room gpg_4 = new Room(common_gpg_north_field, BAO+2, BAO+3, BAO, RoomID.GPG_ROOM_4, newbieArea);
            Room gpg_5 = new Room(common_gpg_north_field, BAO+3, BAO+3, BAO, RoomID.GPG_ROOM_5, newbieArea);
            Room gpg_6 = new Room(common_gpg_east_field,  BAO+4, BAO+3, BAO, RoomID.GPG_ROOM_6, newbieArea);
            Room gpg_7 = new Room("GPG 7",
               BAO-1, BAO+2, BAO, RoomID.GPG_ROOM_7, newbieArea);
            Room gpg_8 = new Room(common_gpg_open_field, BAO, BAO+2, BAO, RoomID.GPG_ROOM_8, newbieArea);
            Room gpg_9 = new Room(common_gpg_open_field, BAO+1, BAO+2, BAO, RoomID.GPG_ROOM_9, newbieArea);
            Room gpg_10 = new Room(common_gpg_open_field, BAO+2, BAO+2, BAO, RoomID.GPG_ROOM_10, newbieArea);
            Room gpg_11 = new Room(common_gpg_open_field, BAO+3, BAO+2, BAO, RoomID.GPG_ROOM_11, newbieArea);
            Room gpg_12 = new Room(common_gpg_east_field, BAO+4, BAO+2, BAO, RoomID.GPG_ROOM_12, newbieArea);
            Room gpg_13 = new Room("GPG 13",
               BAO-1, BAO+1, BAO, RoomID.GPG_ROOM_13, newbieArea);
            Room gpg_14 = new Room(common_gpg_open_field, BAO, BAO+1, BAO, RoomID.GPG_ROOM_14, newbieArea);
            Room gpg_15 = new Room(common_gpg_open_field, BAO+1, BAO+1, BAO, RoomID.GPG_ROOM_15, newbieArea);
            Room gpg_16 = new Room(common_gpg_open_field, BAO+2, BAO+1, BAO, RoomID.GPG_ROOM_16, newbieArea);
            Room gpg_17 = new Room(common_gpg_open_field, BAO+3, BAO+1, BAO, RoomID.GPG_ROOM_17, newbieArea);
            Room gpg_18 = new Room(common_gpg_east_field, BAO+4, BAO+1, BAO, RoomID.GPG_ROOM_18, newbieArea);
            Room gpg_19 = new Room("GPG 19",
               BAO-1, BAO, BAO, RoomID.GPG_ROOM_19, newbieArea);
            Room gpg_21 = new Room(common_gpg_open_field, BAO+1, BAO, BAO, RoomID.GPG_ROOM_21, newbieArea);
            Room gpg_22 = new Room(common_gpg_open_field, BAO+2, BAO, BAO, RoomID.GPG_ROOM_22, newbieArea);
            Room gpg_23 = new Room(common_gpg_open_field, BAO+3, BAO, BAO, RoomID.GPG_ROOM_23, newbieArea);
            Room gpg_24 = new Room(common_gpg_east_field, BAO+4, BAO, BAO, RoomID.GPG_ROOM_24, newbieArea);
            Room gpg_25 = new Room("GPG 25",
               BAO-1, BAO-1, BAO, RoomID.GPG_ROOM_25, newbieArea);
            Room gpg_26 = new Room(common_gpg_open_field, BAO, BAO-1, BAO, RoomID.GPG_ROOM_26, newbieArea);
            Room gpg_27 = new Room(common_gpg_open_field, BAO+1, BAO-1, BAO, RoomID.GPG_ROOM_27, newbieArea);
            Room gpg_28 = new Room(common_gpg_open_field, BAO+2, BAO-1, BAO, RoomID.GPG_ROOM_28, newbieArea);
            Room gpg_29 = new Room(common_gpg_open_field + "there is a small switch built into the side of a rock.\n" +
               "You can't help but wonder what would happen if you used it...\n",
               BAO+3, BAO-1, BAO, RoomID.GPG_ROOM_29, newbieArea);
            Room gpg_30 = new Room(common_gpg_east_field, BAO+4, BAO-1, BAO, RoomID.GPG_ROOM_30, newbieArea);
            Room gpg_31 = new Room("GPG 31", BAO-1, BAO-2, BAO, RoomID.GPG_ROOM_31, newbieArea);
            Room gpg_32 = new Room(common_gpg_south_field, BAO, BAO-2, BAO, RoomID.GPG_ROOM_32, newbieArea);
            Room gpg_33 = new Room(common_gpg_south_field, BAO+1, BAO-2, BAO, RoomID.GPG_ROOM_33, newbieArea);
            Room gpg_34 = new Room(common_gpg_south_field, BAO+2, BAO-2, BAO, RoomID.GPG_ROOM_34, newbieArea);
            Room gpg_35 = new Room(common_gpg_south_field, BAO+3, BAO-2, BAO, RoomID.GPG_ROOM_35, newbieArea);
            Room gpg_36 = new Room(common_gpg_east_field,  BAO+4, BAO-2, BAO, RoomID.GPG_ROOM_36, newbieArea);
            Room gpg_37 = new Room("GPG 37", BAO - 8, BAO + 3, BAO, RoomID.GPG_ROOM_37, newbieArea);
            Room gpg_38 = new Room("GPG 38", BAO-7, BAO + 3, BAO, RoomID.GPG_ROOM_38, newbieArea);
            Room gpg_39 = new Room("GPG 39", BAO - 6, BAO + 3, BAO, RoomID.GPG_ROOM_39, newbieArea);
            Room gpg_40 = new Room("GPG 40", BAO - 5, BAO + 3, BAO, RoomID.GPG_ROOM_40, newbieArea);
            Room gpg_41 = new Room("GPG 41", BAO - 4, BAO + 3, BAO, RoomID.GPG_ROOM_41, newbieArea);
            Room gpg_42 = new Room("GPG 42", BAO - 3, BAO + 3, BAO, RoomID.GPG_ROOM_42, newbieArea);
            Room gpg_43 = new Room("GPG 43", BAO - 2, BAO + 3, BAO, RoomID.GPG_ROOM_43, newbieArea);
            Room gpg_44 = new Room("GPG 44", BAO - 8, BAO + 2, BAO, RoomID.GPG_ROOM_44, newbieArea);
            Room gpg_45 = new Room("GPG 45", BAO - 7, BAO + 2, BAO, RoomID.GPG_ROOM_45, newbieArea);
            Room gpg_46 = new Room("GPG 46", BAO - 6, BAO + 2, BAO, RoomID.GPG_ROOM_46, newbieArea);
            Room gpg_47 = new Room("GPG 47", BAO -5, BAO + 2, BAO, RoomID.GPG_ROOM_47, newbieArea);
            Room gpg_48 = new Room("GPG 48", BAO - 4, BAO + 2, BAO, RoomID.GPG_ROOM_48, newbieArea);
            Room gpg_49 = new Room("GPG 49", BAO - 3, BAO + 2, BAO, RoomID.GPG_ROOM_49, newbieArea);
            Room gpg_50 = new Room("GPG 50", BAO - 2, BAO + 2, BAO, RoomID.GPG_ROOM_50, newbieArea);
            Room gpg_51 = new Room("GPG 51", BAO - 8, BAO + 1, BAO, RoomID.GPG_ROOM_51, newbieArea);
            Room gpg_52 = new Room("GPG 52", BAO - 7, BAO + 1, BAO, RoomID.GPG_ROOM_52, newbieArea);
            Room gpg_53 = new Room("GPG 53", BAO - 6, BAO + 1, BAO, RoomID.GPG_ROOM_53, newbieArea);
            Room gpg_54 = new Room("GPG 54", BAO - 5, BAO + 1, BAO, RoomID.GPG_ROOM_54, newbieArea);
            Room gpg_55 = new Room("GPG 55", BAO - 4, BAO+1, BAO, RoomID.GPG_ROOM_55, newbieArea);
            Room gpg_56 = new Room("GPG 56", BAO - 3, BAO+1, BAO, RoomID.GPG_ROOM_56, newbieArea);
            Room gpg_57 = new Room("GPG 57", BAO - 2, BAO+1, BAO, RoomID.GPG_ROOM_57, newbieArea);
            Room gpg_58 = new Room("GPG 58", BAO -8, BAO, BAO, RoomID.GPG_ROOM_58, newbieArea);
            Room gpg_59 = new Room("GPG 59", BAO -7, BAO, BAO, RoomID.GPG_ROOM_59, newbieArea);
            Room gpg_60 = new Room("GPG 60", BAO - 6, BAO, BAO, RoomID.GPG_ROOM_60, newbieArea);
            Room gpg_61 = new Room("GPG 61", BAO-5, BAO, BAO, RoomID.GPG_ROOM_61, newbieArea);
            Room gpg_62 = new Room("GPG 62", BAO-4, BAO, BAO, RoomID.GPG_ROOM_62, newbieArea);
            Room gpg_63 = new Room("GPG 63", BAO -3, BAO, BAO, RoomID.GPG_ROOM_63, newbieArea);
            Room gpg_64 = new Room("GPG 64", BAO -2, BAO, BAO, RoomID.GPG_ROOM_64, newbieArea);
            Room gpg_65 = new Room("GPG 65", BAO -8, BAO-1, BAO, RoomID.GPG_ROOM_65, newbieArea);
            Room gpg_66 = new Room("GPG 66", BAO - 7, BAO - 1, BAO, RoomID.GPG_ROOM_66, newbieArea);
            Room gpg_67 = new Room("GPG 67", BAO-6, BAO - 1, BAO, RoomID.GPG_ROOM_67, newbieArea);
            Room gpg_68 = new Room("GPG 68", BAO -5, BAO - 1, BAO, RoomID.GPG_ROOM_68, newbieArea);
            Room gpg_69 = new Room("GPG 69", BAO -4, BAO - 1, BAO, RoomID.GPG_ROOM_69, newbieArea);
            Room gpg_70 = new Room("GPG 70", BAO -3, BAO - 1, BAO, RoomID.GPG_ROOM_70, newbieArea);
            Room gpg_71 = new Room("GPG 71", BAO -2, BAO - 1, BAO, RoomID.GPG_ROOM_71, newbieArea);
            Room gpg_72 = new Room("GPG 72", BAO - 8, BAO - 2, BAO, RoomID.GPG_ROOM_72, newbieArea);
            Room gpg_73 = new Room("GPG 73", BAO - 7, BAO - 2, BAO, RoomID.GPG_ROOM_73, newbieArea);
            Room gpg_74 = new Room("GPG 74", BAO - 6, BAO - 2, BAO, RoomID.GPG_ROOM_74, newbieArea);
            Room gpg_75 = new Room("GPG 75", BAO - 5, BAO - 2, BAO, RoomID.GPG_ROOM_75, newbieArea);
            Room gpg_76 = new Room("GPG 76", BAO - 4, BAO - 2, BAO, RoomID.GPG_ROOM_76, newbieArea);
            Room gpg_77 = new Room("GPG 77", BAO - 3, BAO - 2, BAO, RoomID.GPG_ROOM_77, newbieArea);
            Room gpg_78 = new Room("GPG 78", BAO - 2, BAO - 2, BAO, RoomID.GPG_ROOM_77, newbieArea);

            mAreaList.Add(newbieArea);

            foreach (Room room in newbieArea.mRoomList)
            {
                Room nwRoom = getRoom(room.mAreaLoc[0] - 1, room.mAreaLoc[1] + 1, room.mAreaLoc[2], AreaID.AID_NEWBIEAREA);
                Room nRoom = getRoom(room.mAreaLoc[0], room.mAreaLoc[1] + 1, room.mAreaLoc[2], AreaID.AID_NEWBIEAREA);
                Room neRoom = getRoom(room.mAreaLoc[0] + 1, room.mAreaLoc[1] + 1, room.mAreaLoc[2], AreaID.AID_NEWBIEAREA);
                Room wRoom = getRoom(room.mAreaLoc[0] - 1, room.mAreaLoc[1], room.mAreaLoc[2], AreaID.AID_NEWBIEAREA);
                Room eRoom = getRoom(room.mAreaLoc[0] + 1, room.mAreaLoc[1], room.mAreaLoc[2], AreaID.AID_NEWBIEAREA);
                Room swRoom = getRoom(room.mAreaLoc[0] - 1, room.mAreaLoc[1] - 1, room.mAreaLoc[2], AreaID.AID_NEWBIEAREA);
                Room sRoom = getRoom(room.mAreaLoc[0], room.mAreaLoc[1] - 1, room.mAreaLoc[2], AreaID.AID_NEWBIEAREA);
                Room seRoom = getRoom(room.mAreaLoc[0] + 1, room.mAreaLoc[1] - 1, room.mAreaLoc[2], AreaID.AID_NEWBIEAREA);
                Room uRoom = getRoom(room.mAreaLoc[0], room.mAreaLoc[1], room.mAreaLoc[2] + 1, AreaID.AID_NEWBIEAREA);
                Room dRoom = getRoom(room.mAreaLoc[0], room.mAreaLoc[1], room.mAreaLoc[2] - 1, AreaID.AID_NEWBIEAREA);

                if (nwRoom != null)
                {
                    room.mRoomLinks[(int)Direction.NORTHWEST] = nwRoom;
                    nwRoom.mRoomLinks[(int)Direction.SOUTHEAST] = room;
                }
                if (nRoom != null)
                {
                    room.mRoomLinks[(int)Direction.NORTH] = nRoom;
                    nRoom.mRoomLinks[(int)Direction.SOUTH] = room;
                }
                if (neRoom != null)
                {
                    room.mRoomLinks[(int)Direction.NORTHEAST] = neRoom;
                    neRoom.mRoomLinks[(int)Direction.SOUTHWEST] = room;
                }
                if (wRoom != null)
                {
                    room.mRoomLinks[(int)Direction.WEST] = wRoom;
                    wRoom.mRoomLinks[(int)Direction.EAST] = room;
                }
                if (eRoom != null)
                {
                    room.mRoomLinks[(int)Direction.EAST] = eRoom;
                    eRoom.mRoomLinks[(int)Direction.WEST] = room;
                }
                if (swRoom != null)
                {
                    room.mRoomLinks[(int)Direction.SOUTHWEST] = swRoom;
                    swRoom.mRoomLinks[(int)Direction.NORTHEAST] = room;
                }
                if (sRoom != null)
                {
                    room.mRoomLinks[(int)Direction.SOUTH] = sRoom;
                    sRoom.mRoomLinks[(int)Direction.NORTH] = room;
                }
                if (seRoom != null)
                {
                    room.mRoomLinks[(int)Direction.SOUTHEAST] = seRoom;
                    seRoom.mRoomLinks[(int)Direction.NORTHWEST] = room;
                }
                if (uRoom != null)
                {
                    room.mRoomLinks[(int)Direction.UP] = uRoom;
                    uRoom.mRoomLinks[(int)Direction.DOWN] = room;
                }
                if (dRoom != null)
                {
                    room.mRoomLinks[(int)Direction.DOWN] = dRoom;
                    dRoom.mRoomLinks[(int)Direction.UP] = room;
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
            gpg_70 = null;
            gpg_41.removeTripleLinks(Direction.WEST);
            gpg_48.removeTripleLinks(Direction.WEST);
            gpg_55.removeTripleLinks(Direction.WEST);
            gpg_62.removeTripleLinks(Direction.WEST);
            gpg_69.removeTripleLinks(Direction.WEST);
            gpg_76.removeTripleLinks(Direction.WEST);

            Doorway newDoor = new Doorway("door");
            newDoor.mFlagList.Add(MobFlags.FLAG_HIDDEN);
            newDoor.mStartingFlagList.Add(MobFlags.FLAG_HIDDEN);
            gpg_71.addDoor(newDoor, Direction.SOUTH);
            ((gpg_71.mRoomLinks[(int)Direction.SOUTH])).addDoor(newDoor, Direction.NORTH);

            Container wooden_chest = new Container((Container)mFullMobList[(int)MOBLIST.BASIC_CHEST]);
            wooden_chest.mKeyId = 4;
            wooden_chest.mStartingRoom = gpg_56;
            wooden_chest.mStartingArea = newbieArea;
            wooden_chest.mName = "wooden chest";         
            addMob(wooden_chest, gpg_56, newbieArea);

            Mob basic_key = new Mob(mFullMobList[(int)MOBLIST.BASIC_KEY]);
            basic_key.mKeyId = 4;
            basic_key.mName = "brass key";
            basic_key.mStartingRoom = gpg_70;
            basic_key.mStartingArea = newbieArea;
            basic_key.mStartingArea = basic_key.mCurrentArea = null;

            Container small_metal_cage = new Container((Container)mFullMobList[(int)MOBLIST.BASIC_CHEST]);
            small_metal_cage.mName = "small metal cage";
            small_metal_cage.mDescription = "a small metal cage, I wonder what is inside?";
            small_metal_cage.mKeyId = 5;
            small_metal_cage.mStartingRoom = gpg_71;
            small_metal_cage.mStartingArea = newbieArea;
            small_metal_cage.mInventory.Add(basic_key);
            addMob(small_metal_cage, gpg_71, newbieArea);

            Mob steel_key = new Mob(mFullMobList[(int)MOBLIST.BASIC_KEY]);
            steel_key.mFlagList.Add(MobFlags.FLAG_HIDDEN);
            steel_key.mKeyId = 5;
            steel_key.mStartingRoom = gpg_46;
            steel_key.mStartingArea = newbieArea;
            steel_key.mName = "small steel key";
            steel_key.mDescription = "a small steel key, I wonder what it opens?";
            addMob(steel_key, gpg_46, newbieArea);

            Mob basic_switch = new Mob(mFullMobList[(int)MOBLIST.SWITCH]);
            basic_switch.mStartingRoom = gpg_29;
            basic_switch.mStartingArea = newbieArea;
            EventData ed = new EventData();
            ed.data = AreaID.AID_NEWBIEAREA;
            ed.eventFlag = EventFlag.EVENT_GPG_WALL_REMOVE;
            ed.commandName = commandName.COMMAND_USE;
            basic_switch.mEventList.Add(ed);
            addMob(basic_switch, basic_switch.mStartingRoom, basic_switch.mStartingArea);

            basic_switch = new Mob(mFullMobList[(int)MOBLIST.SWITCH]);
            basic_switch.mStartingRoom = gpg_37;
            basic_switch.mStartingArea = newbieArea;
            ed = new EventData();
            ed.data = AreaID.AID_NEWBIEAREA;
            ed.eventFlag = EventFlag.EVENT_GPG_WALL_REMOVE;
            ed.commandName = commandName.COMMAND_USE;
            basic_switch.mEventList.Add(ed);
            addMob(basic_switch, basic_switch.mStartingRoom, basic_switch.mStartingArea);

            Equipment rusty_sword = new Equipment((Equipment)mFullMobList[(int)MOBLIST.BASIC_SWORD]);
            addMob(rusty_sword, gpg_playerStart, newbieArea);

            CombatMob goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_6, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_10, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_17, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_21, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_25, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_29, newbieArea);    

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_32, newbieArea);
            
            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_42, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_47, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_62, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_77, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_60, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_37, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_46, newbieArea);

            goblin_runt2 = new CombatMob((CombatMob)mFullMobList[(int)MOBLIST.GOBLIN_RUNT]);
            addMob(goblin_runt2, gpg_58, newbieArea);

            Mob newMob = mProtoMgr.cloneMob(MOBLIST.GOBLIN_RUNT, gpg_playerStart, "testgoblin");

            ed = new EventData();
            ed.data = AreaID.AID_NEWBIEAREA;
            ed.eventFlag = EventFlag.EVENT_GPG_WALL_ADD;
            ed.commandName = commandName.COMMAND_USE;
            newbieArea.mRevertList.Add(ed);

            mAreaHandler.registerArea(newbieArea);
        }// addNewbieArea

    }// class World

}// Namespace _8th_Circle_Server

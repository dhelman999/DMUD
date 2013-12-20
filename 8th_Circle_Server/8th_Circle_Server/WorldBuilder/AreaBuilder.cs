using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Area geraldineArea = new Area();
            geraldineArea.mName = "Geraldine Estate";
            geraldineArea.mDescription = "The residence of the esteemed Renee and David";
            geraldineArea.mWorld = this;

            Room house1stentranceway = new Room("The entrance to the Geraldine Manor, there are stairs " +
                "leading up.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET, RoomID.GERALD_1ST_ENT);
            house1stentranceway.mCurrentArea = geraldineArea;
            Room house1stHallway = new Room("The west hallway is empty besides a few pictures",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET, RoomID.GERALD_1ST_HALLWAY);
            house1stHallway.mCurrentArea = geraldineArea;
            Room house1stKitchen = new Room("The kitchen has a nice view of the outside to the west; " +
                "there are also stairs leading down and a doorway to the north",
                HOUSE_OFFSET - 2, HOUSE_OFFSET, HOUSE_OFFSET, RoomID.GERALD_1ST_KITCHEN);
            house1stKitchen.mCurrentArea = geraldineArea;
            Room house1stDiningRoom = new Room("The dining room is blue with various pictures; one is " +
                "particularly interesting, featuring a type of chicken",
                HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, HOUSE_OFFSET, RoomID.GERALD_1ST_DININGROOM);
            house1stDiningRoom.mCurrentArea = geraldineArea;
            Room house1stLivingRoom = new Room("The living room is grey with a nice flatscreen tv along " +
                "the north wall",
                HOUSE_OFFSET, HOUSE_OFFSET - 1, HOUSE_OFFSET, RoomID.GERALD_1ST_LIVINGROOM);
            house1stLivingRoom.mCurrentArea = geraldineArea;
            Room house1stBathroom = new Room("The powder room is a nice small comfortable bathroom with " +
                "a sink and toilet",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET, RoomID.GERALD_2ND_BATHROOM);
            house1stBathroom.mCurrentArea = geraldineArea;

            Room house2ndHallway = new Room("The hallway to the 2nd floor.  This is a long corridor\n with " +
                "many rooms attached to it with stairs leading down at the base.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_HALLWAY);
            house2ndHallway.mCurrentArea = geraldineArea;
            Room house2ndKittyroom = new Room("The kittyroom, there is not much here besides some litterboxes.",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_KITTYROOM);
            house2ndKittyroom.mCurrentArea = geraldineArea;
            Room house2ndKittyCloset = new Room("The closet of the kittyroom holds various appliances such as " +
                "vaccuums and other cleaning supplies",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_KITTYCLOSET);
            house2ndKittyCloset.mCurrentArea = geraldineArea;
            Room house2ndBathroom = new Room("A small master bathroom has a sink, shower and toilet",
                HOUSE_OFFSET + 1, HOUSE_OFFSET, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_BATHROOM);
            house2ndBathroom.mCurrentArea = geraldineArea;
            Room house2ndBedroom = new Room("The master bedroom is huge with two sliding door closets\n and " +
                "windows on the north and northwest sides",
                HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_BEDROOM);
            house2ndBedroom.mCurrentArea = geraldineArea;
            Room house2ndBlueroom = new Room("The blueroom has a large bookshelf, a sliding door closet and " +
                "a loveseat",
                HOUSE_OFFSET, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, RoomID.GERALD_2ND_BLUEROOM);
            house2ndBlueroom.mCurrentArea = geraldineArea;

            Room houseBaseentrance = new Room("The bottom of the stairs leads to the basement.\n This " +
                " is a large basement that spans to the south with \nrooms attached on both sides.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART1);
            houseBaseentrance.mCurrentArea = geraldineArea;
            Room houseBasepart2 = new Room("There is a piano here along the wall with light grey\n " +
                "carpet with the walls being a darker grey",
                HOUSE_OFFSET, HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART2);
            houseBasepart2.mCurrentArea = geraldineArea;
            Room houseBasepart3 = new Room("There isn't much to this piece of the basement besides\n " +
                "some pictures on both the west and east walls.",
                HOUSE_OFFSET, HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART3);
            houseBasepart3.mCurrentArea = geraldineArea;
            Room houseBasepart4 = new Room("You have reached the southern corner of the basement.\n " +
                "There is a computer desk here with a glowing PC and monitor.  There are all\n " +
                "sorts of figurines of wonderous power sitting on the desk along with pictures\n " +
                "depicting awesome scenes of wonder and adventure.  Something about this room\n " +
                "seems filled with some sort of power.",
                HOUSE_OFFSET, HOUSE_OFFSET - 3, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART4);
            houseBasepart4.mCurrentArea = geraldineArea;
            Room houseBasepart5 = new Room("The southwest most edge of the basement, there is a\n " +
                "couch on the south end of the wall facing a TV in the corner beside a doorway",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 3, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_PART5);
            houseBasepart5.mCurrentArea = geraldineArea;
            Room houseBaseBathroom = new Room("The bathroom has a standing shower as well as a long \n " +
                "vanity with an accompanying toilet",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_BATHROOM);
            houseBaseBathroom.mCurrentArea = geraldineArea;
            Room houseBaseCloset = new Room("This is a closet that has large holding shelves with\n " +
                "board games from top to bottom.  This is an impressive collection indeed!",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_CLOSET);
            houseBaseCloset.mCurrentArea = geraldineArea;
            Room houseBaseLaundryRoom = new Room("The laundry room has no carpet and has many shelves\n " +
                "with various pieces of hardware and tools",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET - 1, RoomID.GERALD_BASE_LAUNDRYROOM);
            houseBaseLaundryRoom.mCurrentArea = geraldineArea;
            Room houseBaseSumpRoom = new Room("The sump pump room is bare concrete with a few shelves\n " +
                "for storage",
                HOUSE_OFFSET, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, RoomID.GERALD_BASE_SUMPROOM);
            houseBaseSumpRoom.mCurrentArea = geraldineArea;

            house2ndHallway.mCurrentArea = geraldineArea;
            house1stentranceway.mWestLink = house1stHallway;
            house1stentranceway.mSouthLink = house1stLivingRoom;
            house1stHallway.mEastLink = house1stentranceway;
            house1stHallway.mSouthLink = house1stBathroom;
            house1stHallway.mWestLink = house1stKitchen;
            house1stKitchen.mEastLink = house1stHallway;
            house1stKitchen.mSouthLink = house1stDiningRoom;
            house1stDiningRoom.mNorthLink = house1stKitchen;
            house1stDiningRoom.mEastLink = house1stLivingRoom;
            house1stLivingRoom.mWestLink = house1stDiningRoom;
            house1stLivingRoom.mNorthLink = house1stentranceway;
            house1stBathroom.mNorthLink = house1stHallway;

            Doorway newDoor = new Doorway("door", house1stBathroom);
            house1stBathroom.addDoor(newDoor, Direction.NORTH);
            house1stHallway.addDoor(newDoor, Direction.SOUTH);

            newDoor = new Doorway("door", house1stKitchen);
            house1stKitchen.addDoor(newDoor, Direction.DOWN);
            houseBaseentrance.addDoor(newDoor, Direction.UP);

            house1stentranceway.mUpLink = house2ndHallway;
            house2ndHallway.mDownLink = house1stentranceway;
            house2ndHallway.mEastLink = house2ndKittyroom;
            house2ndHallway.mWestLink = house2ndBathroom;
            house2ndHallway.mSouthLink = house2ndBlueroom;
            house2ndHallway.mSouthwestLink = house2ndBedroom;
            house2ndKittyroom.mNorthLink = house2ndKittyCloset;
            house2ndKittyroom.mWestLink = house2ndHallway;
            house2ndKittyCloset.mSouthLink = house2ndKittyroom;
            house2ndBathroom.mEastLink = house2ndHallway;
            house2ndBathroom.mSouthLink = house2ndBedroom;
            house2ndBedroom.mNortheastLink = house2ndHallway;
            house2ndBedroom.mNorthLink = house2ndBathroom;
            house2ndBlueroom.mNorthLink = house2ndHallway;

            newDoor = new Doorway("door", house2ndHallway);
            house2ndHallway.addDoor(newDoor, Direction.WEST);
            house2ndBathroom.addDoor(newDoor, Direction.EAST);

            newDoor = new Doorway("door", house2ndHallway);
            house2ndHallway.addDoor(newDoor, Direction.EAST);
            house2ndKittyroom.addDoor(newDoor, Direction.WEST);

            newDoor = new Doorway("door", house2ndHallway);
            house2ndHallway.addDoor(newDoor, Direction.SOUTH);
            house2ndBlueroom.addDoor(newDoor, Direction.NORTH);

            newDoor = new Doorway("door", house2ndHallway);
            house2ndHallway.addDoor(newDoor, Direction.SOUTHWEST);
            house2ndBedroom.addDoor(newDoor, Direction.NORTHEAST);

            newDoor = new Doorway("door", house2ndBathroom);
            house2ndBathroom.addDoor(newDoor, Direction.SOUTH);
            house2ndBedroom.addDoor(newDoor, Direction.NORTH);

            houseBaseentrance.mUpLink = house1stKitchen;
            house1stKitchen.mDownLink = houseBaseentrance;
            houseBaseentrance.mSouthLink = houseBasepart2;
            houseBaseentrance.mWestLink = houseBaseLaundryRoom;
            houseBaseLaundryRoom.mEastLink = houseBaseentrance;
            houseBasepart2.mNorthLink = houseBaseentrance;
            houseBasepart2.mWestLink = houseBaseCloset;
            houseBasepart2.mEastLink = houseBaseSumpRoom;
            houseBasepart2.mSouthLink = houseBasepart3;
            houseBaseSumpRoom.mWestLink = houseBasepart2;
            houseBaseCloset.mEastLink = houseBasepart2;
            houseBasepart3.mNorthLink = houseBasepart2;
            houseBasepart3.mSouthLink = houseBasepart4;
            houseBasepart4.mNorthLink = houseBasepart3;
            houseBasepart4.mWestLink = houseBasepart5;
            houseBasepart5.mEastLink = houseBasepart4;
            houseBasepart5.mNorthLink = houseBaseBathroom;
            houseBaseBathroom.mSouthLink = houseBasepart5;

            newDoor = new Doorway("door", houseBaseentrance);
            houseBaseentrance.addDoor(newDoor, Direction.WEST);
            houseBaseLaundryRoom.addDoor(newDoor, Direction.EAST);

            newDoor = new Doorway("door", houseBasepart2);
            houseBasepart2.addDoor(newDoor, Direction.WEST);
            houseBaseCloset.addDoor(newDoor, Direction.EAST);

            newDoor = new Doorway("door", houseBasepart2);
            houseBasepart2.addDoor(newDoor, Direction.EAST);
            houseBaseSumpRoom.addDoor(newDoor, Direction.WEST);

            newDoor = new Doorway("door", houseBaseBathroom);
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

            getRoom(0, 0, 0).mWestLink = house1stentranceway;

            addGeraldineNpcs(geraldineArea);
            geraldineArea.mAreaID = AreaID.AID_GERALDINEMANOR;
            mAreaList.Add(geraldineArea);
            mAreaHandler.registerArea(geraldineArea);
        }// geraldineArea

        public void addGeraldineNpcs(Area area)
        {
            addMob(MOBLIST.MAX, area.getRoom(RoomID.GERALD_1ST_LIVINGROOM), area);
            addMob(MOBLIST.FIRST_CIRCLE, area.getRoom(RoomID.GERALD_BASE_PART4), area);
        }// addNpcs

        public void addNewbieArea()
        {
            Area newbieArea = new Area();
            newbieArea.mName = "Goblin Prooving Grounds";
            newbieArea.mDescription = "This area is the leftover pits where unworthy goblins were abandoned " +
             "and forgotten.  The runts here were unworthy to serve any useful purpose in goblin society " +
             "and were cast out until they prooved themselves";
            newbieArea.mWorld = this;
            newbieArea.mAreaID = AreaID.AID_NEWBIEAREA;

            Room gpg_playerStart = new Room("You find yourself in some sort of dark plains.  It spans in " +
                "every direction, although you see some walls to your west",
                BAO, BAO, BAO, RoomID.GPG_PLAYER_START, newbieArea);
            gpg_playerStart.mCurrentArea = newbieArea;
            Room gpg_1 = new Room("GPG 1",
                BAO-1, BAO+3, BAO, RoomID.GPG_ROOM_1, newbieArea);
            Room gpg_2 = new Room("GPG 2",
                BAO, BAO+3, BAO, RoomID.GPG_ROOM_2, newbieArea);
            Room gpg_3 = new Room("GPG 3",
                BAO+1, BAO+3, BAO, RoomID.GPG_ROOM_3, newbieArea);
            Room gpg_4 = new Room("GPG 4",
                BAO+2, BAO+3, BAO, RoomID.GPG_ROOM_4, newbieArea);
            Room gpg_5 = new Room("GPG 5",
                BAO+3, BAO+3, BAO, RoomID.GPG_ROOM_5, newbieArea);
            Room gpg_6 = new Room("GPG 6",
                BAO+4, BAO+3, BAO, RoomID.GPG_ROOM_6, newbieArea);
            Room gpg_7 = new Room("GPG 7",
                BAO-1, BAO+2, BAO, RoomID.GPG_ROOM_7, newbieArea);
            Room gpg_8 = new Room("GPG 8",
                BAO, BAO+2, BAO, RoomID.GPG_ROOM_8, newbieArea);
            Room gpg_9 = new Room("GPG 9",
                BAO+1, BAO+2, BAO, RoomID.GPG_ROOM_9, newbieArea);
            Room gpg_10 = new Room("GPG 10",
                BAO+2, BAO+2, BAO, RoomID.GPG_ROOM_10, newbieArea);
            Room gpg_11 = new Room("GPG 11",
                BAO+3, BAO+2, BAO, RoomID.GPG_ROOM_11, newbieArea);
            Room gpg_12 = new Room("GPG 12",
                BAO+4, BAO+2, BAO, RoomID.GPG_ROOM_12, newbieArea);
            Room gpg_13 = new Room("GPG 13",
                BAO-1, BAO+1, BAO, RoomID.GPG_ROOM_13, newbieArea);
            Room gpg_14 = new Room("GPG 14",
                BAO, BAO+1, BAO, RoomID.GPG_ROOM_14, newbieArea);
            Room gpg_15 = new Room("GPG 15",
                BAO+1, BAO+1, BAO, RoomID.GPG_ROOM_15, newbieArea);
            Room gpg_16 = new Room("GPG 16",
                BAO+2, BAO+1, BAO, RoomID.GPG_ROOM_16, newbieArea);
            Room gpg_17 = new Room("GPG 17",
                BAO+3, BAO+1, BAO, RoomID.GPG_ROOM_17, newbieArea);
            Room gpg_18 = new Room("GPG 18",
                BAO+4, BAO+1, BAO, RoomID.GPG_ROOM_18, newbieArea);
            Room gpg_19 = new Room("GPG 19",
                BAO-1, BAO, BAO, RoomID.GPG_ROOM_19, newbieArea);
            Room gpg_21 = new Room("GPG 21",
                BAO+1, BAO, BAO, RoomID.GPG_ROOM_21, newbieArea);
            Room gpg_22 = new Room("GPG 22",
                BAO+2, BAO, BAO, RoomID.GPG_ROOM_22, newbieArea);
            Room gpg_23 = new Room("GPG 23",
                BAO+3, BAO, BAO, RoomID.GPG_ROOM_23, newbieArea);
            Room gpg_24 = new Room("GPG 24",
                BAO+4, BAO, BAO, RoomID.GPG_ROOM_24, newbieArea);
            Room gpg_25 = new Room("GPG 25",
                BAO-1, BAO-1, BAO, RoomID.GPG_ROOM_25, newbieArea);
            Room gpg_26 = new Room("GPG 26",
                BAO, BAO-1, BAO, RoomID.GPG_ROOM_26, newbieArea);
            Room gpg_27 = new Room("GPG 27",
                BAO+1, BAO-1, BAO, RoomID.GPG_ROOM_27, newbieArea);
            Room gpg_28 = new Room("GPG 28",
                BAO+2, BAO-1, BAO, RoomID.GPG_ROOM_28, newbieArea);
            Room gpg_29 = new Room("GPG 29",
                BAO+3, BAO-1, BAO, RoomID.GPG_ROOM_29, newbieArea);
            Room gpg_30 = new Room("GPG 30",
                BAO+4, BAO-1, BAO, RoomID.GPG_ROOM_30, newbieArea);
            Room gpg_31 = new Room("GPG 31",
                BAO-1, BAO-2, BAO, RoomID.GPG_ROOM_31, newbieArea);
            Room gpg_32 = new Room("GPG 32",
                BAO, BAO-2, BAO, RoomID.GPG_ROOM_32, newbieArea);
            Room gpg_33 = new Room("GPG 33",
                BAO+1, BAO-2, BAO, RoomID.GPG_ROOM_33, newbieArea);
            Room gpg_34 = new Room("GPG 34",
                BAO+2, BAO-2, BAO, RoomID.GPG_ROOM_34, newbieArea);
            Room gpg_35 = new Room("GPG 35",
                BAO+3, BAO-2, BAO, RoomID.GPG_ROOM_35, newbieArea);
            Room gpg_36 = new Room("GPG 36",
                BAO+4, BAO-2, BAO, RoomID.GPG_ROOM_36, newbieArea);
            Room gpg_37 = new Room("GPG 37",
                BAO - 8, BAO + 3, BAO, RoomID.GPG_ROOM_37, newbieArea);
            Room gpg_38 = new Room("GPG 38",
                BAO-7, BAO + 3, BAO, RoomID.GPG_ROOM_38, newbieArea);
            Room gpg_39 = new Room("GPG 39",
                BAO - 6, BAO + 3, BAO, RoomID.GPG_ROOM_39, newbieArea);
            Room gpg_40 = new Room("GPG 40",
                BAO - 5, BAO + 3, BAO, RoomID.GPG_ROOM_40, newbieArea);
            Room gpg_41 = new Room("GPG 41",
                BAO - 4, BAO + 3, BAO, RoomID.GPG_ROOM_41, newbieArea);
            Room gpg_42 = new Room("GPG 42",
                BAO - 3, BAO + 3, BAO, RoomID.GPG_ROOM_42, newbieArea);
            Room gpg_43 = new Room("GPG 43",
                BAO - 2, BAO + 3, BAO, RoomID.GPG_ROOM_43, newbieArea);
            Room gpg_44 = new Room("GPG 44",
                BAO - 8, BAO + 2, BAO, RoomID.GPG_ROOM_44, newbieArea);
            Room gpg_45 = new Room("GPG 45",
                BAO - 7, BAO + 2, BAO, RoomID.GPG_ROOM_45, newbieArea);
            Room gpg_46 = new Room("GPG 46",
                BAO - 6, BAO + 2, BAO, RoomID.GPG_ROOM_46, newbieArea);
            Room gpg_47 = new Room("GPG 47",
                BAO -5, BAO + 2, BAO, RoomID.GPG_ROOM_47, newbieArea);
            Room gpg_48 = new Room("GPG 48",
                BAO - 4, BAO + 2, BAO, RoomID.GPG_ROOM_48, newbieArea);
            Room gpg_49 = new Room("GPG 49",
                BAO - 3, BAO + 2, BAO, RoomID.GPG_ROOM_49, newbieArea);
            Room gpg_50 = new Room("GPG 50",
                BAO - 2, BAO + 2, BAO, RoomID.GPG_ROOM_50, newbieArea);
            Room gpg_51 = new Room("GPG 51",
                BAO - 8, BAO + 1, BAO, RoomID.GPG_ROOM_51, newbieArea);
            Room gpg_52 = new Room("GPG 52",
                BAO - 7, BAO + 1, BAO, RoomID.GPG_ROOM_52, newbieArea);
            Room gpg_53 = new Room("GPG 53",
                BAO - 6, BAO + 1, BAO, RoomID.GPG_ROOM_53, newbieArea);
            Room gpg_54 = new Room("GPG 54",
                BAO - 5, BAO + 1, BAO, RoomID.GPG_ROOM_54, newbieArea);
            Room gpg_55 = new Room("GPG 55",
                BAO - 4, BAO+1, BAO, RoomID.GPG_ROOM_55, newbieArea);
            Room gpg_56 = new Room("GPG 56",
                BAO - 3, BAO+1, BAO, RoomID.GPG_ROOM_56, newbieArea);
            Room gpg_57 = new Room("GPG 57",
                BAO - 2, BAO+1, BAO, RoomID.GPG_ROOM_57, newbieArea);
            Room gpg_58 = new Room("GPG 58",
                BAO -8, BAO, BAO, RoomID.GPG_ROOM_58, newbieArea);
            Room gpg_59 = new Room("GPG 59",
                BAO -7, BAO, BAO, RoomID.GPG_ROOM_59, newbieArea);
            Room gpg_60 = new Room("GPG 60",
                BAO - 6, BAO, BAO, RoomID.GPG_ROOM_60, newbieArea);
            Room gpg_61 = new Room("GPG 61",
                BAO-5, BAO, BAO, RoomID.GPG_ROOM_61, newbieArea);
            Room gpg_62 = new Room("GPG 62",
                BAO-4, BAO, BAO, RoomID.GPG_ROOM_62, newbieArea);
            Room gpg_63 = new Room("GPG 63",
                BAO -3, BAO, BAO, RoomID.GPG_ROOM_63, newbieArea);
            Room gpg_64 = new Room("GPG 64",
                BAO -2, BAO, BAO, RoomID.GPG_ROOM_64, newbieArea);
            Room gpg_65 = new Room("GPG 65",
                BAO -8, BAO-1, BAO, RoomID.GPG_ROOM_65, newbieArea);
            Room gpg_66 = new Room("GPG 66",
                BAO - 7, BAO - 1, BAO, RoomID.GPG_ROOM_66, newbieArea);
            Room gpg_67 = new Room("GPG 67",
                BAO-6, BAO - 1, BAO, RoomID.GPG_ROOM_67, newbieArea);
            Room gpg_68 = new Room("GPG 68",
                BAO -5, BAO - 1, BAO, RoomID.GPG_ROOM_68, newbieArea);
            Room gpg_69 = new Room("GPG 69",
                BAO -4, BAO - 1, BAO, RoomID.GPG_ROOM_69, newbieArea);
            Room gpg_70 = new Room("GPG 70",
                BAO -3, BAO - 1, BAO, RoomID.GPG_ROOM_70, newbieArea);
            Room gpg_71 = new Room("GPG 71",
                BAO -2, BAO - 1, BAO, RoomID.GPG_ROOM_71, newbieArea);
            Room gpg_72 = new Room("GPG 72",
                BAO - 8, BAO - 2, BAO, RoomID.GPG_ROOM_72, newbieArea);
            Room gpg_73 = new Room("GPG 73",
                BAO - 7, BAO - 2, BAO, RoomID.GPG_ROOM_73, newbieArea);
            Room gpg_74 = new Room("GPG 74",
                BAO - 6, BAO - 2, BAO, RoomID.GPG_ROOM_74, newbieArea);
            Room gpg_75 = new Room("GPG 75",
                BAO - 5, BAO - 2, BAO, RoomID.GPG_ROOM_75, newbieArea);
            Room gpg_76 = new Room("GPG 76",
                BAO - 4, BAO - 2, BAO, RoomID.GPG_ROOM_76, newbieArea);
            Room gpg_77 = new Room("GPG 77",
                BAO - 3, BAO - 2, BAO, RoomID.GPG_ROOM_77, newbieArea);
            Room gpg_78 = new Room("GPG 78",
                BAO - 2, BAO - 2, BAO, RoomID.GPG_ROOM_77, newbieArea);

            mAreaList.Add(newbieArea);

            foreach (Room room in newbieArea.mRoomList)
            {
                Room nwRoom = getRoom(room.mWorldLoc[0] - 1, room.mWorldLoc[1] + 1, 
                    room.mWorldLoc[2], AreaID.AID_NEWBIEAREA);
                Room nRoom = getRoom(room.mWorldLoc[0], room.mWorldLoc[1] + 1,
                    room.mWorldLoc[2], AreaID.AID_NEWBIEAREA);
                Room neRoom = getRoom(room.mWorldLoc[0] + 1, room.mWorldLoc[1] + 1,
                    room.mWorldLoc[2], AreaID.AID_NEWBIEAREA);
                Room wRoom = getRoom(room.mWorldLoc[0] - 1, room.mWorldLoc[1],
                    room.mWorldLoc[2], AreaID.AID_NEWBIEAREA);
                Room eRoom = getRoom(room.mWorldLoc[0] + 1, room.mWorldLoc[1],
                    room.mWorldLoc[2], AreaID.AID_NEWBIEAREA);
                Room swRoom = getRoom(room.mWorldLoc[0] - 1, room.mWorldLoc[1] - 1,
                    room.mWorldLoc[2], AreaID.AID_NEWBIEAREA);
                Room sRoom = getRoom(room.mWorldLoc[0], room.mWorldLoc[1] - 1,
                    room.mWorldLoc[2], AreaID.AID_NEWBIEAREA);
                Room seRoom = getRoom(room.mWorldLoc[0] + 1, room.mWorldLoc[1] - 1,
                    room.mWorldLoc[2], AreaID.AID_NEWBIEAREA);

                if (nwRoom != null)
                {
                    room.mNorthwestLink = nwRoom;
                    nwRoom.mSoutheastLink = room;
                }
                if (nRoom != null)
                {
                    room.mNorthLink = nRoom;
                    nRoom.mSouthLink = room;
                }
                if (neRoom != null)
                {
                    room.mNortheastLink = neRoom;
                    neRoom.mSouthwestLink = room;
                }
                if (wRoom != null)
                {
                    room.mWestLink = wRoom;
                    wRoom.mEastLink = room;
                }
                if (eRoom != null)
                {
                    room.mEastLink = eRoom;
                    eRoom.mWestLink = room;
                }
                if (swRoom != null)
                {
                    room.mSouthwestLink = swRoom;
                    swRoom.mNortheastLink = room;
                }
                if (sRoom != null)
                {
                    room.mSouthLink = sRoom;
                    sRoom.mNorthLink = room;
                }
                if (seRoom != null)
                {
                    room.mSoutheastLink = seRoom;
                    seRoom.mNorthwestLink = room;
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

            addMob(MOBLIST.BASIC_CHEST, gpg_56, newbieArea);
            // Find a way to minimize this
            Mob basic_switch = new Mob((Mob)mFullMobList[(int)MOBLIST.SWITCH]);
            basic_switch.mStartingRoom = gpg_29;
            basic_switch.mStartingArea = newbieArea;
            EventData ed = new EventData();
            ed.data = AreaID.AID_NEWBIEAREA;
            ed.eventFlag = EventFlag.EVENT_GPG_WALL_REMOVE;
            ed.commandName = commandName.COMMAND_USE;
            basic_switch.mEventList.Add(ed);
            addMob(basic_switch, basic_switch.mStartingRoom, basic_switch.mStartingArea);

            mAreaHandler.registerArea(newbieArea);
        }// addNewbieArea

    }// class World

}// Namespace _8th_Circle_Server

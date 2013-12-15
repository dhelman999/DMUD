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
        }// addAreas

        private void addGeraldineArea()
        {
            Area geraldineArea = new Area();
            geraldineArea.mName = "Geraldine Estate";
            geraldineArea.mDescription = "The residence of the esteemed Renee and David";
            geraldineArea.mWorld = this;

            Room house1stentranceway = new Room("The entrance to the Geraldine Manor, there are stairs " +
                "leading up.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET);
            house1stentranceway.mCurrentArea = geraldineArea;
            Room house1stHallway = new Room("The west hallway is empty besides a few pictures",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET);
            house1stHallway.mCurrentArea = geraldineArea;
            Room house1stKitchen = new Room("The kitchen has a nice view of the outside to the west; " +
                "there are also stairs leading down and a doorway to the north",
                HOUSE_OFFSET - 2, HOUSE_OFFSET, HOUSE_OFFSET);
            house1stKitchen.mCurrentArea = geraldineArea;
            Room house1stDiningRoom = new Room("The dining room is blue with various pictures; one is " +
                "particularly interesting, featuring a type of chicken",
                HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, HOUSE_OFFSET);
            house1stDiningRoom.mCurrentArea = geraldineArea;
            Room house1stLivingRoom = new Room("The living room is grey with a nice flatscreen tv along " +
                "the north wall",
                HOUSE_OFFSET, HOUSE_OFFSET - 1, HOUSE_OFFSET);
            house1stLivingRoom.mCurrentArea = geraldineArea;
            Room house1stBathroom = new Room("The powder room is a nice small comfortable bathroom with " +
                "a sink and toilet",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET);
            house1stBathroom.mCurrentArea = geraldineArea;

            Room house2ndHallway = new Room("The hallway to the 2nd floor.  This is a long corridor\n with " +
                "many rooms attached to it with stairs leading down at the base.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET + 1);
            house2ndHallway.mCurrentArea = geraldineArea;
            Room house2ndKittyroom = new Room("The kittyroom, there is not much here besides some litterboxes.",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET + 1);
            house2ndKittyroom.mCurrentArea = geraldineArea;
            Room house2ndKittyCloset = new Room("The closet of the kittyroom holds various appliances such as " +
                "vaccuums and other cleaning supplies",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET + 1);
            house2ndKittyCloset.mCurrentArea = geraldineArea;
            Room house2ndBathroom = new Room("A small master bathroom has a sink, shower and toilet",
                HOUSE_OFFSET + 1, HOUSE_OFFSET, HOUSE_OFFSET + 1);
            house2ndBathroom.mCurrentArea = geraldineArea;
            Room house2ndBedroom = new Room("The master bedroom is huge with two sliding door closets\n and " +
                "windows on the north and northwest sides",
                HOUSE_OFFSET + 1, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1);
            house2ndBedroom.mCurrentArea = geraldineArea;
            Room house2ndBlueroom = new Room("The blueroom has a large bookshelf, a sliding door closet and " +
                "a loveseat",
                HOUSE_OFFSET, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1);
            house2ndBlueroom.mCurrentArea = geraldineArea;

            Room houseBaseentrance = new Room("The bottom of the stairs leads to the basement.\n This " +
                " is a large basement that spans to the south with \nrooms attached on both sides.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET - 1);
            houseBaseentrance.mCurrentArea = geraldineArea;
            Room houseBasepart2 = new Room("There is a piano here along the wall with light grey\n " +
                "carpet with the walls being a darker grey",
                HOUSE_OFFSET, HOUSE_OFFSET - 1, HOUSE_OFFSET - 1);
            houseBasepart2.mCurrentArea = geraldineArea;
            Room houseBasepart3 = new Room("There isn't much to this piece of the basement besides\n " +
                "some pictures on both the west and east walls.",
                HOUSE_OFFSET, HOUSE_OFFSET - 2, HOUSE_OFFSET - 1);
            houseBasepart3.mCurrentArea = geraldineArea;
            Room houseBasepart4 = new Room("You have reached the southern corner of the basement.\n " +
                "There is a computer desk here with a glowing PC and monitor.  There are all\n " +
                "sorts of figurines of wonderous power sitting on the desk along with pictures\n " +
                "depicting awesome scenes of wonder and adventure.  Something about this room\n " +
                "seems filled with some sort of power.",
                HOUSE_OFFSET, HOUSE_OFFSET - 3, HOUSE_OFFSET - 1);
            houseBasepart4.mCurrentArea = geraldineArea;
            Room houseBasepart5 = new Room("The southwest most edge of the basement, there is a\n " +
                "couch on the south end of the wall facing a TV in the corner beside a doorway",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 3, HOUSE_OFFSET - 1);
            houseBasepart5.mCurrentArea = geraldineArea;
            Room houseBaseBathroom = new Room("The bathroom has a standing shower as well as a long \n " +
                "vanity with an accompanying toilet",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 2, HOUSE_OFFSET - 1);
            houseBaseBathroom.mCurrentArea = geraldineArea;
            Room houseBaseCloset = new Room("This is a closet that has large holding shelves with\n " +
                "board games from top to bottom.  This is an impressive collection indeed!",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET - 1);
            houseBaseCloset.mCurrentArea = geraldineArea;
            Room houseBaseLaundryRoom = new Room("The laundry room has no carpet and has many shelves\n " +
                "with various pieces of hardware and tools",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET - 1);
            houseBaseLaundryRoom.mCurrentArea = geraldineArea;
            Room houseBaseSumpRoom = new Room("The sump pump room is bare concrete with a few shelves\n " +
                "for storage",
                HOUSE_OFFSET, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1);
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

            addNpcs(geraldineArea);

            mAreaList.Add(geraldineArea);
            mAreaHandler.registerArea(geraldineArea);
        }// geraldineArea

        public void addNpcs(Area area)
        {
            addMob(MOBLIST.MAX, (Room)area.mRoomList[0], area);
        }// addNpcs

    }// class World

}// Namespace _8th_Circle_Server

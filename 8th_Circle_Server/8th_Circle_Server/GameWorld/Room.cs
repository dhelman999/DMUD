using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class Room : ResourceHandler
    {
        private RoomID mRoomID;
        private int []mAreaLoc;
        private Area mCurrentArea;
        private List<Room> mRoomLinks;

        public Room() : base()
        {
            mAreaLoc = new int[3];
            mRoomLinks = new List<Room>();

            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                getRes(ResType.DOORWAY).Add(null);
                mRoomLinks.Add(null);
            }               
        }// Constructor

        public Room(String desc, int xCoord, int yCoord, int zCoord, RoomID roomID, Area area) : base()
        {
            mDescription = desc;
            mAreaLoc = new int[3];
            mRoomLinks = new List<Room>();

            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                getRes(ResType.DOORWAY).Add(null);
                mRoomLinks.Add(null);
            }      

            mAreaLoc[0] = xCoord;
            mAreaLoc[1] = yCoord;
            mAreaLoc[2] = zCoord;
            mRoomID = roomID;
            mCurrentArea = area;
            area.RegisterRoom(mRoomID, this);
        }// Constructor

        public errorCode viewed(ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            clientString = exitString();

            return eCode;
        }// viewed

        public errorCode viewed(String direction, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;
            Room remoteRoom = null;
            bool validRoom = false;
            Direction dir = Mob.DirStrToEnum(direction);

            if (mRoomLinks[(int)dir] != null)
            {
                remoteRoom = mRoomLinks[(int)dir];
                validRoom = true;
            }

            if(validRoom)
            {
                clientString = remoteRoom.exitString();
                eCode = errorCode.E_OK;
            }
            else
                clientString = "you can't look that way";

            return eCode;
        }// viewed

        public String exitString()
        {
            String exitStr = String.Empty;
            String resourceString = String.Empty;

            exitStr += AddExitStrings();

            exitStr += "\n";

            resourceString = Utils.printResources(this, ResType.DOORWAY);

            exitStr += resourceString + "\n";

            exitStr += "Objects: ";

            resourceString = Utils.printResources(this, ResType.OBJECT);

            if (resourceString != String.Empty)
                exitStr += resourceString;
            else
                exitStr += "\n";

            exitStr += "Npcs: ";

            resourceString = Utils.printResources(this, ResType.NPC);

            if (resourceString != String.Empty)
                exitStr += resourceString;
            else
                exitStr += "\n";

            exitStr += "Players: ";

            resourceString = Utils.printResources(this, ResType.PLAYER);

            if (resourceString != String.Empty)
                exitStr += resourceString;
            else
                exitStr += "\n";         

            return exitStr;
        }// exitString

        public void addMobResource(Mob newMob)
        {
            // Remove old references
            if (newMob.GetCurrentRoom() != null && newMob.GetCurrentArea() != null)
            {
                // Allow duplicates to be dropped in the same room
                if(!(getRes(newMob.GetResType()).Contains(newMob)))
                {
                    newMob.GetCurrentArea().removeRes(newMob);
                    newMob.GetCurrentRoom().removeRes(newMob);
                }
            }

            // Add new references
            newMob.SetStartingArea(mCurrentArea);
            newMob.SetCurrentArea(mCurrentArea);
            newMob.SetStartingRoom(this);
            newMob.SetCurrentRoom(this);

            // Add resources
            addRes(newMob);
            newMob.GetCurrentArea().addRes(newMob);
            newMob.GetWorld().addRes(newMob);
        }// addMobResource

        public void addDoor(Doorway door, Direction dir)
        {
            getRes(ResType.DOORWAY)[(int)dir] = door;
            door.GetRoomList()[(int)dir] = this;
        }// addDoor

        public void respawnDoorways()
        {
            List<Mob> doorways = getRes(ResType.DOORWAY);

            for (int i = 0; i < doorways.Count; ++i)
            {
                Doorway dw = (Doorway)doorways[i];

                if (dw != null)
                    dw.reset();
            }// for
        }// respawnDoorways

        public String getDoorString(Doorway door)
        {
            Direction dir;

            for (dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                if(getRes(ResType.DOORWAY)[(int)dir] != null && getRes(ResType.DOORWAY)[(int)dir].Equals(door))
                   break;
            }// for

            return dir.ToString().ToLower() + " " + door.GetName();
        }// getDoorString

        public String AddExitStrings()
        {
            String exitStr = mDescription + "\n" + "Exits: ";

            if (mRoomLinks[(int)Direction.NORTH] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.NORTH] == null ||
                (getRes(ResType.DOORWAY)[(int)Direction.NORTH]).HasFlag(MobFlags.OPEN)))
                exitStr += "North ";

            if (mRoomLinks[(int)Direction.SOUTH] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTH] == null ||
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTH]).HasFlag(MobFlags.OPEN)))
                exitStr += "South ";

            if (mRoomLinks[(int)Direction.EAST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.EAST] == null ||
                (getRes(ResType.DOORWAY)[(int)Direction.EAST]).HasFlag(MobFlags.OPEN)))
                exitStr += "East ";

            if (mRoomLinks[(int)Direction.WEST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.WEST] == null ||
                (getRes(ResType.DOORWAY)[(int)Direction.WEST]).HasFlag(MobFlags.OPEN)))
                exitStr += "West ";

            if (mRoomLinks[(int)Direction.UP] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.UP] == null ||
                (getRes(ResType.DOORWAY)[(int)Direction.UP]).HasFlag(MobFlags.OPEN)))
                exitStr += "Up ";

            if (mRoomLinks[(int)Direction.DOWN] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.DOWN] == null ||
                (getRes(ResType.DOORWAY)[(int)Direction.DOWN]).HasFlag(MobFlags.OPEN)))
                exitStr += "Down ";

            if (mRoomLinks[(int)Direction.NORTHWEST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.NORTHWEST] == null ||
                (getRes(ResType.DOORWAY)[(int)Direction.NORTHWEST]).HasFlag(MobFlags.OPEN)))
                exitStr += "Northwest ";

            if (mRoomLinks[(int)Direction.NORTHEAST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.NORTHEAST] == null ||
                (getRes(ResType.DOORWAY)[(int)Direction.NORTHEAST]).HasFlag(MobFlags.OPEN)))
                exitStr += "Northeast ";

            if (mRoomLinks[(int)Direction.SOUTHWEST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTHWEST] == null ||
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTHWEST]).HasFlag(MobFlags.OPEN)))
                exitStr += "Southwest ";

            if (mRoomLinks[(int)Direction.SOUTHEAST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTHEAST] == null ||
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTHEAST]).HasFlag(MobFlags.OPEN)))
                exitStr += "Southeast ";

            return exitStr;
        }
        public void removeDualLinks(Direction dir)
        {
            if(mRoomLinks[(int)dir] != null)
            {
                Direction oppositeDir = (Direction)(((int)dir + 5) % 10);
                Room linkedRoom = mRoomLinks[(int)dir];

                if (linkedRoom != null && linkedRoom.mRoomLinks[(int)oppositeDir] != null)
                    linkedRoom.mRoomLinks[(int)oppositeDir] = null;

                mRoomLinks[(int)dir] = null;
            }// if
        }// removeDualLinks

        public void removeTripleLinks(Direction dir)
        {
            removeDualLinks((Direction)(((int)dir + 9) % 10));
            removeDualLinks(dir);
            removeDualLinks((Direction)(((int)dir + 1) % 10));
        }// removeTripleLinks

        public void addDualLinks(Room targetRoom, Direction dir)
        {
            if (targetRoom != null)
            {
                Direction oppositeDir = (Direction)(((int)dir + 5) % 10);
                mRoomLinks[(int)dir] = targetRoom;
                targetRoom.mRoomLinks[(int)oppositeDir] = this;
            }
        }// addDualLinks

        // Accessors
        public int[] GetAreaLoc() { return mAreaLoc; }
        public Area GetCurrentArea() { return mCurrentArea;  }
        public List<Room> GetRoomLinks() { return mRoomLinks; }
        
    }// Class Room

}// Namespace _8th_Circle_Server

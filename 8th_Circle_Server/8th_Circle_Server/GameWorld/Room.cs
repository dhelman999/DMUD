using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class Room : ResourceHandler
    {
        // Member Variables
        public RoomID mRoomID;
        public int []mAreaLoc;
        public Area mCurrentArea;
        public List<Room> mRoomLinks;

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

        public Room(string desc) : base()
        {
            mDescription = desc;
            mAreaLoc = new int[3];
            mRoomLinks = new List<Room>();

            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                getRes(ResType.DOORWAY).Add(null);
                mRoomLinks.Add(null);
            }      

            mAreaLoc[0] = mAreaLoc[1] = mAreaLoc[2] = -1;
        }// Constructor

        public Room(string desc, int xCoord, int yCoord, int zCoord, RoomID roomID) : base()
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
        }// Constructor

        public Room(string desc, int xCoord, int yCoord, int zCoord, RoomID roomID, Area area) : base()
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

        public string viewed()
        {
            return exitString();
        }// viewed

        public string viewed(string direction)
        {
            Room remoteRoom = null;
            bool validRoom = false;
            Direction dir = Mob.DirStrToEnum(direction);

            if (mRoomLinks[(int)dir] != null)
            {
                remoteRoom = mRoomLinks[(int)dir];
                validRoom = true;
            }

            if(validRoom)
                return remoteRoom.exitString();

            return "you can't look that way";
        }// viewed

        public string exitString()
        {
            string exitStr = mDescription + "\n" + "Exits: ";
            string tmp = string.Empty;
            int visibleObjects = 0;

            if (mRoomLinks[(int)Direction.NORTH] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.NORTH] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.NORTH]).mIsOpen))
                exitStr += "North ";
            if (mRoomLinks[(int)Direction.SOUTH] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTH] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.SOUTH]).mIsOpen))
                exitStr += "South ";
            if (mRoomLinks[(int)Direction.EAST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.EAST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.EAST]).mIsOpen))
                exitStr += "East ";
            if (mRoomLinks[(int)Direction.WEST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.WEST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.WEST]).mIsOpen))
                exitStr += "West ";
            if (mRoomLinks[(int)Direction.UP] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.UP] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.UP]).mIsOpen))
                exitStr += "Up ";
            if (mRoomLinks[(int)Direction.DOWN] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.DOWN] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.DOWN]).mIsOpen))
                exitStr += "Down ";
            if (mRoomLinks[(int)Direction.NORTHWEST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.NORTHWEST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.NORTHWEST]).mIsOpen))
                exitStr += "Northwest ";
            if (mRoomLinks[(int)Direction.NORTHEAST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.NORTHEAST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.NORTHEAST]).mIsOpen))
                exitStr += "Northeast ";
            if (mRoomLinks[(int)Direction.SOUTHWEST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTHWEST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.SOUTHWEST]).mIsOpen))
                exitStr += "Southwest ";
            if (mRoomLinks[(int)Direction.SOUTHEAST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTHEAST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.SOUTHEAST]).mIsOpen))
                exitStr += "Southeast ";

            exitStr += "\n";

            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                if (getRes(ResType.DOORWAY)[(int)dir] != null)
                {
                    if (!((Doorway)getRes(ResType.DOORWAY)[(int)dir]).mFlagList.Contains(MobFlags.FLAG_HIDDEN))
                    {
                        ++visibleObjects;
                        tmp += dir.ToString().ToLower() + " " + ((Doorway)getRes(ResType.DOORWAY)[(int)dir]).mName + "\n";        
                    }// if
                }// if   
            }// for

            if (visibleObjects != 0)
                exitStr += tmp;

            visibleObjects = 0;
            tmp = string.Empty;

            exitStr += "Objects: ";
            
            // TODO
            // Probably add something like a targetList instead of repeating this
            // multiple times
            for (int i = 0; i < getRes(ResType.OBJECT).Count; ++i)
            {
                if (!(getRes(ResType.OBJECT)[i]).mFlagList.Contains(MobFlags.FLAG_HIDDEN))
                {
                    ++visibleObjects;
                    tmp += (getRes(ResType.OBJECT)[i]).exitString(this) + "\n";
                }// if
            }// for

            if (visibleObjects == 0)
                exitStr += "\n";
            else
                exitStr += tmp;

            tmp = string.Empty;
            exitStr += "Npcs: ";

            if (getRes(ResType.NPC).Count == 0)
                exitStr += "\n";

            for (int i = 0; i < getRes(ResType.NPC).Count; ++i)
            {
                if (!(getRes(ResType.NPC)[i]).mFlagList.Contains(MobFlags.FLAG_HIDDEN))
                    exitStr += (getRes(ResType.NPC)[i]).mName + "\n";
            }// if

            exitStr += "Players: ";

            for (int i = 0; i < getRes(ResType.PLAYER).Count; ++i)
            {
                if (!(getRes(ResType.PLAYER)[i]).mFlagList.Contains(MobFlags.FLAG_HIDDEN))
                    exitStr += (getRes(ResType.PLAYER)[i]).mName + "\n";
            }// if

            exitStr += "\n";

            return exitStr;
        }// exitString

        public void addMobResource(Mob newMob)
        {
            // Remove old references
            if (newMob.mCurrentRoom != null && newMob.mCurrentArea != null)
            {
                // Allow duplicates to be dropped in the same room
                if(!(getRes(newMob.mResType).Contains(newMob)))
                {
                    newMob.mCurrentArea.removeRes(newMob);
                    newMob.mCurrentRoom.removeRes(newMob);
                }
            }

            // Add new references
            newMob.mStartingArea = mCurrentArea;
            newMob.mCurrentArea = mCurrentArea;
            newMob.mStartingRoom = this;
            newMob.mCurrentRoom = this;

            // Add resources
            addRes(newMob);
            newMob.mCurrentArea.addRes(newMob);
            newMob.mWorld.addRes(newMob);
        }// addMobResource

        public void addDoor(Doorway door, Direction dir)
        {
            door.mMemento.registerMemento(door);
            getRes(ResType.DOORWAY)[(int)dir] = door;
            door.mRoomList[(int)dir] = this;
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

        public string getDoorString(Doorway door)
        {
            Direction dir;

            for (dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                if(getRes(ResType.DOORWAY)[(int)dir] != null && getRes(ResType.DOORWAY)[(int)dir].Equals(door))
                   break;
            }// for

            return dir.ToString().ToLower() + " " + door.mName;
        }// getDoorString

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

    }// Class Room

}// Namespace _8th_Circle_Server

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
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.NORTH]).IsOpen()))
                exitStr += "North ";
            if (mRoomLinks[(int)Direction.SOUTH] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTH] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.SOUTH]).IsOpen()))
                exitStr += "South ";
            if (mRoomLinks[(int)Direction.EAST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.EAST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.EAST]).IsOpen()))
                exitStr += "East ";
            if (mRoomLinks[(int)Direction.WEST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.WEST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.WEST]).IsOpen()))
                exitStr += "West ";
            if (mRoomLinks[(int)Direction.UP] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.UP] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.UP]).IsOpen()))
                exitStr += "Up ";
            if (mRoomLinks[(int)Direction.DOWN] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.DOWN] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.DOWN]).IsOpen()))
                exitStr += "Down ";
            if (mRoomLinks[(int)Direction.NORTHWEST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.NORTHWEST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.NORTHWEST]).IsOpen()))
                exitStr += "Northwest ";
            if (mRoomLinks[(int)Direction.NORTHEAST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.NORTHEAST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.NORTHEAST]).IsOpen()))
                exitStr += "Northeast ";
            if (mRoomLinks[(int)Direction.SOUTHWEST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTHWEST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.SOUTHWEST]).IsOpen()))
                exitStr += "Southwest ";
            if (mRoomLinks[(int)Direction.SOUTHEAST] != null &&
                (getRes(ResType.DOORWAY)[(int)Direction.SOUTHEAST] == null ||
                ((Doorway)getRes(ResType.DOORWAY)[(int)Direction.SOUTHEAST]).IsOpen()))
                exitStr += "Southeast ";

            exitStr += "\n";

            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                if (getRes(ResType.DOORWAY)[(int)dir] != null)
                {
                    Doorway currentDoor = (Doorway)getRes(ResType.DOORWAY)[(int)dir];

                    if (!currentDoor.HasFlag(MobFlags.FLAG_HIDDEN))
                    {
                        ++visibleObjects;
                        tmp += dir.ToString().ToLower() + " " + currentDoor.GetName() + "\n";        
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
                Mob currentMob = getRes(ResType.OBJECT)[i];

                if (!currentMob.HasFlag(MobFlags.FLAG_HIDDEN))
                {
                    ++visibleObjects;
                    tmp += currentMob.exitString(this) + "\n";
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
                Mob currentMob = getRes(ResType.NPC)[i];

                if (!currentMob.HasFlag(MobFlags.FLAG_HIDDEN))
                    exitStr += currentMob.GetName() + "\n";
            }// if

            exitStr += "Players: ";

            for (int i = 0; i < getRes(ResType.PLAYER).Count; ++i)
            {
                Mob player = getRes(ResType.PLAYER)[i];

                if (!player.HasFlag(MobFlags.FLAG_HIDDEN))
                    exitStr += player.GetName() + "\n";
            }// if

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
            door.Memento().registerMemento(door);
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

        public string getDoorString(Doorway door)
        {
            Direction dir;

            for (dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                if(getRes(ResType.DOORWAY)[(int)dir] != null && getRes(ResType.DOORWAY)[(int)dir].Equals(door))
                   break;
            }// for

            return dir.ToString().ToLower() + " " + door.GetName();
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

        // Accessors
        public int[] GetAreaLoc() { return mAreaLoc; }
        public Area GetCurrentArea() { return mCurrentArea;  }
        public List<Room> GetRoomLinks() { return mRoomLinks; }

    }// Class Room

}// Namespace _8th_Circle_Server

using System;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class Memento
    {
        private List<Mob> mMementos;

        public Memento(Mob mob)
        {
            mMementos = new List<Mob>();

            for (MementoType i = MementoType.MTYPE_START; i < MementoType.MTYPE_END; ++i)
                mMementos.Add(null);

            registerMemento(mob);
        }// constructor

        public Mob getMemento(MementoType type)
        {
            return mMementos[(int)type];
        }// getMemento

        public void registerMemento(Mob mob)
        {
            if (mob is Container)
                mMementos[(int)MementoType.CONTAINER] = new Container((Container)mob);
            else if (mob is Doorway)
                mMementos[(int)MementoType.DOORWAY] = new Doorway((Doorway)mob);
            else if (mob is Equipment)
                mMementos[(int)MementoType.EQUIPMENT] = mob;
            else
                Console.WriteLine("Error registering memento: %s\n", mob.GetName());
        }// registerMemento

    }// class Memento

}// Namespace _8th_Circle_Server

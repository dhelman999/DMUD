using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Containers : BaseObject
    {
        // Member Variables
        public ArrayList mStorage;

        public Containers() : base()
        {
            mStorage = new ArrayList();
            mStorage.Capacity = 10;
            mPrepList.Add(PrepositionType.PREP_FROM);
            mPrepList.Add(PrepositionType.PREP_IN);
        }// Constructor

    }// Class Containers

}// Namespace _8th_Circle_Server

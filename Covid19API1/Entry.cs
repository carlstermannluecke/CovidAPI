using System;
using System.Collections.Generic;
using System.Text;

namespace Covid19API1
{
    /// <summary>
    /// The instances of the class Entry store the individual entries created by the API class.
    /// They have two timestamps: Time is the timestamp where the entry was last modified.
    /// OriginalTime is the timestamp where the entry was first created.
    /// </summary>
    [Serializable]
    public class Entry
    {
        public long ID { get; set; }//I hope we will never need more space than 2^31 - 1, but just to be on the safe side...
        public State State { get; set; }
        public DateTime Time { get; set; }
        public DateTime OriginalTime { get; set; }
        public Entry()
        {

        }
        public Entry(long id, State state, DateTime time)
        {
            ID = id;
            State = state;
            Time = time;
            OriginalTime = time;
        }
        public override string ToString()
        {
            return "ID: " + ID + ", State: " + State + ", Time of last modification: " + Time + ", Time of first creation: " + OriginalTime;
        }
        public (long id, State state, DateTime time, DateTime originalTime) ToTuple()
        {
            return (ID, State, Time, OriginalTime);
        }
    }
}

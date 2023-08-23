using System;

namespace _3DUI.scripts.scoreboard
{
    public class ScoreBoardEntry : IComparable<ScoreBoardEntry>
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public float Time { get; private set; }

        public ScoreBoardEntry(string id, string name, float time)
        {
            ID = id;
            Name = name;
            Time = time;
        }

        public ScoreBoardEntry(string name, float time) : this(new Guid().ToString(), name, time)
        {
            
        }
        public int CompareTo(ScoreBoardEntry other)
        {
            return this.Time.CompareTo(other.Time);
        }

        public new string ToString()
        {
            return ID + ";" + Name + ";" + Time;
        }
    }
}
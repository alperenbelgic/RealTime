using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchangeMachine
{
    public class Profilable
    {
        public Profilable()
        {
            ProfileInfo.AddConstruction(this);
        }

        ~Profilable()
        {
            ProfileInfo.AddDestruction(this);

        }
    }

    public class ProfileInfo
    {
        static Dictionary<string, ProfileInfo> _Info = new Dictionary<string, ProfileInfo>();


        public string ClassName { get; set; }
        public int Constructed { get; set; }
        public int Destructed { get; set; }

        static object lockObject = new object();

        internal static void AddConstruction(Profilable profilable)
        {
            string className = profilable.GetType().Name;
            lock (lockObject)
            {
                if (!_Info.ContainsKey(className))
                {
                    _Info.Add(className, new ProfileInfo());
                } 
            }

            _Info[className].Constructed++;
        }

        internal static void AddDestruction(Profilable profilable)
        {
            string className = profilable.GetType().Name;

            _Info[className].Destructed++;

        }

        public override string ToString()
        {
            return $"{ClassName}-{Constructed}-{Destructed}";
        }
    }
}

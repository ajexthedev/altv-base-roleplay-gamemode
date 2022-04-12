using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltV.TPlayer
{
    public class TPlayer : Player
    {
        public enum AdminRanks {Oyuncu,Moderator,Admin};
        public int PlayerID { get; set; }
        public String PlayerName { get; set; }
        public long Money { get; set; }
        public int AdminLevel { get; set; }
        public bool IsLogged { get; set; }
        public int Payday { get; set; }
        public int DeathTime { get; set; }
        public TPlayer(ICore server, IntPtr nativePointer, ushort id) : base(server, nativePointer, id)
        {
            Money = 5000;
            AdminLevel = 0;
            IsLogged = false;
            Payday = 60;
            DeathTime = 0;
        }

        public bool IsPlayerAdmin(int alvl)
        {
            return AdminLevel >= alvl;        }

        internal void GetPosition(object x, object y, object z)
        {
            throw new NotImplementedException();
        }
    }
}

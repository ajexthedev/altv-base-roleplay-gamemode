using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.TPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AltV
{
    class Server : Resource
    {
        public override void OnStart()
        {
            Alt.Log("Sunucu baslatildi!");
            //MySQL
            Database.InitConnection();
            //Timer
            Timer paydayTimer = new Timer(OnPaydayTimer, null, 10000, 10000);
            Timer deathTimer = new Timer(PlayerDeadTimer, null, 1000, 1000);
        }

        public static void OnPaydayTimer(object state)
        {
            foreach(TPlayer.TPlayer tplayer in Alt.GetAllPlayers())
            {
                tplayer.Payday--;
                if(tplayer.Payday <= 0)
                {
                    tplayer.Money += 500;
                    tplayer.Payday = 60;
                    Utils.sendNotification(tplayer, "info", "Saatlik maaşın $500 olarak hesabına yatırıldı.");
                    //tplayer.SendChatMessage("{2986cc}[PAYDAY] Saatlik maaşın $500 olarak hesabına yatırıldı.");
                }
            }
        }

        public static void PlayerDeadTimer(object state)
        {
            foreach(TPlayer.TPlayer tplayer in Alt.GetAllPlayers())
            {
                if(tplayer.DeathTime > 0)
                {
                    tplayer.DeathTime--;
                    Utils.sendNotification(tplayer, "info", "Doğmana " + tplayer.DeathTime + " saniye kaldı!");
                }
                else if(tplayer.DeathTime <= 0)
                {
                    tplayer.DeathTime = 0;
                    tplayer.Health = 200;
                    
                }
            }
        }

        public override void OnStop()
        {
            Alt.Log("Sunucu kapatildi!");
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new TPlayerFactory();
        }
    }
}

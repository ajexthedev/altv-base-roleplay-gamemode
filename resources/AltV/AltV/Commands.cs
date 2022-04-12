using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltV
{
    public class Commands : IScript
    {
        [CommandEvent(CommandEventType.CommandNotFound)]
        public void OnCommandNotFound(TPlayer.TPlayer tplayer, string command)
        {
            tplayer.SendChatMessage("{e06666}HATA:{ffffff} " + command + " adlı komut bulunamadı. ({bcbcbc}/yardim{ffffff})");
            return;
        }

        [Command("arac")]
        public void CMD_arac(TPlayer.TPlayer tplayer, string VehicleName, int R = 0, int G = 0, int B = 0)
        {
            if(!tplayer.IsPlayerAdmin((int)TPlayer.TPlayer.AdminRanks.Moderator))
            {
                tplayer.SendChatMessage("{e06666}HATA:{ffffff} Yetersiz yetki.");
                return;
            }
            IVehicle veh = Alt.CreateVehicle(Alt.Hash(VehicleName), new AltV.Net.Data.Position(tplayer.Position.X, tplayer.Position.Y + 1.0f, tplayer.Position.Z), tplayer.Rotation);
            if(veh != null)
            {
                veh.PrimaryColorRgb = new AltV.Net.Data.Rgba((byte)R, (byte)G, (byte)B, 255);
                tplayer.SendChatMessage("{e2b016}BILGI:{ffffff} Aracın başarıyla spawn oldu!");
            }
            else
            {
                tplayer.SendChatMessage("{e06666}HATA:{ffffff} Aracın spawn olamadı!");
            }
        }

        [Command("freezeme")]
        public void CMD_freezeme(TPlayer.TPlayer tplayer, bool freeze)
        {
            tplayer.Emit("freezePlayer", freeze);
            tplayer.SendChatMessage("{e2b016}BILGI:{ffffff} Başarıyla donduruldun.");
        }

        [Command("telexyz")]
        public void CMD_telexyz(TPlayer.TPlayer tplayer, float x, float y, float z)
        {
            if (!tplayer.IsPlayerAdmin((int)TPlayer.TPlayer.AdminRanks.Admin))
            {
                tplayer.SendChatMessage("{e06666}HATA:{ffffff} Yetersiz yetki.");
                Utils.sendNotification(tplayer, "error", "Yetersiz yetki!");
                return;
            }
            AltV.Net.Data.Position position = new AltV.Net.Data.Position(x, y, z + 0.2f);
            tplayer.Position = position;
            tplayer.SendChatMessage("{e2b016}BILGI:{ffffff} Başarıyla X: " + x + " Y: "+ y + " Z: " + z + " koordinatlarına ışınlandın.");
            return;
        }

        [Command("me")]
        public void CMD_me(TPlayer.TPlayer tplayer, String emote)
        {
            tplayer.SendChatMessage("{9a82e1}* " + tplayer.PlayerName + " " + emote);
        }

        [Command("do")]
        public void CMD_do(TPlayer.TPlayer tplayer, String emote)
        {
            tplayer.SendChatMessage("{7ed15a}" + emote + " (( " + tplayer.PlayerName + " ))");
        }
    }
}

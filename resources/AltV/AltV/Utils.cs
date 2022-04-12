using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltV
{
    public class Utils
    {
        public static void sendNotification(TPlayer.TPlayer tplayer, string status, string text)
        {
            tplayer.Emit("sendNotification", status, text);
        }
    }
}

using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltV
{
    public class Events : IScript
    {
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public void OnPlayerConnect(TPlayer.TPlayer tplayer, string reason)
        {
            Alt.Log($"{tplayer.Name} adlı oyuncu sunucuya giriş yaptı.");
            tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
            tplayer.Model = (uint)PedModel.Business01AMM;
            tplayer.GiveWeapon(AltV.Net.Enums.WeaponModel.AdvancedRifle, 90, true);
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void OnPlayerDisconnect(TPlayer.TPlayer tplayer, string reason)
        {
            Alt.Log($"{tplayer.Name} adlı oyuncu sunucudan çıkış yaptı. Sebep: {reason}");
        }

        [ScriptEvent(ScriptEventType.PlayerDead)]
        public void OnPlayerDead(TPlayer.TPlayer tplayer, IEntity attacker, uint weapon)
        {
            tplayer.DeathTime = 60;
        }

        [ClientEvent("Event.Register")]
        public void OnPlayerRegister(TPlayer.TPlayer tplayer, String name, String password)
        {
            if (!Database.DoesAccountAlreadyExists(name))
            {
                if (!tplayer.IsLogged && name.Length > 3 && password.Length > 5)
                {
                    tplayer.PlayerName = name;
                    Database.CreateNewAccount(name, password);
                    tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
                    tplayer.Model = (uint)PedModel.Business01AMM;
                    tplayer.IsLogged = true;
                    tplayer.Emit("CloseLoginHud");
                    tplayer.SendChatMessage("{cc0000}[KAYIT] Başarıyla kayıt oldun!");
                    Utils.sendNotification(tplayer, "info", "İyi eğlenceler!");
                }
            }
            else
            {
                tplayer.Emit("SendErrorMessage", "Zaten böyle bir hesap kayıtlı!");
            }
        }

        [ClientEvent("Event.Login")]
        public void OnPlayerLogin(TPlayer.TPlayer tplayer, String name, String password)
        {
            if(Database.DoesAccountAlreadyExists(name))
            {
                if(!tplayer.IsLogged && name.Length > 3 && password.Length > 5)
                {
                    if(Database.PasswordCheck(name, password))
                    {
                        tplayer.PlayerName = name;
                        Database.LoadAccount(tplayer);
                        tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
                        tplayer.Model = (uint)PedModel.Business01AMM;
                        tplayer.IsLogged = true;
                        tplayer.Emit("CloseLoginHud");
                        tplayer.SendChatMessage("{8fce00}[GIRIS] Başarıyla giriş yaptın!");
                        Utils.sendNotification(tplayer, "info", "İyi eğlenceler!");
                    }
                    else
                    {
                        tplayer.Emit("SendErrorMessage", "Hatalı şifre!");
                    }
                }
                else
                {
                    tplayer.Emit("SendErrorMessage", "Geçersiz şifre, doğrusunu girin.");
                }
            }
            else
            {
                tplayer.Emit("SendErrorMessage", "Böyle bir hesap bulunamadı!");
            }
        }
    }
}

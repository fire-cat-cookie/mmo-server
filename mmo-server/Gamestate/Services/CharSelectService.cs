using mmo_server.Communication;
using mmo_server.Gamestate;
using mmo_server.Debug;
using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using mmo_server.Persistence;
using mmo_shared;

namespace mmo_server.Gamestate
{
    class CharSelectService {
        private readonly Database db;
        private readonly PlayerService players;
        private readonly ZoneService zones;
        private readonly MessageSender messageSender;
        private readonly Config config;
        private readonly SessionService sessionHandler;

        public CharSelectService(Database db, PlayerService players, ZoneService zones, MessageSender messageSender, 
            Config config, SessionService sessionHandler) {
            this.db = db;
            this.players = players;
            this.zones = zones;
            this.messageSender = messageSender;
            this.config = config;
            this.sessionHandler = sessionHandler;

            sessionHandler.PlayerLoggedIn += ShowCharacters;
        }

        private void ShowCharacters(Login login, IPEndPoint source) {
            CharSlotInfo[] charInfoArray;
            uint accountId = players.ByIP[source].AccountId;
            if (db.GetCharacterIds(accountId, out List<uint> charIds)) {
                charInfoArray = new CharSlotInfo[charIds.Count];
                for (int i = 0; i < charIds.Count; i++) {
                    if (db.GetCharacter(charIds[i], out Character character)) {
                        CharSlotInfo info = new CharSlotInfo(character.Name, character.Level, character.Class, character.ZoneId, character.Slot);
                        charInfoArray[i] = info;
                    } else {
                        Debug.Debug.Log("Could not retrieve characters: " + login.Username, Debug.Debug.MessageTypes.Login);
                    }
                }
                messageSender.SendTo(source, new CharSelectInfo(charInfoArray));
            } else {
                Debug.Debug.Log("Could not retrieve characters: " + login.Username, Debug.Debug.MessageTypes.Login);
            }
        }

        public CharSlotInfo GetCharSlotInfo(ActiveCharacter c) {
            return new CharSlotInfo(c.Entity.Name, c.Entity.Level, c.Entity.Class, c.Entity.ZoneId, c.Entity.Slot);
        }

    }
}

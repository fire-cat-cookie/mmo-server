using mmo_server.Communication;
using mmo_server.ControlTower;
using mmo_server.Gamestate;
using mmo_shared;
using mmo_shared.Messages;

namespace mmo_server.Gamestate {
    class BroadcastService {
        PlayerService playerService;
        ZoneService zoneRegistry;
        MessageSender messageSender;

        public BroadcastService(PlayerService players, ZoneService zones, MessageSender messageSender, GameLoop gameLoop) {
            this.playerService = players;
            this.zoneRegistry = zones;
            this.messageSender = messageSender;

            gameLoop.Tick += OnTick;
        }

        private void OnTick(float time) {
            //for every zone:
            //send position updates from all characters in that zone, to all characters in that zone.
            //foreach (Zone zone in zoneRegistry.Zones.Values) {
            //    for (int i=0; i<zone.Characters.Count; i++) {
            //        Character c = zone.Characters[i];
            //        PositionUpdate update = new PositionUpdate(c.AccountId, c.Position, c.Destination, c.Velocity);
            //        DistributeInZone(zone, update);
            //    }
            //}
        }

        public void DistributeGlobally(Message message) {
            var allPlayers = playerService.ByAccountId.Values;
            foreach (Player player in allPlayers) {
                messageSender.SendTo(player.Ip, message);
            }
        }

        public void DistributeInZone(Zone zone, Message message) {
            foreach (ActiveCharacter c in zone.Characters) {
                Player recipient = playerService.FindPlayer(c);
                messageSender.SendTo(recipient.Ip, message);
            }
        }

        public void DistributeNearby(ActiveCharacter c, Message message) {
            Player p = playerService.FindPlayer(c);
            DistributeNearby(p, message);
        }

        public void DistributeNearby(Player sender, Message message) {
            Zone zone = zoneRegistry.Zones[sender.CurrentCharacter.Entity.ZoneId];
            DistributeInZone(zone, message);
        }
    }
}

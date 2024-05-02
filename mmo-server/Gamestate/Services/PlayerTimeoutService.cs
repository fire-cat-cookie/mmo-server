using mmo_server.Communication;
using mmo_server.Gamestate;
using mmo_shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using mmo_shared.Messages;
using mmo_server.ControlTower;

namespace mmo_server.Gamestate {
    class PlayerTimeoutService{
        private PacketPublisher packetPublisher;
        private SessionService sessionService;

        private Dictionary<IPEndPoint, Stopwatch> timers = new Dictionary<IPEndPoint, Stopwatch>(); 

        public PlayerTimeoutService(PacketPublisher packetPublisher, SessionService sessionService, GameLoop gameLoop) {
            this.packetPublisher = packetPublisher;
            this.sessionService = sessionService;

            gameLoop.Tick += Update;
            sessionService.PlayerLoggedIn += OnPlayerLoggedIn;
            sessionService.PlayerLoggedOut += OnPlayerLoggedOut;
        }

        public void ResetTimer(IPEndPoint ip) {
            if (!timers.ContainsKey(ip)) {
                return;
            }
            timers[ip].Restart();
        }

        private void Update(float time) {
            List<IPEndPoint> timedOutPlayers = new List<IPEndPoint>();
            foreach (var ip in timers.Keys) {
                if (timers[ip].ElapsedMilliseconds > 15000f) {
                    timedOutPlayers.Add(ip);
                }
            }
            foreach (var ip in timedOutPlayers) {
                sessionService.Disconnect(ip);
            }
        }

        private void OnPlayerLoggedIn(Login login, IPEndPoint ip) {
            timers[ip] = new Stopwatch();
            timers[ip].Start();
        }

        private void OnPlayerLoggedOut(IPEndPoint ip) {
            timers[ip].Stop();
            timers.Remove(ip);
        }
    }
}

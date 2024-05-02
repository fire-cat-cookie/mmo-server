using mmo_server.Communication;
using mmo_server.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace mmo_server.ControlTower {
    class GameLoop {
        private readonly PacketPublisher packetPublisher;
        private Config config;

        public delegate void TickHandler(float elapsedMilliseconds);
        public event TickHandler Tick = delegate { };

        public GameLoop(PacketPublisher packetPublisher, Config config) {
            this.packetPublisher = packetPublisher;
            this.config = config;
        }

        public void Start() {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true) {
                packetPublisher.Update();
                float time = stopwatch.ElapsedMilliseconds;
                if (time >= 1000 / config.game.TargetTickrate) {
                    Tick(time);
                    stopwatch.Restart();
                }
            }
        }
    }
}

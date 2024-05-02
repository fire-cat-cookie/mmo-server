using mmo_server.ControlTower;
using mmo_server.Persistence;
using mmo_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using mmo_server.Communication;
using mmo_shared.Messages;
using System.Security.Policy;

namespace mmo_server.Gamestate;

internal class MobSpawnService
{
    private readonly BroadcastService broadcastService;
    private readonly ZoneService zoneService;

    public MobSpawnService(GameLoop gameLoop, BroadcastService broadcastService, ZoneService zoneService)
    {
        this.broadcastService = broadcastService;
        this.zoneService = zoneService;
        gameLoop.Tick += Update;
    }

    private void Update(float elapsedTime)
    {
        foreach (Zone zone in zoneService.MobsInZone.Keys)
        {
            if (ServerConfig.Zones.numberOfMobs.ContainsKey(zone.Id))
            {
                while (zoneService.MobsInZone[zone].Count < ServerConfig.Zones.numberOfMobs[zone.Id])
                {
                    SpawnMob(zone);
                }
            }
        }
    }

    private void SpawnMob(Zone zone)
    {
        var mob = new Mob() {
            AttackCooldown = SharedConfig.Characters.baseAttackCooldown,
            MaxHealth = 50,
            CurrentHealth = 50,
            AttackRange = 1,
            Position = new Vector2(
                (float)new Random().NextDouble() * (350 - 50) + 50,
                (float)new Random().NextDouble() * (350 - 50) + 50),
            AttackDamage = 10,
            MoveSpeed = 0.5f
        };
        mob.Destination = mob.Position;
        zoneService.AddActor(zone, mob, out uint actorId);
        broadcastService.DistributeInZone(zone, Converter.CreateMobInfo(mob));
    }
}

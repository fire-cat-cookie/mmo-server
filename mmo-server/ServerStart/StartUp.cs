using mmo_server.Communication;
using mmo_server.Gamestate;
using mmo_server.Debug;
using mmo_server.MessageHandlers;
using mmo_server.Cryptography;
using mmo_server.Persistence;
using mmo_shared;
using mmo_server.ControlTower;
using mmo_server.SkillHandlers;
using SimpleInjector;

namespace mmo_server.ServerStart {
    class StartUp {
        static void Main(string[] args) {

            //register services..
            var container = new Container();
            container.RegisterSingleton<Config>();
            container.RegisterSingleton<PacketEncryption>();
            container.RegisterSingleton<PasswordHashing>();
            container.RegisterSingleton<Database>();
            container.RegisterSingleton<ClientConnector>();
            container.RegisterSingleton<PacketPublisher>();
            container.RegisterSingleton<Serializer>();
            container.RegisterSingleton<MessageSender>();
            container.RegisterSingleton<PacketHandler>();
            container.RegisterSingleton<SkillPublisher>();
            container.RegisterSingleton<PlayerService>();
            container.RegisterSingleton<ZoneService>();
            container.RegisterSingleton<BroadcastService>();
            container.RegisterSingleton<UnitVerificationService>();
            container.RegisterSingleton<MovementService>();
            container.RegisterSingleton<InterruptService>();
            container.RegisterSingleton<HealthService>();
            container.RegisterSingleton<CharacterLoginService>();
            container.RegisterSingleton<SessionService>();
            container.RegisterSingleton<CharSelectService>();
            container.RegisterSingleton<CreateAccountService>();
            container.RegisterSingleton<CharacterCreationService>();
            container.RegisterSingleton<ChatService>();
            container.RegisterSingleton<PlayerTimeoutService>();
            container.RegisterSingleton<AutoAttackService>();
            container.RegisterSingleton<RespawnService>();
            container.RegisterSingleton<CooldownService>();
            container.RegisterSingleton<CollisionService>();
            container.RegisterSingleton<ProjectileService>();
            container.RegisterSingleton<AutoAttackHandler>();
            container.RegisterSingleton<ChatMessageHandler>();
            container.RegisterSingleton<AliveHandler>();
            container.RegisterSingleton<CreateAccountHandler>();
            container.RegisterSingleton<CreateCharacterHandler>();
            container.RegisterSingleton<LoginCharacterHandler>();
            container.RegisterSingleton<MoveCommandHandler>();
            container.RegisterSingleton<SessionHandler>();
            container.RegisterSingleton<RespawnHandler>();
            container.RegisterSingleton<SkillHandler>();
            container.RegisterSingleton<BlinkHandler>();
            container.RegisterSingleton<PewHandler>();
            container.RegisterSingleton<GameLoop>();

            //verify services
            container.Verify();

            //set logger
            var config = container.GetInstance<Config>();
            Debug.Debug.SetInstance(config);

            //server start
            container.GetInstance<ClientConnector>().Start();
            container.GetInstance<PacketHandler>().Start();
            container.GetInstance<GameLoop>().Start();
        }
    }
}

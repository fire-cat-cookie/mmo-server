using mmo_server.Communication;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using mmo_server.Gamestate;
using mmo_server.Debug;
using mmo_server.Cryptography;
using mmo_server.Persistence;
using mmo_shared;
using mmo_shared.Messages;

namespace mmo_server.Gamestate {
    /// <summary>
    /// Handles player login requests and disconnects.
    /// </summary>
    class SessionService{
        private Database db;
        private PlayerService players;
        private ZoneService zones;
        private PasswordHashing passwordHashing;
        private Config config;
        private BroadcastService broadcastService;
        private readonly CharacterLoginService characterLoginService;

        public delegate void LoginEventHandler(Login login, IPEndPoint ip);
        public event LoginEventHandler PlayerLoggedIn = delegate { };
        public delegate void LogoutEventHandler(IPEndPoint ip);
        public event LogoutEventHandler PlayerLoggedOut = delegate { };

        private Dictionary<IPEndPoint, byte[]> sessionIds = new Dictionary<IPEndPoint, byte[]>();

        public SessionService(Database db, PlayerService players, ZoneService zones, PasswordHashing passwordHashing,
            Config config, BroadcastService broadcastService, CharacterLoginService characterLoginService) {
            this.db = db;
            this.players = players;
            this.zones = zones;
            this.passwordHashing = passwordHashing;
            this.config = config;
            this.broadcastService = broadcastService;
            this.characterLoginService = characterLoginService;
        }

        public byte[] CreateSessionId(IPEndPoint source) {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) {
                byte[] sessionId = new byte[16];
                rng.GetBytes(sessionId);
                sessionIds[source] = sessionId;
                return sessionId;
            }
        }

        /// <summary>
        /// Login a player, if the login is valid. A player that is already logged in will be logged out.
        /// </summary>
        public LoginResponse.Types Login(Login login, IPEndPoint source) {
            LoginResponse.Types response = LoginResponse.Types.IncorrectUsernameOrPassword;

            if (!VerifySessionId(login, source)) {
                return response;
            }

            if (login.Password.Length > config.accounts.MaxCharactersForPassword
                || login.Username.Length > config.accounts.MaxCharactersForUsername) {
                return response;
            }

            if (UserIsLoggedIn(login, out uint accountId)) {
                response = LoginResponse.Types.AlreadyLoggedIn;
                Debug.Debug.Log("User already Logged in: " + login.Username, Debug.Debug.MessageTypes.Login);
                Disconnect(players.ByAccountId[accountId]);
            } else if (Authenticate(login)) {
                Player p = new Player(source, accountId);
                players.AddPlayer(p);
                PlayerLoggedIn(login, p.Ip);
                response = LoginResponse.Types.Success;
                Debug.Debug.Log("Login successful: " + login.Username, Debug.Debug.MessageTypes.Login);
            } else {
                Debug.Debug.Log("Login failed: " + login.Username, Debug.Debug.MessageTypes.Login);
            }

            return response;
        }
        
        public void Disconnect(IPEndPoint ip) {
            if (!players.ByIP.ContainsKey(ip)) {
                return;
            }
            sessionIds.Remove(ip);
            Player p = players.ByIP[ip];
            Disconnect(p);
        }

        private void Disconnect(Player p) {
            ActiveCharacter c = p.CurrentCharacter;
            if (c != null) {
                characterLoginService.LogoutCharacter(c);
            }
            players.RemovePlayer(p);
            PlayerLoggedOut(p.Ip);
            Debug.Debug.Log("User disconnected: " + p.Ip, Debug.Debug.MessageTypes.Login);
        }

        private bool UserIsLoggedIn(Login login, out uint accountId) {
            if (db.GetAccountId(login.Username, out accountId)) {
                if (players.ByAccountId.ContainsKey(accountId)) {
                    return true;
                }
            }
            return false;
        }

        private bool VerifySessionId(Login login, IPEndPoint source) {
            if (!sessionIds.ContainsKey(source)){
                return false;
            }

            byte[] expectedSessionId = sessionIds[source];
            for (int i=0; i<expectedSessionId.Length; i++) {
                if (!expectedSessionId[i].Equals(login.SessionId[i])) {
                    return false;
                }
            }
            return true;
        }
        
        private bool Authenticate(Login login) {
            if (!db.GetPassword(login.Username, out byte[] combinedKey)){
                return false;
            }
            byte[] salt = new byte[16];
            byte[] correctKey = new byte[20];
            Array.Copy(combinedKey, 0, salt, 0, 16);
            Array.Copy(combinedKey, 16, correctKey, 0, 20);

            byte[] newKey = passwordHashing.GenerateHash(login.Password, salt);
            for (int i = 0; i < correctKey.Length; i++) {
                if (newKey[i] != correctKey[i]) {
                    return false;
                }
            }
            return true;
        }
    }
}

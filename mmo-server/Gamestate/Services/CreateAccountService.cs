using mmo_server.Communication;
using mmo_server.Cryptography;
using mmo_server.Debug;
using mmo_server.Persistence;
using mmo_shared;
using mmo_shared.Messages;
using System;
using System.Collections.Generic;
using System.Net;

namespace mmo_server.Gamestate {
    class CreateAccountService {
        private Database db;
        private PasswordHashing passwordHashing;
        private Config config;

        public CreateAccountService(Database db, PasswordHashing passwordHashing, Config config) {
            this.db = db;
            this.passwordHashing = passwordHashing;
            this.config = config;
        }

        /// <summary>
        /// create a new account if the registration is valid.
        /// </summary>
        public RegistrationResponse.Types CreateAccount(CreateAccount create, IPEndPoint source) {
            RegistrationResponse.Types response = RegistrationResponse.Types.Invalid;

            string username = create.Username;
            string pass = create.Password;

            if (db.GetAccountId(username, out uint accountId)) {
                response = RegistrationResponse.Types.UsernameTaken;
                Log($"{username}: Username already taken");
            }
            else if (username.Length < config.accounts.MinCharactersForUsername
                      || username.Length > config.accounts.MaxCharactersForUsername) {
                response = RegistrationResponse.Types.UsernameInvalid;
                Log($"{username}: Username invalid");
            }
            else if (pass.Length < config.accounts.MinCharactersForPassword
                      || pass.Length > config.accounts.MaxCharactersForPassword) {
                response = RegistrationResponse.Types.PasswordInvalid;
                Log($"{username}: Password invalid");
            }
            else if (db.CreateUser(username, passwordHashing.GenerateSaltedPasswordHash(pass))) {
                response = RegistrationResponse.Types.Success;
                Log($"{username}: Account created");
            }
            return response;
        }

        private void Log(string message) {
            Debug.Debug.Log(message, Debug.Debug.MessageTypes.Login);
        }
    }
}

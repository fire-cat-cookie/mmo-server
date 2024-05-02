using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using mmo_server.Gamestate;
using mmo_shared;
using System.Linq;

namespace mmo_server.Persistence
{
    class Database {
        private readonly Config config;
        private readonly MmoContext mmoContext;

        public Database(Config config) {
            mmoContext = new MmoContext();
            this.config = config;
        }

        /// <summary>
        /// return all character IDs belonging to an account.
        /// </summary>
        public bool GetCharacterIds(uint accountId, out List<uint> charIds)
        {
            charIds = mmoContext.Characters.Where(c => c.AccountId == accountId).Select(c => c.AccountId).ToList();
            return charIds.Count > 0;
        }

        /// <summary>
        /// returns a character for a given accountId and character slot.
        /// </summary>
        public bool GetCharacter(uint accountId, byte slot, out Character character)
        {
            character = mmoContext.Characters.Where(c => c.AccountId == accountId && c.Slot == slot).FirstOrDefault();
            return character != null;
        }

        /// <summary>
        /// returns a character by charId
        /// </summary>
        public bool GetCharacter(uint charId, out Character character)
        {
            character = mmoContext.Characters.Where(c => c.Id == charId).FirstOrDefault();
            return character != null;
        }

        /// <summary>
        /// return accountId by username
        /// </summary>
        public bool GetAccountId(string username, out uint accountId)
        {
            var account = mmoContext.Accounts.Where(a => a.Username == username).FirstOrDefault();
            accountId = account?.Id ?? 0;
            return account != null;
        }

        /// <summary>
        /// returns the encrypted password for a given username.
        /// </summary>
        public bool GetPassword(string username, out byte[] encryptedPassword)
        {
            var account = mmoContext.Accounts.Where(a => a.Username.Equals(username)).FirstOrDefault();
            encryptedPassword = account?.Password;
            return account != null;
        }

        /// <summary>
        /// Save a character or create if it does not exist.
        /// </summary>
        public bool Save(Character c)
        {
            mmoContext.Update(c);
            mmoContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Creates new user. Cancels if username already exists.
        /// </summary>
        public bool CreateUser(string username, byte[] password)
        {
            var userExists = mmoContext.Accounts.Where(a => a.Username == username).FirstOrDefault() != null;
            if (userExists)
            {
                return false;
            }
            mmoContext.Update(new Account() { Username = username, Password = password });
            mmoContext.SaveChanges();
            return true;
        }

    }
}

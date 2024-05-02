using mmo_server.Communication;
using mmo_server.Gamestate;
using mmo_server.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using mmo_server.Persistence;
using mmo_shared;
using mmo_shared.Messages;

namespace mmo_server.Gamestate
{
    class CharacterCreationService {
        private Database db;
        private Config config;

        public CharacterCreationService(Database db, Config config) {
            this.db = db;
            this.config = config;
        }

        /// <summary>
        /// Create a new character, if the character creation parameters are valid.
        /// </summary>
        public CreateCharacterResponse.Types CreateCharacter(Player player, CreateCharacter create, out ActiveCharacter newCharacter) {
            CreateCharacterResponse.Types response = CreateCharacterResponse.Types.Invalid;
            newCharacter = null;

            if (db.GetCharacter(player.AccountId, create.Slot, out Character existingCharacter))
            {
                return response;
            }

            if (create.Name.Length < config.accounts.MinCharactersForUsername
                      || create.Name.Length > config.accounts.MaxCharactersForUsername) {
                return CreateCharacterResponse.Types.NameInvalid;
            }

            Vector2 spawnPoint = config.characters.StartingPosition;
            var entity = new Character()
            {
                Name = create.Name,
                AccountId = player.AccountId,
                Class = 11,
                Level = 1,
                PositionX = (ushort)spawnPoint.X,
                PositionY = (ushort)spawnPoint.Y,
                ZoneId = config.characters.StartingZone,
                Slot = create.Slot
            };
            ActiveCharacter character = Converter.CreateDefaultCharacter(entity, config);

            if (db.Save(character.Entity)) {
                newCharacter = character;
                response = CreateCharacterResponse.Types.Success;
            }

            return response;
        }
    }
}

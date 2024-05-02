using mmo_shared;
using System;

namespace mmo_server.Gamestate
{
    public class ActiveCharacter : Positionable{
        public Character Entity { get; set; }
        private Vector2 _position;
        public Vector2 Position { 
            get { return _position; }
            set{ _position = value;  Entity.PositionX = (ushort)Math.Round(value.X*10); Entity.PositionY = (ushort)Math.Round(value.Y*10); }
        }
        public Vector2 Destination { get; set; }
        public float Velocity { get; set; } = 0f; //meters per second
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public float AttackRange { get; set; }
        public float AttackCooldown { get; set; }
        public bool Alive { get; set; }

        public ActiveCharacter(Character entity)
        {
            this.Entity = entity;
            _position = new Vector2( ((float)(Entity.PositionX) / 10), ((float)(Entity.PositionY)) / 10);
            Destination = _position;
        }
    }
}

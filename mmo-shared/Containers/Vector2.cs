using System;

namespace mmo_shared {
    public struct Vector2{
        public float X { get; private set; }
        public float Y { get; private set; }

        public Vector2(float x, float y) {
            this.X = x;
            this.Y = y;
        }

        public static bool operator ==(Vector2 a, Vector2 b) {
            return (a.X == b.X && a.Y == b.Y);
        }

        public static bool operator !=(Vector2 a, Vector2 b) {
            return (a.X != b.X || a.Y != b.Y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b) {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator *(Vector2 a, float b) {
            return new Vector2(a.X * b, a.Y * b);
        }

        public static Vector2 operator /(Vector2 a, float b) {
            return new Vector2(a.X / b, a.Y / b);
        }

        public float Length() {
            return (float)(Math.Sqrt(X * X + Y * Y));
        }

        public Vector2 Normalize() {
            return this / Length();
        }

        public float Distance(Vector2 b) {
            return (this - b).Length();
        }

        /// <summary>
        /// Returns a normalized vector pointing towards the target.
        /// </summary>
        public Vector2 DirectionOf(Vector2 target) {
            return (target - this).Normalize();
        }

        /// <summary>
        /// Moves a point a certain distance towards a target. If the movement would result in overshooting,
        /// return the target position instead.
        /// </summary>
        public Vector2 MoveTowards(Vector2 target, float step) {
            if (Distance(target) > step) {
                return this + ((target - this).Normalize() * step);
            } else {
                return target;
            }
        }

        public override bool Equals(object o) {
            if (o.GetType() != this.GetType()){
                return false;
            }
            return (Vector2)o == this;
        }

        public override int GetHashCode() {
            return X.GetHashCode() * 17 + Y.GetHashCode();
        }
    }
}

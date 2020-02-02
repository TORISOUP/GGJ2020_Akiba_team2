using System;

namespace GGJ2020.Damages
{
    public struct Damage : IEquatable<Damage>
    {
        public Damage(float stunSeconds)
        {
            StunSeconds = stunSeconds;
        }

        public float StunSeconds { get; }

        public bool Equals(Damage other)
        {
            return StunSeconds.Equals(other.StunSeconds);
        }

        public override bool Equals(object obj)
        {
            return obj is Damage other && Equals(other);
        }

        public override int GetHashCode()
        {
            return StunSeconds.GetHashCode();
        }
    }
}
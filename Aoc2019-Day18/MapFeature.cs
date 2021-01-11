using System;

namespace Aoc2019_Day18
{
    internal struct MapFeature
    {

        public (int Row, int Column) Position;
        public FeatureType Type;
        public char Letter;

        public MapFeature(FeatureType type, (int row, int column) position, char letter)
        {
            Type = type;
            Position = position;
            Letter = char.ToUpperInvariant(letter);
        }

        public override string ToString()
        {
            return (Type == FeatureType.Gate ? char.ToUpper(Letter) :
                Type == FeatureType.Key ? char.ToLower(Letter) : Letter).ToString();
            //return $"{Type} {Letter} at {Position}";
        }

        public bool Equals(MapFeature other)
        {
            return Position.Equals(other.Position) && Type == other.Type && Letter == other.Letter;
        }

        public override bool Equals(object? obj)
        {
            return obj is MapFeature other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, (int) Type, Letter);
        }

        public static bool operator ==(MapFeature left, MapFeature right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MapFeature left, MapFeature right)
        {
            return !left.Equals(right);
        }
    }
}

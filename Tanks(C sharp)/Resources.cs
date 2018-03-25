using System;
using System.Drawing;
namespace Tanks
{
    class consts
    {
        public const int TimerInterval = 40;
        public const int BulletSize = 10;
        public const int MaxBulletSpeed = 25;
        public const int TankSize = 50;
        public const int TankMaxSpeed = 10;
        public const int MaxHP = 600;
        public const int MaxDamage = 250;
        public const int MinReload = 1500;
    }
    class NotReloadedException : Exception
    {
    }

    enum Bonus
    {
        health = 2
        //повышенные скорострельность, урон, скорость, скорость полёта снаряда
        //аптечка
    }

    enum Direction
    {
        Up = 0,
        Left = 1,
        Right = 2,
        Down = 3
    }

    enum PlayerColor
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Yelow = 3
    }

    struct BulletParams
    {
        public int damage;
        public int speed;
        public Point location;
        public PlayerColor plrColor;
        public Direction direction;

        public BulletParams(int damage, int bulletSpeed, Point location, PlayerColor plrColor, Direction direction)
        {
            this.damage = damage;
            this.speed = bulletSpeed;
            this.location = location;
            this.plrColor = plrColor;
            this.direction = direction;
        }
    }
}

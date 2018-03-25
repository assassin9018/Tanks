using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
    class Tank
    {
        int damage = 50;
        int bulletSpeed = 15;
        int currentMaxReload = 200;
        int currentReload = 0;
        bool isReloaded = true;

        int hp = 200;
        int currentMaxHP = 200;
        public bool isLive { get; private set; }

        int currentSpeed = 8;
        bool isMoving;

        PictureBox tank;
        public PlayerColor plrColor { get; private set; }
        Direction direction;
        bool[] activeDirections = { false, false, false, false };
        Size battleFieldSize;

        public Point getLocation()
        {
            return tank.Location;
        }

        public Tank(Point location, PlayerColor plrColor, Direction direction, Panel battleField)
        {
            isLive = true;
            isMoving = false;
            this.plrColor = plrColor;
            tank = new PictureBox();
            tank.SizeMode = PictureBoxSizeMode.Zoom;
            tank.Width = consts.TankSize;
            tank.Height = consts.TankSize;
            tank.Location = location;
            setDirection(direction);
            battleField.Controls.Add(tank);
            battleFieldSize = battleField.Size;
        }

        public void setDirection(Direction direction)
        {
            this.direction = direction;
            tank.Image = (Bitmap)Tanks.Properties.Resources.ResourceManager.GetObject(plrColor.ToString() + direction.ToString());
        }

        public void addActiveDirection(Direction direction)
        {
            isMoving = true;
            activeDirections[(int)direction] = true;
            setDirection(direction);
        }

        public void abortActiveDirection(Direction direction)
        {
            activeDirections[(int)direction] = false;
            isMoving = false;
            int i = 0;
            while ((i < activeDirections.Length) && !isMoving)
                isMoving = activeDirections[i++];
            if (isMoving)
            {
                i--;
                setDirection((Direction)i);
            }
            else setDirection(direction);
        }
        
        private bool pointInTank(Point point, Point tankLocation)
        {
            if ((tankLocation.X <= point.X) && (tankLocation.X + consts.TankSize >= point.X) && (tankLocation.Y <= point.Y) && (tankLocation.Y + consts.TankSize >= point.Y))
                return true;
            else return false;
        }

        private bool crossTansk(Tank[] tanks, Point newLocation)
        {
            for (byte i = 0; i < tanks.Length; i++)
            {
                Point point = tanks[i].tank.Location;
                int distance = (int)Math.Sqrt(Math.Pow(newLocation.X - tanks[i].tank.Location.X, 2) + Math.Pow(newLocation.Y - tanks[i].tank.Location.Y, 2));
                if (distance <= consts.TankSize * 1.5)
                    if (plrColor != tanks[i].plrColor)
                    {//проверяем войдут ли угловые точки других танков в область текущего
                        if (pointInTank(point, newLocation))
                            return true;
                        if (pointInTank(new Point(point.X + consts.TankSize, point.Y), newLocation))
                            return true;
                        if (pointInTank(new Point(point.X, point.Y + consts.TankSize), newLocation))
                            return true;
                        if (pointInTank(new Point(point.X + consts.TankSize, point.Y + consts.TankSize), newLocation))
                            return true;
                    }
            }
            return false;
        }

        public void tryMove(Tank[] tanks)
        {
            if (isMoving)
            {
                Point newLocation;
                switch (direction)
                {
                    case Direction.Up:
                        {
                            newLocation = new Point(tank.Location.X, tank.Location.Y - currentSpeed);
                            if (!crossTansk(tanks, newLocation))
                                if (newLocation.Y >= 0)
                                    tank.Location = newLocation;
                                else
                                    tank.Location = new Point(tank.Location.X, 0);
                        }
                        break;
                    case Direction.Left:
                        {
                            newLocation = new Point(tank.Location.X - currentSpeed, tank.Location.Y);
                            if (!crossTansk(tanks, newLocation))
                                if (newLocation.X >= 0)
                                    tank.Location = newLocation;
                                else
                                    tank.Location = new Point(0, tank.Location.Y);
                        }
                        break;
                    case Direction.Down:
                        {
                            newLocation = new Point(tank.Location.X, tank.Location.Y + currentSpeed);
                            if (!crossTansk(tanks, newLocation))
                                if (newLocation.Y + consts.TankSize <= battleFieldSize.Height)
                                    tank.Location = newLocation;
                                else
                                    tank.Location = new Point(tank.Location.X, battleFieldSize.Height - consts.TankSize);
                        }
                        break;
                    case Direction.Right:
                        {
                            newLocation = new Point(tank.Location.X + currentSpeed, tank.Location.Y);
                            if (!crossTansk(tanks, newLocation))
                                if (newLocation.X + consts.TankSize <= battleFieldSize.Width)
                                    tank.Location = newLocation;
                                else
                                    tank.Location = new Point(battleFieldSize.Width - consts.TankSize, tank.Location.Y);
                        }
                        break;
                }
            }
        }

        public int getHP()
        {
            return hp;
        }

        public void takeDamage(int damage)
        {
            hp -= damage;
            if (hp < 0)
            {
                isLive = false;
                hp = 0;
            }
        }

        private Point getBulletPoint()
        {
            switch (direction)
            {
                case Direction.Up: return new Point(tank.Location.X + consts.TankSize / 2, tank.Location.Y - consts.BulletSize / 2 - bulletSpeed);
                case Direction.Left: return new Point(tank.Location.X - consts.BulletSize / 2 - bulletSpeed / 2, tank.Location.Y + consts.TankSize / 2);
                case Direction.Down: return new Point(tank.Location.X + consts.TankSize / 2, tank.Location.Y + consts.TankSize + consts.BulletSize / 2 + bulletSpeed);
                case Direction.Right: return new Point(tank.Location.X + consts.TankSize + consts.BulletSize / 2 + bulletSpeed, tank.Location.Y + consts.TankSize / 2);
                default: return new Point(0, 0);//чтобы компилятор отъебался
            }
        }

        public BulletParams fire()
        {
            if (isReloaded)
            {
                currentReload = currentMaxReload;
                isReloaded = false;
                return new BulletParams(damage, bulletSpeed, getBulletPoint(), plrColor, direction);;
            }
            else
            {
                throw new NotReloadedException();
            }
        }

        public void reload()
        {
            if (!isReloaded)
                currentReload -= consts.TimerInterval;
            if (currentReload <= 0)
                isReloaded = true;
        }
    }
}
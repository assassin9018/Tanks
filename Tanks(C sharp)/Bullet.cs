using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
    class Bullet
    {
        int damage;
        int speed;
        Size battleFieldSize;
        Direction direction;
        Point location;
        PictureBox bullet;

        public int getDamage()
        {
            return damage;
        }

        public Bullet(BulletParams properties, Panel battleField)
        {
            damage = properties.damage;
            speed = properties.speed;
            location = properties.location;
            direction = properties.direction;
            battleFieldSize = battleField.Size;
            bullet = new PictureBox();
            bullet.Location = location;
            bullet.Width = consts.BulletSize;
            bullet.Height = consts.BulletSize;
            bullet.SizeMode = PictureBoxSizeMode.Zoom;
            battleField.Controls.Add(bullet);
            bullet.Image = (Bitmap)Tanks.Properties.Resources.ResourceManager.GetObject(properties.plrColor.ToString() + "Bullet");
        }

        public Point getLocation()
        {
            return bullet.Location;
        }

        public void destruct()
        {
            bullet.Dispose();
        }

        public bool move()
        {
            bool canMove = true;
            switch (direction)
            {
                case Direction.Up:
                    if (bullet.Location.Y - speed >= 0)
                        bullet.Location = new Point(bullet.Location.X, bullet.Location.Y - speed);
                    else canMove = false;
                    break;
                case Direction.Left:
                    if (bullet.Location.X - speed >= 0)
                        bullet.Location = new Point(bullet.Location.X - speed, bullet.Location.Y);
                    else canMove = false;
                    break;
                case Direction.Down:
                    if (bullet.Location.Y + speed <= battleFieldSize.Height)
                        bullet.Location = new Point(bullet.Location.X, bullet.Location.Y + speed);
                    else canMove = false;
                    break;
                case Direction.Right:
                    if (bullet.Location.X + speed <= battleFieldSize.Width)
                        bullet.Location = new Point(bullet.Location.X + speed, bullet.Location.Y);
                    else canMove = false;
                    break;
            }
            return canMove;
        }

    }
}

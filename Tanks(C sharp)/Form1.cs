using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{    
    public partial class Form1 : Form
    {
        bool gameActive = false;
        Tank[] tanks = new Tank[2];
        LinkedList<Bullet> bulletList = new LinkedList<Bullet>();

        public Form1()
        {
            InitializeComponent();
            timer.Interval = consts.TimerInterval;
        }

        private bool roundCrossRect(Point rnd, Point rect)
        {
            rnd = new Point(rnd.X + consts.BulletSize / 2, rnd.Y + consts.BulletSize / 2);
            int distance = (int)Math.Sqrt(Math.Pow(rect.X + consts.TankSize / 2 - rnd.X, 2) + Math.Pow(rect.Y + consts.TankSize / 2 - rnd.Y, 2));
            if (distance > consts.TankSize / 2 + consts.BulletSize - 5)
                return false;
            else
                return true;
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About frm = new About();
            frm.ShowDialog();
        }

        private void параметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная функция пока не доступна. Сорян.");
        }

        private void новаяИграToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tanks[0] = new Tank(new Point(0, 0), PlayerColor.Red, Direction.Right, battleField);
            tanks[1] = new Tank(new Point(battleField.Width - consts.TankSize, battleField.Height - consts.TankSize), PlayerColor.Green, Direction.Left, battleField);
            timer.Enabled = true;
            gameActive = true;
        }

        private void checkTanksProperties()//костыль
        {
            for(int i=0;i<tanks.Length;i++)
                if (!tanks[i].isLive)
                {
                    timer.Enabled = false;
                    battleField.Controls.Clear();
                    MessageBox.Show(tanks[i].plrColor.ToString()+" уничтожен.");
                    gameActive = false;
                }
        }

        private void moveAllObjectsOnBattleField()
        {
            for (int i = 0; i < tanks.Length; i++)
                tanks[i].tryMove(tanks);
            LinkedListNode<Bullet> bulListItem = bulletList.First;
            while (bulListItem != null)
            {
                if (bulListItem.Value.move())
                    bulListItem = bulListItem.Next;
                else
                {
                    LinkedListNode<Bullet> itemForRemove = bulListItem;
                    bulListItem = bulListItem.Next;
                    itemForRemove.Value.destruct();
                    bulletList.Remove(itemForRemove);
                }
            }
        }

        private void reloadAllTanks()
        {
            for (int i = 0; i < tanks.Length; i++)
                tanks[i].reload();
        }

        private void checkCrossBulletsAndTanks()
        {
            LinkedListNode<Bullet> bulListItem;
            for (int i = 0; i < tanks.Length; i++)
            {
                bulListItem = bulletList.First;
                while (bulListItem != null)
                    if (roundCrossRect(bulListItem.Value.getLocation(), tanks[i].getLocation()))
                    {
                        tanks[i].takeDamage(bulListItem.Value.getDamage());
                        LinkedListNode<Bullet> itemForRemove = bulListItem;
                        bulListItem = bulListItem.Next;
                        itemForRemove.Value.destruct();
                        bulletList.Remove(itemForRemove);
                    }
                    else bulListItem = bulListItem.Next;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            moveAllObjectsOnBattleField();
            checkCrossBulletsAndTanks();
            reloadAllTanks();
            checkTanksProperties();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameActive)
                if ((Keys.W == e.KeyCode) || (Keys.A == e.KeyCode) || (Keys.S == e.KeyCode) || (Keys.D == e.KeyCode))
                {
                    switch (e.KeyCode)
                    {
                        case Keys.W: tanks[0].addActiveDirection(Direction.Up); break;
                        case Keys.A: tanks[0].addActiveDirection(Direction.Left); break;
                        case Keys.S: tanks[0].addActiveDirection(Direction.Down); break;
                        case Keys.D: tanks[0].addActiveDirection(Direction.Right); break;
                    }
                }
                else if ((Keys.Up == e.KeyCode) || (Keys.Left == e.KeyCode) || (Keys.Down == e.KeyCode) || (Keys.Right == e.KeyCode))
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up: tanks[1].addActiveDirection(Direction.Up); break;
                        case Keys.Left: tanks[1].addActiveDirection(Direction.Left); break;
                        case Keys.Down: tanks[1].addActiveDirection(Direction.Down); break;
                        case Keys.Right: tanks[1].addActiveDirection(Direction.Right); break;
                    }
                }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)//прописать выстрел
        {
            if (gameActive)
                if (e.KeyChar == (char)Keys.Space)
                    try
                    {
                        BulletParams bulProp = tanks[0].fire();
                        bulletList.AddLast(new Bullet(bulProp, battleField));
                    }
                    catch (NotReloadedException)
                    {
                        //обработка исключения не требуется
                    }
                else if (e.KeyChar == (char)Keys.D0)
                    try
                    {
                        BulletParams bulProp = tanks[1].fire();
                        bulletList.AddLast(new Bullet(bulProp, battleField));
                    }
                    catch (NotReloadedException)
                    {
                        //обработка исключения не требуется
                    }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (gameActive)
                if ((Keys.W == e.KeyCode) || (Keys.A == e.KeyCode) || (Keys.S == e.KeyCode) || (Keys.D == e.KeyCode))
                {
                    switch (e.KeyCode)
                    {
                        case Keys.W: tanks[0].abortActiveDirection(Direction.Up); break;
                        case Keys.A: tanks[0].abortActiveDirection(Direction.Left); break;
                        case Keys.S: tanks[0].abortActiveDirection(Direction.Down); break;
                        case Keys.D: tanks[0].abortActiveDirection(Direction.Right); break;
                    }
                }
                else if ((Keys.Up == e.KeyCode) || (Keys.Left == e.KeyCode) || (Keys.Down == e.KeyCode) || (Keys.Right == e.KeyCode))
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up: tanks[1].abortActiveDirection(Direction.Up); break;
                        case Keys.Left: tanks[1].abortActiveDirection(Direction.Left); break;
                        case Keys.Down: tanks[1].abortActiveDirection(Direction.Down); break;
                        case Keys.Right: tanks[1].abortActiveDirection(Direction.Right); break;
                    }
                }
        }
    }
}

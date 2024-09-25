using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfApp1.Class;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PersonInfo Player = new PersonInfo("Trash", 100, 10, 1, 0, 0, 5, new BitmapImage(new Uri("m.png", UriKind.Relative)));
        public List<PersonInfo> Enemys = new List<PersonInfo>();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public PersonInfo Enemy;


        public MainWindow()
        {
            InitializeComponent();
            UserInfoPlayer();

            Enemys.Add(new PersonInfo("Nagito", 60, 10, 1, 15, 15, 10, new BitmapImage(new Uri("n2.png", UriKind.Relative))));
            Enemys.Add(new PersonInfo("Nagito Super", 80, 15, 1, 25, 30, 15, new BitmapImage(new Uri("n1.png", UriKind.Relative))));
            Enemys.Add(new PersonInfo("Nagito Ultra", 100, 20, 1, 30, 100, 20, new BitmapImage(new Uri("n3.png", UriKind.Relative))));

            dispatcherTimer.Tick += AttackPlayer;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();

            SelectEnemy();
        }

        private void AttackPlayer(object sender, EventArgs e)
        {
            int deltaDamage = Convert.ToInt32(Enemy.Damage * 100f / (100f - Player.Armor));

            if (Player.Health - deltaDamage <= 0)
            {
                Player.Health = 0;
                dispatcherTimer.Stop();
                MessageBox.Show("You Lose!");
                GameRestart();
            }
            else
            {
                Player.Health -= deltaDamage;
            }

            UserInfoPlayer();

        }

        public void SelectEnemy()
        {
            int Id = new Random().Next(0, Enemys.Count);
            Enemy = new PersonInfo(
                Enemys[Id].Name,
                Enemys[Id].Health,
                Enemys[Id].Armor,
                Enemys[Id].Level,
                Enemys[Id].Glasses,
                Enemys[Id].Money,
                Enemys[Id].Damage,
                Enemys[Id].Image
                );
            enemyImage.Source = Enemy.Image;
        }

        public void UserInfoPlayer()
        {
            if (Player.Glasses > Player.Level * 100)
            {
                Player.Level++;
                Player.Glasses = 0;
                Player.Health += 100;
                Player.Damage++;
                Player.Armor++;
            }
            playerHealth.Content = "HP: " + Player.Health;
            playerArmor.Content = "DEF: " + Player.Armor;
            playerLevel.Content = "LV: " + Player.Level;
            playerGlasses.Content = "EXP: " + Player.Glasses;
            playerMoney.Content = "Coins: " + Player.Money;
        }

        private void AttackEnemy(object sender, MouseButtonEventArgs e)
        {
            Enemy.Health -= Convert.ToInt32(Player.Damage * 100f / (100f - Enemy.Armor));
            if (Enemy.Health <= 0)
            {
                Player.Glasses += Enemy.Glasses;
                Player.Money += Enemy.Money;
                UserInfoPlayer();
                SelectEnemy();
            }
            else
            {
                enemyHealth.Content = "HP: " + Enemy.Health;
                enemyArmor.Content = "DEF: " + Enemy.Armor;

            }

        }
        public void GameRestart()
        {
            Player = new PersonInfo("Trash", 100, 10, 1, 0, 0, 5, new BitmapImage(new Uri("m.png", UriKind.Relative)));
            SelectEnemy();
            UserInfoPlayer();
            dispatcherTimer.Start();
        }

    }
}
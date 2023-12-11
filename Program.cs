// import kelas sistem, generik dan theading

namespace UAS;

class Program
{
  static void Main(string[] args)
  {
    // Deklarasikan dan inisialisasi variabel
    // 2 buat list: Tank dan tanks, Player(Objek tank) dan nilai random
    List<Tank> Tanks = new();
    List<Tank> AllTanks = new();
    Random rnd = new();
    // Tambahkan player dari 3 tank lain (AI) ke dalam list Tanks
    Tank Player = new(true, 8, 5, Aset.Direction.Right);
    Tanks.Add(Player);
    Tanks.Add(new Tank(false, 8, 21, Aset.Direction.Right));
    Tanks.Add(new Tank(false, 66, 5, Aset.Direction.Left));
    Tanks.Add(new Tank(false, 66, 21, Aset.Direction.Left));

    AllTanks.AddRange(Tanks);

    // Set width dan height console window supaya game leluasa untuk dimainkan
    Console.CursorVisible = false;
    if (OperatingSystem.IsWindows())
    {
      Console.WindowWidth = Math.Max(Console.WindowWidth, 90);
      Console.WindowHeight = Math.Max(Console.WindowHeight, 35);
    }
    // Clearr console
    Console.Clear();
    Console.SetCursorPosition(0, 0);
    // Render game map
    Render(Aset.Map);
    // Tuliskan intro "Gunakan WASD untuk bergerak dan tombol panah untuk menembak
    Console.WriteLine();
    Console.WriteLine("Gunakan (W, A, S, D) untuk berpindah dan tombol panah kiri kanan untuk menembak");

    // Gameplaye
    while (Tanks.Contains(Player) && Tanks.Count > 1)
    {
      foreach (Tank tank in Tanks)
      {
        // Buat logika arah tembakan pemain/tank
        if (tank.IsShooting)
        {
          tank.Bullet = new(
            tank.Direction == Aset.Direction.Left ? tank.x - 3 : tank.Direction == Aset.Direction.Right ? tank.x + 3 : tank.x,
            tank.y,
            tank.Direction
          );
          tank.IsShooting = false;
        }
        // Buat logika untuk menampilkan animasi ledakan bom, jika bertabrakan
        // dengan tank lain atau terkena tembakan tank lain (bullet)
        if (tank.IsExploding())
        {
          tank.ExplodingFrame++;
          Console.SetCursorPosition(tank.x - 2, tank.y - 1);
          Render(tank.ExplodingFrame > 9
            ? Aset.TankExploding[2]
            : Aset.TankExploding[tank.ExplodingFrame % 2], true);
          continue;
        }

        bool CheckGerakan(Tank gerakTank, int x, int y)
        {
          // Buat pendeteksi tabrakan tank dengan tank batas dinding
          foreach (var tank in Tanks)
          {
            if (tank == gerakTank) continue;
            if (Math.Abs(tank.x - x) <= 4 && Math.Abs(tank.y - y) <= 2) return false; // tabrakan dengan tank lain
          }
          if (x < 3 || x > 71 || y < 2 || y > 25) return false; // tabrakan dengan dinding
          if (3 < x && x < 13 && 11 < y && y < 15) return false; // tabrakan dengan hambatan dikiri
          if (34 < x && x < 40 && 2 < y && y < 8) return false; // tabrakan dengan hambatan diatas
          if (34 < x && x < 40 && 19 < y && y < 25) return false; // tabrakan dengan hambatan dibawah

          return true;
        }

        void GerakTank(Aset.Direction arah)
        {
          // Metode untuk gerakan pemain/tank (atas, bawah, kiri, kanan)
          switch (arah)
          {
            case Aset.Direction.Up:
              if (CheckGerakan(tank, tank.x, tank.y - 1))
              {
                Console.SetCursorPosition(tank.x - 2, tank.y + 1);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x - 1, tank.y + 1);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x, tank.y + 1);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x + 1, tank.y + 1);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x + 2, tank.y + 1);
                Console.Write(' ');
                tank.y--;
              }
              break;
            case Aset.Direction.Down:
              if (CheckGerakan(tank, tank.x, tank.y + 1))
              {
                Console.SetCursorPosition(tank.x - 2, tank.y - 1);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x - 1, tank.y - 1);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x, tank.y - 1);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x + 1, tank.y - 1);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x + 2, tank.y - 1);
                Console.Write(' ');
                tank.y++;
              }
              break;
            case Aset.Direction.Left:
              if (CheckGerakan(tank, tank.x - 1, tank.y))
              {
                Console.SetCursorPosition(tank.x + 2, tank.y - 1);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x + 2, tank.y);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x + 2, tank.y + 1);
                Console.Write(' ');
                tank.x--;
              }
              break;
            case Aset.Direction.Right:
              if (CheckGerakan(tank, tank.x + 1, tank.y))
              {
                Console.SetCursorPosition(tank.x - 2, tank.y - 1);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x - 2, tank.y);
                Console.Write(' ');
                Console.SetCursorPosition(tank.x - 2, tank.y + 1);
                Console.Write(' ');
                tank.x++;
              }
              break;
          }
        }

        if (tank.IsPlayer)
        {
          // Implementasi player input (keyboard) untuk gerak dan menembak
          Aset.Direction? move = null;
          Aset.Direction? shoot = null;

          while (Console.KeyAvailable)
          {
            switch (Console.ReadKey(true).Key)
            {
              // Move Direction
              case ConsoleKey.W: move = Aset.Direction.Up; break;
              case ConsoleKey.S: move = Aset.Direction.Down; break;
              case ConsoleKey.A: move = Aset.Direction.Left; break;
              case ConsoleKey.D: move = Aset.Direction.Right; break;

              // Shoot Direction
              case ConsoleKey.LeftArrow: shoot = Aset.Direction.Left; break;
              case ConsoleKey.RightArrow: shoot = Aset.Direction.Right; break;

              // Close Game
              case ConsoleKey.Escape:
                Console.Clear();
                Console.Write("Permainan ditutup");
                return;
            }
            tank.IsShooting = shoot != null && shoot != null && tank.Bullet == null;

            if (tank.IsShooting) tank.Direction = shoot ?? tank.Direction;
            else tank.Direction = move == Aset.Direction.Left ? Aset.Direction.Left : move == Aset.Direction.Right ? Aset.Direction.Right : tank.Direction;

            if (move.HasValue) GerakTank(move.Value);
          }
        }
        else
        {
          // Implementasi gerakan dan tembakan dari AI
          int randomIndex = rnd.Next(0, 6);
          if (randomIndex < 4)
            GerakTank((Aset.Direction)randomIndex + 1);

          if (tank.Bullet == null)
          {
            Aset.Direction shoot = tank.x < Player.x ? Aset.Direction.Right : Aset.Direction.Left;
            tank.Direction = shoot;
            tank.IsShooting = true;
          }
        }
        // Render tank, bullet dan map ke konsol
        Console.SetCursorPosition(tank.x - 2, tank.y - 1);
        Render(tank.IsExploding()
          ? Aset.TankExploding[tank.ExplodingFrame % 2]
          : tank.IsShooting
            ? Aset.TankShooting[(int)tank.Direction]
            : Aset.Tank[(int)tank.Direction],
          true);
      }

      bool BulletCollisionCheck(Bullet bullet, out Tank? collidingTank)
      {
        collidingTank = null;
        foreach (var tank in Tanks)
        {
          if (Math.Abs(bullet.x - tank.x) < 3 && Math.Abs(bullet.y - tank.y) < 2)
          {
            collidingTank = tank;
            return true;
          }
        }
        if (bullet.x == 0 || bullet.x == 74 || bullet.y == 0 || bullet.y == 27) return true;
        if (5 < bullet.x && bullet.x < 11 && bullet.y == 13) return true; // tabrakan dengan hambatan dikiri
        if (bullet.x == 37 && 3 < bullet.y && bullet.y < 7) return true; // tabrakan dengan hambatan diatas
        if (bullet.x == 37 && 20 < bullet.y && bullet.y < 24) return true; // tabrakan dengan hambatan dibawah
        return false;
      }

      foreach (var tank in AllTanks)
      {
        if (tank.Bullet is not null)
        {
          // Implementasi logika bullet dan collision bullet
          Bullet bullet = tank.Bullet;
          Console.SetCursorPosition(bullet.x, bullet.y);
          Console.Write(' ');
          switch (bullet.Direction)
          {
            case Aset.Direction.Left: bullet.x--; break;
            case Aset.Direction.Right: bullet.x++; break;
          }
          Console.SetCursorPosition(bullet.x, bullet.y);
          bool collision = BulletCollisionCheck(bullet, out Tank? collisionTank);
          Console.Write(collision
            ? '█'
            : Aset.Bullet[(int)bullet.Direction]);
          if (collision)
          {
            if (collisionTank != null && --collisionTank.Health <= 0)
            {
              collisionTank.ExplodingFrame = 1;
            }
            tank.Bullet = null;
          }
        }
      }

      // Hapus tank yang sudah meledak dari list
      for (int i = 0; i < Tanks.Count; i++)
      {
        if (Tanks[i].ExplodingFrame > 10)
        {
          Tanks.RemoveAt(i);
          i--;
        }
      }
      // Render map
      Console.SetCursorPosition(0, 0);
      Render(Aset.Map);
      // Beri delay agar animasi pada gameplay smoth
      Thread.Sleep(TimeSpan.FromMilliseconds(10));
    }
    // Tampilkan win/lose condition dari game
    Console.WriteLine();
    Console.SetCursorPosition(0, 33);
    Console.Write(Tanks.Contains(Player)
      ? "You Win."
      : "You Lose.");
    Console.ReadLine();
  }

  // Buat metode render untuk menampilkan teks dan sprite pada console app
  static void Render(string @string, bool renderSpace = false)
  {
    int x = Console.CursorLeft;
    int y = Console.CursorTop;
    foreach (char c in @string)
      if (c == '\n') Console.SetCursorPosition(x, ++y);
      else if (c != ' ' || renderSpace) Console.Write(c);
      else Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
  }

  // Buat sprite untuk tank, animasi tembak, animasi explode, bullet dan map
}

// Buat enum untuk arah
// Buat kelas Tank dengan properti dan methodnya
// Buat kelas bullet
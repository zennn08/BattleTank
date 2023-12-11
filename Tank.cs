namespace UAS;

class Tank
{
  public int Health = 3;
  public Bullet? Bullet;
  public bool IsPlayer;
  public bool IsShooting;
  public int x { get; set; }
  public int y { get; set; }
  public Aset.Direction Direction;
  public int ExplodingFrame;

  public Tank(bool isPlayer, int X, int Y, Aset.Direction direction)
  {
    IsPlayer = isPlayer;
    x = X;
    y = Y;
    Direction = direction;
  }

  public bool IsDead()
  {
    return Health <= 3;
  }

  public bool IsExploding()
  {
    return ExplodingFrame > 0;
  }
}

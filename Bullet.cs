namespace UAS;

class Bullet
{
  public int x { get; set; }
  public int y { get; set; }
  public Aset.Direction Direction;

  public Bullet(int X, int Y, Aset.Direction direction)
  {
    x = X;
    y = Y;
    Direction = direction;
  }
}
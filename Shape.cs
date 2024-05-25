using System.Diagnostics;
using System.Numerics;


namespace GlassPieces;

public class Shape : Panel {
  
  /*---------------------------Static-------------------------------*/
  static float minDistance = 2f; //Minimum distance between points in border
  static float connectionThreshold = 1f;
  static int scale = 4;
  public static Connection IsConnected(Shape x, Shape y) {

    Connection min = new Connection {
      connectedPoints = 0
    };

    GlassPieces.WriteLine($"Running the IsConnected for!");
    foreach (PointF pointX in x.border) {
      foreach (PointF pointY in y.border) {
        for (float angle = 0; angle < 2 * Math.PI; angle += 1f) {
          List<PointF> rotatedY = RotatePoints(y.border, pointY, pointX, angle);

          Console.WriteLine($"Angle={angle}");

          int totalNumberOfConnectedPoints = CalculateTotalDistance(x.border, pointX, rotatedY, pointY);

          if (totalNumberOfConnectedPoints > min.connectedPoints) {
            min.X = x;
            min.Y = y;
            min.x = pointX;
            min.y = pointY;
            min.angle = angle;
            min.connectedPoints = totalNumberOfConnectedPoints;
          }
        }
      }
    }

    return min;
  }

  static List<PointF> RotatePoints(List<PointF> points, PointF center, PointF target, float angle) {
    List<PointF> rotatedPoints = new List<PointF>();

    foreach (PointF point in points) {
      float x = center.X + (float)((point.X - center.X) * Math.Cos(angle) - (point.Y - center.Y) * Math.Sin(angle));
      float y = center.Y + (float)((point.X - center.X) * Math.Sin(angle) + (point.Y - center.Y) * Math.Cos(angle));
      rotatedPoints.Add(new PointF(x, y));
    }

    return rotatedPoints;
  }

  static int CalculateTotalDistance(List<PointF> borderX, PointF pointX, List<PointF> borderY, PointF pointY) {
    int connectedPoints = 0;

    int indexX = borderX.IndexOf(pointX);
    int indexY = borderY.IndexOf(pointY);

    if (indexX == -1 || indexY == -1) return connectedPoints;

    for (int i = 0; i < Math.Min(borderX.Count - indexX, borderY.Count - indexY); i++) {
      if (Distance(borderX[indexX + i], borderY[indexY + i]) < connectionThreshold) {
        connectedPoints++;
      } else {
        break;
      }
    }

    for (int i = 1; i <= Math.Min(indexX, indexY); i++) {
      if (Distance(borderX[indexX - i], borderY[indexY - i]) < connectionThreshold) {
        connectedPoints++;
      } else {
        break;
      }
    }

    return connectedPoints;
  }

  static float Distance(PointF p1, PointF p2) {
    return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
  }
  /*---------------------------Static-------------------------------*/


  /*---------------------------Fields-------------------------------*/
  public List<PointF> points = new List<PointF>(); //used for drawing
  private Bitmap image; //used for drawing
  public List<PointF> border = new List<PointF>(); //used for caculating
  /*---------------------------Fields-------------------------------*/

  public Shape(string pathToImg, string pathToOutline) {
    BackColor = Color.RoyalBlue;
    Size = new Size(6016 / scale, 4016 / scale); //set size so image fits

    try {
      //Dont uset the default background image cause you have to crop it
      image = new Bitmap(pathToImg);
      image = new Bitmap(image, new Size(image.Width / scale, image.Height / scale));
    } catch (Exception e) {
      MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    using (StreamReader reader = new StreamReader(pathToOutline)) {
      string line;
      while ((line = reader.ReadLine()) != null) {
        var coords = line.Split('\t');
        if (coords.Length >= 2) {
          //in the future you should take care of precision here
          float x = float.Parse(coords[0]) / scale;
          float y = float.Parse(coords[1]) / scale;
          points.Add(new PointF(x, y));
        }
      }
    }

    if (points.Count > 0) {
      GenerateBorderPoints();
    }
  }

  private void GenerateBorderPoints() {
    border.Add(points[0]);
    for (int i = 0; i < points.Count - 1; i++) {
      PointF point1 = border[border.Count - 1];
      PointF point2 = points[i + 1];
      Vector2 direction = new Vector2(point2.X - point1.X, point2.Y - point1.Y);
      float distance = direction.Length();
      direction /= distance; // Normalize the direction

      int steps = (int)(distance / minDistance);
      for (int j = 1; j <= steps; j++) {
        border.Add(new PointF(
            point1.X + direction.X * minDistance * j,
            point1.Y + direction.Y * minDistance * j
        ));
      }
    }
  }

  protected override void OnPaint(PaintEventArgs e) {
    using (Graphics g = e.Graphics) {
      base.OnPaint(e);

      if (image == null) {
        g.DrawString(
          "No image uploaded!",
          new Font("Consolas", 16),
          new SolidBrush(Color.Black),
          new Point(10, 10)
        ); return;
      }

      if (points.Count <= 2) {
        g.DrawString(
          "No image uploaded!",
          new Font("Consolas", 16),
          new SolidBrush(Color.Black),
          new Point(10, 50)
        ); return;
      };

      using (TextureBrush textureBrush = new TextureBrush(image)) {
        g.FillPolygon(textureBrush, points.ToArray());
        g.DrawPolygon(Pens.Black, points.ToArray());
      }


      // Very lay to show all of this small points
      float radius = 1f;
      foreach (PointF borderPoint in border) {
        g.FillEllipse(Brushes.Red, borderPoint.X - radius, borderPoint.Y - radius, 2 * radius, 2 * radius);
      }
    }
  }
}
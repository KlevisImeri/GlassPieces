using System.Drawing.Drawing2D;
using System.Reflection.Metadata.Ecma335;


namespace GlassPieces;
public class Shape : Panel {
  private List<PointF> points = new List<PointF>();
  private Bitmap image;
  private int scale = 10;

  public Shape(string pathToImg, string pathToOutline) {
    BackColor = Color.RoyalBlue;
    try {
      //Dont uset the default background image cause you have to crop it
      image = new Bitmap(pathToImg);
      image = new Bitmap(image, new Size(image.Width / scale, image.Height / scale));
      // MessageBox.Show(image.Size.ToString());
    } catch (Exception e) {
      MessageBox.Show(e.Message);
    }

    using (StreamReader reader = new StreamReader(pathToOutline)) {
      string line;
      while ((line = reader.ReadLine()) != null) {
        var coords = line.Split('\t');
        if (coords.Length >= 2) {
          float x = float.Parse(coords[0]) / scale;
          float y = float.Parse(coords[1]) / scale;
          points.Add(new PointF(x, y));
        }
      }
    }

    Size = new Size(6016 / scale, 4016 / scale);
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
    }
  }
}
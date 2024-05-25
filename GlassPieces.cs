
namespace GlassPieces;
public partial class GlassPieces : Form {
  List<Shape> shapes = new List<Shape>(); //Could have used the Control Collection
  List<Connection> edges = new List<Connection>();
  string directoryPath;
  FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel {
    Dock = DockStyle.Fill, // Make the panel fill the entire form
    AutoScroll = true // Enable scrolling if the content exceeds the panel bounds
  };
  public GlassPieces(string directoryPath) {
    this.directoryPath = directoryPath;

    // Get all the XXX.png files and all the XXX.txt files
    foreach (string file in Directory.GetFiles(directoryPath, "*.png")) {
      // Extract the base name of the file (without extension) to use as the corresponding.txt file name
      string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
      string txtFilePath = Path.Combine(directoryPath, fileNameWithoutExtension + ".txt");

      // Check if the corresponding.txt file exists
      if (File.Exists(txtFilePath)) {
        // Instantiate the Shape with the corresponding.png and.txt files
        Shape shape = new Shape(Path.Combine(directoryPath, file), txtFilePath);
        shapes.Add(shape);
        flowLayoutPanel.Controls.Add(shape);
      }
    }
    
    Controls.Add(flowLayoutPanel);


    Connection conn = Shape.IsConnected(shapes[0], shapes[1]);

    MessageBox.Show(conn.ToString());
  }
}


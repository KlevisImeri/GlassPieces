using System.Runtime.InteropServices;

namespace GlassPieces;
public partial class GlassPieces : Form {

  /*------------------------Degug Console-----------------------------*/
  public static TextBox console =  new TextBox() {
    Multiline = true,
    AcceptsReturn = true,
    AcceptsTab = true,
    ScrollBars = ScrollBars.Vertical,
    Size = new Size(500, 500)
  };

  static public void WriteLine(string text) {
    if (console.InvokeRequired) {
      console.Invoke((MethodInvoker)(() => console.Text += text + "\n"));
    } else {
      console.Text += text + "\n";
    }
    console.Invalidate();
  }
  /*------------------------Degug Console-----------------------------*/



  /*---------------------------Fields-------------------------------*/
  List<Shape> shapes = new List<Shape>(); //Could have used the Control Collection
  List<Connection> edges = new List<Connection>();
  string directoryPath;

  FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel {
    Dock = DockStyle.Fill, // Make the panel fill the entire form
    AutoScroll = true // Enable scrolling if the content exceeds the panel bounds
  };
  /*---------------------------Fields-------------------------------*/

  public GlassPieces(string directoryPath) {
    /*---------------------GlassPices Settings--------------------------*/
    this.directoryPath = directoryPath;
    console.Text += "Debug Information: \n";
    //Controls.Add(console); //if you wan to add teh console 
    /*---------------------GlassPices Settings--------------------------*/


    /*------------------------Add Controls-----------------------------*/
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
    /*------------------------Add Controls-----------------------------*/


    /*------------------------Runn Threads-----------------------------*/
    Thread cal = new Thread(Caculate);
    cal.Start();
    /*------------------------Runn Threads-----------------------------*/
  }


  public void Caculate() {
    Connection conn = Shape.IsConnected(shapes[0], shapes[1]);
    MessageBox.Show(conn.ToString());
  }

}


namespace GlassPieces;
public partial class Form1 : Form {
  public Form1() {
    string directoryPath = @"D:\Users\Admin\Desktop\PROJECTS\GlassPieces\track1\train\1";

    FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel {
      Dock = DockStyle.Fill, // Make the panel fill the entire form
      AutoScroll = true // Enable scrolling if the content exceeds the panel bounds
    };
    Controls.Add(flowLayoutPanel);
    // Get all the XXX.png files and all the XXX.txt files
    foreach (string file in Directory.GetFiles(directoryPath, "*.png")) {
      // Extract the base name of the file (without extension) to use as the corresponding.txt file name
      string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
      string txtFilePath = Path.Combine(directoryPath, fileNameWithoutExtension + ".txt");

      // Check if the corresponding.txt file exists
      if (File.Exists(txtFilePath)) {
        // Instantiate the Shape with the corresponding.png and.txt files
        Shape shape = new Shape(Path.Combine(directoryPath, file), txtFilePath);
        flowLayoutPanel.Controls.Add(shape);
      } 
    }
  }
}


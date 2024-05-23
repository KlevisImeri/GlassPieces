namespace GlassPieces;

static class Program {
  [STAThread]
  static void Main() {
    Application.EnableVisualStyles();
    ApplicationConfiguration.Initialize();
    Application.Run(new Form1());
  }
}
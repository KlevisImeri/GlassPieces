namespace GlassPieces;

static class Program {
  [STAThread]
  static void Main() {
    Application.EnableVisualStyles();
    ApplicationConfiguration.Initialize();
    Application.Run(new GlassPieces(@"D:\Users\Admin\Desktop\PROJECTS\GlassPieces\track1\train\1"));
  }
}
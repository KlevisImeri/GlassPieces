using System.Runtime.InteropServices;

namespace GlassPieces;

static class Program {
 
  [STAThread]

  [DllImport("kernel32.dll")]
  static extern bool AllocConsole();

  static void Main() {
    AllocConsole();
    Application.EnableVisualStyles();
    ApplicationConfiguration.Initialize();
    Application.Run(new GlassPieces(@"D:\Users\Admin\Downloads\san_vitale_challenge_dataset\track1\train\1"));
  }
}
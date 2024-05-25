namespace GlassPieces;

public class Connection { 
  public Shape X;
  public PointF x; //Shape X rotation point
  public Shape Y;
  public PointF y; //Shape Y rotation point
  public float angle;  //Angle of the rotation [rad]
  public int connectedPoints;  //Sum up of distances for each point 
                           //on shape X with the corressponding 
                           //one in shape Y where x and y are the
                           //the starging points correspoindingly.

  public override string ToString() {
    return $"[{x},{y},Angle={angle},NrPoints={connectedPoints}]";
  }
}
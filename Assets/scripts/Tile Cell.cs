
using UnityEngine;

public class TileCell : MonoBehaviour
{
 
    public Vector2Int Coordinates { get;set; }
    public  Tile tile { get;set; }
    public bool empty=>tile==null;// if thier is no cell in the tile 
    public bool occupide=>tile!=null;
}

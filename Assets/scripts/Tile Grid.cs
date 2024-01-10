
using System.Data;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
  public TileRow[] rows { get; private set; }
    public TileCell[] cells { get; private set; }

    public int size=>cells.Length;
    public int hight => rows.Length;
    public int width => size/hight;
    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }
    private void Start()
    {
        for(int y=0;y<rows.Length; y++)
        {
            for(int x=0;x<rows[y].cells.Length; x++)
            {
                rows[y].cells[x].Coordinates = new Vector2Int(x, y);
            }
        }
    }
    public TileCell getcell(int X,int Y)
    {
        if (X >= 0 && X < width && Y >= 0 && Y < hight)
        {
            return rows[Y].cells[X];
        }
        else
        {
            return null;    
        }
      
    }
    public TileCell getcell(Vector2Int coordinates)
    {
        return getcell(coordinates.x, coordinates.y);
    }
    public TileCell getadjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.Coordinates;
        coordinates.x += direction.x;
        coordinates.y -=direction.y;

        return getcell(coordinates);

    }
   
    public TileCell Getrandomemptycell()
    {
     int index=Random.Range(0,cells.Length);
     int startingIndex=index;
        while (cells[index].occupide)
        {
            index++;
            if(index>= cells.Length)
            {
                index = 0;
            }
            if (index == startingIndex)
            {
                return null;
            }
        }
        return cells[index];
    }


}


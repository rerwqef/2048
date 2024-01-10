using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBord : MonoBehaviour
{
    public GameManger gameManger;
    public Tile tileprefab;
    public TileState[] tilestates;

   private  TileGrid grid;
    private List<Tile> tiles;
    private bool waiting;

    private void Awake()
    {
        grid =GetComponentInChildren<TileGrid>();
        tiles=new List<Tile>(16);
    }
   public void clearBord()
    {
        foreach(var cell in grid.cells)
        {
            cell.tile = null;
        }
        foreach(var tile in tiles)
        {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
    }
    public   void CreateTile()
    {
      Tile tile=  Instantiate(tileprefab, grid.transform);
        tile.setstate(tilestates[0], 2);
        tile.spwan(grid.Getrandomemptycell());
        tiles.Add(tile);
    }
    private void Update()
    {
        if (!waiting)
        { 
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movetiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movetiles(Vector2Int.down, 0, 1, grid.hight - 2, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movetiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movetiles(Vector2Int.right, grid.hight - 2, -1, 0, 1);
        }
    }

    }
    private void movetiles(Vector2Int directtion,int startindeX,int incrementX, int startindeY, int incrementY)
    {
        bool changed = false;
        for(int x=startindeX;x>=0&&x<grid.width; x+=incrementX)
        {
            for(int y=startindeY;y>=0&&y<grid.hight; y+=incrementY)
            {
             TileCell cell= grid.getcell(x,y);
                if (cell.occupide)
                {
             changed |=   moveTile(cell.tile, directtion);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(waitforchanges());
        }
    }
    private bool moveTile(Tile tile,Vector2Int direction)
    {
        TileCell newcell = null;
        TileCell adjacent=grid.getadjacentCell(tile.cell,direction); 

       while(adjacent!=null)
        {
            if(adjacent.occupide)
            {
                if (canmeerge(tile, adjacent.tile)){
                    merge(tile,adjacent.tile);
                    return true;
                }
                break;
            }
            newcell=adjacent;
            adjacent=grid.getadjacentCell(adjacent,direction);

        }

       if(newcell != null)
        {
            tile.moveto(newcell);
            return true;
        } 
       return false;
    }


    private bool canmeerge(Tile a ,Tile b)
    {
        return a.num == b.num&&!b.locked;
    }

    private void merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.merge(b.cell);

        int index = Mathf.Clamp(indexof(b.state) + 1, 0, tilestates.Length - 1);
        int num= b.num * 2;
        b.setstate(tilestates[index], num);

        gameManger.incresescore(num);
    }
    private int indexof(TileState state)
    {
        for (int i = 0; i < tilestates.Length; i++)
        {
            if (state == tilestates[i])
            {
                return i;
            }
        }
        return -1;
    }

    private IEnumerator waitforchanges()
    {
        waiting = true;

        yield return new WaitForSeconds(0.1f);
        waiting = false;
        foreach(var tile in tiles)
        {
            tile.locked = false;
        }
        if (tiles.Count != grid.size)
        {
            CreateTile();
        }
        if (checkforgameover())
        {
            gameManger.gameover();
        }
    }
    private bool checkforgameover()
    {
        if(tiles.Count != grid.size)
        {
                 return false;
        }
        foreach(var tile in tiles)
        {
            TileCell up = grid.getadjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.getadjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.getadjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.getadjacentCell(tile.cell, Vector2Int.right);

            if (up != null && canmeerge(tile, up.tile))
            {
                return false;
            }
            if (down != null && canmeerge(tile, down.tile))
            {
                return false;
            }
            if (left != null && canmeerge(tile, left.tile))
            {
                return false;
            }
            if (right != null && canmeerge(tile, right.tile))
            {
                return false;
            }
        }

        return true;
    }
}

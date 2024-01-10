using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
  public TileState state { get; private set; }  
 public TileCell cell { get; private set; }
    public int num { get; private set; }
    public bool locked { get;  set; }

    private Image backGround;
    private TextMeshProUGUI text;
    private void Awake()
    {
        backGround = GetComponent<Image>();
        text=GetComponentInChildren<TextMeshProUGUI>();
    }
    public void setstate(TileState state,int num)
    {
        this.state = state;
        this.num = num;
        backGround.color = state.backgroundColor;
        text.color = state.textColor;
        text.text=num.ToString();
    }   
    public void spwan(TileCell cell)
    {
        if (this.cell != null)
        {
            {
                this.cell.tile = null;
            }
        }
         this.cell = cell;
        this.cell.tile = this;
        transform.position = cell.transform.position;
    }

    public void moveto(TileCell cell)
    {
        if (this.cell != null)
        {
            {
                this.cell.tile = null;
            }
        }
        this.cell = cell;
        this.cell.tile = this;
        StartCoroutine(animate(cell.transform.position,false));
    }
    private IEnumerator animate(Vector3 to,bool merging)
    {
        float elapsed = 0f;

        float durartion = 0.1f;
        Vector3 from = transform.position;
        while(elapsed<durartion)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / durartion);
            elapsed += Time.deltaTime;
            yield return null;
        }

           transform.position = to;
        if(merging)
        {
            Destroy(gameObject);
        }
    }
    public void merge(TileCell cell)
    {
        if(this.cell != null)
        {
            this.cell.tile = null;
        }
        this.cell = null;
        cell.tile.locked = true;
        StartCoroutine(animate(cell.transform.position,true));
    }    
}

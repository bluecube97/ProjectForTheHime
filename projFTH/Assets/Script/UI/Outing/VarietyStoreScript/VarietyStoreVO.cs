using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListVO { 
    public int ITEMNO { get; set; }
    public string ITEMNAME { get; set; }
    public int ITEMPR { get; set; }

    public ItemListVO()
    {
    }
    public ItemListVO(int itemno, string itemnm, int itempr)
    {
        ITEMNO = itemno;
        ITEMNAME = itemnm;
        ITEMPR = itempr;
    }
}
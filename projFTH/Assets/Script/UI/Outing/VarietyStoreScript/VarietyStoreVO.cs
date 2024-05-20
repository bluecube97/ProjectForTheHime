using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListVO { 
    public string ITEMNO { get; set; }
    public string TYPEID { get; set; }
    public string ITEMNAME { get; set; }
    
    public string ITEMDESC { get; set; }

    public string ITEMPR { get; set; }

    public ItemListVO()
    {
    }
    public ItemListVO(string itemno, string typeid, string itemnm, string itemsdesc, string itempr)
    {
        ITEMNO = itemno;
        TYPEID = typeid;
        ITEMNAME = itemnm;
        ITEMDESC = itemsdesc;
        ITEMPR = itempr;
        
    }
}
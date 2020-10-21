using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnGround : MonoBehaviour
{
    public Item item;

    private void Start()
    {
        //randomize this items properties

        item = new Item(Binary.EmptyItem);
    }
}

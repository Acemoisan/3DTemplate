/*
 *  Copyright © 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using UnityEngine;
using System;

[Serializable]
public class SerializedItem
{
    public string itemName;
    public int count;
    public int itemLevel;

    public SerializedItem(string itemName)
    {
        this.itemName = itemName;
        this.count = 1;
    }

    public SerializedItem(string itemName, int count, int itemLevel = 0)
    {
        this.itemName = itemName;
        this.count = count;
        this.itemLevel = itemLevel;
    }
}
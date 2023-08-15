using System.Collections.Generic;
using UnityEngine;

public interface IMergeAble
{
    void Merge(List<Item> itemList, string receivedName, Vector3 mergePosition);
}

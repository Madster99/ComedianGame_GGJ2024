using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetRandomIndex<T>
{
    public static T GetRandomIndexFromList(List<T> list)
    {
        int i = Random.Range(0, list.Count);
        return list[i];
    }
}

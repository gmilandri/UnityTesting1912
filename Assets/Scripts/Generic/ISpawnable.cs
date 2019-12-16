using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ISpawnable
{
    bool IsBeyondMinimumDistance(ISpawnable other);

    bool HasBeenGathered();

    GameObject ThisGameObject();

    float MyMinimumDistance();

    void InstantiateThis(float positiveMax, List<ISpawnable> spawns);

    void SetAside();

}

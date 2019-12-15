using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ISpawnable
{

    bool IsBeyondMinimumDistance(ISpawnable other);

    GameObject ThisGameObject();

    float MyMinimumDistance();

    void InstantiateThis(float negativeMax, float positiveMax, List<ISpawnable> spawns);

}

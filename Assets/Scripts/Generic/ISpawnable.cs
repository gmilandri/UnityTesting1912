using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ISpawnable
{

    bool HasBeenGathered();

    GameObject ThisGameObject();

    void InstantiateThis(List<ISpawnable> spawns);

    void SetAside();

}

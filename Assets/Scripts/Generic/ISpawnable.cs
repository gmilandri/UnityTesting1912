using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ISpawnable
{

    GameObject ThisGameObject();

    void InstantiateThis(List<ISpawnable> spawns);

    void SetAside();

}

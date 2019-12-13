using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour, ICreature {

    [HideInInspector]
    public IMovement HumanMovement { get; private set; }

    void Start()
    {
        HumanMovement = new WalkMovement();
    }

    void Update()
    {

    }

}

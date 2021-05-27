using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use for positionate coins and bin in differents lanes

public class ChangeLane : MonoBehaviour
{
    public void PositionLane(int randomLane)
    {
        transform.position = new Vector3(randomLane, transform.position.y, transform.position.z);
    }
}

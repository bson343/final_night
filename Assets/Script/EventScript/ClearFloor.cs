using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearFloor : MonoBehaviour
{
    // Start is called before the first frame update
    public void ClearFloorPlus()
    {
        GameOverView.clearfloor++;
        Debug.Log(GameOverView.clearfloor);
    }
}

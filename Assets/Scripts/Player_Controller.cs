using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public static Player_Controller Instance;

    private void Awake()
    {
        Instance = this;
    }
}

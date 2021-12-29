using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCardFilled : MonoBehaviour
{
    public bool isOpenCardSlotFilled = false;
    void Start()
    {
        //isOpenCardSlotFilled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.childCount > 0)
        {
            isOpenCardSlotFilled = true;
        }
        else
        {
            isOpenCardSlotFilled = false;
        }

    }
}

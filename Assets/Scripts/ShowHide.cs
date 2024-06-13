using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHide : MonoBehaviour
{
    private bool isActive = false;
    public void Click()
    {
        if (isActive)
        {
            isActive = false;
        }
        else
        {
            isActive = true;
        }

        gameObject.SetActive(isActive);
    }
}

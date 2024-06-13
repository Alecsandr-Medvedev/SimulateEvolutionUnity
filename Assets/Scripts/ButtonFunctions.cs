using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    [UnityEngine.SerializeField]
    private TMPro.TextMeshProUGUI nameStart;

    public void ChangeName()
    {
        nameStart.SetText("Перезапуск");
    }
}

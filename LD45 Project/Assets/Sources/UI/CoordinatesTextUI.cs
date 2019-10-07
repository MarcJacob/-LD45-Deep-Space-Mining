using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoordinatesTextUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    void Update()
    {
        text.text = (int)Camera.main.transform.position.x + " ; " + (int)Camera.main.transform.position.y;
    }
}

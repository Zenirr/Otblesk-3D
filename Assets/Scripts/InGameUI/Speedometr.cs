using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Speedometr : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Vehicle machine;
    private Rigidbody body;
    private void Start()
    {
        body = machine.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        text.text = Mathf.FloorToInt(body.linearVelocity.magnitude*3.6f).ToString();
    }
}

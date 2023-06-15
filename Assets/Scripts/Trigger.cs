using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public string checkTag;
    public UnityEvent onTriggerEnter, onTriggerStay, onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER");
        if (other.gameObject.CompareTag(checkTag))
            onTriggerEnter.Invoke();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(checkTag))
            onTriggerStay.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(checkTag))
            onTriggerExit.Invoke();
    }

}

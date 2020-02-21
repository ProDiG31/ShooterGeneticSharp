using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OutZoneHandler : MonoBehaviour
{
    public UnityEvent OnTriggerExitEvent;

    void OnTriggerExit(Collider other)
    {
        Debug.Log("TriggerExit" + other.name);
        OnTriggerExitEvent.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityEvent Interact;

    public void OnUse()
    {
        Interact.Invoke();
    }
}

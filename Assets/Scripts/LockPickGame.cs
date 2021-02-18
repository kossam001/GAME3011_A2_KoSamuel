using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LockPickGame : MonoBehaviour
{
    [SerializeField] private RectTransform outerPanel;
    [SerializeField] private RectTransform marker;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float spotRadius;

    private bool isTurning = false;

    private void Start()
    {
        marker.localPosition = new Vector2(
                Random.Range(-outerPanel.sizeDelta.x * 0.5f, outerPanel.sizeDelta.x * 0.5f),
                Random.Range(-outerPanel.sizeDelta.x * 0.5f, outerPanel.sizeDelta.x * 0.5f)                
            );

    }

    public void OnLockPickTurn(InputValue vector2)
    {
        if (vector2.Get<Vector2>().x > 0.1f)
        {
            StopCoroutine(nameof(StartTurning));
            isTurning = true;
            StartCoroutine(StartTurning(1));
        }
        else if (vector2.Get<Vector2>().x < -0.1f)
        {
            StopCoroutine(nameof(StartTurning));
            isTurning = true;
            StartCoroutine(StartTurning(-1));
        }
        else
        {
            StopCoroutine(nameof(StartTurning));
            isTurning = false;
        }
    }

    private IEnumerator StartTurning(int direction)
    {
        while (isTurning)
        {
            outerPanel.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    //public void OnLockPickTurn(InputValue Vec)
    //{
    //    if (button.isPressed)
    //    {
    //        InvokeRepeating(nameof(StartTurningCW), 0, 0.1f);
    //    }
    //    else
    //    {
    //        CancelInvoke(nameof(StartTurning));
    //    }
    //}

    //public void OnLockPickTurnCCW(InputValue button)
    //{
    //    if (button.isPressed)
    //    {
    //        InvokeRepeating(nameof(StartTurningCW), 0, 0.1f);
    //    }
    //    else
    //    {
    //        CancelInvoke(nameof(StartTurning));
    //    }
    //}

    //private void StartTurningCW()
    //{
    //    outerPanel.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    //}

    //private void StartTurningCCW()
    //{
    //    outerPanel.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
    //}
}

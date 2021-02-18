using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LockPickGame : MonoBehaviour
{
    [SerializeField] private RectTransform outerPanel;
    [SerializeField] private RectTransform marker;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private RectTransform pick1;
    [SerializeField] private Image pick1Image;

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

    public void OnCursorMove(InputValue position)
    {
        Vector2 cursorPos = position.Get<Vector2>();
        cursor.transform.position = cursorPos;

        Vector2 markerPos = marker.position;

        float distance = Vector2.Distance(cursorPos, markerPos);
        float quarterDistance = cursor.sizeDelta.x * 0.25f;

        cursor.gameObject.GetComponent<Image>().color = new Color(distance / quarterDistance, (quarterDistance * 4 - distance) / (quarterDistance * 4), 0.0f);

        // Rotate pick in direction of cursor
        float angle = Mathf.Atan2(cursorPos.y - pick1.position.y, cursorPos.x - pick1.position.x) * Mathf.Rad2Deg;
        pick1.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

        pick1.GetComponent<Slider>().value = Vector2.Distance(cursorPos, pick1.position) / pick1.sizeDelta.x;
        pick1Image.color = new Color(distance / quarterDistance, (quarterDistance * 4 - distance) / (quarterDistance * 4), 0.0f);
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

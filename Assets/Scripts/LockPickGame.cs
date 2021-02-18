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
    [SerializeField] private RectTransform pick2;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float spotRadius;
    [SerializeField] private float goalRotation;
    
    private bool isTurning = false;
    private bool gameOver = false;

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

        // Using the quarter distance to shift from green to yellow to red
        float quarterDistance = cursor.sizeDelta.x * 0.25f;

        // cursor.gameObject.GetComponent<Image>().color = new Color(distance / quarterDistance, (quarterDistance * 4 - distance) / (quarterDistance * 4), 0.0f);

        // Rotate pick in direction of cursor
        float angle = Mathf.Atan2(cursorPos.y - pick1.position.y, cursorPos.x - pick1.position.x) * Mathf.Rad2Deg;
        pick1.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

        pick1.GetComponent<Slider>().value = Vector2.Distance(cursorPos, pick1.position) / pick1.sizeDelta.x;
        pick1Image.color = new Color(distance / quarterDistance, (quarterDistance * 4 - distance) / (quarterDistance * 4), 0.0f);
    }

    public void OnLockPickTurn(InputValue vector2)
    {
        if (gameOver) return;

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
            isTurning = false;
            StartCoroutine(ReturnRotation());
            StopCoroutine(nameof(StartTurning));
        }
    }

    private IEnumerator StartTurning(int direction)
    {
        while (isTurning)
        {
            outerPanel.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
            pick2.rotation = outerPanel.rotation;

            yield return null;
        }
    }

    private IEnumerator ReturnRotation()
    {
        while (!isTurning && Mathf.Abs(pick2.rotation.z) >= 0.0f)
        {
            outerPanel.rotation = Quaternion.RotateTowards(outerPanel.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), rotationSpeed * Time.deltaTime);
            pick2.rotation = outerPanel.rotation;

            yield return null;
        }
    }

    private void BreakLockPick()
    {

    }

    private void Unlock()
    {
        if (Mathf.Abs(outerPanel.rotation.z - goalRotation) <= 0.01f)
        {
            StopAllCoroutines();
        }
    }
}

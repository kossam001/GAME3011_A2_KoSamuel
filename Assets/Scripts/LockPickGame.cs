using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LockPickGame : MonoBehaviour
{
    [Header("Minigame Components")]
    [SerializeField] private RectTransform pivot;
    [SerializeField] private RectTransform marker;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private RectTransform pick1;
    [SerializeField] private RectTransform pick2;
    [SerializeField] private Image pick1Image;

    [Header("Minigame Parameters")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float spotRadius;
    [SerializeField] private float goalRotation;

    [Header("Minigame UI")]
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject resultScreen;
    [SerializeField] private TMP_Text resultText;

    private bool isTurning = false;
    private bool gameOver = false;
    private int lives = 3;
    private Vector2 cursorPos;

    private void Start()
    {
        marker.localPosition = new Vector2(
                Random.Range(-pivot.sizeDelta.x * 0.5f, pivot.sizeDelta.x * 0.5f),
                Random.Range(-pivot.sizeDelta.x * 0.5f, pivot.sizeDelta.x * 0.5f)                
            );

    }

    // Need update to check for when player does not move
    private void Update()
    {
        DistanceCheck();
    }

    // Only happens when mouse moves
    public void OnCursorMove(InputValue position)
    {
        cursorPos = position.Get<Vector2>();
        cursor.transform.position = cursorPos;
    }

    public void DistanceCheck()
    {
        Vector2 markerPos = marker.position;
        float distance = Vector2.Distance(cursorPos, markerPos);

        // Using the quarter distance to shift from green to yellow to red
        float quarterDistance = cursor.sizeDelta.x * 0.25f;

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
            pivot.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
            pick2.rotation = pivot.rotation;

            BreakLockPick();
            Unlock();

            yield return null;
        }
    }

    private IEnumerator ReturnRotation()
    {
        while (!isTurning && Mathf.Abs(pick2.rotation.z) >= 0.0001f)
        {
            pick2.rotation = pivot.rotation = Quaternion.RotateTowards(pivot.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), rotationSpeed * Time.deltaTime);

            BreakLockPick();

            yield return null;
        }
    }

    private void BreakLockPick()
    {
        if (gameOver) return;

        Vector2 markerPos = marker.position;
        float distance = Vector2.Distance(cursorPos, markerPos);

        if (distance >= cursor.sizeDelta.x * 0.5f + marker.sizeDelta.x * 0.5f)
        {
            StopAllCoroutines();
            pick2.rotation = pivot.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

            lives -= 1;
            livesText.text = lives.ToString();

            if (lives <= 0)
            {
                GameOver("FAIL");
            }
        }
    }

    private void Unlock()
    {
        if (Mathf.Abs(pivot.rotation.eulerAngles.z - goalRotation) <= 0.1f)
        {
            GameOver("SUCCESS");
        }
    }

    private void GameOver(string message)
    {
        gameOver = true;
        StopAllCoroutines();

        resultScreen.SetActive(true);
        resultText.text = message;
    }

    public void OnUse()
    {
        if (!gameOver) return;

        SceneManager.UnloadSceneAsync("LockpickScene");
    }
}

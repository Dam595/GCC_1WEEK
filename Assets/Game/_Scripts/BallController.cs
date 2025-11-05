using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float maxPower = 10f;
    [SerializeField] private float power = 2f;
    [SerializeField] private float maxSpeed = 4f;

    private bool isDragging = false;
    private bool isInHole = false;
    private Camera mainCamera;
    private Vector2 mousePosition;
    private float distance;
    private bool shouldShoot = false;
    private Vector2 releasePos;

    public event Action<Vector2, float> OnAiming;
    public event Action OnAimingEnd;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Nhiệm vụ: Xử lý việc kéo và thả chuột để xác định hướng và lực bắn cho quả bóng.
    /// </summary>
    private void Update()
    {
        if (rb.velocity.magnitude > 0.1f) return;

        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(transform.position, mousePosition);

        if (Input.GetMouseButtonDown(0) && distance <= 0.5f)
        {
            isDragging = true;
        }
        if (isDragging)
        {
            OnAiming?.Invoke(mousePosition, distance);
        }
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            releasePos = mousePosition;
            shouldShoot = true;
            isDragging = false;
            OnAimingEnd?.Invoke();
        }

        if (isDragging)
        {
            Debug.DrawLine(transform.position, mousePosition, Color.red);
        }
    }

    private void FixedUpdate()
    {
        if (shouldShoot)
        {
            DragRelease(releasePos);
            shouldShoot = false;
        }

        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        if (rb.velocity.magnitude < 0.05f)
            rb.velocity = Vector2.zero;
    }

    private void DragRelease(Vector2 pos)
    {
        float distance = Vector2.Distance(transform.position, pos);

        if (distance < 1f)
        {
            return;
        }

        Vector2 direction = (Vector2)transform.position - pos;
        Vector2 force = direction.normalized * Mathf.Min(distance * power, maxPower);
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class BallTrajectoryRenderer : MonoBehaviour
{
    [SerializeField] private BallController ball;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask wallMask;

    private LineRenderer lineRenderer;
    private TrajectoryCalculator trajectoryCalculator;
    /// <summary>
    /// Nhiệm vụ: Vẽ đường bay của quả bóng khi người chơi kéo chuột để xác định hướng và lực bắn.
    /// </summary>
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 0;

        trajectoryCalculator = new TrajectoryCalculator(maxBounces: 2, stepTime: 0.1f, maxDistance: 3f, wallMask: wallMask);
    }

    private void OnEnable()
    {
        ball.OnAiming += DrawTrajectory;
        ball.OnAimingEnd += ClearTrajectory;
    }

    private void OnDisable()
    {
        ball.OnAiming -= DrawTrajectory;
        ball.OnAimingEnd -= ClearTrajectory;
    }

    private void DrawTrajectory(Vector2 mousePos, float distance)
    {
        Vector2 direction = (Vector2)ball.transform.position - mousePos;
        Vector2 force = direction.normalized * Mathf.Min(distance * 2f, 3f);
        Vector2 startVel = force / rb.mass;

        List<Vector3> points = trajectoryCalculator.CalculatePoints(ball.transform.position, startVel);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
    }
}

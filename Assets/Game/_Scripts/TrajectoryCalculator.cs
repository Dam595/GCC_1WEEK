using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCalculator
{
    private readonly int maxBounces;
    private readonly float stepTime;
    private readonly float maxDistance;
    private readonly LayerMask wallMask;

    /// <summary>
    /// Nhiệm vụ: Tính toán các điểm trên quỹ đạo của một vật thể di chuyển trong không gian 2D, bao gồm các lần nảy lên khi va chạm với tường.
    /// </summary>
    public TrajectoryCalculator(int maxBounces, float stepTime, float maxDistance, LayerMask wallMask)
    {
        this.maxBounces = maxBounces;
        this.stepTime = stepTime;
        this.maxDistance = maxDistance;
        this.wallMask = wallMask;
    }

    public List<Vector3> CalculatePoints(Vector2 startPos, Vector2 startVel)
    {
        List<Vector3> points = new List<Vector3> { startPos };
        Vector2 currentPos = startPos;
        Vector2 currentVel = startVel;

        for (int bounce = 0; bounce <= maxBounces; bounce++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPos, currentVel.normalized, maxDistance, wallMask);
            if (hit.collider != null)
            {
                points.Add(hit.point);

                currentVel = Vector2.Reflect(currentVel, hit.normal);

                currentPos = hit.point + currentVel.normalized * 0.01f;
            }
            else
            {
                for (int i = 1; i <= 10; i++)
                {
                    Vector2 pos = currentPos + currentVel * stepTime * i;
                    points.Add(pos);
                }
                break;
            }
        }

        return points;
    }
}

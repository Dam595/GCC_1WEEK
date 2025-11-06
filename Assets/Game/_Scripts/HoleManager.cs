using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HoleManager : MonoBehaviour
{
    [SerializeField] private float maxCaptureSpeed = 0.5f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        var ball = other.GetComponent<BallController>();
        if (ball == null) return;

        Vector2 dir = transform.position - other.transform.position;

        if (dir.magnitude < 0.2f && ball.CurrentSpeed < maxCaptureSpeed)
        {
            ball.Stop();
            GameManager.Instance.WinGame();
            other.gameObject.SetActive(false);
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HoleManager : MonoBehaviour
{
    [SerializeField] private float maxCaptureSpeed = 0.5f;
    [SerializeField] private GameObject holeParticlePrefab;
    [SerializeField] private float delayBeforeWin = 2f;

    private void OnTriggerStay2D(Collider2D other) 
    { 
        if (!other.CompareTag("Ball")) return; 
        var ball = other.GetComponent<BallController>(); 
        if (ball == null) return; 
        Vector2 dir = transform.position - other.transform.position; 
        if (dir.magnitude < 0.2f && ball.CurrentSpeed < maxCaptureSpeed) 
        { 
            ball.Stop();
            if (holeParticlePrefab != null)
            {
                GameObject fx = Instantiate(holeParticlePrefab, ball.transform.position, Quaternion.identity);
                Destroy(fx, 1.5f);
            }
            GameManager.Instance.WinGame();
            ball.transform.position = new Vector3(99, 99, 0);
        }
    }
}

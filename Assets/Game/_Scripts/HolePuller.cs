using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HolePuller : MonoBehaviour
{
    [SerializeField] private float pullForce = 10f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector2 dir = (transform.position - other.transform.position);
        rb.AddForce(dir.normalized * pullForce);
    }
}

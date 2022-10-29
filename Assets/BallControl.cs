using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public Vector3 Position => rb.position;
    public bool IsMoving => rb.velocity != Vector3.zero;

    private void Awake()
    {
        if (rb == null)
            rb.GetComponent<Rigidbody>();
    }

    internal void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero && rb.velocity.magnitude < 0.5f)
                rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FinishTag"))
        {
            GameManager.instance.GameOver();
        }
    }
}

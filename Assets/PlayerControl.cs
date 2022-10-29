using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]BallControl ball;
    [SerializeField]LayerMask ballLayer;
    [SerializeField] LayerMask rayLayer;
    [SerializeField]Transform cameraPivot;
    [SerializeField]Camera cam;
    [SerializeField]Vector2 camSensitivity;
    [SerializeField] float shootForce;
    public GameObject arrow;

    Vector3 lastMousePosition;
    float ballDistance;

    bool isShooting = false;

    Vector3 forceDir;
    float forceFactor;

    private void Start()
    {
        ballDistance = Vector3.Distance(
            cam.transform.position, ball.Position) + 1;
    }

    private void Update()
    {
        if (ball.IsMoving)
        {
            return;
        }

        if(this.transform.position != ball.Position)
            this.transform.position = ball.Position;

        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, 1000f, ballLayer))
            {
                arrow.SetActive(true);
                isShooting = true;
            }
        }

        if (Input.GetMouseButton(0) && isShooting)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, ballDistance * 2, rayLayer))
            {
                Debug.DrawLine(ball.Position, hit.point);

                var forceVector = ball.Position - hit.point;
                forceDir  = forceVector.normalized;
                var forceMagnitude = forceVector.magnitude;
                Debug.Log(forceMagnitude);
                forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 5);
                forceFactor = forceMagnitude/ 5;
            }

            arrow.transform.LookAt(transform.position + forceDir);
            arrow.transform.localScale = new Vector3(50f * forceFactor, 50f * forceFactor, 50 * forceFactor);
        }

        if (Input.GetMouseButton(0) && !isShooting)
        {
            var current = cam.ScreenToViewportPoint(Input.mousePosition);
            var last = cam.ScreenToViewportPoint(lastMousePosition);
            var delta =current -last;
                
            cameraPivot.transform.RotateAround(
                ball.Position,
                Vector3.up,
                delta.x * camSensitivity.x);
                
            cameraPivot.transform.RotateAround(
                ball.Position,
                cam.transform.right,
                -delta.y * camSensitivity.y);

            var angle = Vector3.SignedAngle(
                Vector3.up, cam.transform.up, cam.transform.right);
                
            if (angle < 3)
            {
                cameraPivot.transform.RotateAround(
                    ball.Position,
                    cam.transform.right,
                    3 - angle);
            }                    
            else if (angle > 65){
                cameraPivot.transform.RotateAround(
                    ball.Position,
                    cam.transform.right,
                    65 - angle);
            }            
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isShooting)
            {
                GameManager.instance.score++;
                GameManager.instance.UpdateText();
            }
            ball.AddForce(forceDir * shootForce * forceFactor);
            forceFactor = 0;
            forceDir = Vector3.zero;
            isShooting = false;
            arrow.SetActive(false);

        }            
        lastMousePosition = Input.mousePosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovementScript : MonoBehaviour
{
    Rigidbody droneRb;                  // rigidbody attached to drone
    public float verticalForce;         // numeric value indicating force applied to movement (up and down)
    float speed = 500.0f;               // numeric value indicating speed of movement (forward and backwards)
    float tiltValue = 0;                // numeric value controlling tilting movement
    float tiltVelocity;                 // numeric value indicating speed of tilting movement
    float wantedYRotation;
    float currentYRotation;
    float rotateAmountByKeys = 2.5f;
    float rotationYVelocity;
    float sideMovement = 300.0f;        // numeric value indicating the swerving movement
    float sideTiltValue;                // numeric value controlling lateral tilting
    float sideTiltVelocity;             // numeric value indicating speed of lateral tilting   
    Vector3 smoothingVelocity;          // velocity smoothing damp to zero

    void Awake()
    {
        droneRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        VerticalMovement();
        HorizontalMovement();
        DroneRotation();
        ClampingSpeedValues();
        Swerve();
        
        droneRb.AddRelativeForce(Vector3.up * verticalForce);
        droneRb.rotation = Quaternion.Euler(new Vector3(tiltValue, currentYRotation, sideTiltValue));
    }

    void VerticalMovement()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K))
            {
                droneRb.velocity = droneRb.velocity;
            }
            if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
            {
                droneRb.velocity = new Vector3(droneRb.velocity.x, Mathf.Lerp(droneRb.velocity.y, 0, Time.deltaTime * 5), droneRb.velocity.z);
                verticalForce = 281;
            }
            if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)))
            {
                droneRb.velocity = new Vector3(droneRb.velocity.x, Mathf.Lerp(droneRb.velocity.y, 0, Time.deltaTime * 5), droneRb.velocity.z);
                verticalForce = 110;
            }
            if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
            {
                verticalForce = 410;
            }
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            verticalForce = 135;
        }

        if (Input.GetKey(KeyCode.I))
        {
            verticalForce = 450;
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
            {
                verticalForce = 500;
            }
        }
        else if (Input.GetKey(KeyCode.K))
        {
            verticalForce = -200;
        }
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f))
        {
            verticalForce = 98.1f;
        }
    }

    void HorizontalMovement()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            droneRb.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * speed);
            tiltValue = Mathf.SmoothDamp(tiltValue, 20 * Input.GetAxis("Vertical"), ref tiltVelocity, 0.1f);
        }
    }

    void DroneRotation()
    {
        if (Input.GetKey(KeyCode.J))
        {
            wantedYRotation -= rotateAmountByKeys;
        }
        if (Input.GetKey(KeyCode.L))
        {
            wantedYRotation += rotateAmountByKeys;
        }
        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }

    void ClampingSpeedValues()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            droneRb.velocity = Vector3.ClampMagnitude(droneRb.velocity, Mathf.Lerp(droneRb.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            droneRb.velocity = Vector3.ClampMagnitude(droneRb.velocity, Mathf.Lerp(droneRb.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            droneRb.velocity = Vector3.ClampMagnitude(droneRb.velocity, Mathf.Lerp(droneRb.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            droneRb.velocity = Vector3.SmoothDamp(droneRb.velocity, Vector3.zero, ref smoothingVelocity, 0.95f);
        }
    }

    void Swerve()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            droneRb.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * sideMovement);
            sideTiltValue = Mathf.SmoothDamp(sideTiltValue, -20 * Input.GetAxis("Horizontal"), ref sideTiltVelocity, 0.1f);
        }
        else
        {
            sideTiltValue = Mathf.SmoothDamp(sideTiltValue, 0, ref sideTiltVelocity, 0.1f);
        }
    }
}

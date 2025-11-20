using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public float accel;
    public float turnSpeed;

    public Transform carModel;
    private UnityEngine.Vector3 startModelOffset;

    public bool canControl;

    public float groundCheckRate;
    private float lastGroundCheckTime;

    private float curYRotation;

    private bool accelInput;
    private float turnInput;

    public TrackZone curTrackZone;
    public int zonesPassed;
    public int racePosition;
    public int curLap;


    public Rigidbody rig;

    void Start()
    {
        startModelOffset = carModel.transform.localPosition;
        GameManager.instance.cars.Add(this);
        transform.position = GameManager.instance.spawnPoints[GameManager.instance.cars.Count - 1].position;
    }


    void Update()
    {
        if(!canControl)
            turnInput = 0.0f;

        float turnRate = UnityEngine.Vector3.Dot(rig.linearVelocity.normalized, carModel.forward);
        turnRate = Mathf.Abs(turnRate);

        curYRotation += turnInput * turnSpeed * turnRate * Time.deltaTime;

        carModel.position = transform.position + startModelOffset;
        //carModel.eulerAngles = new UnityEngine.Vector3(0, curYRotation, 0);

        CheckGround();
    }

    void FixedUpdate()
    {   

        if(!canControl)
        return;

        if(accelInput == true)
        {
            rig.AddForce(carModel.forward * accel, ForceMode.Acceleration);
        }
    }

    void CheckGround () 
    {
        Ray ray = new Ray(transform.position + new UnityEngine.Vector3(0, -0.75f, 0), UnityEngine.Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1.0f))
        {
            carModel.up = hit.normal;
        }
        else
        {
            carModel.up = UnityEngine.Vector3.up;
        }

        carModel.Rotate(new UnityEngine.Vector3(0, curYRotation, 0), Space.Self);
    }

    public void OnAccelInput (InputAction.CallbackContext context) 
    {
        if (context.phase == InputActionPhase.Performed)
        accelInput = true;
        else
        accelInput = false;
    }

    public void OnTurnInput (InputAction.CallbackContext context)
    {
        turnInput = context.ReadValue<float>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    private float motor;
    private float steering;
    private float speed;
    private float brakeTorque;
    private bool Revers = false;
    PhotonView view;
    // Start is called before the first frame update
    private void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            transform.GetChild(2).GetComponent<Camera>().enabled = true;
        }

        GetComponent<Rigidbody>().centerOfMass = transform.GetChild(3).localPosition;
    }
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        if (view.IsMine)
        {
            speed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            brakeTorque = 0;


            if (Input.GetKey(KeyCode.W))
            {
                motor = maxMotorTorque * Input.GetAxis("Vertical");
                if (speed < 1) Revers = false;
            }
            else
                motor = 0;


            if (Input.GetKey(KeyCode.S))
            {
                if (speed < 1) Revers = true;

                if (Revers)
                {
                    brakeTorque = 0;
                    motor = maxMotorTorque / 3 * Input.GetAxis("Vertical");
                }
                else
                {
                    brakeTorque = 2000;
                    motor = 0;
                }


            }

            steering = maxSteeringAngle * Input.GetAxis("Horizontal");


            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                    axleInfo.leftWheel.brakeTorque = brakeTorque;
                    axleInfo.rightWheel.brakeTorque = brakeTorque;
                }
                if (axleInfo.motor)
                {
                    if (Input.GetKey(KeyCode.Space)) //Ручной тормоз
                    {
                        axleInfo.leftWheel.brakeTorque = 3000;
                        axleInfo.rightWheel.brakeTorque = 3000;
                        axleInfo.leftWheel.motorTorque = 0;
                        axleInfo.rightWheel.motorTorque = 0;
                    }
                    else
                    {
                        axleInfo.leftWheel.motorTorque = motor;
                        axleInfo.rightWheel.motorTorque = motor;
                        axleInfo.leftWheel.brakeTorque = brakeTorque;
                        axleInfo.rightWheel.brakeTorque = brakeTorque;
                    }
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);
            }
            //Debug.Log("Скорость:" + speed*3.6);
            Debug.Log("brakeTorque" + brakeTorque);

            if (Input.GetKey(KeyCode.R))
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }

    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor; // is this wheel attached to motor?
        public bool steering; // does this wheel apply steer angle?
    }
}


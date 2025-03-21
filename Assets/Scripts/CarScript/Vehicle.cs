using AfterGlow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Vehicle : MonoBehaviour
{
    public float recoverySpeed = 96f;

    private class SpringData
    {
        public float CurrentLength;
        public float CurrentVelocity;
    }

    private static readonly Wheel[] s_Wheels = new Wheel[]
    {
            Wheel.FrontLeft, Wheel.FrontRight, Wheel.BackLeft, Wheel.BackRight
    };

    private static readonly Wheel[] s_FrontWheels = new Wheel[] { Wheel.FrontLeft, Wheel.FrontRight };
    private static readonly Wheel[] s_BackWheels = new Wheel[] { Wheel.BackLeft, Wheel.BackRight };

    [SerializeField] private VehicleSettings m_Settings;

    private Transform m_Transform;
    private BoxCollider m_BoxCollider;
    private Rigidbody m_Rigidbody;
    private Dictionary<Wheel, SpringData> m_SpringDatas;


    private float m_SteerInput;
    private float m_AccelerateInput;
    private float m_TorqueInput;
    private bool m_IsBrake;
    public VehicleSettings Settings => m_Settings;
    public Vector3 Forward => m_Transform.forward;
    public Vector3 Velocity => m_Rigidbody.linearVelocity;
    public bool IsBrake => m_IsBrake;

    private void Awake()
    {
        m_Transform = transform;
        InitializeCollider();
        InitializeBody();

        m_SpringDatas = new Dictionary<Wheel, SpringData>();
        foreach (Wheel wheel in s_Wheels)
        {
            m_SpringDatas.Add(wheel, new());
        }
    }

    private void Start()
    {
        InputController.Instance.OnMovementInput += InputController_OnMovementInput;
        InputController.Instance.OnBrakePerformed += Instance_OnBrakePerformed;
        InputController.Instance.OnBrakeCanceled += Instance_OnBrakeCanceled;
    }

    private void Instance_OnBrakeCanceled(object sender, System.EventArgs e)
    {
        m_IsBrake = false;
    }

    private void Instance_OnBrakePerformed(object sender, System.EventArgs e)
    {
        m_IsBrake = true;
    }

    private void InputController_OnMovementInput(object sender, InputController.InputMovementEventArgs e)
    {
        m_SteerInput = Mathf.Clamp(e.inputValues.x, -1.0f, 1.0f);
        m_AccelerateInput = Mathf.Clamp(e.inputValues.y, -1.0f, 1.0f);
    }

    private void OnDestroy()
    {
        InputController.Instance.OnMovementInput -= InputController_OnMovementInput;
        InputController.Instance.OnBrakePerformed -= Instance_OnBrakePerformed;
        InputController.Instance.OnBrakeCanceled -= Instance_OnBrakeCanceled;
    }

    private void FixedUpdate()
    {
        UpdateSuspension();

        UpdateSteering();

        UpdateAccelerate();

        UpdateBrakes();

        UpdateAirResistance();

        UpdateRecovery();

    }

    private void UpdateRecovery()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > 89f)
        {
            Vector3 recoveryTorque = Vector3.Cross(transform.up, Vector3.up);


            recoveryTorque.Normalize();
            m_Rigidbody.AddTorque(recoveryTorque * recoverySpeed, ForceMode.Acceleration);
        }
    }

    public float GetSpringCurrentLength(Wheel wheel)
    {
        return m_SpringDatas[wheel].CurrentLength;
    }
    public float GetSteerInput()
    {
        return m_SteerInput;
    }
    public float GetWheelRotationAngle()
    {
        return m_Settings.SteerCurve.Evaluate(m_Rigidbody.linearVelocity.magnitude);
    }
    #region vehicle initialization
    private void InitializeCollider()
    {
        if (!TryGetComponent(out m_BoxCollider))
        {
            m_BoxCollider = gameObject.AddComponent<BoxCollider>();
        }

        m_BoxCollider.center = Vector3.zero;
        m_BoxCollider.size = new Vector3(m_Settings.Width, m_Settings.Height, m_Settings.Length);
        m_BoxCollider.isTrigger = false;
        m_BoxCollider.enabled = true;
    }

    private void InitializeBody()
    {
        if (!TryGetComponent(out m_Rigidbody))
        {
            m_Rigidbody = gameObject.AddComponent<Rigidbody>();
        }

        const int WHEELS_COUNT = 4;
        m_Rigidbody.mass = m_Settings.ChassiMass + m_Settings.TireMass * WHEELS_COUNT;
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.linearDamping = 0.0f;
        m_Rigidbody.angularDamping = 0.0f;
        m_Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        m_Rigidbody.constraints = RigidbodyConstraints.None;
    }
    #endregion
    // To be called once per physics frame per spring.
    // Updates the spring currentVelocity and currentLength.
    private void CastSpring(Wheel wheel)
    {
        Vector3 position = GetSpringPosition(wheel);

        float previousLength = m_SpringDatas[wheel].CurrentLength;

        float currentLength;

        if (Physics.Raycast(position, -m_Transform.up, out var hit, m_Settings.SpringRestLength))
        {
            currentLength = hit.distance;
        }
        else
        {
            currentLength = m_Settings.SpringRestLength;
        }

        m_SpringDatas[wheel].CurrentVelocity = (currentLength - previousLength) / Time.fixedDeltaTime;
        m_SpringDatas[wheel].CurrentLength = currentLength;
    }

    private Vector3 GetSpringRelativePosition(Wheel wheel)
    {
        Vector3 boxSize = m_BoxCollider.size;
        float boxBottom = boxSize.y * -0.5f;

        float paddingX = m_Settings.WheelsPaddingX;
        float paddingZ = m_Settings.WheelsPaddingZ;

        return wheel switch
        {
            Wheel.FrontLeft => new Vector3(boxSize.x * (paddingX - 0.5f), boxBottom, boxSize.z * (0.5f - paddingZ)),
            Wheel.FrontRight => new Vector3(boxSize.x * (0.5f - paddingX), boxBottom, boxSize.z * (0.5f - paddingZ)),
            Wheel.BackLeft => new Vector3(boxSize.x * (paddingX - 0.5f), boxBottom, boxSize.z * (paddingZ - 0.5f)),
            Wheel.BackRight => new Vector3(boxSize.x * (0.5f - paddingX), boxBottom, boxSize.z * (paddingZ - 0.5f)),
            _ => default,
        };
    }

    private Vector3 GetSpringPosition(Wheel wheel)
    {
        return m_Transform.localToWorldMatrix.MultiplyPoint3x4(GetSpringRelativePosition(wheel));
    }

    private Vector3 GetSpringHitPosition(Wheel wheel)
    {
        Vector3 vehicleDown = -m_Transform.up;
        return GetSpringPosition(wheel) + m_SpringDatas[wheel].CurrentLength * vehicleDown;
    }

    private Vector3 GetWheelRollDirection(Wheel wheel)
    {
        bool frontWheel = wheel == Wheel.FrontLeft || wheel == Wheel.FrontRight;

        if (frontWheel)
        {
            var steerQuaternion = Quaternion.AngleAxis(m_SteerInput * m_Settings.SteerCurve.Evaluate(m_Rigidbody.linearVelocity.magnitude), Vector3.up);
            return steerQuaternion * m_Transform.forward;
        }
        else
        {
            return m_Transform.forward;
        }
    }

    private Vector3 GetWheelSlideDirection(Wheel wheel)
    {
        Vector3 forward = GetWheelRollDirection(wheel);
        return Vector3.Cross(m_Transform.up, forward);
    }

    private Vector3 GetWheelTorqueRelativePosition(Wheel wheel)
    {
        Vector3 boxSize = m_BoxCollider.size;

        float paddingX = m_Settings.WheelsPaddingX;
        float paddingZ = m_Settings.WheelsPaddingZ;

        return wheel switch
        {
            Wheel.FrontLeft => new Vector3(boxSize.x * (paddingX - 0.5f), 0.0f, boxSize.z * (0.5f - paddingZ)),
            Wheel.FrontRight => new Vector3(boxSize.x * (0.5f - paddingX), 0.0f, boxSize.z * (0.5f - paddingZ)),
            Wheel.BackLeft => new Vector3(boxSize.x * (paddingX - 0.5f), 0.0f, boxSize.z * (paddingZ - 0.5f)),
            Wheel.BackRight => new Vector3(boxSize.x * (0.5f - paddingX), 0.0f, boxSize.z * (paddingZ - 0.5f)),
            _ => default,
        };
    }

    private Vector3 GetWheelTorquePosition(Wheel wheel)
    {
        return m_Transform.localToWorldMatrix.MultiplyPoint3x4(GetWheelTorqueRelativePosition(wheel));
    }

    private float GetWheelGripFactor(Wheel wheel)
    {
        bool frontWheel = wheel == Wheel.FrontLeft || wheel == Wheel.FrontRight;
        return frontWheel ? m_Settings.FrontWheelsGripFactor : m_Settings.RearWheelsGripFactor;
    }

    private bool IsGrounded(Wheel wheel)
    {
        return m_SpringDatas[wheel].CurrentLength < m_Settings.SpringRestLength;
    }

    private void UpdateSuspension()
    {
        foreach (Wheel id in m_SpringDatas.Keys)
        {
            CastSpring(id);
            float currentLength = m_SpringDatas[id].CurrentLength;
            float currentVelocity = m_SpringDatas[id].CurrentVelocity;

            float force = SpringMath.CalculateForceDamped(currentLength, currentVelocity,
                m_Settings.SpringRestLength, m_Settings.SpringStrength,
                m_Settings.SpringDamper);

            m_Rigidbody.AddForceAtPosition(force * m_Transform.up, GetSpringPosition(id));
        }
    }

    private void UpdateSteering()
    {
        foreach (Wheel wheel in s_Wheels)
        {
            if (!IsGrounded(wheel))
            {
                continue;
            }

            Vector3 springPosition = GetSpringPosition(wheel);

            Vector3 slideDirection = GetWheelSlideDirection(wheel);
            float slideVelocity = Vector3.Dot(slideDirection, m_Rigidbody.GetPointVelocity(springPosition));

            float desiredVelocityChange = -slideVelocity * GetWheelGripFactor(wheel);
            float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;

            Vector3 force = desiredAcceleration * m_Settings.TireMass * slideDirection;
            m_Rigidbody.AddForceAtPosition(force, GetWheelTorquePosition(wheel));
        }
    }

    private void UpdateAccelerate()
    {
        if (Mathf.Approximately(m_AccelerateInput, 0.0f) || m_IsBrake)
        {
            return;
        }

        float forwardSpeed = Vector3.Dot(m_Transform.forward, m_Rigidbody.linearVelocity);
        bool movingForward = forwardSpeed > 0.0f;
        float speed = Mathf.Abs(forwardSpeed);

        if (movingForward && speed > m_Settings.MaxSpeed)
        {
            return;
        }
        else if (!movingForward && speed > m_Settings.MaxReverseSpeed)
        {
            return;
        }

        foreach (Wheel wheel in s_Wheels)
        {
            if (!IsGrounded(wheel))
            {
                continue;
            }

            Vector3 position = GetWheelTorquePosition(wheel);
            Vector3 wheelForward = GetWheelRollDirection(wheel);
            m_Rigidbody.AddForceAtPosition(m_AccelerateInput * m_Settings.AcceleratePower * wheelForward, position);
        }
    }

    private void UpdateBrakes()
    {
        float forwardSpeed = Vector3.Dot(m_Transform.forward, m_Rigidbody.linearVelocity);
        float speed = Mathf.Abs(forwardSpeed);

        float brakesRatio;

        const float ALMOST_STOPPING_SPEED = 2.0f;
        bool almostStopping = speed < ALMOST_STOPPING_SPEED;
        if (almostStopping || m_IsBrake)
        {
            brakesRatio = 1.0f;
        }
        else
        {
            bool accelerateContrary =
                !Mathf.Approximately(m_AccelerateInput, 0.0f) &&
                Vector3.Dot(m_AccelerateInput * m_Transform.forward, m_Rigidbody.linearVelocity) < 0.0f;
            if (accelerateContrary)
            {
                brakesRatio = 1.0f;
            }
            else if (Mathf.Approximately(m_AccelerateInput, 0.0f)) // No accelerate input
            {
                brakesRatio = 0.1f;
            }
            else
            {
                return;
            }
        }


        foreach (Wheel wheel in s_BackWheels)
        {
            if (!IsGrounded(wheel))
            {
                continue;
            }

            Vector3 springPosition = GetSpringPosition(wheel);
            Vector3 rollDirection = GetWheelRollDirection(wheel);
            float rollVelocity = Vector3.Dot(rollDirection, m_Rigidbody.GetPointVelocity(springPosition));

            float desiredVelocityChange = -rollVelocity * m_Settings.BrakesPower * brakesRatio;
            float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;

            Vector3 force = desiredAcceleration * m_Settings.TireMass * rollDirection;
            m_Rigidbody.AddForceAtPosition(force, GetWheelTorquePosition(wheel));
        }
    }

    private void UpdateAirResistance()
    {
        m_Rigidbody.AddForce(m_BoxCollider.size.magnitude * m_Settings.AirResistance * -m_Rigidbody.linearVelocity);
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Vector3 vehicleDown = -transform.up;

            foreach (Wheel wheel in m_SpringDatas.Keys)
            {
                // Spring
                Vector3 position = GetSpringPosition(wheel);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(position, position + vehicleDown * m_Settings.SpringRestLength);
                Gizmos.color = Color.red;
                Gizmos.DrawCube(GetSpringHitPosition(wheel), Vector3.one * 0.08f);

                // Wheel
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(position, GetWheelRollDirection(wheel));
                Gizmos.color = Color.red;
                Gizmos.DrawRay(position, GetWheelSlideDirection(wheel));
            }
        }
        else
        {
            if (m_Settings != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position,
                    new Vector3(
                        m_Settings.Width,
                        m_Settings.Height,
                        m_Settings.Length));
            }
        }
    }
#endif
}
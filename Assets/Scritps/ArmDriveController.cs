using UnityEngine;

public class ArmDriveController : MonoBehaviour
{
    [Header("Assign joints (Boom then Stick)")]
    public ConfigurableJoint boomJoint;
    public ConfigurableJoint stickJoint;

    [Header("Angles in degrees")]
    public float boomAngle = 15f;   // around joint X
    public float stickAngle = -10f; // around joint X

    [Header("Drive settings")]
    public float spring = 15000f;
    public float damper = 20f;
    public float maxForce = 10000f;

    void Start()
    {
        SetupJoint(boomJoint);
        SetupJoint(stickJoint);
    }

    void FixedUpdate()
    {
        // TargetRotation is expressed in joint space. We'll drive around X only.
        SetTargetX(boomJoint, boomAngle);
        SetTargetX(stickJoint, stickAngle);
    }

    void SetupJoint(ConfigurableJoint j)
    {
        if (!j) return;

        var drive = j.angularXDrive;
        drive.positionSpring = spring;
        drive.positionDamper = damper;
        drive.maximumForce = maxForce;
        j.angularXDrive = drive;

        // Also set Y/Z drives to avoid drift even if locked
        var yz = j.angularYZDrive;
        yz.positionSpring = spring;
        yz.positionDamper = damper;
        yz.maximumForce = maxForce;
        j.angularYZDrive = yz;

        j.rotationDriveMode = RotationDriveMode.XYAndZ;
    }

    void SetTargetX(ConfigurableJoint j, float angleDeg)
    {
        if (!j) return;

        // Unity expects targetRotation as a quaternion in joint space.
        // Negative sign often matches intuitive direction; adjust if inverted.
        var target = Quaternion.Euler(-angleDeg, 0f, 0f);
        j.targetRotation = target;
    }
}
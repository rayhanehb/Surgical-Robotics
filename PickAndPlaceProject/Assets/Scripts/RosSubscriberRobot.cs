using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;

public class JointAngleSubscriber : MonoBehaviour
{
    public ArticulationBody[] articulationBodies;  // Array to hold references to your ArticulationBodies

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the ROS topic where joint angles are being published
        ROSConnection.GetOrCreateInstance().Subscribe<JointStateMsg>("/joint_angles", ApplyJointAngles);
    }

    // This method is called every time a new JointState message is received
    void ApplyJointAngles(JointStateMsg jointState)
    {
        for (int i = 0; i < articulationBodies.Length; i++)
        {
            if (i < jointState.position.Length)
            {
                // Convert radians to degrees
                float targetAngleDegrees = (float)jointState.position[i] * Mathf.Rad2Deg;

                // Access the current xDrive of the articulation body
                ArticulationDrive drive = articulationBodies[i].xDrive;

                // Set the target position (angle) for the joint
                drive.target = targetAngleDegrees;

                // Ensure damping, stiffness, and force limits are set
                drive.stiffness = 100f;   // Adjust as necessary
                drive.damping = 10f;      // Adjust as necessary
                drive.forceLimit = 100f;  // Adjust as necessary

                // Assign the drive back to the articulation body
                articulationBodies[i].xDrive = drive;  // Apply the changes
            }
        }
    }

}

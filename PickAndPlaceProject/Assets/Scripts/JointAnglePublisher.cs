using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;  // For sensor_msgs/JointState
using Unity.Robotics.UrdfImporter;  // For UrdfJointRevolute
using UnityEngine;

public class JointAnglePublisher : MonoBehaviour
{
    const int k_NumRobotJoints = 9;

    public static readonly string[] LinkNames = 
    {
        "fr3_link0", "fr3_link1", "fr3_link2", "fr3_link3", "fr3_link4", "fr3_link5", "fr3_link6", "fr3_link7", "fr3_link8"
    };

    [SerializeField] GameObject m_Robot;
    [SerializeField] GameObject[] m_JointLinks = new GameObject[k_NumRobotJoints];

    ROSConnection m_Ros;
    string m_TopicName = "/robot/joint_states";

    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<JointStateMsg>(m_TopicName);

        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            string jointName = LinkNames[i];
            Debug.Log($"Looking for joint: {jointName}");

            // Check if the joint GameObject is assigned
            if (m_JointLinks[i] == null)
            {
                Debug.LogError($"Joint {jointName} GameObject not assigned in the Inspector.");
                continue;
            }

            // Use Transform to get joint angle (e.g., rotation)
            Debug.Log($"Joint {jointName} position: {m_JointLinks[i].transform.localRotation.eulerAngles}");
        }
    }

    public void PublishJointAngles()
    {
        var jointStateMessage = new JointStateMsg
        {
            header = new RosMessageTypes.Std.HeaderMsg()
        };

        jointStateMessage.name = new string[k_NumRobotJoints];
        jointStateMessage.position = new double[k_NumRobotJoints];

        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            jointStateMessage.name[i] = LinkNames[i];
            // Use localRotation or any other transform method
            jointStateMessage.position[i] = m_JointLinks[i].transform.localRotation.eulerAngles.z; // Example for joint angle
        }

        m_Ros.Publish(m_TopicName, jointStateMessage);
    }
}

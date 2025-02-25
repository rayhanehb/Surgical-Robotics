using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleKinematicController : MonoBehaviour
{
    public ArticulationBody[] joints; // Assign manually or find in Start()
    public float moveSpeed = 10f;

    private int selectedJoint = 0;

    void Update()
    {
        // Cycle through joints
        if (Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            selectedJoint = (selectedJoint + 1) % joints.Length;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            selectedJoint = (selectedJoint - 1 + joints.Length) % joints.Length;
        }

        // Move selected joint
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        if (move != 0)
        {
            var jointPosition = joints[selectedJoint].jointPosition;
            jointPosition[0] += move;
            joints[selectedJoint].jointPosition = jointPosition;
        }
    }
}

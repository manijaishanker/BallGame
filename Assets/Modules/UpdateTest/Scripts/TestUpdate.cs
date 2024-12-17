using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUpdate : MonoBehaviour
{
    [SerializeField] public GameObject TestAgent1;
    [SerializeField] public GameObject TestAgent2;

    [SerializeField] public float TestAgent1MaxSpeed;
    [SerializeField] public float TestAgent2MaxSpeed;

    public float TestAgent1CurrentVelocity;
    public float TestAgent2CurrentVelocity;

    public Vector3 _testAgent1CurrentPos;
    public Vector3 _testAgent2CurrentPos;

    private void Awake()
    {

    }

    private void Update()
    {
        if (Mathf.Approximately(TestAgent1.transform.position.x, 20f))
            Debug.Log("!!!!!!! TEST AGENT 1 DONE: " + TestAgent1.transform.position.x);
        TestAgent1.transform.position += Vector3.right * TestAgent1MaxSpeed * Time.deltaTime;
        _testAgent1CurrentPos = TestAgent1.transform.position;
    }

    private void FixedUpdate()
    {
        if (Mathf.Approximately(TestAgent2.transform.position.x, 20f))
            Debug.Log("!!!!!!! TEST AGENT 2DONE: " + TestAgent2.transform.position.x);

        TestAgent2.transform.position += Vector3.right * TestAgent2MaxSpeed * Time.deltaTime;
        _testAgent2CurrentPos = TestAgent2.transform.position;
    }

}

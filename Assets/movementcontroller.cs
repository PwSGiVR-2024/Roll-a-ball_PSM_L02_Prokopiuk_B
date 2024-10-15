using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementcontroller : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public float speed = 5.0f;
    public float spit = -5.0f;
    public float rotationSpeed = 100.0f;

    void Start()
    {
     m_Rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        //float translation = Input.GetAxis("Vertical") * speed;
        //float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        //translation *= Time.deltaTime;
        //rotation *= Time.deltaTime;
        //transform.Translate(0, 0, translation);
        //transform.Rotate(0, rotation, 0);
        if(Input.GetKey(KeyCode.W))
        {
            m_Rigidbody.AddForce(0, 0, speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_Rigidbody.AddForce(0, 0, spit);
        }

        if (Input.GetKey(KeyCode.D))
        {
            m_Rigidbody.AddForce(speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_Rigidbody.AddForce(spit, 0, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float speed = 0.05f;
    private float sensitivity = 0.01f;
    private Vector2 center;
    private bool isTouched = false;

    void Start()
    {
        speed = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().speed;
        sensitivity = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().sensitivity;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed);
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!isTouched)
            {
                isTouched = true;
                if (touch.position.y > center.y)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                }
                if (touch.position.x > center.x)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                }
                center = touch.position;
            }
            else
            {
                if (touch.position.y > center.y)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + ((touch.position.y - center.y) * sensitivity), transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + ((touch.position.y - center.y) * sensitivity), transform.position.z);
                }
                if (touch.position.x > center.x)
                {
                    transform.position = new Vector3(transform.position.x + ((touch.position.x - center.x) * sensitivity), transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x + ((touch.position.x - center.x) * sensitivity), transform.position.y, transform.position.z);
                }
                center = touch.position;
            }
        }
        else
        {
            isTouched = false;
        }
    }

    public void SpeedUp(float speed)
    {
        this.speed = speed;
    }


}

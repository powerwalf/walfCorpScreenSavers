using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    [SerializeField] GameObject m_target;

    [SerializeField] float m_mouseSpeedScalar = 0.2f;
    [SerializeField] float m_scrollWheelSpeedScalar = 3f;

    [SerializeField] float m_minYRotation = -90f;
    [SerializeField] float m_maxYRotation = 90f;

    [SerializeField] float m_distance = 5f;
    [SerializeField] float m_minDistance = 2f;
    [SerializeField] float m_maxDistance = 5f;

    float m_x;
    float m_y;
    float m_prevDistance;

    void Start()
    {
        Vector3 angles = this.transform.eulerAngles;
        m_x = angles.y;
        m_y = angles.x;
    }

    void Update()
    {
        m_distance -= Input.GetAxis("Mouse ScrollWheel") * m_scrollWheelSpeedScalar;
        m_distance = Mathf.Clamp(m_distance, m_minDistance, m_maxDistance);

        if(Input.GetMouseButton(0) || m_prevDistance !=  m_distance)
        {
            m_x += Input.GetAxis("Mouse X") * m_mouseSpeedScalar;

            m_y -= Input.GetAxis("Mouse Y") * m_mouseSpeedScalar;
            m_y = ClampAngle(m_y, m_minYRotation, m_maxYRotation);

            Quaternion rotation = Quaternion.Euler(m_y, m_x, 0f);
            Vector3 position = rotation * new Vector3(0f, 0f, -m_distance) + m_target.transform.position;
            transform.rotation = rotation;
            transform.position = position;

            m_prevDistance = m_distance;
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
		{
            angle += 360;
		}
        else if (angle > 360)
		{
            angle -= 360;
		}

        return Mathf.Clamp(angle, min, max);
    }
}


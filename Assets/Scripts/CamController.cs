using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float lerpTime;
    public float xOffset;
    bool m_canLerp;
    float m_lerpXDist;

    private void Update()
    {
        if (m_canLerp)
            MoveLerp();
    }

    void MoveLerp()
    {
        float xPos = transform.position.x;
        xPos = Mathf.Lerp(xPos, m_lerpXDist, lerpTime * Time.deltaTime);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

        if(transform.position.x >= (m_lerpXDist - xOffset))
        {
            m_canLerp = false;
        }
    }

    public void LerpTrigger(float dist)
    {
        m_canLerp = true;
        m_lerpXDist = dist;
    }
}

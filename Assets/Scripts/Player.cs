using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 jumpForce;
    public Vector2 jumbForceUp;
    public float minForceX;
    public float maxForceX;
    public float minForceY;
    public float maxForceY;

    [HideInInspector]

    public int lastPlatformId;

    bool m_didJump;
    bool m_powerSetted;

    Rigidbody2D m_rb;
    Animator m_anim;
    float m_curPowerBarVal = 0;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Ins.IsGameStarted)
        {
            SetPower();

            if (Input.GetMouseButtonDown(0))
            {
                SetPower(true);
            }

            if (Input.GetMouseButtonUp(0))
            {
                SetPower(false);
            }            
        }
    }

    void SetPower()
    {
        if(m_powerSetted && !m_didJump)
        {
            jumpForce.x += jumbForceUp.x * Time.deltaTime;
            jumpForce.y += jumbForceUp.y * Time.deltaTime;

            jumpForce.x = Mathf.Clamp(jumpForce.x, minForceX, maxForceX);
            jumpForce.y = Mathf.Clamp(jumpForce.y, minForceY, maxForceY);

            m_curPowerBarVal += GameManager.Ins.powerBarUp * Time.deltaTime;

            GameGUIManager.Ins.UpdatePowerBar(m_curPowerBarVal, 1);
        }
    }

    public void SetPower(bool isHoldingMouse)
    {
        m_powerSetted = isHoldingMouse;

        if(!m_powerSetted && !m_didJump)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (!m_rb || jumpForce.x <= 0 || jumpForce.y <= 0) return;

        m_rb.velocity = jumpForce;

        m_didJump = true;

        if (m_anim)
            m_anim.SetBool("didJump",true);
        AudioController.Ins.PlaySound(AudioController.Ins.jump);

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(TagConsts.GROUND))
        {
            Platform p = col.transform.root.GetComponent<Platform>();

            if (m_didJump)
            {
                m_didJump = false;

                if (m_anim)
                    m_anim.SetBool("didJump", false);

                if (m_rb)
                {
                    m_rb.velocity = Vector2.zero;
                }

                jumpForce = Vector2.zero;

                m_curPowerBarVal = 0;

                GameGUIManager.Ins.UpdatePowerBar(m_curPowerBarVal, 1);
            }

            if(p && p.id != lastPlatformId)
            {
                GameManager.Ins.CreatePlatformAndLerp(transform.position.x);

                lastPlatformId = p.id;

                GameManager.Ins.AddScore();
            }
        }

        if (col.CompareTag(TagConsts.DEATH_ZONE))
        {
            GameGUIManager.Ins.ShowGameoverDialog();

            AudioController.Ins.PlaySound(AudioController.Ins.gameover);


            Destroy(gameObject);
        }
    }
}

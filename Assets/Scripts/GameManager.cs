using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player playerPrefab;
    public Platform platformPrefab;
    public float minSpawnX;
    public float maxSpawnX;
    public float minSpawnY;
    public float maxSpawnY;
    public CamController mainCam;
    public float powerBarUp;
    Player m_player;
    int m_score;
    bool m_isGameStarted;

    public bool IsGameStarted { get => m_isGameStarted;}

    public override void Awake()
    {
        MakeSingleton(false);
    }
    public override void Start()
    {
        base.Start();

        GameGUIManager.Ins.UpdateScoreCouting(m_score);        

        GameGUIManager.Ins.UpdatePowerBar(0, 1);

        AudioController.Ins.PlayBackgroundMusic();
    }

    public void PlayGame()
    {
        StartCoroutine(PlatformInit());

        GameGUIManager.Ins.ShowGameGui(true);
    }

    IEnumerator PlatformInit()
    {
        Platform platformClone = null;

        if (platformPrefab)
        {
            platformClone = Instantiate(platformPrefab, new Vector2(0, Random.Range(minSpawnY, maxSpawnY)), Quaternion.identity);
            platformClone.id = platformClone.gameObject.GetInstanceID();
        }

        yield return new WaitForSeconds(0.5f);

        if (playerPrefab)
        {
            m_player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            m_player.lastPlatformId = platformClone.id;
        }

        if (platformPrefab)
        {
            float spawnX = m_player.transform.position.x + minSpawnX;
            float spawnY = Random.Range(minSpawnY, maxSpawnY);

            Platform platformClone02 = Instantiate(platformPrefab, new Vector2(spawnX, spawnY),Quaternion.identity);
            platformClone02.id = platformClone02.gameObject.GetInstanceID();

        }

        yield return new WaitForSeconds(0.5f);

        m_isGameStarted = true;
    }

    public void CreatePlatform()
    {
        if (!platformPrefab || !m_player) return;

        float spawnX = Random.Range(m_player.transform.position.x + minSpawnX,m_player.transform.position.x + maxSpawnX);

        float spawnY = Random.Range(minSpawnY,maxSpawnY);

        Platform platformClone = Instantiate(platformPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
        platformClone.id = platformClone.gameObject.GetInstanceID();
    }

    public void CreatePlatformAndLerp(float playerXpos)
    {
        if (mainCam)
        {
            mainCam.LerpTrigger(playerXpos + minSpawnX);

            CreatePlatform();
        }
    }
    
    public void AddScore()
    {
        m_score++;

        Prefs.bestScore = m_score;

        GameGUIManager.Ins.UpdateScoreCouting(m_score);
        AudioController.Ins.PlaySound(AudioController.Ins.getScore);
    }
}

﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Variables
    #endregion
    public Text m_ControlText;

    public GameData m_GData;
    public PlayerData m_PData;
    public SpawnPlayer m_PSpawn;
    public EnemySpawn m_ESpawn;
    public Waves m_Waves;

    public GameObject m_Player;

    public Canvas m_Win;
    public Canvas m_Lose;
    public CanvasGroup m_BossPanel;
    public Canvas m_MainUI;

    public UIControl m_UIControl;

    public int m_Level;

    public int m_Lives = 3; //Number of lives the player has
    public int m_Score = 0; //Player's current score
    public int m_TotalScore; //For stat pruposes
    public int m_Kills;
    public int m_Salvage;

    public List<String> m_TempItems;

    public int m_TempKills;
    public int m_TempScore;
    public int m_TempSalvage;
    public int m_TempWaveNum;
    public int m_TempEnemiesKilledLifetime;
    public int m_TempTotalScore;
    public int m_TempWavesCompleted;
    public int m_TempShipLevel;
    public int m_TempEngineLevel;
    public int m_TempDamageLevel;
    public int m_TempHealthLevel;
    public int m_TempShieldLevel;

    public bool m_TempHasShield;

    public int m_TempHP;
    public int m_TempShield;
    public int m_TempDamageModifer;
    public int m_TempEngineModifier;

    public bool m_Restart;
    public bool m_GameOver;
    public bool m_Play;

    // Use this for initialization
    void Awake()
    {
        m_BossPanel.alpha = 0;

        m_MainUI.enabled = true;

        m_Lose.enabled = false;
        m_Win.enabled = false;

        m_Restart = false;
        m_GameOver = false;
        m_Play = false;

        m_PSpawn.Spawn();
        m_Player = m_PSpawn.m_Player;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            m_GData.Save();
        }

        if (!m_Play)
        {
            m_Player.GetComponent<PlayerController>().enabled = false;//disable player
            m_ControlText.text = "Press 'Z' to begin";

            if (Input.GetKey(KeyCode.Z))
            {
                m_Play = true;
                Play();
            }
        }


        if (m_Restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                m_Restart = false;
                m_GData.Save();
                Application.LoadLevel("StarMap");
            }
        }
     }

    public void Play()
    {
        if(m_Play)
        {
            m_ControlText.text = "";
            m_Player.GetComponent<PlayerController>().enabled = true;//enable player
            m_ESpawn.SetShipPrefab();
            m_ESpawn.AISpawn();
        }
    }

    public void GameOver()
    {
        m_Play = false;
        m_MainUI.enabled = false;
        m_ESpawn.DestroyAllEnemies();
        m_Lose.enabled = true;
    }

    public void Win()
    {
        m_Play = false;
        m_MainUI.enabled = false;
        m_ESpawn.DestroyAllEnemies();
        m_Win.enabled = true;
    }
	
	public void Respawn()
    {
        m_ControlText.text = "";
        m_Restart = false;
        m_Waves.RestartCurrentWave();
    }

    public void EnableRestart()
    {
        m_Lives -= 1;

        if (m_Lives > 0)
        {
            m_ESpawn.DestroyAllEnemies();

            m_Restart = true;

            m_ControlText.text = "Press 'R' for Restart or 'Q' to Quit";
        }
        else
        {
            GameOver();
        }
    }

    public void SetPlayerSave(GameObject player)
    {
        m_PData.m_EnemiesKilledLifetime += m_Kills;
        m_PData.m_TotalScore += m_Score;
        m_PData.m_Salvage += m_Salvage;
        for (int i = 0; i < m_PSpawn.m_Player.GetComponent<ShipData>().m_Inventory.Count; ++i)
        {
            for(int j = 0; j < m_PData.m_Items.Count; ++j)
            {
                m_PData.m_Items[j] = m_PSpawn.m_Player.GetComponent<ShipData>().m_Inventory[i].name;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
 
    public int numRoundsToWin = 5;
    public float startDelay = 3f;
    public float endDelay = 3f;
    public Camera_Follow cameraControl;
    public Text message_Text;
    public GameObject tankPrefab;
    public AIController[] tanks;

    private int roundNumber;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private AIController roundWinner;
    private AIController gameWinner;

    void Start()
    {
        // The delay's we will use to make our game wait through our GameLoop
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        // spawn tanks, set the camera and start the game
        SpawnAllTanks();
       // SetCameraTargets();
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        
    }

    private void SpawnAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].instance = Instantiate(tankPrefab, tanks[i].spawnPoint.position, tanks[i].spawnPoint.rotation) as GameObject;
            Debug.Log("hit once");
            tanks[i].instance = Instantiate(tankPrefab, tanks[i].spawnPoint.position, tanks[i].spawnPoint.rotation) as GameObject;

            tanks[i].playerNumber = i + 1;
            tanks[i].Setup();

        }
    }

    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = tanks[i].instance.transform;
        }

        cameraControl.targets = targets;
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (gameWinner != null)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

       //cameraControl.SetStartPositionAndSize();

        roundNumber++;
        message_Text.text = "ROUND " + roundNumber;

        yield return startWait;
    }

    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        message_Text.text = string.Empty;

        while (!OneTankLeft())
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        roundWinner = null;

        roundWinner = GetRoundWinner();

        if (roundWinner != null)
        {
            roundWinner.wins++;
        }

        gameWinner = GetGameWinner();

        string message = EndMessage();
        message_Text.text = message;

        yield return endWait;
    }

    private bool OneTankLeft()
    {
        int numTanksLeft = 0;
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
            {
                numTanksLeft++;
                
            }

            if (numTanksLeft >= i)
            {
                GetRoundWinner();
                
            }

            return numTanksLeft <= 1;
        }
        return true;
    }

    private AIController GetRoundWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
            {
                return tanks[i];
            }
  
        }

        return null;
    }

    private AIController GetGameWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].wins == numRoundsToWin) 
            {
                return tanks[i];
            }
    
        }
        return null;
    }

    private string EndMessage()
    { 
        string message = roundWinner.coloredPlayerText + " WINS THE ROUND1";

        if (roundWinner == null)
        {
            message = "DRAW!";
        }

        message += "\n\n\n\n";

        for (int i = 0; i < tanks.Length; i++)
        {
            message += tanks[i].coloredPlayerText + ": " + tanks[i].wins + " WINS\n";
        }

        if (gameWinner != null)
        {
            message = gameWinner.coloredPlayerText + " WINS THE GAME!";
        }

        return message;

    }

    private void ResetAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].Reset();
        }
    }

    private void DisableTankControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].DisableControl();
        }
    }
    
    private void EnableTankControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].EnableControl();
        }
    }


}
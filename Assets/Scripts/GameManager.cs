using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public bool startPlaying;
    public BeatScroller theBS;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public Text scoreText;
    public Text multiplyScoreText;

    public int currentMultiplyer;
    public int multiplyerTracker;
    public int[] multiplyerThreshold;

    public GameObject resultScreen;

    [SerializeField] float totalNotes, normalNotes, goodNotes, perfectNotes, missedNotes;

    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;

    public static GameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        scoreText.text = "Score = 0";
        currentMultiplyer = 1;
        totalNotes = FindObjectsOfType<NoteManager>().Length;
    }

    // Update is called once per frame
    void Update()
    {
     if (!startPlaying)
        {
            if (Input.anyKeyDown) 
            {
                startPlaying = true;
                theBS.hasStarted = true;
                theMusic.Play();
            }
        } 
        else
        {
            if (!theMusic.isPlaying && !resultScreen.activeInHierarchy)
            {
                resultScreen.SetActive(true);
                normalsText.text = normalNotes.ToString();
                goodsText.text = goodNotes.ToString();
                perfectsText.text = "" + perfectNotes;
                missesText.text = "" + missedNotes;

                float totalHits = normalNotes + goodNotes + perfectNotes;
                float percentOfAllHits = (totalHits / totalNotes) * 100f;
                // F1 is a shortcut to a one decimal after . in a number
                percentHitText.text = percentOfAllHits.ToString("F1") + "%";

                string rankVal = "F";

                if (percentOfAllHits > 40)
                {
                    rankVal = "Decent";
                    if (percentOfAllHits > 65)
                    {
                        rankVal = "Good enough";
                        if (percentOfAllHits > 80)
                        {
                            rankVal = "Cool...";
                            if (percentOfAllHits > 95)
                            {
                                rankVal = "Perfecto";
                            }
                        }
                    }
                }

                rankText.text = rankVal;
                finalScoreText.text = currentScore.ToString();
            }
        }
    }

    public void NoteHit ()
    {
        UnityEngine.Debug.Log("Hit");
        if (currentMultiplyer - 1 < multiplyerThreshold.Length)
        {
            multiplyerTracker++;
            if (multiplyerThreshold[currentMultiplyer - 1] <= multiplyerTracker)
            {
                multiplyerTracker = 0;
                currentMultiplyer++;
            }
        }

        multiplyScoreText.text = "Multiplyer x " + currentMultiplyer;
        

        //currentScore += scorePerNote * currentMultiplyer;
        scoreText.text = "Score " + currentScore;
    }

    public void NormalHit()
    {
        NoteHit();
        currentScore += scorePerNote * currentMultiplyer;
        normalNotes++;
    }

    public void GoodHit()
    {
        NoteHit();
        currentScore += scorePerGoodNote * currentMultiplyer;
        goodNotes++;
    }

    public void PerfectHit()
    {
        NoteHit();
        currentScore += scorePerPerfectNote * currentMultiplyer;
        perfectNotes++;
    }

    public void NoteMissed()
    {
        UnityEngine.Debug.Log("Missed note");

        missedNotes++;

        currentMultiplyer = 1;
        multiplyerTracker = 0;
        multiplyScoreText.text = "Multiplyer x " + currentMultiplyer;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public int numHeads, numTails;

    [SerializeField]
    private GameObject canvasObj;
    [SerializeField]
    private TMP_Text textHeads, textTails, textStreak, textLongStreak, textSpeedUp, textNumFlips;
    [SerializeField]
    private GameObject graphHolder, graphPoint;
    [SerializeField]
    private UILineRenderer graphLineRenderer;
    [SerializeField]
    private CoinFlip coinFlip;

    private List<float> percentsToBeGraphed = new List<float>();

    private float percentHeads, percentTails;
    private int totalFlips;
    private int streak, longStreak, gameSpeed = 1;
    private bool autoFlip, graphShown = true;
    private string lastFlip;
    private float renderGraphCooldownElapsed;

    private void Start() {
        Time.timeScale = 5f;
    }

    private void Update() {
        if(autoFlip) {
            coinFlip.FlipButtonPressed();
        }

        renderGraphCooldownElapsed += Time.unscaledDeltaTime;
        RenderGraph();
        if (renderGraphCooldownElapsed >= 0.5f) {
            
            renderGraphCooldownElapsed = 0f;
        }
    }

    public void UpdateNumbers(string thisFlip) {
        totalFlips = numHeads + numTails;

        if (totalFlips > 0) {
            percentHeads = (float) numHeads / totalFlips * 100f;
            percentTails = (float) numTails / totalFlips * 100f;

            textHeads.text = "Heads: " + numHeads.ToString("N0") + " (" + percentHeads.ToString("F2") + "%)";
            textTails.text = "Tails: " + numTails.ToString("N0") + " (" + percentTails.ToString("F2") + "%)";

            textNumFlips.text = "Total: " + totalFlips.ToString("N0");

            if (lastFlip == thisFlip || streak == 0) {
                streak++;
            } else {
                streak = 1;
            }

            string streakInfo = streak + " (" + thisFlip + ")";

            if (streak > longStreak) {
                longStreak = streak;
                textLongStreak.text = "Longest Streak: " + streakInfo;
            }

            textStreak.text = "Streak: " + streakInfo;
        }

        lastFlip = thisFlip;

        percentsToBeGraphed.Add(percentHeads);
    }

    public void UpdateSpeed() {
        if (gameSpeed < 16) {
            gameSpeed *= 2;
        } else {
            gameSpeed = 1;
        }

        textSpeedUp.text = "Change Speed (x" + gameSpeed + ")";
        Time.timeScale = gameSpeed * 5;
    }

    public void AutoFlipToggle() {
        if (autoFlip) {
            autoFlip = false;
        } else {
            autoFlip = true;
        }
    }

    public void GraphToggle() {
        if (graphShown) {
            graphShown = false;
            graphHolder.SetActive(false);
        } else {
            graphShown = true;
            graphHolder.SetActive(true);
        }
    }

    private void RenderGraph() {
        //int skipEvery = Mathf.CeilToInt(totalFlips / (1 + totalFlips * 0.05f));
        //print(skipEvery);
        //int numIncluded = 0;
        float incrementX = 450f / totalFlips;
        float lastX = incrementX;

        graphLineRenderer.points = new Vector2[totalFlips];

        graphLineRenderer.thickness = 10f / Mathf.Pow(totalFlips, 0.3f);

        for (int i = 0; i < percentsToBeGraphed.Count; i++) {
            graphLineRenderer.points[i] = new Vector2(lastX - 225f, percentsToBeGraphed[i] * 3.15f - 157.5f);

            lastX += incrementX;
        }

        //Tell Unity to update the graphic
        graphLineRenderer.SetVerticesDirty();
    }
}
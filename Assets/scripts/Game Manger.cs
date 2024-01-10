using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManger : MonoBehaviour
{
    public TileBord bord;
    public CanvasGroup gameoverui ;

    public TextMeshProUGUI scoretext;
    public TextMeshProUGUI highScoreText;
    private int score;
    private void Start()
    {
        newgame();

    }
    public  void newgame()
    {
        setscore(0);
        highScoreText.text =LoadHighscore().ToString();
        gameoverui.alpha = 0f;
        gameoverui.interactable=false;
        bord.clearBord();
        bord.CreateTile();
        bord.CreateTile();
        bord.enabled = true;
    }
    public void gameover()
    {
        bord.enabled=false;
        gameoverui.interactable = true;
        StartCoroutine(fade(gameoverui,1f,1f));
    }

    private IEnumerator fade(CanvasGroup canvasGroup, float to,float delay)
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;
        while(elapsed < duration) {
            canvasGroup.alpha = Mathf.Lerp(from, to,elapsed/duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
       canvasGroup.alpha = to;
    }
    private void setscore(int score)
    {
        this.score = score;
        scoretext.text = score.ToString();
        save();
    }
    public void incresescore(int points)
    {
        setscore(score+points);
    }
    private void save()
    {
        int highscore=LoadHighscore();
        if (highscore < score)
        {
            PlayerPrefs.SetInt("highScore", score);
        }
    }
    private int LoadHighscore()
    {
        return PlayerPrefs.GetInt("highScore", 0);
    }
}

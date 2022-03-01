using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameTeams
{
    teamRed,
    teamBlue
}

public class ScoreManager : MonoBehaviour
{

    public GameTeams team;

    [SerializeField]
    Text blueScoreText, redScoreText;

    int blueScoreNum = 0, redScoreNum = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ChangeScore(GameTeams team)
    {

        if (team == GameTeams.teamRed)
        {
            redScoreNum++;
            redScoreText.text = redScoreNum.ToString();
        }
        else
        {
            blueScoreNum++;
            blueScoreText.text = blueScoreNum.ToString();
        }
    }

}

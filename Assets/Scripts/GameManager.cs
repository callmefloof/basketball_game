using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private static GameManager _instance;
        public List<Baller> Ballers = new List<Baller>();
        public Dictionary<string, int> Scores = new Dictionary<string, int>() { { "Team 1", 0 }, { "Team 2", 0 } };
        public Color TeamOneColor = Color.red;
        public Color TeamTwoColor = Color.blue;
        
        //TextMeshProUGUI to show the score on the UI
        public TextMeshProUGUI scoreTeam1;
        public TextMeshProUGUI scoreTeam2;
        
        //Integers to hold the values of the scores for each team , will be used to change the scene when either team wins
        public int scoreTeamRed;
        public int scoreTeamBlue;

        public static GameManager Instance

        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<GameManager>();
                }

                return _instance;
            }


        }

        // TODO: Implement player spawn
        public void SpawnPlayers()
        {

        }

        void Start()
        {
            var teamOneHoopMesh = GameObject.FindGameObjectWithTag("HoopOne").GetComponent<MeshRenderer>();
            var teamTwoHoopMesh = GameObject.FindGameObjectWithTag("HoopTwo").GetComponent<MeshRenderer>();
            var materialTeamOne = teamOneHoopMesh.material;
            materialTeamOne.SetColor("_BallerTeamColor", GameManager.Instance.TeamOneColor);
            teamOneHoopMesh.material = materialTeamOne;
            var materialTeamTwo = teamTwoHoopMesh.material;
            materialTeamTwo.SetColor("_BallerTeamColor", GameManager.Instance.TeamTwoColor);
            teamTwoHoopMesh.material = materialTeamTwo;
        }

        // Update is called once per frame
        void Update()
        {
            //Updates the UI score for each team
            scoreTeam2.text = GameManager.Instance.Scores["Team 1"].ToString();
            scoreTeam1.text = GameManager.Instance.Scores["Team 2"].ToString();

            scoreTeamRed= GameManager.Instance.Scores["Team 1"];
            scoreTeamBlue = GameManager.Instance.Scores["Team 2"];
            
            //If Scores of red team reach 20 or greater, play the end screen for red team
            if (scoreTeamRed >= 20)
            {
                SceneManager.LoadScene("EndScreenRed");
            }
            
            //If Scores of blue team reach 20 or greater, play the end screen for blue team
            if (scoreTeamBlue >= 20)
            {
                SceneManager.LoadScene("EndScreenBlue");
            }
            
        }

    }
}

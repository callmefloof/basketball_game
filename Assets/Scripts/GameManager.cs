using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private static GameManager _instance;
        public Dictionary<string, int> Scores = new Dictionary<string, int>() { { "Team 1", 0 } , {"Team 2", 0} };
        private GameObject scoreBoardText = GameObject.Find("ScoreBoardText");
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
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void UpdateScoreBoard()
        {
                scoreBoardText.GetComponent<TextMeshPro>().SetText("Score <br> {0} - {1}", /*Team1score*/1, /*Team2score*/1); 
        }

    }
}

using Assets.Scripts.AI.State_Machine.Demo_StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private static GameManager _instance;
        public List<Baller> Ballers = new List<Baller>();
        public Dictionary<string, int> Scores = new Dictionary<string, int>() { { "Team 1", 0 } , {"Team 2", 0} };
        public Color TeamOneColor = Color.red;
        public Color TeamTwoColor = Color.blue;
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
            materialTeamOne.SetColor("_BallerTeamColor",GameManager.Instance.TeamOneColor);
            teamOneHoopMesh.material = materialTeamOne;
            var materialTeamTwo = teamTwoHoopMesh.material;
            materialTeamTwo.SetColor("_BallerTeamColor", GameManager.Instance.TeamTwoColor);
            teamTwoHoopMesh.material = materialTeamTwo;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

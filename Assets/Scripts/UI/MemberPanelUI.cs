using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MemberPanelUI : MonoBehaviour
{
    // Start is called before the first frame update


    public float Aggression = 0.5f;
    public float Defensiveness = 0.5f;
    public float Speed = 500f;
    public int Team = 1;
    public int Member = 1;

    public TextMeshProUGUI AggressionValue;
    public TextMeshProUGUI SpeedValue;
    public TextMeshProUGUI DefensivenessValue;
    public TextMeshProUGUI MemberString;
    public GameObject AggressionObj;
    public GameObject SpeedObj;
    public GameObject DefensivenessObj;
    public GameObject MemberObj;


    void Start()
    {
        AggressionValue = AggressionObj.GetComponent<TextMeshProUGUI>();
        SpeedValue = SpeedObj.GetComponent<TextMeshProUGUI>();
        DefensivenessValue = DefensivenessObj.GetComponent<TextMeshProUGUI>();
        MemberString = MemberObj.GetComponent<TextMeshProUGUI>();



        MemberString.text = $"Member: {Member}";
    }

    public void OnAgressionChange(float value)
    {
        AggressionValue.text = value.ToString();
        Aggression = value;
    }


    public void OnSpeedChange(float value)
    {
        SpeedValue.text = value.ToString();
        Speed = value;
    }

    public void OnDefensivenessChange(float value)
    {
        DefensivenessValue.text = value.ToString();
        Defensiveness = value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

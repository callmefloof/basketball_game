using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TeamPanelUI : MonoBehaviour
{
    // Start is called before the first frame update
    
    // Agression, Defensiveness, Speed, Team
    public List<(float, float, float, int)> BallerValues = new List<(float, float, float, int)>();

    public int Team = 1;
    public ScrollView BallerUIList;


    public VisualElement BallerSettingsPanel;


    void Start()
    {
        //BallerSettingsPanel = Find
        Func<VisualElement> makeitem = () => GameObject.FindGameObjectWithTag("MemberPanel").GetComponent<VisualElement>();

        BallerUIList = GetComponentInChildren<ScrollView>();
        BallerValues.Add((0.5f, 0.5f, 500f, Team));
        //BallerUIList.Add()

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

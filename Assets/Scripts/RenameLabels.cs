using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using GameInterfaces;

public class RenameLabels : MonoBehaviour
{
    [SerializeField] IStats<ComplexStats> player;
    private Text health;
    private Text magic;
    private Text experience;

    // Start is called before the first frame update
    void Start()
    {
        health = this.transform.Find("Health/Health Percentage").gameObject.GetComponent<Text>();
        magic = this.transform.Find("Mana/Mana Percentage").gameObject.GetComponent<Text>();
        experience = this.transform.Find("LVL").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        health.text = player.GetHP() + "%";
        magic.text = player.GetMP() + "%";
        experience.text = player.GetEXP() + "/" + player.GetLVLCap() + "XP";
    }
}

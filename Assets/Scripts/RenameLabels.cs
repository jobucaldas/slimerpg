using UnityEngine;
using UnityEngine.UI;
using GameInterfaces.CharacterInterface;

public class RenameLabels : MonoBehaviour
{
    [SerializeField] Player player; // Stats inside player
    private Text health;            // HP label
    private Text magic;             // MP label
    private Text experience;        // EXP label

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
        health.text = player.stats.hp + "%";
        magic.text = player.stats.mp + "%";
        experience.text = player.stats.exp + "/" + player.stats.FindNextLVLUp() + "XP";
    }
}

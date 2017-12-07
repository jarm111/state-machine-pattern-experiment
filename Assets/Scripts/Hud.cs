using UnityEngine;

public class Hud : MonoBehaviour {

    private PlayerMovement player;
    private GUIStyle font;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        font = new GUIStyle();
        font.fontSize = 25;
        font.normal.textColor = Color.green;
    }

    private void OnGUI()
    {
        GUI.TextField(new Rect(60, 60, 400, 100), "Player state: " + player.PlayerState, font);
    }

}

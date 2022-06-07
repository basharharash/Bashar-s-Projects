using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player_damage : MonoBehaviour {

    private Text life_text;
    private int life_count;
    private bool can_damage;
    // Start is called before the first frame update
    void Awake() {
        life_text = GameObject.Find("life_text").GetComponent<Text>();
        life_count = 3;
        life_text.text = "X" + life_count;

        can_damage = true;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void deal_damage() {
        if (can_damage) {
            life_count--;
            if (life_count >= 0) {
                life_text.text = "X" + life_count;
            }

            if (life_count == 0) {
                // Restart the game
                Time.timeScale = 0f;
                StartCoroutine(restart_game());
            }
            can_damage = false;
            StartCoroutine(wait_for_damage());
        }
    }

    IEnumerator wait_for_damage() {
        yield return new WaitForSeconds(2f);
        can_damage = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "spike") {
            if(can_damage) {
                gameObject.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, 7f);
                deal_damage();
            }
        }
    }

    IEnumerator restart_game() {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene("MainMenu");
        score.score_count = 0;
        Time.timeScale = 1f;
    }
}

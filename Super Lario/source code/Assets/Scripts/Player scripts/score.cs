using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour {

    
    public AudioClip coin;

    private Text coin_text_score;
    private AudioSource audio_manager;
    public static int score_count = 0;
    

    private void Awake() {
        audio_manager = GetComponent<AudioSource>();
    }

    void Start() {
        coin_text_score = GameObject.Find("coin_text").GetComponent<Text>();
    }


    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == my_tags.coin) {
            collision.gameObject.SetActive(false);
            score_count++;
            coin_text_score.text = "X" + score_count;
            audio_manager.PlayOneShot(coin, 0.6f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == my_tags.golden_block) {
            score_count++;
            coin_text_score.text = "X" + score_count;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main_menu_script : MonoBehaviour {
    
    public void play_game() {
        score.score_count = 0;
        SceneManager.LoadScene("GamePlay");
    }

    public void quit_game() {
        Application.Quit();
    }
}

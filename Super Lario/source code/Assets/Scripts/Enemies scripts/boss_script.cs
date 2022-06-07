using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_script : MonoBehaviour {

    public GameObject stone;
    public Transform attack_inst;
    public Transform bubble;

    public LayerMask player_layer;
    private Animator anim;

    private string coroutine_name = "start_attack";

    private bool can_attack;
    private bool need_to_check;

    private void Awake() {
        can_attack = false;
        need_to_check = true;
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start() {
        if (can_attack) {
            StartCoroutine(coroutine_name);
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (need_to_check) {
            RaycastHit2D bubble_hit = Physics2D.Raycast(bubble.position,
            Vector3.left, 5f, player_layer);
            if (bubble_hit) {
                can_attack = true;
                need_to_check = false;
                Start();
            }
        }
        
    }

    void attack() {
        GameObject stone_ = Instantiate(stone, attack_inst.position, Quaternion.identity);
        stone_.GetComponent<Rigidbody2D>().velocity = new Vector2 (Random.Range(-5,-18), 0);
    }
    void back_to_idle() {
        anim.Play("idle");
    }

    public void deactivate_boss_script() {
        StopCoroutine(coroutine_name);
        enabled = false;
    }

    IEnumerator start_attack() {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        anim.Play("attack");
        StartCoroutine(coroutine_name);
    }
}

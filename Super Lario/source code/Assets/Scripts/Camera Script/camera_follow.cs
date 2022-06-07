using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour {

    public float reset_speed = 0.5f;
    public float camera_speed = 0.3f;

    public Bounds camera_bounds;

    private Transform target;

    private float offset_z;
    private Vector3 last_target_pos;
    private Vector3 current_velocity;

    private bool follow_player;

    void Awake() {

        BoxCollider2D my_col = GetComponent<BoxCollider2D>();
        my_col.size = new Vector2(Camera.main.aspect * 2f * Camera.main.orthographicSize, 15f);
        camera_bounds = my_col.bounds;
        
    }
    // Start is called before the first frame update
    void Start() {
        target = GameObject.FindGameObjectWithTag(my_tags.player_tag).transform;
        last_target_pos = target.position;
        offset_z = (transform.position - target.position).z;
        follow_player = true;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (follow_player) {
            Vector3 ahead_target_pos = target.position + Vector3.forward * offset_z;
            if (ahead_target_pos.x >= transform.position.x) {
                Vector3 new_camera_pos = Vector3.SmoothDamp(transform.position, ahead_target_pos,
                    ref current_velocity, camera_speed);
                transform.position = new Vector3(new_camera_pos.x, transform.position.y,
                    new_camera_pos.z);
                last_target_pos = target.position;
            }
        }
    }
}

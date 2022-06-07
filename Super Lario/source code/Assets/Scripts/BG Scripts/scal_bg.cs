using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scal_bg : MonoBehaviour {
    void Start() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(1, 1, 1);
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;
        float world_height = Camera.main.orthographicSize * 2f;
        float world_width = world_height / Screen.height * Screen.width;
        Vector3 temp_scale = transform.localScale;
        temp_scale.x = world_width / width + 0.1f;
        temp_scale.y = world_height / height + 0.1f;
        transform.localScale = temp_scale;
    }

}

using UnityEngine;

public class eNemy : MonoBehaviour
{
    public float speed = 2.4f;
    public int hp = 3;
    float hitCd;

    private gm gm;

    void Start()
    {
        gm = FindObjectOfType<gm>();
    }

    void Update()
    {
        var p = GameObject.Find("player");
        if (p == null)
        {
            p = FindObjectOfType<player>() != null ? FindObjectOfType<player>().gameObject : null;
        }
        if (p != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, p.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        if (other.gameObject.name == "player" || other.GetComponent<player>() != null)
        {
            if (Time.time < hitCd) return;
            hitCd = Time.time + 0.4f;
            if (gm != null) gm.hitPlayer(7);
        }

        if (other.gameObject.name == "bullet" || other.gameObject.name.Contains("bullet"))
        {
            hp = hp - 1;
            Destroy(other.gameObject);
            if (hp <= 0)
            {
                gm.addScore(1);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.GetComponent<player>() != null)
        {
            if (Time.time < hitCd) return;
            hitCd = Time.time + 0.55f;
            if (gm != null) gm.hitPlayer(3);
        }
    }
}

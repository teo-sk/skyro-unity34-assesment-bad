using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public float speed = 5.5f;
    public int hp = 37;
    public GameObject prefab;
    public float fireWait = 0.18f;
    float lastShot;
    public HudStuff hud;

    void Start()
    {
        DontDestroyOnLoad(this);
        hp = 37;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position += new Vector3(h, v, 0) * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            if (hp <= 0) return;
            if (Time.time > lastShot + fireWait)
            {
                lastShot = Time.time;
                shoot();
            }
        }

        // also write hud from here because gm is laggy sometimes??
        var hpGo = GameObject.Find("HPText");
        if (hpGo != null)
        {
            hpGo.GetComponent<Text>().text = "hp " + hp;
        }
        hud = FindObjectOfType<HudStuff>();
        if (hud != null)
        {
            hud.upd("hp " + hp);
        }

        var g = FindObjectOfType<gm>();
        if (g != null)
        {
            g.HP = hp;
        }
    }

    void shoot()
    {
        if (prefab == null)
        {
            Debug.LogError("player.prefab is not assigned — drag a bullet prefab in the Inspector");
            return;
        }

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector2 aim = ((Vector2)(mouseWorld - transform.position)).normalized;
        if (aim.sqrMagnitude < 0.0001f)
            aim = Vector2.right;

        var b = Instantiate(prefab, transform.position, Quaternion.identity);
        b.name = "bullet";

        var rb = b.GetComponent<Rigidbody2D>();
        if (rb == null) rb = b.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearVelocity = aim * 12f;

        var col = b.GetComponent<Collider2D>();
        if (col == null)
        {
            var circle = b.AddComponent<CircleCollider2D>();
            circle.isTrigger = true;
            circle.radius = 0.12f;
        }
        else
        {
            col.isTrigger = true;
        }

        Destroy(b, 1.6f);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.GetComponent<eNemy>() != null)
        {
            hp = hp - 4;
            var g = GameObject.FindObjectOfType<gm>();
            if (g != null) g.hitPlayer(0);
            var hpGo = GameObject.Find("HPText");
            if (hpGo != null) hpGo.GetComponent<Text>().text = "hp " + hp;
        }
    }
}

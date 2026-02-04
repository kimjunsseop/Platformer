using UnityEditor.Callbacks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public float Speed {get;set;}
    [field:SerializeField] public float jumpUp {get;set;}
    public Vector2 direction;
    Animator anim;
    Rigidbody2D rig;
    SpriteRenderer render;
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        direction = Vector2.zero;
    }
    void KeyInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        if(direction.x < 0)
        {
            render.flipX = true;
        }
        else if(direction.x > 0)
        {
            render.flipX = false;
        }
    }
    void Update()
    {
        KeyInput();
        transform.Translate(direction * Speed * Time.deltaTime);
    }
    void FixedUpdate()
    {
        // rig.MovePosition(rig.position + direction * Time.deltaTime);
    }
}

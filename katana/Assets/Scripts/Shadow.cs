using UnityEngine;

public class Shadow : MonoBehaviour
{
    GameObject player;
    public float TwSpeed = 10;
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = Vector3.Lerp(transform.position, player.transform.position, TwSpeed * Time.deltaTime);
    }
    
}

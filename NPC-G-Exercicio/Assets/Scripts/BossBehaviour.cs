using System.Collections;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform weapon;
    [SerializeField] private float rateOfFire;    
    [SerializeField] private GameObject[] enemyPositions;

    private bool moving;
    private float curretSpeed;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void Start()
    {
        curretSpeed = speed;
        StartCoroutine(Actions());
    }

    
    void Update()
    {
        Move();
        AnimatiorManager();
    }
    

    IEnumerator Shoot()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(rateOfFire);
            Instantiate(bullet, weapon.position, weapon.rotation);
        }        
    }


    void Move()
    {
        if (moving == false) return;

        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    IEnumerator Actions()
    {
        moving = false;
        speed = 0;
                
        StartCoroutine(Shoot());

        yield return new WaitForSeconds(3.0f);        
        moving = true;

        yield return new WaitForSeconds(1.0f);
        speed = curretSpeed;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyPosition")
        {
            if(transform.position.x < 0)
            {

                enemyPositions[0].GetComponent<Collider2D>().enabled = true;
                enemyPositions[1].GetComponent<Collider2D>().enabled = false;
                transform.eulerAngles = new Vector2(0, 180);
            }
            else if(transform.position.x > 0)
            {                
                enemyPositions[0].GetComponent<Collider2D>().enabled = false;
                enemyPositions[1].GetComponent<Collider2D>().enabled = true;
                transform.eulerAngles = new Vector2(0, 0);
            }

            StartCoroutine(Actions());
            
        }
    }

    void AnimatiorManager()
    {
        animator.SetBool("pMove", moving);
        animator.SetInteger("pSpeed", (int)speed);
    }

}

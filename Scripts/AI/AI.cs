using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;

    public Transform[] destinations;

    public float distanceToFollowPath = 2;

    private int i = 0;

    [Header("----------FollowPlayer?---------")]

    public bool followPlayer;

    private GameObject player;

    private float distanceToPlayer;

    public float distanceToFollowPlayer = 10;

    public int lifes = 3;


    void Start()
    {
        if (destinations ==null || destinations.Length == 0)
        {
            transform.gameObject.GetComponent<AI>().enabled = false;
        }
        else
        {
            navMeshAgent.destination = destinations[0].transform.position;

            player = FindObjectOfType<PlayerMovement>().gameObject;
        }
       
    }

   
    void Update()
    {

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer<=distanceToFollowPlayer && followPlayer)
        {
            FollowPlayer();
        }
        else
        {
            EnemyPath();
        }

    }


    public void EnemyPath()
    {
        navMeshAgent.destination = destinations[i].position;

        if (Vector3.Distance(transform.position, destinations[i].position) <= distanceToFollowPath )
        {
            if (destinations[i] != destinations[destinations.Length - 1])
            {
                i++;
            }
            else
            {
                i = 0;
            }
        }

    }




    public void FollowPlayer()
    {
        navMeshAgent.destination = player.transform.position;
    }


    public void GrenadeImpact()
    {
        LooseLife(2);
    }

    public void LooseLife(int lifesToLoose)
    {
        lifes = lifes - lifesToLoose;

        if (lifes<= 0)
        {
            Destroy(gameObject);
        }

    }

}

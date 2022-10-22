using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBossAI : MonoBehaviour
{
    private GameObject player;
    private float _attackRange = 40.0f;
    private float _rayDistance = 5.0f;
    private float _stoppingDistance = 1.5f;
    
    private Vector3 _destination;
    private Quaternion _desiredRotation;
    private Vector3 _direction;
    private BossState _currentState;
    private int count = 1;
    
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    private void GetDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4f)) +
                               new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-3f, 3f),
                                   UnityEngine.Random.Range(-10f, 10f));

        _destination = new Vector3(testPosition.x, System.Math.Min(System.Math.Max(testPosition.y, 27), 40), testPosition.z);
        _direction = Vector3.Normalize(_destination - transform.position);
        _desiredRotation = Quaternion.LookRotation(_direction);
    }

    private void Update()
    {
        switch (_currentState)
        {
            case BossState.Wander:
            {
                Debug.Log("wander");
                
                
                if(NeedsDestination())
                {
                    GetDestination();
                }
                count++;
                transform.rotation = _desiredRotation;
                transform.Translate(Vector3.forward * Time.deltaTime * 5f);
                var rayColor = IsPathBlocked() ? Color.red : Color.green;
                Debug.DrawRay(transform.position, _direction * _rayDistance, rayColor);

                while (IsPathBlocked())
                {
                    Debug.Log("Path Blocked");
                    GetDestination();
                }
 
                if (Vector3.Distance(transform.position, player.transform.position) < 40f)
                {
                    _currentState = BossState.Chase;
                } 
                break;
            }
            case BossState.Chase:
            {   
                Debug.Log("chase");
                if (Vector3.Distance(transform.position, player.transform.position) >= 40f)
                {
                    _currentState = BossState.Wander;
                    count = 1;
                    return;
                }
                
                transform.LookAt(player.transform);
                transform.Translate(transform.forward * Time.deltaTime * 5f);

                if (Vector3.Distance(transform.position, player.transform.position) < _attackRange)
                {
                    _currentState = BossState.Attack;
                }
                break;
            }
            case BossState.Attack:
            {
                if (Vector3.Distance(transform.position, player.transform.position) >= _attackRange)
                {
                        _currentState = BossState.Chase;
                        return;
                }
                transform.RotateAround(player.transform.position, Vector3.up, 10 * Time.deltaTime);
                transform.LookAt(player.transform);
                Debug.Log("Attack");
                break;
            }
        }
    }


    private bool IsPathBlocked()
    {
        Ray ray = new Ray(transform.position, _direction);
        var hitSomething = Physics.RaycastAll(ray, _rayDistance);
        return hitSomething.Any();
    }

    private bool NeedsDestination()
    {  
        if (count == 1 || Vector3.Distance(transform.position, _destination) <= _stoppingDistance)
        {
            return true;
        }
        else
        {
            return false;
        } 
    }
}

public enum BossState
{
    Wander,
    Chase,
    Attack
}
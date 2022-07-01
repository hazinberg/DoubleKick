using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    private MovementInput movementInput;
    private CombatScript combatScript;

    public LayerMask layerMask;
    public LayerMask obstructionMask;

    [SerializeField] Vector3 inputDirection;
    [SerializeField] private EnemyScript currentTarget;

    public GameObject cam;

    public float angle = 90f; // задал в сцене
    float radius = 10f;

    private void Start()
    {
        movementInput = GetComponentInParent<MovementInput>();
        combatScript = GetComponentInParent<CombatScript>();
    }

    private void Update()
    {
        var forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        inputDirection = forward;
        inputDirection = inputDirection.normalized;

        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, layerMask);
        
        if (rangeChecks.Length > 0) {
            currentTarget = null;
            float distanceToTarget = float.MaxValue;

            foreach (var enemy in rangeChecks) {
                Transform target = enemy.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(forward, directionToTarget) < angle / 2) {
                    float dist = Vector3.Distance(transform.position, target.position);

                    if (dist < distanceToTarget) {
                        distanceToTarget = dist;
                        currentTarget = target.transform.GetComponent<EnemyScript>();

                        //Debug.Log (dist.ToString());
                        //Debug.Log (currentTarget.health);
                    }
                } 
            }
        } else { 
            currentTarget = null;
        }
    }

    private void Update1()
    {

        var camera = Camera.main;
        var forward = camera.transform.forward;
        var right = camera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        //inputDirection = forward * movementInput.moveAxis.y + right * movementInput.moveAxis.x; // Направление врага относительно камеры 

        inputDirection = forward;
        inputDirection = inputDirection.normalized;

        RaycastHit info;
        // if (Physics.SphereCast(transform.position, 3f, inputDirection, out info, 10,layerMask)) 
        if (Physics.SphereCast(transform.position, 1f, inputDirection, out info, 10, layerMask)) // определяем врага на пути
        {
            if(info.collider.transform.GetComponent<EnemyScript>().IsAttackable()) {
                currentTarget = info.collider.transform.GetComponent<EnemyScript>();
                //Debug.Log (currentTarget.health);
                //Debug.Log (inputDirection.ToString());
                //Debug.Log ("EEEEEEEEEEEEEEEEE");  
            }
        } else {
            // добавил
            currentTarget = null;
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public EnemyScript CurrentTarget()
    {
        return currentTarget;
    }

    public void SetCurrentTarget(EnemyScript target)
    {
        currentTarget = target;
    }

    public float InputMagnitude()
    {
        return inputDirection.magnitude;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, inputDirection);
        Gizmos.DrawWireSphere(transform.position, 1);
        if(CurrentTarget() != null)
            Gizmos.DrawSphere(CurrentTarget().transform.position, .5f);


        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, radius);

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(transform.position, transform.position + viewAngle01 * radius);
        Handles.DrawLine(transform.position, transform.position + viewAngle02 * radius);

        if (currentTarget != null)
        {
            Handles.color = Color.green;
            //Handles.DrawLine(transform.position, playerRef.transform.position);
            Handles.DrawLine(transform.position, currentTarget.transform.position);

        }
    }
}

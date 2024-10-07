using _Project._Scripts;
using System.Linq;
using UnityEngine;

namespace AI
{
    public class MeleeEnemyInput : Controller
    {
        [Header("AI")]
        [Tooltip("The AI doesn't correct between this angle and it's negative value")]
        [SerializeField] private float angleErrorMargin = 0.1f;
        [Tooltip("The steering starts smoothing out when the angle is lower than this")]
        [SerializeField] private float steeringSmoothMinAngle = 4;
        [Tooltip("What angle should the angle pass in order to reverse")]
        [SerializeField] private float reverseAngle = 90;
        [SerializeField] private float decelSpeed = 100;

        private SpringHandler springHandler;
        private Vector3 previousVector = Vector3.zero;

        protected override void Start()
        {
            base.Start();
            moveInput = 1;
            springHandler = new SpringHandler(GetComponents<SpringComponent>().Where(x => x.Data.SpringTag != SpringEnum.overrideE).ToList());
            GetComponent<CarStats>().destroyedLocal += MeleeEnemyInput_destroyed;
        }

        private void MeleeEnemyInput_destroyed()
        {
            GetComponent<SpringEntity>().springTag = SpringEnum.solid;
            var entities = GetComponentsInChildren<SpringEntity>();
            foreach (var item in entities)
            {
                item.springTag = SpringEnum.solid;
            } 
        }

        protected override void GetPlayerInput()
        {
            var _vector = springHandler.CalculateDirectionVector();
            var mag = _vector.magnitude;
            /*_vector.Normalize();
            _vector *= mag;*/
            steerInput = SetSteering(_vector);
            //Debug.Log(steerInput);
            moveInput = SetAcceleration(_vector);
            //isBreaking = (verticalInput <= 0);
        }

        /* float SetAcceleration(Vector3 _direction)
         {
             var _horizontalDir = -AngleDir(transform.right, _direction, Vector3.up, angleErrorMargin);
             Vector2 perp = Vector3.Cross(transform.right, _direction);
             float dir = -Vector2.Dot(perp, Vector3.up);

             if (dir > angleErrorMargin + 0.3f)
             {
                 return 1f;
             }

             if (dir < -angleErrorMargin + 0.3f)
             {
                 return -1f;
             }
             Vector3 localVelocity = transform.InverseTransformDirection(carRB.velocity);

 *//*            if (localVelocity.z > decelSpeed)
                 return 0f;*//*

             return steerInput;
         }

         float SetSteering(Vector3 _direction)
         {
             var _dir = AngleDir(transform.forward, _direction, Vector3.up, 0.02f);

             var _angle = Vector3.Angle(transform.forward, _direction);

             if (_angle <= steeringSmoothMinAngle)       //Keep the enemy from passing the desired rotation
             {
                 return Mathf.Clamp(_dir * _angle / steeringSmoothMinAngle, -1, 1);
             }

             if (SetAcceleration(_direction) > 0)
             {
                 return _dir;
             }

             return -1 * _dir;
         }*/

        float SetAcceleration(Vector3 _direction)
        {
            var _horizontalDir = -AngleDir(transform.right, _direction, Vector3.up, angleErrorMargin);

            Vector2 perp = Vector3.Cross(transform.right, _direction);
            float dir = -Vector2.Dot(perp, Vector3.up);

            if (dir > angleErrorMargin - 0.5f)
            {
                return 1f;
            }

            if (dir < -angleErrorMargin - 0.5f)
            {
                return -1f;
            }

            return moveInput;
        }

        float SetSteering(Vector3 _direction)
        {
            var _dir = AngleDir(transform.forward, _direction, Vector3.up, angleErrorMargin);

            var _angle = Vector2.Angle(transform.up, _direction);

            if (_angle <= steeringSmoothMinAngle)       //Keep the enemy from passing the desired rotation
            {
                _dir = Mathf.Clamp(_dir * _angle / steeringSmoothMinAngle, -1, 1);
            }
            if (SetAcceleration(_direction) < 0) return -1 * _dir;
            return _dir;
        }

        float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up, float errorMargin) //-1 if to the left, 1 if to the right
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);

            if (dir > errorMargin)
            {
                return 1f;
            }

            if (dir < -errorMargin)
            {
                return -1f;
            }

            return 0f;
        }

        private void OnDrawGizmos()
        {
            /*if (Application.isPlaying && springHandler != null)
            {
                var _direction = springHandler.CalculateDirectionVector();
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + _direction * 15);
                Gizmos.DrawLine(transform.position, transform.position + transform.forward * 15);
            }*/
        }
    }
}

using System.Linq;
using UnityEngine;

namespace AI
{
    public class MeleeEnemyInput : Controller
    {
        [SerializeField] private float angleErrorMargin = 0.1f;
        [SerializeField] private float turnFactor = 90;

        private SpringHandler springHandler;
        private Vector3 previousVector = Vector3.zero;

        protected override void Start()
        {
            base.Start();
            springHandler = new SpringHandler(GetComponents<SpringComponent>().Where(x => x.Data.SpringTag != SpringEnum.overrideE).ToList());
        }

        protected override void GetPlayerInput()
        {
            var _vector = springHandler.CalculateDirectionVector();
            steerInput = SetSteering(_vector);
            //Debug.Log(steerInput);
            moveInput = 1;//SetAcceleration(_vector);
            //isBreaking = (verticalInput <= 0);
        }

        float SetAcceleration(Vector3 _direction)
        {
            var _horizontalDir = -AngleDir(transform.right, _direction, Vector3.up, angleErrorMargin);

            if (_horizontalDir != 0)
            {
                return 1;
            }

            return -1;
        }

        float SetSteering(Vector3 _direction)
        {
            var _dir = AngleDir(transform.forward, _direction, Vector3.up, angleErrorMargin);

            var _angle = Vector3.Angle(transform.up, _direction);

            if (_angle <= turnFactor)       //Keep the enemy from passing the desired rotation
            {
                return Mathf.Clamp(_dir * _angle / turnFactor, -1, 1);
            }

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
            if (Application.isPlaying && springHandler != null)
            {
                var _direction = springHandler.CalculateDirectionVector();
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + _direction * 15);
                Gizmos.DrawLine(transform.position, transform.position + transform.forward * 15);
            }
        }
    }
}

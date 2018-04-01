using UnityEngine;
using System.Collections;
using Svelto.ECS;
using Svelto.Tasks;
using Svelto.DataStructures;

namespace ECS.Tanks.Camera
{
    public class CameraEngine : SingleEntityViewEngine<CameraEntityView>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { get; set; }

        private readonly ITime _Time;
        //private readonly ITaskRoutine _TaskRoutine;
        private CameraEntityView _CameraEntityView;
        private FasterReadOnlyList<CameraTargetEntityView> _CameraTargetEntityViews;

        private Vector3 _MoveVelocity;
        private Vector3 _DesiredPosition;
        private float _ZoomSpeed;

        public CameraEngine(ITime time)
        {
            _Time = time;
        }

        public void Ready() {
            Tick().RunOnSchedule(StandardSchedulers.physicScheduler);
        }

        protected override void Add(CameraEntityView entityView)
        {
            _CameraEntityView = entityView;
        }

        protected override void Remove(CameraEntityView entityView)
        {
            _CameraEntityView = null;
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                _CameraTargetEntityViews = entityViewsDB.QueryEntityViews<CameraTargetEntityView>();
                if (_CameraTargetEntityViews.Count > 0)
                {
                    Move();
                    Zoom();
                }
                yield return null;
            }
        }

        private void Move()
        {
            FindAveragePosition();

            _CameraEntityView.TransformComponent.Position = Vector3.SmoothDamp(_CameraEntityView.TransformComponent.Position, _DesiredPosition, ref _MoveVelocity, _CameraEntityView.CameraRigComponent.DampTime);
        }

        private void Zoom()
        {
            float requiredSize = FindRequiredSize();
            _CameraEntityView.CameraComponent.OrthographicSize = Mathf.SmoothDamp(_CameraEntityView.CameraComponent.OrthographicSize, requiredSize, ref _ZoomSpeed, _CameraEntityView.CameraRigComponent.DampTime);
        }

        private float FindRequiredSize()
        {
            Vector3 desiredLocalPos = _CameraEntityView.CameraRigComponent.InverseTransformPoint(_DesiredPosition);

            float size = 0f;

            for (int i = 0; i < _CameraTargetEntityViews.Count; i++)
            {
                if (!_CameraTargetEntityViews[i].CameraTargetComponent.ActiveSelf)
                    continue;

                Vector3 targetLocalPos = _CameraEntityView.CameraRigComponent.InverseTransformPoint(_CameraTargetEntityViews[i].CameraTargetComponent.Position);


                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / _CameraEntityView.CameraComponent.Aspect);
            }

            size += _CameraEntityView.CameraRigComponent.ScreenEdgeBuffer;

            size = Mathf.Max(size, _CameraEntityView.CameraRigComponent.MinSize);

            return size;
        }

        private void FindAveragePosition()
        {
            Vector3 averagePos = new Vector3();
            int numTargets = 0;

            for (int i = 0; i < _CameraTargetEntityViews.Count; i++)
            {

                if (!_CameraTargetEntityViews[i].CameraTargetComponent.ActiveSelf)
                    continue;

                averagePos += _CameraTargetEntityViews[i].CameraTargetComponent.Position;
                numTargets++;
            }

            if (numTargets > 0)
                averagePos /= numTargets;

            averagePos.y = _CameraEntityView.PositionComponent.Position.y;

            _DesiredPosition = averagePos;
        }
    }
}

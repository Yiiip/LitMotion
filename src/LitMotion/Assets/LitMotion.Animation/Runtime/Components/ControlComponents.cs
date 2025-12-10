using System;
using UnityEngine;
using UnityEngine.Events;

namespace LitMotion.Animation.Components
{
    [Serializable]
    [LitMotionAnimationComponentMenu("Control/Delay")]
    public sealed class DelayComponent : LitMotionAnimationComponent
    {
        [SerializeField] float delay;
        [SerializeField] bool ignoreTimeScale = true; //新增字段，是否忽略时间缩放
        public float Delay
        {
            get => delay;
            set => delay = value;
        }
        public bool IgnoreTimeScale
        {
            get => ignoreTimeScale;
            set => ignoreTimeScale = value;
        }

        public override MotionHandle Play()
        {
            var builder = LMotion.Create(0f, 1f, delay);
            if (ignoreTimeScale)
            {
                builder.WithIgnoreTimeScale();
            }
            return builder.RunWithoutBinding();
        }

        public override void OnStop() { }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Control/Event")]
    public sealed class EventComponent : LitMotionAnimationComponent
    {
        [Space(5f)]
        [SerializeField] UnityEvent onPlay;
        [SerializeField] UnityEvent onStop;

        public override MotionHandle Play()
        {
            onPlay.Invoke();
            return LMotion.Create(0f, 1f, 0f).RunWithoutBinding();
        }

        public override void OnStop()
        {
            onStop.Invoke();
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Control/Play LitMotion Animation")]
    public sealed class PlayLitMotionAnimationComponent : LitMotionAnimationComponent
    {
        [SerializeField] LitMotionAnimation target;

        public override MotionHandle Play()
        {
            target.Play();
            return LMotion.Create(0f, 1f, float.MaxValue)
                .Bind(this, (x, state) =>
                {
                    if (target == null) TrackedHandle.TryComplete();
                    if (!target.IsPlaying)
                    {
                        TrackedHandle.TryComplete();
                        target.Cancel(); //新增代码
                    }
                });
        }

        public override void OnResume()
        {
            target.Play();
        }

        public override void OnPause()
        {
            target.Pause();
        }

        public override void OnStop()
        {
            target.Stop();
        }
    }
}
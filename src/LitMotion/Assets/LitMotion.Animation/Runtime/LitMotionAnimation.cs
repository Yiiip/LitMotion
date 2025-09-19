using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitMotion.Collections;
using UnityEngine;

namespace LitMotion.Animation
{
    [AddComponentMenu("LitMotion Animation")]
    public sealed class LitMotionAnimation : MonoBehaviour
    {
        enum AnimationMode
        {
            Parallel,
            Sequential
        }

        public string notes; //修改了源码，新增备注字段
        [SerializeField] bool playOnAwake = false; //修改了源码的默认值
        [SerializeField] AnimationMode animationMode;

        [SerializeReference]
        LitMotionAnimationComponent[] components;

        Queue<LitMotionAnimationComponent> queue = new();
        FastListCore<LitMotionAnimationComponent> playingComponents;

        public IReadOnlyList<LitMotionAnimationComponent> Components => components;

        void Start()
        {
            if (playOnAwake) Play();
        }

        void MoveNextMotion()
        {
            if (queue.TryDequeue(out var queuedComponent))
            {
                try
                {
                    var handle = queuedComponent.Play();
                    var isActive = handle.IsActive();

                    if (isActive)
                    {
                        handle.Preserve();
                        MotionManager.GetManagedDataRef(handle, false).OnCompleteAction += MoveNextMotion;
                    }

                    queuedComponent.TrackedHandle = handle;
                    playingComponents.Add(queuedComponent);

                    if (!isActive)
                    {
                        MoveNextMotion();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        public void Play()
        {
            var isPlaying = false;

            foreach (var component in playingComponents.AsSpan())
            {
                var handle = component.TrackedHandle;
                if (handle.IsActive())
                {
                    handle.PlaybackSpeed = 1f;
                    isPlaying = true;

                    component.OnResume();
                }
            }

            if (isPlaying) return;

            playingComponents.Clear();

            switch (animationMode)
            {
                case AnimationMode.Sequential:
                    foreach (var component in components)
                    {
                        if (component == null) continue;
                        if (!component.Enabled) continue;
                        queue.Enqueue(component);
                    }

                    MoveNextMotion();
                    break;
                case AnimationMode.Parallel:
                    foreach (var component in components)
                    {
                        if (component == null) continue;
                        if (!component.Enabled) continue;

                        try
                        {
                            var handle = component.Play();
                            component.TrackedHandle = handle;

                            if (handle.IsActive())
                            {
                                handle.Preserve();
                            }

                            playingComponents.Add(component);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    }
                    break;
            }
        }

        public void Pause()
        {
            foreach (var component in playingComponents.AsSpan())
            {
                var handle = component.TrackedHandle;
                if (handle.IsActive())
                {
                    handle.PlaybackSpeed = 0f;
                    component.OnPause();
                }
            }
        }

        public void Stop()
        {
            var span = playingComponents.AsSpan();
            span.Reverse();
            foreach (var component in span)
            {
                var handle = component.TrackedHandle;
                handle.TryCancel();
                component.OnStop();
                component.TrackedHandle = handle;
            }

            playingComponents.Clear();
            queue.Clear();
        }

        public void Cancel() //修改了源码，新增方法
        {
            var span = playingComponents.AsSpan();
            span.Reverse();
            foreach (var component in span)
            {
                var handle = component.TrackedHandle;
                handle.TryCancel();
                //不走OnStop()
                component.TrackedHandle = handle;
            }

            playingComponents.Clear();
            queue.Clear();
        }

        public void Restart()
        {
            Stop();
            Play();
        }

        private System.Threading.CancellationTokenSource _cancelPlayAsync;
        public async UniTask PlayAsync(Action onShowEnd = null) //修改了源码，新增方法
        {
            if (_cancelPlayAsync != null)
            {
                _cancelPlayAsync.Cancel();
                _cancelPlayAsync.Dispose();
            }
            _cancelPlayAsync = new();
            Cancel();
            Play();
            await UniTask.WaitUntil(() => !IsPlaying, cancellationToken: _cancelPlayAsync.Token);
            onShowEnd?.Invoke();
        }

        public bool IsActive
        {
            get
            {
                if (queue.Count > 0) return true;

                foreach (var component in playingComponents.AsSpan())
                {
                    var handle = component.TrackedHandle;
                    if (handle.IsActive()) return true;
                }

                return false;
            }
        }

        public bool IsPlaying
        {
            get
            {
                if (queue.Count > 0) return true;

                foreach (var component in playingComponents.AsSpan())
                {
                    var handle = component.TrackedHandle;
                    if (handle.IsPlaying()) return true;
                }

                return false;
            }
        }

        void OnDestroy()
        {
            if (_cancelPlayAsync != null)
            {
                _cancelPlayAsync.Cancel();
                _cancelPlayAsync.Dispose();
            }
            Stop();
        }
    }
}
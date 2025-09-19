
using System;
using LitMotion.Adapters;
using UnityEngine;

namespace LitMotion.Animation.Components
{
    public abstract class TransformPositionAnimationBase<TOptions, TAdapter> : PropertyAnimationComponent<Transform, Vector3, TOptions, TAdapter>
        where TOptions : unmanaged, IMotionOptions
        where TAdapter : unmanaged, IMotionAdapter<Vector3, TOptions>
    {
        [SerializeField] bool useWorldSpace;

        protected override Vector3 GetValue(Transform target)
        {
            return useWorldSpace ? target.position : target.localPosition;
        }

        protected override void SetValue(Transform target, in Vector3 value)
        {
            if (useWorldSpace) target.position = value;
            else target.localPosition = value;
        }

        protected override Vector3 GetRelativeValue(in Vector3 startValue, in Vector3 relativeValue)
        {
            return startValue + relativeValue;
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Position")]
    public sealed class TransformPositionAnimation : TransformPositionAnimationBase<NoOptions, Vector3MotionAdapter> { }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Position (Punch)")]
    public sealed class TransformPositionPunchAnimation : TransformPositionAnimationBase<PunchOptions, Vector3PunchMotionAdapter> { }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Position (Shake)")]
    public sealed class TransformPositionShakeAnimation : TransformPositionAnimationBase<ShakeOptions, Vector3ShakeMotionAdapter> { }

    public abstract class TransformRotationAnimationBase<TOptions, TAdapter> : PropertyAnimationComponent<Transform, Vector3, TOptions, TAdapter>
        where TOptions : unmanaged, IMotionOptions
        where TAdapter : unmanaged, IMotionAdapter<Vector3, TOptions>
    {
        [SerializeField] bool useWorldSpace;

        protected override Vector3 GetValue(Transform target)
        {
            return useWorldSpace ? target.eulerAngles : target.localEulerAngles;
        }

        protected override void SetValue(Transform target, in Vector3 value)
        {
            if (useWorldSpace) target.eulerAngles = value;
            else target.localEulerAngles = value;
        }

        protected override Vector3 GetRelativeValue(in Vector3 startValue, in Vector3 relativeValue)
        {
            return startValue + relativeValue;
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Rotation")]
    public sealed class TransformRotationAnimation : TransformRotationAnimationBase<NoOptions, Vector3MotionAdapter> { }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Rotation (Punch)")]
    public sealed class TransformRotationPunchAnimation : TransformRotationAnimationBase<PunchOptions, Vector3PunchMotionAdapter> { }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Rotation (Shake)")]
    public sealed class TransformRotationShakeAnimation : TransformRotationAnimationBase<ShakeOptions, Vector3ShakeMotionAdapter> { }

    public abstract class TransformScaleAnimationBase<TOptions, TAdapter> : PropertyAnimationComponent<Transform, Vector3, TOptions, TAdapter>
        where TOptions : unmanaged, IMotionOptions
        where TAdapter : unmanaged, IMotionAdapter<Vector3, TOptions>
    {
        protected override Vector3 GetValue(Transform target)
        {
            return target.localScale;
        }

        protected override void SetValue(Transform target, in Vector3 value)
        {
            target.localScale = value;
        }

        protected override Vector3 GetRelativeValue(in Vector3 startValue, in Vector3 relativeValue)
        {
            return startValue + relativeValue;
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Scale")]
    public sealed class TransformScaleAnimation : TransformScaleAnimationBase<NoOptions, Vector3MotionAdapter> { }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Scale (Punch)")]
    public sealed class TransformScalePunchAnimation : TransformScaleAnimationBase<PunchOptions, Vector3PunchMotionAdapter> { }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Scale (Shake)")]
    public sealed class TransformScaleShakeAnimation : TransformScaleAnimationBase<ShakeOptions, Vector3ShakeMotionAdapter> { }



    //修改了源码，新增支持了xyz分量
    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Position X")]
    public sealed class TransformPositionXAnimation : FloatPropertyAnimationComponent<Transform>
    {
        [SerializeField] bool useWorldSpace;
        protected override float GetValue(Transform target) => useWorldSpace ? target.position.x : target.localPosition.x;
        protected override void SetValue(Transform target, in float value)
        {
            if (useWorldSpace)
            {
                var p = target.position;
                p.x = value;
                target.position = p;
            }
            else
            {
                var p = target.localPosition;
                p.x = value;
                target.localPosition = p;
            }
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Position Y")]
    public sealed class TransformPositionYAnimation : FloatPropertyAnimationComponent<Transform>
    {
        [SerializeField] bool useWorldSpace;
        protected override float GetValue(Transform target) => useWorldSpace ? target.position.y : target.localPosition.y;
        protected override void SetValue(Transform target, in float value)
        {
            if (useWorldSpace)
            {
                var p = target.position;
                p.y = value;
                target.position = p;
            }
            else
            {
                var p = target.localPosition;
                p.y = value;
                target.localPosition = p;
            }
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Position Z")]
    public sealed class TransformPositionZAnimation : FloatPropertyAnimationComponent<Transform>
    {
        [SerializeField] bool useWorldSpace;
        protected override float GetValue(Transform target) => useWorldSpace ? target.position.z : target.localPosition.z;
        protected override void SetValue(Transform target, in float value)
        {
            if (useWorldSpace)
            {
                var p = target.position;
                p.z = value;
                target.position = p;
            }
            else
            {
                var p = target.localPosition;
                p.z = value;
                target.localPosition = p;
            }
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Rotation X")]
    public sealed class TransformRotationXAnimation : FloatPropertyAnimationComponent<Transform>
    {
        [SerializeField] bool useWorldSpace;
        protected override float GetValue(Transform target) => useWorldSpace ? target.eulerAngles.x : target.localEulerAngles.x;
        protected override void SetValue(Transform target, in float value)
        {
            if (useWorldSpace)
            {
                var e = target.eulerAngles;
                e.x = value;
                target.eulerAngles = e;
            }
            else
            {
                var e = target.localEulerAngles;
                e.x = value;
                target.localEulerAngles = e;
            }
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Rotation Y")]
    public sealed class TransformRotationYAnimation : FloatPropertyAnimationComponent<Transform>
    {
        [SerializeField] bool useWorldSpace;
        protected override float GetValue(Transform target) => useWorldSpace ? target.eulerAngles.y : target.localEulerAngles.y;
        protected override void SetValue(Transform target, in float value)
        {
            if (useWorldSpace)
            {
                var e = target.eulerAngles;
                e.y = value;
                target.eulerAngles = e;
            }
            else
            {
                var e = target.localEulerAngles;
                e.y = value;
                target.localEulerAngles = e;
            }
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Rotation Z")]
    public sealed class TransformRotationZAnimation : FloatPropertyAnimationComponent<Transform>
    {
        [SerializeField] bool useWorldSpace;
        protected override float GetValue(Transform target) => useWorldSpace ? target.eulerAngles.z : target.localEulerAngles.z;
        protected override void SetValue(Transform target, in float value)
        {
            if (useWorldSpace)
            {
                var e = target.eulerAngles;
                e.z = value;
                target.eulerAngles = e;
            }
            else
            {
                var e = target.localEulerAngles;
                e.z = value;
                target.localEulerAngles = e;
            }
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Scale X")]
    public sealed class TransformScaleXAnimation : FloatPropertyAnimationComponent<Transform>
    {
        protected override float GetValue(Transform target) => target.localScale.x;
        protected override void SetValue(Transform target, in float value)
        {
            var s = target.localScale;
            s.x = value;
            target.localScale = s;
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Scale Y")]
    public sealed class TransformScaleYAnimation : FloatPropertyAnimationComponent<Transform>
    {
        protected override float GetValue(Transform target) => target.localScale.y;
        protected override void SetValue(Transform target, in float value)
        {
            var s = target.localScale;
            s.y = value;
            target.localScale = s;
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("Transform/Scale Z")]
    public sealed class TransformScaleZAnimation : FloatPropertyAnimationComponent<Transform>
    {
        protected override float GetValue(Transform target) => target.localScale.z;
        protected override void SetValue(Transform target, in float value)
        {
            var s = target.localScale;
            s.z = value;
            target.localScale = s;
        }
    }
}
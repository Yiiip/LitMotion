using System;
using LitMotion.Adapters;
using UnityEngine;

namespace LitMotion.Animation.Components
{
    [Serializable]
    [LitMotionAnimationComponentMenu("UI/Rect Transform/Size Delta")]
    public sealed class RectTransformSizeDeltaAnimation : Vector2PropertyAnimationComponent<RectTransform>
    {
        protected override Vector2 GetValue(RectTransform target) => target.sizeDelta;
        protected override void SetValue(RectTransform target, in Vector2 value) => target.sizeDelta = value;
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("UI/Rect Transform/Pivot")]
    public sealed class RectTransformPivotAnimation : Vector2PropertyAnimationComponent<RectTransform>
    {
        protected override Vector2 GetValue(RectTransform target) => target.pivot;
        protected override void SetValue(RectTransform target, in Vector2 value) => target.pivot = value;
    }

    // 修改了源码，新增AnchoredPosition的类型
    [Serializable]
    public abstract class RectTransformAnchoredPositionAnimationBase<TOptions, TAdapter> : PropertyAnimationComponent<RectTransform, Vector2, TOptions, TAdapter>
        where TOptions : unmanaged, IMotionOptions
        where TAdapter : unmanaged, IMotionAdapter<Vector2, TOptions>
    {
        protected override Vector2 GetValue(RectTransform target)
        {
            return target.anchoredPosition;
        }

        protected override void SetValue(RectTransform target, in Vector2 value)
        {
            target.anchoredPosition = value;
        }

        protected override Vector2 GetRelativeValue(in Vector2 startValue, in Vector2 relativeValue)
        {
            return startValue + relativeValue;
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("UI/Rect Transform/Anchored Position")]
    public sealed class RectTransformAnchoredPositionAnimation : RectTransformAnchoredPositionAnimationBase<NoOptions, Vector2MotionAdapter> { }

    [Serializable]
    [LitMotionAnimationComponentMenu("UI/Rect Transform/Anchored Position X")]
    public sealed class RectTransformAnchoredPositionXAnimation : FloatPropertyAnimationComponent<RectTransform>
    {
        protected override float GetValue(RectTransform target) => target.anchoredPosition.x;
        protected override void SetValue(RectTransform target, in float value)
        {
            var p = target.anchoredPosition;
            p.x = value;
            target.anchoredPosition = p;
        }
    }

    [Serializable]
    [LitMotionAnimationComponentMenu("UI/Rect Transform/Anchored Position Y")]
    public sealed class RectTransformAnchoredPositionYAnimation : FloatPropertyAnimationComponent<RectTransform>
    {
        protected override float GetValue(RectTransform target) => target.anchoredPosition.y;
        protected override void SetValue(RectTransform target, in float value)
        {
            var p = target.anchoredPosition;
            p.y = value;
            target.anchoredPosition = p;
        }
    }
}
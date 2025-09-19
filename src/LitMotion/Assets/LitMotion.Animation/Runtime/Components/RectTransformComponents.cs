using System;
using LitMotion;
using LitMotion.Adapters;
using LitMotion.Animation;
using UnityEngine;

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
public abstract class TransformAnchoredPositionAnimationBase<TOptions, TAdapter> : PropertyAnimationComponent<RectTransform, Vector3, TOptions, TAdapter>
    where TOptions : unmanaged, IMotionOptions
    where TAdapter : unmanaged, IMotionAdapter<Vector3, TOptions>
{
    protected override Vector3 GetValue(RectTransform target)
    {
        return target.anchoredPosition;
    }

    protected override void SetValue(RectTransform target, in Vector3 value)
    {
        target.anchoredPosition = value;
    }

    protected override Vector3 GetRelativeValue(in Vector3 startValue, in Vector3 relativeValue)
    {
        return startValue + relativeValue;
    }
}

[Serializable]
[LitMotionAnimationComponentMenu("UI/Rect Transform/Anchored Position")]
public sealed class TransformAnchoredPositionAnimation : TransformAnchoredPositionAnimationBase<NoOptions, Vector3MotionAdapter> { }
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace LitMotion.Editor
{
    [CustomPropertyDrawer(typeof(SerializableMotionSettings<,>))]
    public class SerializableMotionSettingsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var foldout = new Foldout()
            {
                text = property.displayName,
            };

            foldout.BindProperty(property);

            Group(foldout, group =>
            {
                AddPropertyField(group, property, "dynamicStartValue", "Dynamic Start Value", "如果启用，则StartValue为当前值；否则为用户设定的值", onValueChanged: e =>
                {
                    group.Q("startValue").enabledSelf = !property.FindPropertyRelative("dynamicStartValue").boolValue;
                });

                var valueType = fieldInfo.FieldType.GenericTypeArguments[0];
                if (valueType == typeof(FixedString32Bytes))
                {
                    group.Add(PropertyFieldHelper.CreateFixedStringField(property.FindPropertyRelative("startValue"), FixedString32Bytes.UTF8MaxLengthInBytes));
                    group.Add(PropertyFieldHelper.CreateFixedStringField(property.FindPropertyRelative("endValue"), FixedString32Bytes.UTF8MaxLengthInBytes));
                }
                else if (valueType == typeof(FixedString64Bytes))
                {
                    group.Add(PropertyFieldHelper.CreateFixedStringField(property.FindPropertyRelative("startValue"), FixedString64Bytes.UTF8MaxLengthInBytes));
                    group.Add(PropertyFieldHelper.CreateFixedStringField(property.FindPropertyRelative("endValue"), FixedString64Bytes.UTF8MaxLengthInBytes));
                }
                else if (valueType == typeof(FixedString128Bytes))
                {
                    group.Add(PropertyFieldHelper.CreateFixedStringField(property.FindPropertyRelative("startValue"), FixedString128Bytes.UTF8MaxLengthInBytes));
                    group.Add(PropertyFieldHelper.CreateFixedStringField(property.FindPropertyRelative("endValue"), FixedString128Bytes.UTF8MaxLengthInBytes));
                }
                else if (valueType == typeof(FixedString512Bytes))
                {
                    group.Add(PropertyFieldHelper.CreateFixedStringField(property.FindPropertyRelative("startValue"), FixedString512Bytes.UTF8MaxLengthInBytes));
                    group.Add(PropertyFieldHelper.CreateFixedStringField(property.FindPropertyRelative("endValue"), FixedString512Bytes.UTF8MaxLengthInBytes));
                }
                else if (valueType == typeof(FixedString4096Bytes))
                {
                    group.Add(PropertyFieldHelper.CreateFixedStringField(property.FindPropertyRelative("startValue"), FixedString4096Bytes.UTF8MaxLengthInBytes));
                    group.Add(PropertyFieldHelper.CreateFixedStringField(property.FindPropertyRelative("endValue"), FixedString4096Bytes.UTF8MaxLengthInBytes));
                }
                else
                {
                    AddPropertyField(group, property, "startValue");
                    AddPropertyField(group, property, "endValue");
                }
                group.Q("startValue").enabledSelf = !property.FindPropertyRelative("dynamicStartValue").boolValue;

                AddPropertyField(group, property, "duration", "Duration (s)");
            });

            Group(foldout, group =>
            {
                var ease = property.FindPropertyRelative("ease");
                group.Add(new PropertyField(ease));

                var customEaseCurve = new PropertyField(property.FindPropertyRelative("customEaseCurve"));
                customEaseCurve.style.display = ease.enumValueIndex == (int)Ease.CustomAnimationCurve ? DisplayStyle.Flex : DisplayStyle.None;

                group.Add(customEaseCurve);
                customEaseCurve.schedule
                    .Execute(() =>
                    {
                        try
                        {
                            customEaseCurve.style.display = ease.enumValueIndex == (int)Ease.CustomAnimationCurve ? DisplayStyle.Flex : DisplayStyle.None;
                        }
                        catch (InvalidOperationException)
                        {
                            // do nothing
                        }
                    })
                    .Every(10);
            });

            Group(foldout, group =>
            {
                AddPropertyField(group, property, "delay", "Delay (s)");
                AddPropertyField(group, property, "delayType");
            });

            Group(foldout, group =>
            {
                AddPropertyField(group, property, "loops", onValueChanged: OnLoopsPropertyValueChanged);
                AddPropertyField(group, property, "loopType");
            });

            var options = property.FindPropertyRelative("options");
            if (options.hasChildren)
            {
                Group(foldout, group =>
                {
                    group.style.paddingLeft = 15f;
                    group.Add(new PropertyField(options));
                });
            }

            Group(foldout, group =>
            {
                FoldoutGroup(group, "Additional Settings", property.FindPropertyRelative("additionalSettings"), group =>
                {
                    group.style.marginLeft = 15f;
                    AddPropertyField(group, property, "cancelOnError");
                    AddPropertyField(group, property, "skipValuesDuringDelay");
                    AddPropertyField(group, property, "immediateBind");
                    group.Add(new PropertyField(property.FindPropertyRelative("schedulerType"), "Scheduler"));
                });
            });

            return foldout;
        }

        void Group(VisualElement root, Action<VisualElement> configure)
        {
            var group = new UnityEngine.UIElements.Box() { style = { marginBottom = 3f } };
            configure(group);
            root.Add(group);
        }

        void FoldoutGroup(VisualElement root, string label, SerializedProperty property, Action<VisualElement> configure)
        {
            var group = new Foldout
            {
                text = label,
                style = { marginBottom = 3f },
            };
            group.BindProperty(property);
            configure(group);
            root.Add(group);
        }

        //修改了源码，支持了更多可选参数
        void AddPropertyField(VisualElement root, SerializedProperty property, string name, string label = null, string tooltip = null, EventCallback<SerializedPropertyChangeEvent> onValueChanged = null)
        {
            var propertyField = new PropertyField(property.FindPropertyRelative(name), label);
            propertyField.name = name;
            if (!string.IsNullOrEmpty(tooltip))
            {
                propertyField.tooltip = tooltip;
            }
            propertyField.RegisterValueChangeCallback(onValueChanged);
            root.Add(propertyField);
        }

        void OnLoopsPropertyValueChanged(SerializedPropertyChangeEvent evt)
        {
            if (evt.changedProperty.intValue < -1)
            {
                evt.changedProperty.intValue = -1;
                evt.changedProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        IEnumerable<SerializedProperty> GetChildren(SerializedProperty property)
        {
            var currentProperty = property.Copy();
            var nextProperty = property.Copy();
            nextProperty.Next(false);

            if (currentProperty.Next(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(currentProperty, nextProperty)) break;
                    yield return currentProperty;
                }
                while (currentProperty.Next(false));
            }
        }
    }
}

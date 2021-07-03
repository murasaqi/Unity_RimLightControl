using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(RimLightControlBehaviour))]
public class RimLightControlDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var overwritingProp = property.FindPropertyRelative("overwriting");
        var singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(singleFieldRect, overwritingProp);

        if (!overwritingProp.boolValue) return;

        var rimLightProp = property.FindPropertyRelative("overwriteParameter.rimLight");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, rimLightProp);

        var colorProp = property.FindPropertyRelative("overwriteParameter.color");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, colorProp);

        var isNormalMapToRimLightProp = property.FindPropertyRelative("overwriteParameter.isNormalMapToRimLight");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, isNormalMapToRimLightProp);

        var rimLightPowerProp = property.FindPropertyRelative("overwriteParameter.rimLightPower");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, rimLightPowerProp);

        var distanceDecayCurveProp = property.FindPropertyRelative("overwriteParameter.distanceDecayCurve");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, distanceDecayCurveProp);

        var rimLightInsideMaskProp = property.FindPropertyRelative("overwriteParameter.rimLightInsideMask");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, rimLightInsideMaskProp);

        var rimLightFeatherOffProp = property.FindPropertyRelative("overwriteParameter.rimLightFeatherOff");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, rimLightFeatherOffProp);

        var lightDirectionMaskOnProp = property.FindPropertyRelative("overwriteParameter.lightDirectionMaskOn");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, lightDirectionMaskOnProp);

        var tweakLightDirectionMaskLevelProp =
            property.FindPropertyRelative("overwriteParameter.tweakLightDirectionMaskLevel");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, tweakLightDirectionMaskLevelProp);

        var addAntipodeanRimLightProp = property.FindPropertyRelative("overwriteParameter.addAntipodeanRimLight");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, addAntipodeanRimLightProp);

        var apRimLightColorProp = property.FindPropertyRelative("overwriteParameter.apRimLightColor");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, apRimLightColorProp);

        var apRimLightPowerProp = property.FindPropertyRelative("overwriteParameter.apRimLightPower");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, apRimLightPowerProp);

        var apRimLightFeatherOffProp = property.FindPropertyRelative("overwriteParameter.apRimLightFeatherOff");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, apRimLightFeatherOffProp);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (property.FindPropertyRelative("overwriting").boolValue ? 14 : 1) * EditorGUIUtility.singleLineHeight;
    }
}
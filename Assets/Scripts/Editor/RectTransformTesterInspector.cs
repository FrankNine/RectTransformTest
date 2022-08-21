using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RectTransformTester))]
public class RectTransformTesterInspector : Editor
{
    private const float ArrowSize = 0.2f;

    public override void OnInspectorGUI()
    {
        var rectTransform = (target as RectTransformTester).RectTransform;
        
        rectTransform.localPosition = EditorGUILayout.Vector3Field("LocalPosition", rectTransform.localPosition);
        rectTransform.position = EditorGUILayout.Vector3Field("WorldPosition", rectTransform.position);
        EditorGUILayout.Space();

        rectTransform.pivot = EditorGUILayout.Vector2Field("Pivot", rectTransform.pivot);
        rectTransform.anchorMin = EditorGUILayout.Vector2Field("Anchor Min", rectTransform.anchorMin);
        rectTransform.anchorMax = EditorGUILayout.Vector2Field("Anchor Max", rectTransform.anchorMax);
        EditorGUILayout.Space();

        rectTransform.anchoredPosition = EditorGUILayout.Vector2Field("AnchoredPosition", rectTransform.anchoredPosition);
        rectTransform.sizeDelta = EditorGUILayout.Vector2Field("SizeDelta", rectTransform.sizeDelta);
        rectTransform.offsetMin = EditorGUILayout.Vector2Field("OffsetMin", rectTransform.offsetMin);
        rectTransform.offsetMax = EditorGUILayout.Vector2Field("OffsetMax", rectTransform.offsetMax);
        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.RectField("Rect", rectTransform.rect);
        EditorGUI.EndDisabledGroup();

        Vector3[] corners = new Vector3[4];

        rectTransform.GetLocalCorners(corners);
        EditorGUILayout.LabelField("LocalCorners");
        EditorGUI.indentLevel = 1;
        EditorGUILayout.LabelField("[0]", corners[0].ToString());
        EditorGUILayout.LabelField("[1]", corners[1].ToString());
        EditorGUILayout.LabelField("[2]", corners[2].ToString());
        EditorGUILayout.LabelField("[3]", corners[3].ToString());
        EditorGUI.indentLevel = 0;

        rectTransform.GetWorldCorners(corners);
        EditorGUILayout.LabelField("WorldCorners");
        EditorGUI.indentLevel = 1;
        EditorGUILayout.LabelField("[0]", corners[0].ToString());
        EditorGUILayout.LabelField("[1]", corners[1].ToString());
        EditorGUILayout.LabelField("[2]", corners[2].ToString());
        EditorGUILayout.LabelField("[3]", corners[3].ToString());
        EditorGUI.indentLevel = 0;
    }

    private void OnSceneGUI()
    {
        Handles.DrawLine(Vector3.zero, Vector3.one * 100);

        var rectTransformTester = target as RectTransformTester;
        var rectTranform = rectTransformTester.RectTransform;
        var parentTransform = rectTranform.parent;
        Vector3 worldPosition = rectTranform.position;

        _DrawAnnotatedArrow(worldPosition, ArrowSize, ArrowDirection.UpLeft, Color.yellow, "World Position");

        Rect rect = rectTranform.rect;

        Vector3 parentlocalPosition = rectTranform.localPosition;
        Vector3 parentlocalRectVector = new Vector3(rect.x, rect.y, 0);
        _DrawLocalAxisAlignedLines(parentTransform, parentlocalPosition, parentlocalRectVector, Color.magenta);

        Vector3 parentlocalRectOrigin = parentlocalPosition + parentlocalRectVector;
        _DrawAnnotatedArrowLocal(parentTransform, parentlocalRectOrigin, ArrowSize, 
                                 ArrowDirection.UpRight, Color.magenta, "Rect Origin");

        Vector3 parentlocalRectXminYmax = parentlocalPosition + new Vector3(rect.xMin, rect.yMax, 0);
        _DrawAnnotatedArrowLocal(parentTransform, parentlocalRectXminYmax, ArrowSize,
                                 ArrowDirection.DownRight, Color.magenta, "Rect Xmin Ymax");

        Vector3 parentlocalRectXmaxYmin = parentlocalPosition + new Vector3(rect.xMax, rect.yMin, 0);
        _DrawAnnotatedArrowLocal(parentTransform, parentlocalRectXmaxYmin, ArrowSize,
                                 ArrowDirection.UpLeft, Color.magenta, "Rect Xmax Ymin");

        Vector3 parentlocalRectXmaxYmax = parentlocalPosition + new Vector3(rect.xMax, rect.yMax, 0);
        _DrawAnnotatedArrowLocal(parentTransform, parentlocalRectXmaxYmax, ArrowSize,
                                 ArrowDirection.DownLeft, Color.magenta, "Rect Xmax Ymax");

        Vector3 parentlocalPivot = parentlocalPosition + new Vector3(
                                 Mathf.LerpUnclamped(rect.xMin, rect.xMax, rectTranform.pivot.x),
                                 Mathf.LerpUnclamped(rect.yMin, rect.yMax, rectTranform.pivot.y),0);
        _DrawAnnotatedArrowLocal(parentTransform, parentlocalPivot, ArrowSize,
                                 ArrowDirection.DownLeft, Color.red, "Pivot");


        Vector3[] worldCorners = new Vector3[4];
        rectTranform.GetWorldCorners(worldCorners);
        _DrawAnnotatedArrow(worldCorners[0], ArrowSize, ArrowDirection.UpRight, Color.green, "WorldCorner[0]");
        _DrawAnnotatedArrow(worldCorners[1], ArrowSize, ArrowDirection.DownRight, Color.green, "WorldCorner[1]");
        _DrawAnnotatedArrow(worldCorners[2], ArrowSize, ArrowDirection.DownLeft, Color.green, "WorldCorner[2]");
        _DrawAnnotatedArrow(worldCorners[3], ArrowSize, ArrowDirection.UpLeft, Color.green, "WorldCorner[3]");

        if (parentTransform && parentTransform is RectTransform)
        {
            var parentRectTransform = parentTransform as RectTransform;
            Vector3[] parentWorldCorners = new Vector3[4];
            parentRectTransform.GetWorldCorners(parentWorldCorners);

            _DrawAnnotatedArrow(parentWorldCorners[0], ArrowSize, ArrowDirection.UpRight, Color.magenta, "parentWorldCorners[0]");
            _DrawAnnotatedArrow(parentWorldCorners[1], ArrowSize, ArrowDirection.DownRight, Color.magenta, "parentWorldCorners[1]");
            _DrawAnnotatedArrow(parentWorldCorners[2], ArrowSize, ArrowDirection.DownLeft, Color.magenta, "parentWorldCorners[2]");
            _DrawAnnotatedArrow(parentWorldCorners[3], ArrowSize, ArrowDirection.UpLeft, Color.magenta, "parentWorldCorners[3]");

            Vector3 parentAnchorMin = new Vector3
            (
                Mathf.LerpUnclamped(parentRectTransform.rect.xMin, parentRectTransform.rect.xMax, rectTranform.anchorMin.x),
                Mathf.LerpUnclamped(parentRectTransform.rect.yMin, parentRectTransform.rect.yMax, rectTranform.anchorMin.y),
                0
            );

            Vector3 parentAnchorMax = new Vector3
            (
                Mathf.LerpUnclamped(parentRectTransform.rect.xMin, parentRectTransform.rect.xMax, rectTranform.anchorMax.x),
                Mathf.LerpUnclamped(parentRectTransform.rect.yMin, parentRectTransform.rect.yMax, rectTranform.anchorMax.y),
                0
            );

            _DrawAnnotatedArrowLocal(parentTransform, parentAnchorMin, ArrowSize, ArrowDirection.DownLeft, Color.magenta, "AnchorMin");
            _DrawAnnotatedArrowLocal(parentTransform, parentAnchorMax, ArrowSize, ArrowDirection.UpLeft, Color.magenta, "AnchorMax");

            Vector3 parentAnchorReference =new Vector3
            (
                Mathf.LerpUnclamped(parentAnchorMin.x, parentAnchorMax.x, rectTranform.pivot.x),
                Mathf.LerpUnclamped(parentAnchorMin.y, parentAnchorMax.y, rectTranform.pivot.y),
                0
            );
            _DrawAnnotatedArrowLocal(parentTransform, parentAnchorReference, ArrowSize, 
                                     ArrowDirection.DownLeft, Color.magenta, "Anchor Reference");

            Vector3 parentAnchorReferencePlusAnchoredPosition =
                parentAnchorReference + new Vector3(rectTranform.anchoredPosition.x, rectTranform.anchoredPosition.y);
            Vector3 worldAnchorReferencePlusAnchoredPosition =
                parentTransform.TransformPoint(parentAnchorReferencePlusAnchoredPosition);
            _DrawAnnotatedArrow(worldAnchorReferencePlusAnchoredPosition, ArrowSize, ArrowDirection.DownRight, Color.magenta, "Anchor Reference + Anchored Position");

            Vector3 parentAnchorMinPlusOffsetMin = parentAnchorMin + _GetVector3(rectTranform.offsetMin);
            _DrawAnnotatedArrowLocal(parentTransform, parentAnchorMinPlusOffsetMin, ArrowSize, ArrowDirection.DownRight, Color.magenta, "Anchor Min + Offset Min");

            Vector3 parentAnchorMaxPlusOffsetMax = parentAnchorMax + _GetVector3(rectTranform.offsetMax);
            _DrawAnnotatedArrowLocal(parentTransform, parentAnchorMaxPlusOffsetMax, ArrowSize, ArrowDirection.DownRight, Color.magenta, "Anchor Max + Offset Max");

        }
    }

    private Vector3 _GetWorldPosition(Transform transform, Vector3 localPosition)
    {
        return transform ? transform.TransformPoint(localPosition) : localPosition;
    }

    private Vector3 _GetVector3(Vector2 vec2)
    {
        return new Vector3(vec2.x, vec2.y, 0);
    }

    private enum ArrowDirection
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    private void _DrawArrow
    (
        Vector3 tipPosition,
        float scaleFactor,
        ArrowDirection direction
    )
    {
        bool isPointingTop = direction == ArrowDirection.UpLeft ||
                             direction == ArrowDirection.UpRight;
        bool isPointingLeft = direction == ArrowDirection.UpLeft ||
                              direction == ArrowDirection.DownLeft;

        Vector3[] trianglePoints =
        {
            tipPosition + scaleFactor * new Vector3(0, 0, 0),
            tipPosition + scaleFactor * new Vector3(0.5f * (isPointingLeft?1:-1), 0.86f* (isPointingTop?-1:1), 0),
            tipPosition + scaleFactor * new Vector3(0.86f* (isPointingLeft?1:-1), 0.5f * (isPointingTop?-1:1), 0)
        };

        Handles.DrawAAConvexPolygon(trianglePoints);
    }

    private void _DrawAnnotatedArrowLocal
    (
        Transform transform,
        Vector3 localPosition,
        float arrowSize,
        ArrowDirection direction,
        Color color,
        string text
    )
    {
        Vector3 worldPosition = _GetWorldPosition(transform, localPosition);
        _DrawAnnotatedArrow(worldPosition, arrowSize, direction, color, text);
    }

    private void _DrawAnnotatedArrow
    (
        Vector3 worldPosition,
        float arrowSize,
        ArrowDirection direction,
        Color color,
        string text
    )
    {
        float adjustedArrowSize = arrowSize * HandleUtility.GetHandleSize(worldPosition);

        Handles.color = color;
        _DrawArrow(worldPosition, adjustedArrowSize, direction);

        GUIStyle textStyle = new GUIStyle
        {
            alignment = _GetTextAnchor(direction),
            normal = { textColor = color }
        };
        Vector3 lablePosition = worldPosition + _GetLabelOffset(direction, adjustedArrowSize);
        Handles.Label(lablePosition, text, textStyle);
    }

    // bug: Handles.Label does not react to TextAnchor.xxLeft nor TextAnchor.xxRight
    private TextAnchor _GetTextAnchor(ArrowDirection arrowDirection)
    {
        switch(arrowDirection)
        {
            case ArrowDirection.UpLeft:
            case ArrowDirection.UpRight:
            case ArrowDirection.DownLeft: 
            case ArrowDirection.DownRight: return TextAnchor.UpperCenter;
        }

        return TextAnchor.UpperLeft;
    }

    private Vector3 _GetLabelOffset(ArrowDirection arrowDirection, float arrowSize)
    {
        switch (arrowDirection)
        {
            case ArrowDirection.UpLeft: 
            case ArrowDirection.UpRight: return new Vector3(0, -arrowSize, 0);
            case ArrowDirection.DownLeft: 
            case ArrowDirection.DownRight: return new Vector3(0, 2*arrowSize, 0);
        }

        return Vector3.zero;
    }

    private void _DrawLocalAxisAlignedLines
    (
        Transform transform,
        Vector3 localPosition, 
        Vector3 localVector, 
        Color color,
        bool isDotted = true
    )
    {
        Handles.color = color;

        Vector3 worldPosition = _GetWorldPosition(transform, localPosition);
        Vector3 moveYWorldPosition = _GetWorldPosition(transform, localPosition + new Vector3(0, localVector.y, 0));
        Vector3 destinationWorldPosition = _GetWorldPosition(transform, localPosition + localVector);

        if (isDotted)
        {
            Handles.DrawDottedLine(worldPosition, moveYWorldPosition, 3);
            Handles.DrawDottedLine(moveYWorldPosition, destinationWorldPosition, 3);
        }
        else
        {
            Handles.DrawLine(worldPosition, moveYWorldPosition);
            Handles.DrawLine(moveYWorldPosition, destinationWorldPosition);
        }
    }
}
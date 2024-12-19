using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TextToTextMeshPro : Editor
{
    public class TextMeshProSettings
    {
        public bool Enabled;
        public FontStyles FontStyle;
        public float FontSize;
        public float FontSizeMin;
        public float FontSizeMax;
        public float LineSpacing;
        public bool EnableRichText;
        public bool EnableAutoSizing;
        public TextAlignmentOptions TextAlignmentOptions;
        public bool WrappingEnabled;
        public TextOverflowModes TextOverflowModes;
        public string Text;
        public Color Color;
        public bool RayCastTarget;
    }

    static private bool recursive = false;
    static private bool applyOnDisabled = false;
    static private bool removeEffectComponents = false;

    [MenuItem("Tools/TextMesh To TextMeshPro", false, 4000)]
    static void DoIt()
    {
        if (TMPro.TMP_Settings.defaultFontAsset == null)
        {
            EditorUtility.DisplayDialog("ERROR!", "Assign a default font asset in project settings!", "OK", "");
            return;
        }

        /* recursive = EditorUtility.DisplayDialog("TextMesh To TextMeshPro", "Process all the children of the selected objects recursively?", "Yes", "No"); */
        recursive = true;
        applyOnDisabled = true;
        removeEffectComponents = true;
        /* if (recursive) applyOnDisabled = EditorUtility.DisplayDialog("TextMesh To TextMeshPro", "Process disabled objects too?", "Yes", "No"); */

        /* removeEffectComponents = EditorUtility.DisplayDialog("TextMesh To TextMeshPro", "Remove obsolete component effects Shadow and Outline?\n\nThis components have no effect on TextMeshPro and can be safely removed.\nTo apply shadows and outlines on TextMeshPro components, use different materials instead.", "Yes", "No"); */

        foreach (GameObject gameObject in Selection.gameObjects)
        {
            if (recursive)
            {
                var childs = gameObject.GetComponentsInChildren<Transform>(applyOnDisabled);
                foreach (var ch in childs)
                    ConvertTextToTextMeshPro(ch.gameObject);
            }
            else
                ConvertTextToTextMeshPro(gameObject);
        }
    }

    static void ConvertTextToTextMeshPro(GameObject target)
    {
        if (removeEffectComponents)
        {
            var sha = target.GetComponents<UnityEngine.UI.Shadow>();
            foreach (var cmp in sha) Undo.DestroyObjectImmediate(cmp);
            var outl = target.GetComponents<UnityEngine.UI.Outline>();
            foreach (var cmp in outl) Undo.DestroyObjectImmediate(cmp);
        }

        TextMesh uiTextMesh = target.GetComponent<TextMesh>();
        if (uiTextMesh == null) return;


        TextMeshProSettings settings = GetTextMeshProSettings(uiTextMesh);

        Undo.RecordObject(target, "TextMesh To TextMeshPro");
        Undo.DestroyObjectImmediate(uiTextMesh);

        TextMeshPro tmp = target.AddComponent<TextMeshPro>();
        tmp.enabled = settings.Enabled;
        tmp.fontStyle = settings.FontStyle;
        tmp.fontSize = settings.FontSize;
        tmp.fontSizeMin = settings.FontSizeMin;
        tmp.fontSizeMax = settings.FontSizeMax;
        tmp.lineSpacing = settings.LineSpacing;
        tmp.richText = settings.EnableRichText;
        tmp.enableAutoSizing = settings.EnableAutoSizing;
        tmp.alignment = settings.TextAlignmentOptions;
        tmp.enableWordWrapping = settings.WrappingEnabled;
        tmp.overflowMode = settings.TextOverflowModes;
        tmp.text = settings.Text;
        tmp.color = settings.Color;
        tmp.raycastTarget = settings.RayCastTarget;

        Debug.Log(target.name + " converted to TextMeshProUGUI");
    }

    static TextMeshProSettings GetTextMeshProSettings(TextMesh uiTextMesh)
    {
        if (uiTextMesh == null)
        {
            EditorUtility.DisplayDialog("ERROR!", "You must select a Unity UI TextMesh Object to convert.", "OK", "");
            return null;
        }

        return new TextMeshProSettings
        {
            Enabled = true,
            FontSize = 500,
            LineSpacing = 1,
            TextAlignmentOptions = TextAlignmentOptions.Left,
            WrappingEnabled = false,
            Text = uiTextMesh.text,
            Color = uiTextMesh.color,
        };
    }

    static bool HorizontalWrapModeToBool(HorizontalWrapMode overflow)
    {
        return overflow == HorizontalWrapMode.Wrap;
    }

    static TextOverflowModes VerticalWrapModeToTextOverflowModes(VerticalWrapMode verticalOverflow)
    {
        return verticalOverflow == VerticalWrapMode.Truncate ? TextOverflowModes.Truncate : TextOverflowModes.Overflow;
    }

    static FontStyles FontStyleToFontStyles(FontStyle fontStyle)
    {
        switch (fontStyle)
        {
            case FontStyle.Normal:
                return FontStyles.Normal;

            case FontStyle.Bold:
                return FontStyles.Bold;

            case FontStyle.Italic:
                return FontStyles.Italic;

            case FontStyle.BoldAndItalic:
                return FontStyles.Bold | FontStyles.Italic;
        }

        Debug.LogWarning("Unhandled font style " + fontStyle);
        return FontStyles.Normal;
    }

    static TextAlignmentOptions TextAnchorToTextAlignmentOptions(TextAnchor textAnchor)
    {
        switch (textAnchor)
        {
            case TextAnchor.UpperLeft:
                return TextAlignmentOptions.TopLeft;

            case TextAnchor.UpperCenter:
                return TextAlignmentOptions.Top;

            case TextAnchor.UpperRight:
                return TextAlignmentOptions.TopRight;

            case TextAnchor.MiddleLeft:
                return TextAlignmentOptions.Left;

            case TextAnchor.MiddleCenter:
                return TextAlignmentOptions.Center;

            case TextAnchor.MiddleRight:
                return TextAlignmentOptions.Right;

            case TextAnchor.LowerLeft:
                return TextAlignmentOptions.BottomLeft;

            case TextAnchor.LowerCenter:
                return TextAlignmentOptions.Bottom;

            case TextAnchor.LowerRight:
                return TextAlignmentOptions.BottomRight;
        }

        Debug.LogWarning("Unhandled text anchor " + textAnchor);
        return TextAlignmentOptions.TopLeft;
    }
}

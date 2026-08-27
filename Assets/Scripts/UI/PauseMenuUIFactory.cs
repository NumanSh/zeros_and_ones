using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZerosAndOnes.UI
{
    /// <summary>
    /// Builds the pause menu's uGUI widgets in code so no prefab or per-scene Editor setup is needed.
    /// Follows the same widget mix as MainMenuMap: UnityEngine.UI controls with TextMeshPro labels.
    /// </summary>
    public static class PauseMenuUIFactory
    {
        // Palette approximating the menu's wooden/stone GUI art.
        public static readonly Color PanelColor = new Color(0.16f, 0.11f, 0.07f, 0.96f);
        public static readonly Color PanelBorderColor = new Color(0.42f, 0.30f, 0.17f, 1f);
        public static readonly Color ButtonColor = new Color(0.45f, 0.31f, 0.17f, 1f);
        public static readonly Color ButtonHighlightColor = new Color(0.60f, 0.43f, 0.24f, 1f);
        public static readonly Color ButtonPressedColor = new Color(0.33f, 0.22f, 0.12f, 1f);
        public static readonly Color TextColor = new Color(0.97f, 0.92f, 0.80f, 1f);
        public static readonly Color DimColor = new Color(0f, 0f, 0f, 0.65f);

        // Sprites are generated procedurally rather than pulled from Unity's UI skin:
        // Resources.GetBuiltinResource only reaches "unity default resources", while the UI/Skin
        // sprites live in unity_builtin_extra, which is only reachable from Editor code. Generating
        // them keeps the menu working identically in the Editor and in a build.

        /// <summary>
        /// Board art for the panel background, loaded from Assets/Resources/UI/PausePanel.png.
        /// Resources is the only way code-built UI can reach a project sprite, since there is no
        /// prefab to assign one in. Swap that file to restyle the panel; if it is missing the
        /// generated rounded box is used instead, so the menu never breaks.
        /// </summary>
        private const string PanelArtResourcePath = "UI/PausePanel";
        private const string ButtonArtResourcePath = "UI/PauseButton";

        // Parchment art is light, so its label has to be dark rather than the cream used on stone.
        public static readonly Color ButtonArtTextColor = new Color(0.24f, 0.15f, 0.07f, 1f);
        public static readonly Color ButtonArtHighlightColor = new Color(1f, 0.95f, 0.82f, 1f);
        public static readonly Color ButtonArtPressedColor = new Color(0.78f, 0.70f, 0.58f, 1f);

        /// <summary>
        /// Shrinks the 9-slice border at draw time. The parchment's ragged edge is 28px in the
        /// source, which would swallow a 62px tall button, so it is halved here.
        /// </summary>
        private const float ButtonArtPixelsPerUnitMultiplier = 2f;

        private static Sprite _roundedSprite;
        private static Sprite _knobSprite;
        private static Sprite _checkmarkSprite;
        private static Sprite _dropdownArrowSprite;
        private static Sprite _plainSprite;
        private static Sprite _panelArtSprite;
        private static bool _panelArtLoaded;
        private static Sprite _buttonArtSprite;
        private static bool _buttonArtLoaded;

        private static Sprite PanelArtSprite
        {
            get
            {
                // Cached because Resources.Load hits disk, and a miss should only be paid once.
                if (!_panelArtLoaded)
                {
                    _panelArtSprite = Resources.Load<Sprite>(PanelArtResourcePath);
                    _panelArtLoaded = true;
                }

                return _panelArtSprite;
            }
        }

        private static Sprite ButtonArtSprite
        {
            get
            {
                if (!_buttonArtLoaded)
                {
                    _buttonArtSprite = Resources.Load<Sprite>(ButtonArtResourcePath);
                    _buttonArtLoaded = true;
                }

                return _buttonArtSprite;
            }
        }

        /// <summary>Rounded rectangle with a 9-slice border, used for panels, buttons and tracks.</summary>
        private static Sprite RoundedSprite
        {
            get
            {
                // Unity's overloaded == is what catches a destroyed sprite after a domain reload.
                if (_roundedSprite == null) _roundedSprite = BuildRoundedRectSprite("PauseMenu_Rounded", 48, 12);
                return _roundedSprite;
            }
        }

        private static Sprite BackgroundSprite => RoundedSprite;

        private static Sprite KnobSprite
        {
            get
            {
                if (_knobSprite == null) _knobSprite = BuildCircleSprite("PauseMenu_Knob", 32);
                return _knobSprite;
            }
        }

        private static Sprite CheckmarkSprite
        {
            get
            {
                if (_checkmarkSprite == null) _checkmarkSprite = BuildCheckmarkSprite("PauseMenu_Checkmark", 32);
                return _checkmarkSprite;
            }
        }

        private static Sprite DropdownArrowSprite
        {
            get
            {
                if (_dropdownArrowSprite == null) _dropdownArrowSprite = BuildDownArrowSprite("PauseMenu_Arrow", 32);
                return _dropdownArrowSprite;
            }
        }

        /// <summary>Opaque square used as the dropdown viewport's stencil shape.</summary>
        private static Sprite MaskSprite
        {
            get
            {
                if (_plainSprite == null) _plainSprite = BuildSprite("PauseMenu_Plain", 4, Vector4.zero, (u, v) => true);
                return _plainSprite;
            }
        }

        public static Canvas CreateCanvas(string name, int sortingOrder)
        {
            var go = new GameObject(name, typeof(RectTransform));

            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            go.AddComponent<GraphicRaycaster>();

            return canvas;
        }

        /// <summary>Creates an image stretched to fill its parent. Used for the dim backdrop.</summary>
        public static Image CreateFullScreenImage(Transform parent, string name, Color color)
        {
            var image = CreateImage(parent, name, color, null);
            Stretch(image.rectTransform);
            return image;
        }

        public static Image CreateImage(Transform parent, string name, Color color, Sprite sprite)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);

            var image = go.AddComponent<Image>();
            image.color = color;
            if (sprite != null)
            {
                image.sprite = sprite;
                // Only 9-slice sprites that actually carry a border; the rest would render
                // identically as Simple but log a warning under Sliced.
                image.type = sprite.border == Vector4.zero ? Image.Type.Simple : Image.Type.Sliced;
            }

            return image;
        }

        /// <summary>
        /// Creates the panel body: a single bordered box with a vertical layout for its children.
        /// Deliberately one GameObject, so callers can show/hide the whole panel with SetActive
        /// on the returned transform without leaving a stray border behind.
        /// </summary>
        public static RectTransform CreatePanel(Transform parent, string name, Vector2 size, float spacing, RectOffset padding)
        {
            // Real board art when it is available, generated rounded box otherwise. Art is tinted
            // white so the sprite shows its own colours instead of being multiplied by the palette.
            var art = PanelArtSprite;
            var panel = art != null
                ? CreateImage(parent, name, Color.white, art)
                : CreateImage(parent, name, PanelColor, RoundedSprite);

            var panelRect = panel.rectTransform;
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.anchoredPosition = Vector2.zero;
            panelRect.sizeDelta = size;

            // The board art draws its own frame, so the painted outline is only for the fallback.
            if (art == null)
            {
                var outline = panel.gameObject.AddComponent<Outline>();
                outline.effectColor = PanelBorderColor;
                outline.effectDistance = new Vector2(4f, 4f);
            }

            var layout = panel.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = padding;
            layout.spacing = spacing;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            // Must be true, otherwise the group ignores each child's LayoutElement.preferredHeight
            // and they all keep the RectTransform default of 100, overflowing the panel.
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            return panelRect;
        }

        public static TextMeshProUGUI CreateLabel(Transform parent, string name, string text, float fontSize,
            FontStyles style = FontStyles.Normal, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);

            // Font is left unset on purpose: TMP assigns the project default (LiberationSans SDF),
            // which is the only font asset here and the one every MainMenuMap label already uses.
            var label = go.AddComponent<TextMeshProUGUI>();
            label.text = text;
            label.fontSize = fontSize;
            label.fontStyle = style;
            label.alignment = alignment;
            label.color = TextColor;
            label.raycastTarget = false;

            SetPreferredHeight(go, fontSize * 1.6f);

            return label;
        }

        public static Button CreateButton(Transform parent, string name, string text, float height, UnityEngine.Events.UnityAction onClick)
        {
            // Tint comes from the ColorBlock, so the graphic itself stays white.
            var art = ButtonArtSprite;
            var image = CreateImage(parent, name, Color.white, art != null ? art : RoundedSprite);
            SetPreferredHeight(image.gameObject, height);

            if (art != null)
            {
                image.pixelsPerUnitMultiplier = ButtonArtPixelsPerUnitMultiplier;
            }

            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;

            var colors = button.colors;
            // With art the base tint must stay white or it muddies the parchment; the flat
            // fallback instead carries its colour entirely through these tints.
            colors.normalColor = art != null ? Color.white : ButtonColor;
            colors.highlightedColor = art != null ? ButtonArtHighlightColor : ButtonHighlightColor;
            colors.pressedColor = art != null ? ButtonArtPressedColor : ButtonPressedColor;
            colors.selectedColor = colors.normalColor;
            colors.disabledColor = new Color(0.30f, 0.26f, 0.22f, 0.6f);
            colors.fadeDuration = 0.05f;
            button.colors = colors;

            var label = CreateLabel(image.rectTransform, "Label", text, 30f);
            if (art != null)
            {
                label.color = ButtonArtTextColor;
            }

            Stretch(label.rectTransform);
            label.rectTransform.offsetMin = new Vector2(24f, 0f);
            label.rectTransform.offsetMax = new Vector2(-24f, 0f);

            if (onClick != null)
            {
                button.onClick.AddListener(onClick);
            }

            return button;
        }

        /// <summary>Creates a "Label ........ [slider]" row and returns the slider.</summary>
        public static Slider CreateSliderRow(Transform parent, string name, string labelText)
        {
            CreateRow(parent, name, 46f, out var labelArea, out var controlArea);

            var label = CreateLabel(labelArea, "Label", labelText, 26f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            Stretch(label.rectTransform);

            var sliderGo = new GameObject("Slider", typeof(RectTransform));
            sliderGo.transform.SetParent(controlArea, false);
            Stretch((RectTransform)sliderGo.transform);

            var background = CreateImage(sliderGo.transform, "Background", new Color(0.10f, 0.07f, 0.04f, 1f), BackgroundSprite);
            var backgroundRect = background.rectTransform;
            backgroundRect.anchorMin = new Vector2(0f, 0.3f);
            backgroundRect.anchorMax = new Vector2(1f, 0.7f);
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;

            var fillArea = new GameObject("Fill Area", typeof(RectTransform));
            fillArea.transform.SetParent(sliderGo.transform, false);
            var fillAreaRect = (RectTransform)fillArea.transform;
            fillAreaRect.anchorMin = new Vector2(0f, 0.3f);
            fillAreaRect.anchorMax = new Vector2(1f, 0.7f);
            fillAreaRect.offsetMin = new Vector2(8f, 0f);
            fillAreaRect.offsetMax = new Vector2(-8f, 0f);

            var fill = CreateImage(fillAreaRect, "Fill", ButtonHighlightColor, RoundedSprite);
            fill.rectTransform.sizeDelta = new Vector2(10f, 0f);

            var handleArea = new GameObject("Handle Slide Area", typeof(RectTransform));
            handleArea.transform.SetParent(sliderGo.transform, false);
            var handleAreaRect = (RectTransform)handleArea.transform;
            Stretch(handleAreaRect);
            handleAreaRect.offsetMin = new Vector2(8f, 0f);
            handleAreaRect.offsetMax = new Vector2(-8f, 0f);

            var handle = CreateImage(handleAreaRect, "Handle", TextColor, KnobSprite);
            handle.rectTransform.sizeDelta = new Vector2(20f, 0f);

            var slider = sliderGo.AddComponent<Slider>();
            slider.fillRect = fill.rectTransform;
            slider.handleRect = handle.rectTransform;
            slider.targetGraphic = handle;
            slider.direction = Slider.Direction.LeftToRight;
            slider.minValue = 0f;
            slider.maxValue = 1f;

            return slider;
        }

        /// <summary>Creates a "Label ........ [toggle]" row and returns the toggle.</summary>
        public static Toggle CreateToggleRow(Transform parent, string name, string labelText)
        {
            CreateRow(parent, name, 46f, out var labelArea, out var controlArea);

            var label = CreateLabel(labelArea, "Label", labelText, 26f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            Stretch(label.rectTransform);

            var background = CreateImage(controlArea, "Background", new Color(0.10f, 0.07f, 0.04f, 1f), BackgroundSprite);
            var backgroundRect = background.rectTransform;
            backgroundRect.anchorMin = new Vector2(0f, 0.5f);
            backgroundRect.anchorMax = new Vector2(0f, 0.5f);
            backgroundRect.pivot = new Vector2(0f, 0.5f);
            backgroundRect.anchoredPosition = Vector2.zero;
            backgroundRect.sizeDelta = new Vector2(30f, 30f);

            var checkmark = CreateImage(backgroundRect, "Checkmark", ButtonHighlightColor, CheckmarkSprite);
            checkmark.type = Image.Type.Simple;
            var checkmarkRect = checkmark.rectTransform;
            Stretch(checkmarkRect);
            checkmarkRect.offsetMin = new Vector2(3f, 3f);
            checkmarkRect.offsetMax = new Vector2(-3f, -3f);

            var toggle = background.gameObject.AddComponent<Toggle>();
            toggle.targetGraphic = background;
            toggle.graphic = checkmark;

            return toggle;
        }

        /// <summary>Creates a "Label ........ [dropdown]" row and returns the dropdown.</summary>
        public static TMP_Dropdown CreateDropdownRow(Transform parent, string name, string labelText)
        {
            CreateRow(parent, name, 46f, out var labelArea, out var controlArea);

            var label = CreateLabel(labelArea, "Label", labelText, 26f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            Stretch(label.rectTransform);

            var dropdownImage = CreateImage(controlArea, "Dropdown", ButtonColor, RoundedSprite);
            var dropdownRect = dropdownImage.rectTransform;
            Stretch(dropdownRect);

            var captionText = CreateLabel(dropdownRect, "Label", string.Empty, 22f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            Stretch(captionText.rectTransform);
            captionText.rectTransform.offsetMin = new Vector2(12f, 0f);
            captionText.rectTransform.offsetMax = new Vector2(-30f, 0f);

            var arrow = CreateImage(dropdownRect, "Arrow", TextColor, DropdownArrowSprite);
            arrow.type = Image.Type.Simple;
            var arrowRect = arrow.rectTransform;
            arrowRect.anchorMin = new Vector2(1f, 0.5f);
            arrowRect.anchorMax = new Vector2(1f, 0.5f);
            arrowRect.pivot = new Vector2(1f, 0.5f);
            arrowRect.anchoredPosition = new Vector2(-10f, 0f);
            arrowRect.sizeDelta = new Vector2(20f, 20f);

            // Template: built once, disabled, and cloned by TMP_Dropdown when opened.
            var template = CreateImage(dropdownRect, "Template", PanelColor, RoundedSprite);
            var templateRect = template.rectTransform;
            templateRect.anchorMin = new Vector2(0f, 0f);
            templateRect.anchorMax = new Vector2(1f, 0f);
            templateRect.pivot = new Vector2(0.5f, 1f);
            templateRect.anchoredPosition = new Vector2(0f, 2f);
            templateRect.sizeDelta = new Vector2(0f, 180f);

            // The mask graphic must be opaque to define the stencil area; showMaskGraphic keeps it
            // from actually being drawn.
            var viewport = CreateImage(templateRect, "Viewport", Color.white, MaskSprite);
            var viewportRect = viewport.rectTransform;
            Stretch(viewportRect);
            viewportRect.pivot = new Vector2(0f, 1f);
            viewport.gameObject.AddComponent<Mask>().showMaskGraphic = false;

            var content = new GameObject("Content", typeof(RectTransform));
            content.transform.SetParent(viewportRect, false);
            var contentRect = (RectTransform)content.transform;
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(0f, 36f);

            var item = new GameObject("Item", typeof(RectTransform));
            item.transform.SetParent(contentRect, false);
            var itemRect = (RectTransform)item.transform;
            itemRect.anchorMin = new Vector2(0f, 0.5f);
            itemRect.anchorMax = new Vector2(1f, 0.5f);
            itemRect.pivot = new Vector2(0.5f, 0.5f);
            itemRect.sizeDelta = new Vector2(0f, 36f);

            var itemBackground = CreateImage(itemRect, "Item Background", ButtonColor, null);
            Stretch(itemBackground.rectTransform);

            var itemCheckmark = CreateImage(itemRect, "Item Checkmark", ButtonHighlightColor, CheckmarkSprite);
            itemCheckmark.type = Image.Type.Simple;
            var itemCheckmarkRect = itemCheckmark.rectTransform;
            itemCheckmarkRect.anchorMin = new Vector2(0f, 0.5f);
            itemCheckmarkRect.anchorMax = new Vector2(0f, 0.5f);
            itemCheckmarkRect.pivot = new Vector2(0f, 0.5f);
            itemCheckmarkRect.anchoredPosition = new Vector2(6f, 0f);
            itemCheckmarkRect.sizeDelta = new Vector2(20f, 20f);

            var itemLabel = CreateLabel(itemRect, "Item Label", string.Empty, 22f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
            Stretch(itemLabel.rectTransform);
            itemLabel.rectTransform.offsetMin = new Vector2(32f, 0f);
            itemLabel.rectTransform.offsetMax = new Vector2(-10f, 0f);

            var itemToggle = item.AddComponent<Toggle>();
            itemToggle.targetGraphic = itemBackground;
            itemToggle.graphic = itemCheckmark;

            var scrollRect = template.gameObject.AddComponent<ScrollRect>();
            scrollRect.content = contentRect;
            scrollRect.viewport = viewportRect;
            scrollRect.horizontal = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 20f;

            template.gameObject.SetActive(false);

            var dropdown = dropdownImage.gameObject.AddComponent<TMP_Dropdown>();
            dropdown.targetGraphic = dropdownImage;
            dropdown.template = templateRect;
            dropdown.captionText = captionText;
            dropdown.itemText = itemLabel;

            return dropdown;
        }

        /// <summary>A fixed-height row split into a left label area (55%) and a right control area (45%).</summary>
        private static RectTransform CreateRow(Transform parent, string name, float height, out RectTransform labelArea, out RectTransform controlArea)
        {
            var row = new GameObject(name, typeof(RectTransform));
            row.transform.SetParent(parent, false);
            var rowRect = (RectTransform)row.transform;
            SetPreferredHeight(row, height);

            labelArea = CreateArea(rowRect, "LabelArea", 0f, 0.55f);
            controlArea = CreateArea(rowRect, "ControlArea", 0.58f, 1f);

            return rowRect;
        }

        private static RectTransform CreateArea(Transform parent, string name, float anchorMinX, float anchorMaxX)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);

            var rect = (RectTransform)go.transform;
            rect.anchorMin = new Vector2(anchorMinX, 0f);
            rect.anchorMax = new Vector2(anchorMaxX, 1f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            return rect;
        }

        private static void SetPreferredHeight(GameObject go, float height)
        {
            var element = go.AddComponent<LayoutElement>();
            element.preferredHeight = height;
            element.minHeight = height;
        }

        public static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        // ---------------------------------------------------------------- procedural sprites

        private static Sprite BuildRoundedRectSprite(string name, int size, int cornerRadius)
        {
            float r = cornerRadius / (float)size;
            var border = new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius);

            return BuildSprite(name, size, border, (u, v) =>
            {
                float dx = Mathf.Max(Mathf.Abs(u - 0.5f) - (0.5f - r), 0f);
                float dy = Mathf.Max(Mathf.Abs(v - 0.5f) - (0.5f - r), 0f);
                return dx * dx + dy * dy <= r * r;
            });
        }

        private static Sprite BuildCircleSprite(string name, int size)
        {
            return BuildSprite(name, size, Vector4.zero, (u, v) =>
            {
                float dx = u - 0.5f;
                float dy = v - 0.5f;
                return dx * dx + dy * dy <= 0.25f;
            });
        }

        private static Sprite BuildCheckmarkSprite(string name, int size)
        {
            var a = new Vector2(0.18f, 0.52f);
            var b = new Vector2(0.42f, 0.26f);
            var c = new Vector2(0.84f, 0.74f);
            const float halfThickness = 0.075f;

            return BuildSprite(name, size, Vector4.zero, (u, v) =>
            {
                var p = new Vector2(u, v);
                return DistanceToSegment(p, a, b) <= halfThickness
                       || DistanceToSegment(p, b, c) <= halfThickness;
            });
        }

        private static Sprite BuildDownArrowSprite(string name, int size)
        {
            // v is bottom-origin, so the apex sits low and the flat edge sits high.
            const float apexV = 0.28f;
            const float topV = 0.70f;
            const float halfWidth = 0.34f;

            return BuildSprite(name, size, Vector4.zero, (u, v) =>
            {
                if (v < apexV || v > topV) return false;
                float t = (v - apexV) / (topV - apexV);
                return Mathf.Abs(u - 0.5f) <= halfWidth * t;
            });
        }

        /// <summary>
        /// Rasterizes a shape predicate (in normalized 0..1 space) into a white texture whose alpha
        /// carries the shape, 4x4 supersampled for smooth edges.
        /// </summary>
        private static Sprite BuildSprite(string name, int size, Vector4 border, System.Func<float, float, bool> isInside)
        {
            const int samples = 4;
            const int samplesSquared = samples * samples;

            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = name + "_Tex",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                // Survives scene loads, since the sprites are cached statically and reused.
                hideFlags = HideFlags.HideAndDontSave
            };

            var pixels = new Color32[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int hits = 0;
                    for (int sy = 0; sy < samples; sy++)
                    {
                        for (int sx = 0; sx < samples; sx++)
                        {
                            float u = (x + (sx + 0.5f) / samples) / size;
                            float v = (y + (sy + 0.5f) / samples) / size;
                            if (isInside(u, v)) hits++;
                        }
                    }

                    pixels[y * size + x] = new Color32(255, 255, 255, (byte)(255 * hits / samplesSquared));
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply();

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f),
                100f, 0, SpriteMeshType.FullRect, border);
            sprite.name = name;
            sprite.hideFlags = HideFlags.HideAndDontSave;

            return sprite;
        }

        private static float DistanceToSegment(Vector2 point, Vector2 start, Vector2 end)
        {
            Vector2 segment = end - start;
            float lengthSquared = segment.sqrMagnitude;
            if (lengthSquared < Mathf.Epsilon) return Vector2.Distance(point, start);

            float t = Mathf.Clamp01(Vector2.Dot(point - start, segment) / lengthSquared);
            return Vector2.Distance(point, start + t * segment);
        }
    }
}

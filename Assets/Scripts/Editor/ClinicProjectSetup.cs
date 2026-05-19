#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using VirtualEyeClinic.Core;
using VirtualEyeClinic.Interaction;
using VirtualEyeClinic.Interaction.EyeClinicObjects;
using VirtualEyeClinic.Player;
using VirtualEyeClinic.UI;

namespace VirtualEyeClinic.Editor
{
    public static class ClinicProjectSetup
    {
        private const string ClinicScenePath = "Assets/Scenes/ClinicScene.unity";
        private const string MenuScenePath = "Assets/Scenes/MainMenu.unity";

        [MenuItem("Virtual Eye Clinic/1. Create Scenes And Build Settings")]
        public static void CreateScenes()
        {
            EnsureLayer("Interactable", 8);
            CreateMainMenuScene();
            CreateClinicScene();
            ConfigureBuildSettings();
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog(
                "Virtual Eye Clinic",
                "Scenes created.\n\nNext:\n1. Assign audio clips in the Inspector\n2. Add a retina Sprite to RetinaDisplay\n3. Build APK (File > Build Settings > Android)",
                "OK");
        }

        [MenuItem("Virtual Eye Clinic/2. Add Interactable Layer To Selection")]
        public static void SetSelectionInteractableLayer()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                go.layer = LayerMask.NameToLayer("Interactable");
            }
        }

        private static void CreateMainMenuScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var light = new GameObject("Directional Light");
            var lightComp = light.AddComponent<Light>();
            lightComp.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            var camera = new GameObject("Main Camera");
            camera.tag = "MainCamera";
            camera.AddComponent<Camera>();
            camera.AddComponent<AudioListener>();

            var canvasGo = CreateCanvas("MenuCanvas");
            var canvas = canvasGo.GetComponent<Canvas>();

            if (Object.FindObjectOfType<EventSystem>() == null)
            {
                var es = new GameObject("EventSystem");
                es.AddComponent<EventSystem>();
                es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            }


            var title = CreateText(canvas.transform, "Title", "Virtual Eye Clinic", 48, new Vector2(0, 120));
            var startBtn = CreateButton(canvas.transform, "StartButton", "Enter Clinic", new Vector2(0, 0));
            var quitBtn = CreateButton(canvas.transform, "QuitButton", "Quit", new Vector2(0, -80));

            var menu = canvasGo.AddComponent<MainMenuController>();

            startBtn.onClick.AddListener(menu.OnStartButtonPressed);
            quitBtn.onClick.AddListener(menu.OnQuitButtonPressed);

            var managers = new GameObject("_Managers");
            managers.AddComponent<GameManager>();
            var audio = managers.AddComponent<AudioManager>();
            var sfx = managers.AddComponent<AudioSource>();
            var bgm = managers.AddComponent<AudioSource>();
            bgm.loop = true;
            bgm.volume = 0.35f;
            SetSerialized(audio, "sfxSource", sfx);
            SetSerialized(audio, "bgmSource", bgm);

            EditorSceneManager.SaveScene(scene, MenuScenePath);
        }

        private static void CreateClinicScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var light = new GameObject("Directional Light");
            var lightComp = light.AddComponent<Light>();
            lightComp.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(55f, -35f, 0f);

            BuildClinicRoom();
            var player = BuildPlayer();
            var ui = BuildGameplayUI();
            BuildManagers();

            EditorSceneManager.SaveScene(scene, ClinicScenePath);
        }

        private static void BuildClinicRoom()
        {
            var room = new GameObject("ClinicRoom");

            CreateFloor(room.transform, new Vector3(0, 0, 0), new Vector3(12, 1, 12));
            CreateWall(room.transform, "Wall_N", new Vector3(0, 2.5f, 6), new Vector3(12, 5, 0.2f));
            CreateWall(room.transform, "Wall_S", new Vector3(0, 2.5f, -6), new Vector3(12, 5, 0.2f));
            CreateWall(room.transform, "Wall_E", new Vector3(6, 2.5f, 0), new Vector3(0.2f, 5, 12));
            CreateWall(room.transform, "Wall_W", new Vector3(-6, 2.5f, 0), new Vector3(0.2f, 5, 12));

            CreateInteractable<SlitLamp>(room.transform, "SlitLamp", new Vector3(-2f, 0.6f, 2f), new Vector3(0.8f, 1.2f, 0.8f), new Color(0.75f, 0.78f, 0.82f));
            CreateInteractable<RetinaImage>(room.transform, "RetinaDisplay", new Vector3(2.5f, 1.8f, -4.8f), new Vector3(1.4f, 1f, 0.1f), new Color(0.2f, 0.25f, 0.35f));
            CreateInteractable<DoctorDesk>(room.transform, "DoctorDesk", new Vector3(3f, 0.5f, 1f), new Vector3(1.6f, 1f, 0.9f), new Color(0.55f, 0.38f, 0.22f));
            CreateInteractable<EyeChart>(room.transform, "EyeChart", new Vector3(-4.8f, 2f, 0f), new Vector3(0.1f, 1.2f, 0.9f), new Color(0.95f, 0.95f, 0.95f));
            CreateInteractable<Autorefractor>(room.transform, "Autorefractor", new Vector3(0f, 0.7f, -2f), new Vector3(0.9f, 1.4f, 0.8f), new Color(0.85f, 0.85f, 0.9f));
        }

        private static GameObject BuildPlayer()
        {
            var player = new GameObject("Player");
            player.transform.position = new Vector3(0, 0.1f, -4f);

            var controller = player.AddComponent<CharacterController>();
            controller.height = 1.8f;
            controller.radius = 0.3f;
            controller.center = new Vector3(0, 0.9f, 0);

            player.AddComponent<PlayerController>();

            var camPivot = new GameObject("CameraPivot");
            camPivot.transform.SetParent(player.transform);
            camPivot.transform.localPosition = new Vector3(0, 1.6f, 0);

            var camGo = new GameObject("PlayerCamera");
            camGo.tag = "MainCamera";
            camGo.transform.SetParent(camPivot.transform);
            camGo.transform.localPosition = Vector3.zero;
            var cam = camGo.AddComponent<Camera>();
            cam.nearClipPlane = 0.1f;
            camGo.AddComponent<AudioListener>();

            var look = camPivot.AddComponent<CameraLook>();
            SetSerialized(look, "playerBody", player.transform);

            var interaction = player.AddComponent<InteractionController>();
            SetSerialized(interaction, "playerCamera", cam);
            SetLayerMask(interaction, "interactableLayer", LayerMask.GetMask("Interactable"));

            return player;
        }

        private static GameObject BuildGameplayUI()
        {
            var canvasGo = CreateCanvas("GameCanvas");
            var canvas = canvasGo.GetComponent<Canvas>();

            if (Object.FindObjectOfType<EventSystem>() == null)
            {
                var es = new GameObject("EventSystem");
                es.AddComponent<EventSystem>();
                es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            }

            var touchControls = new GameObject("TouchControls", typeof(RectTransform));
            touchControls.transform.SetParent(canvas.transform, false);
            StretchFull(touchControls.GetComponent<RectTransform>());

            var joystickBg = CreateUiImage(touchControls.transform, "JoystickBackground", new Vector2(160, 160), new Vector2(140, 140), new Color(1, 1, 1, 0.25f));
            var joystickHandle = CreateUiImage(joystickBg.transform, "Handle", new Vector2(70, 70), Vector2.zero, new Color(1, 1, 1, 0.85f));

            var joystick = joystickBg.gameObject.AddComponent<VirtualJoystick>();
            SetSerialized(joystick, "background", joystickBg.GetComponent<RectTransform>());
            SetSerialized(joystick, "handle", joystickHandle.GetComponent<RectTransform>());

            var crosshairPanel = new GameObject("CrosshairPanel", typeof(RectTransform));
            crosshairPanel.transform.SetParent(canvas.transform, false);
            StretchFull(crosshairPanel.GetComponent<RectTransform>());

            var crosshair = CreateText(crosshairPanel.transform, "CrosshairPrompt", "Look and tap to interact", 22, new Vector2(0, -220));
            crosshair.color = Color.white;

            var infoPanel = CreatePanel(canvas.transform, "InfoPopup", new Color(0, 0, 0, 0.75f));
            var infoTitle = CreateText(infoPanel.transform, "Title", "Info", 30, new Vector2(0, 120));
            var infoBody = CreateText(infoPanel.transform, "Body", "Content", 22, new Vector2(0, 20));
            infoBody.GetComponent<RectTransform>().sizeDelta = new Vector2(620, 260);
            var closeInfo = CreateButton(infoPanel.transform, "CloseInfo", "Close", new Vector2(0, -150));

            var imagePanel = CreatePanel(canvas.transform, "ImageView", new Color(0, 0, 0, 0.85f));
            var image = CreateUiImage(imagePanel.transform, "RetinaImage", new Vector2(700, 500), Vector2.zero, Color.white);
            var closeImage = CreateButton(imagePanel.transform, "CloseImage", "Close", new Vector2(0, -280));

            var ui = canvasGo.AddComponent<UIManager>();
            SetSerialized(ui, "crosshairPanel", crosshairPanel);
            SetSerialized(ui, "promptText", crosshair);
            SetSerialized(ui, "infoPopupPanel", infoPanel);
            SetSerialized(ui, "infoTitleText", infoTitle);
            SetSerialized(ui, "infoContentText", infoBody);
            SetSerialized(ui, "imageViewPanel", imagePanel);
            SetSerialized(ui, "largeImageView", image);
            SetSerialized(ui, "touchControlsPanel", touchControls);

            var popupHandler = canvasGo.AddComponent<PopupCloseHandler>();
            closeInfo.onClick.AddListener(popupHandler.CloseInfoPopup);
            closeImage.onClick.AddListener(popupHandler.CloseImageView);

            infoPanel.SetActive(false);
            imagePanel.SetActive(false);

            var player = GameObject.Find("Player");
            if (player != null)
            {
                var pc = player.GetComponent<PlayerController>();
                SetSerialized(pc, "movementJoystick", joystick);
            }

            return canvasGo;
        }

        private static void BuildManagers()
        {
            if (Object.FindObjectOfType<GameManager>() != null)
            {
                return;
            }

            var managers = new GameObject("_Managers");
            managers.AddComponent<GameManager>();
            var audio = managers.AddComponent<AudioManager>();
            var sfx = managers.AddComponent<AudioSource>();
            var bgm = managers.AddComponent<AudioSource>();
            bgm.loop = true;
            bgm.volume = 0.3f;
            SetSerialized(audio, "sfxSource", sfx);
            SetSerialized(audio, "bgmSource", bgm);
        }

        private static void ConfigureBuildSettings()
        {
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ClinicScenePath, true),
                new EditorBuildSettingsScene(MenuScenePath, true)
            };
        }

        private static GameObject CreateCanvas(string name)
        {
            var canvasGo = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;
            return canvasGo;
        }

        private static void CreateFloor(Transform parent, Vector3 pos, Vector3 scale)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Floor";
            floor.transform.SetParent(parent);
            floor.transform.position = pos;
            floor.transform.localScale = scale;
            ApplyMaterial(floor, new Color(0.82f, 0.82f, 0.8f));
        }

        private static void CreateWall(Transform parent, string name, Vector3 pos, Vector3 scale)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.SetParent(parent);
            wall.transform.position = pos;
            wall.transform.localScale = scale;
            ApplyMaterial(wall, new Color(0.9f, 0.92f, 0.95f));
        }

        private static void CreateInteractable<T>(Transform parent, string name, Vector3 pos, Vector3 scale, Color color) where T : MonoBehaviour
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent);
            go.transform.position = pos;
            go.transform.localScale = scale;
            go.layer = LayerMask.NameToLayer("Interactable");
            go.AddComponent<T>();
            go.AddComponent<InteractableHighlight>();
            ApplyMaterial(go, color);
        }

        private static void ApplyMaterial(GameObject go, Color color)
        {
            var renderer = go.GetComponent<Renderer>();
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            var mat = new Material(shader);
            mat.color = color;
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
            }

            renderer.sharedMaterial = mat;
        }

        private static TextMeshProUGUI CreateText(Transform parent, string name, string text, int size, Vector2 anchoredPos)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.alignment = TextAlignmentOptions.Center;
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(700, 120);
            rt.anchoredPosition = anchoredPos;
            return tmp;
        }

        private static Button CreateButton(Transform parent, string name, string label, Vector2 anchoredPos)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            var img = go.GetComponent<Image>();
            img.color = new Color(0.15f, 0.45f, 0.75f, 0.95f);
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(280, 70);
            rt.anchoredPosition = anchoredPos;

            var text = CreateText(go.transform, "Label", label, 26, Vector2.zero);
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.offsetMin = Vector2.zero;
            text.rectTransform.offsetMax = Vector2.zero;

            return go.GetComponent<Button>();
        }

        private static GameObject CreatePanel(Transform parent, string name, Color bg)
        {
            var panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            StretchFull(panel.GetComponent<RectTransform>());
            panel.GetComponent<Image>().color = bg;
            return panel;
        }

        private static Image CreateUiImage(Transform parent, string name, Vector2 size, Vector2 pos, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = size;
            rt.anchoredPosition = pos;
            go.GetComponent<Image>().color = color;
            return go.GetComponent<Image>();
        }

        private static void StretchFull(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private static void EnsureLayer(string layerName, int userLayer)
        {
            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layers = tagManager.FindProperty("layers");
            if (string.IsNullOrEmpty(layers.GetArrayElementAtIndex(userLayer).stringValue))
            {
                layers.GetArrayElementAtIndex(userLayer).stringValue = layerName;
                tagManager.ApplyModifiedProperties();
            }
        }

        private static void SetSerialized(Object target, string fieldName, Object value)
        {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(fieldName);
            if (prop != null)
            {
                prop.objectReferenceValue = value;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void SetLayerMask(Object target, string fieldName, int mask)
        {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(fieldName);
            if (prop != null)
            {
                prop.intValue = mask;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}
#endif

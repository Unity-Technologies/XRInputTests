#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace XR.Input.Debug
{
    // Multi-column TreeView that shows Device Features
    class XRDeviceFeaturesTreeView : TreeView
    {
        public static XRDeviceFeaturesTreeView Create(ref TreeViewState treeState, ref MultiColumnHeaderState headerState)
        {
            if (treeState == null)
                treeState = new TreeViewState();

            var newHeaderState = CreateHeaderState();
            if (headerState != null)
                MultiColumnHeaderState.OverwriteSerializedFields(headerState, newHeaderState);
            headerState = newHeaderState;

            var header = new MultiColumnHeader(headerState);
            return new XRDeviceFeaturesTreeView(treeState, header);
        }

        const float kRowHeight = 20f;

        class Item : TreeViewItem
        {
            public string deviceRole;
            public string featureType;
            public string featureValue;
        }

        enum ColumnId
        {
            Name,
            Role,
            Type,
            Value,

            COUNT
        }

        static MultiColumnHeaderState CreateHeaderState()
        {
            var columns = new MultiColumnHeaderState.Column[(int)ColumnId.COUNT];

            columns[(int)ColumnId.Name] =
                new MultiColumnHeaderState.Column
                {
                    width = 240,
                    minWidth = 60,
                    headerContent = new GUIContent("Name")
                };
            columns[(int)ColumnId.Role] =
                new MultiColumnHeaderState.Column
                {
                    width = 200,
                    minWidth = 60,
                    headerContent = new GUIContent("Role")
                };
            columns[(int)ColumnId.Type] =
                new MultiColumnHeaderState.Column { width = 200, headerContent = new GUIContent("Type") };
            columns[(int)ColumnId.Value] =
                new MultiColumnHeaderState.Column { width = 200, headerContent = new GUIContent("Value") };

            return new MultiColumnHeaderState(columns);
        }

        XRDeviceFeaturesTreeView(TreeViewState state, MultiColumnHeader header)
            : base(state, header)
        {
            showBorder = false;
            rowHeight = kRowHeight;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var rootItem = BuildInteractableTree();

            // Wrap root control in invisible item required by TreeView.
            return new Item
            {
                displayName = "Input Devices",
                id = 0,
                children = new List<TreeViewItem> { rootItem },
                depth = -1
            };
        }

        string GetFeatureValue(InputDevice device, InputFeatureUsage featureUsage)
        {
            switch (featureUsage.type.ToString())
            {
                case "System.Boolean":
                    bool boolValue;
                    if (device.TryGetFeatureValue(featureUsage.As<bool>(), out boolValue))
                        return boolValue.ToString();
                    break;
                case "System.UInt32":
                    uint uintValue;
                    if (device.TryGetFeatureValue(featureUsage.As<uint>(), out uintValue))
                        return uintValue.ToString();
                    break;
                case "System.Single":
                    float floatValue;
                    if (device.TryGetFeatureValue(featureUsage.As<float>(), out floatValue))
                        return floatValue.ToString();
                    break;
                case "UnityEngine.Vector2":
                    Vector2 Vector2Value;
                    if (device.TryGetFeatureValue(featureUsage.As<Vector2>(), out Vector2Value))
                        return Vector2Value.ToString();
                    break;
                case "UnityEngine.Vector3":
                    Vector3 Vector3Value;
                    if (device.TryGetFeatureValue(featureUsage.As<Vector3>(), out Vector3Value))
                        return Vector3Value.ToString();
                    break;
                case "UnityEngine.Quaternion":
                    Quaternion QuaternionValue;
                    if (device.TryGetFeatureValue(featureUsage.As<Quaternion>(), out QuaternionValue))
                        return QuaternionValue.ToString();
                    break;
                case "UnityEngine.XR.Hand":
                    Hand HandValue;
                    if (device.TryGetFeatureValue(featureUsage.As<Hand>(), out HandValue))
                        return HandValue.ToString();
                    break;
                case "UnityEngine.XR.Bone":
                    Bone BoneValue;
                    if (device.TryGetFeatureValue(featureUsage.As<Bone>(), out BoneValue))
                        return BoneValue.ToString();
                    break;
                case "UnityEngine.XR.Eyes":
                    Eyes EyesValue;
                    if (device.TryGetFeatureValue(featureUsage.As<Eyes>(), out EyesValue))
                        return EyesValue.ToString();
                    break;
            }

            return "";
        }

        TreeViewItem BuildInteractableTree()
        {
            int id = 0;
            var rootItem = new Item
            {
                id = id++,
                displayName = "Devices",
                depth = 0
            };

            // Build children.
            var inputDevices = new List<InputDevice>();
            InputDevices.GetDevices(inputDevices);

            var deviceChildren = new List<TreeViewItem>();

            // Add device children
            foreach (var device in inputDevices)
            {
                var deviceItem = new Item
                {
                    id = device.GetHashCode(),
                    displayName = device.name,
                    deviceRole = device.role.ToString(),
                    depth = 1
                };
                deviceItem.parent = rootItem;
                
                List<InputFeatureUsage> featureUsages = new List<InputFeatureUsage>();
                device.TryGetFeatureUsages(featureUsages);
                
                var featureChildren = new List<TreeViewItem>();
                foreach (var featureUsage in featureUsages)
                {
                    Type featureType = featureUsage.type;
                    var featureItem = new Item
                    {
                        id = device.GetHashCode() ^ (featureUsage.GetHashCode() >> 1),
                        displayName = featureUsage.name,
                        featureType = featureType.ToString(),
                        featureValue = GetFeatureValue(device, featureUsage),
                        depth = 2
                    };
                    featureItem.parent = deviceItem;
                    featureChildren.Add(featureItem);
                }

                deviceItem.children = featureChildren;
                deviceChildren.Add(deviceItem);
            }

            // Sort deviceChildren by name.
            deviceChildren.Sort((a, b) => string.Compare(a.displayName, b.displayName));
            rootItem.children = deviceChildren;

            return rootItem;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (Item)args.item;

            var columnCount = args.GetNumVisibleColumns();
            for (var i = 0; i < columnCount; ++i)
            {
                ColumnGUI(args.GetCellRect(i), item, args.GetColumn(i), ref args);
            }
        }

        void ColumnGUI(Rect cellRect, Item item, int column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);

            if (column == (int)ColumnId.Name)
            {
                args.rowRect = cellRect;
                base.RowGUI(args);
            }

            switch (column)
            {
                case (int)ColumnId.Role:
                    GUI.Label(cellRect, item.deviceRole);
                    break;
                case (int)ColumnId.Type:
                    if (item.depth == 2)
                        GUI.Label(cellRect, item.featureType);
                    break;
                case (int)ColumnId.Value:
                    if (item.depth == 2)
                        GUI.Label(cellRect, item.featureValue);
                    break;
            }
        }
    }

    class HapticsTreeViewState : TreeViewState
    {
        [SerializeField] Vector2 m_ScrollPosition;
    }

    // Multi-column TreeView that shows Device Haptics
    class XRDeviceHapticsTreeView : TreeView
    {
        public static XRDeviceHapticsTreeView Create(ref TreeViewState treeState, ref MultiColumnHeaderState headerState)
        {
            if (treeState == null)
                treeState = new TreeViewState();

            var newHeaderState = CreateHeaderState();
            if (headerState != null)
                MultiColumnHeaderState.OverwriteSerializedFields(headerState, newHeaderState);
            headerState = newHeaderState;

            var header = new MultiColumnHeader(headerState);
            return new XRDeviceHapticsTreeView(treeState, header);
        }

        const float kRowHeight = 20f;

        class Item : TreeViewItem
        {
            public InputDevice device;
            public float amplitude = 1.0f;
            public float duration = 1.0f;
        }

        enum ColumnId
        {
            Name,
            Role,
            SupportsImpulse,
            SupportsBuffer,
            Amplitude,
            Duration,
            Trigger,

            COUNT
        }

        static MultiColumnHeaderState CreateHeaderState()
        {
            var columns = new MultiColumnHeaderState.Column[(int)ColumnId.COUNT];

            columns[(int)ColumnId.Name] =
                new MultiColumnHeaderState.Column
                {
                    width = 240,
                    minWidth = 60,
                    headerContent = new GUIContent("Name")
                };
            columns[(int)ColumnId.Role] =
                new MultiColumnHeaderState.Column
                {
                    width = 200,
                    minWidth = 60,
                    headerContent = new GUIContent("Role")
                };
            columns[(int)ColumnId.SupportsImpulse] =
                new MultiColumnHeaderState.Column { width = 60, headerContent = new GUIContent("SupportsImpulse") };
            columns[(int)ColumnId.SupportsBuffer] =
                new MultiColumnHeaderState.Column { width = 60, headerContent = new GUIContent("SupportsBuffer") };
            columns[(int)ColumnId.Amplitude] =
                new MultiColumnHeaderState.Column { width = 200, headerContent = new GUIContent("Amplitude") };
            columns[(int)ColumnId.Duration] =
                new MultiColumnHeaderState.Column { width = 200, headerContent = new GUIContent("Duration") };
            columns[(int)ColumnId.Trigger] =
                new MultiColumnHeaderState.Column { width = 60, headerContent = new GUIContent("Trigger") };

            return new MultiColumnHeaderState(columns);
        }

        XRDeviceHapticsTreeView(TreeViewState state, MultiColumnHeader header)
            : base(state, header)
        {
            showBorder = false;
            rowHeight = kRowHeight;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var rootItem = BuildHapticsTree();

            // Wrap root control in invisible item required by TreeView.
            return new Item
            {
                displayName = "Devices",
                id = 0,
                children = new List<TreeViewItem> { rootItem },
                depth = -1
            };
        }

        TreeViewItem BuildHapticsTree()
        {
            int id = 0;
            var rootItem = new Item
            {
                id = id++,
                displayName = "Devices",
                depth = 0
            };

            // Build children.
            var inputDevices = new List<InputDevice>();
            InputDevices.GetDevices(inputDevices);
            var deviceChildren = new List<TreeViewItem>();

            // Add device children
            foreach (var device in inputDevices)
            {
                var deviceItem = new Item
                {
                    id = device.GetHashCode(),
                    displayName = device.name,
                    device = device,
                    depth = 1
                };
                deviceItem.parent = rootItem;
                deviceChildren.Add(deviceItem);
            }

            // Sort deviceChildren by name.
            deviceChildren.Sort((a, b) => string.Compare(a.displayName, b.displayName));
            rootItem.children = deviceChildren;

            return rootItem;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (Item)args.item;

            var columnCount = args.GetNumVisibleColumns();
            for (var i = 0; i < columnCount; ++i)
            {
                ColumnGUI(args.GetCellRect(i), item, args.GetColumn(i), ref args);
            }
        }

        void ColumnGUI(Rect cellRect, Item item, int column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);

            if (column == (int)ColumnId.Name)
            {
                args.rowRect = cellRect;
                base.RowGUI(args);
            }

            switch (column)
            {
                case (int)ColumnId.Role:
                    if (item.depth > 0)
                        GUI.Label(cellRect, item.device.role.ToString());
                    break;
                case (int)ColumnId.SupportsImpulse:
                    if (item.depth > 0)
                    {
                        HapticCapabilities hapticCapabilities = new HapticCapabilities();
                        if (item.device.TryGetHapticCapabilities(out hapticCapabilities) && hapticCapabilities.supportsImpulse)
                            GUI.Toggle(cellRect, true, "");
                        else
                            GUI.Toggle(cellRect, false, "");
                    }
                    break;
                case (int)ColumnId.SupportsBuffer:
                    if (item.depth > 0)
                    {
                        HapticCapabilities hapticCapabilities = new HapticCapabilities();
                        if (item.device.TryGetHapticCapabilities(out hapticCapabilities) && hapticCapabilities.supportsBuffer)
                            GUI.Toggle(cellRect, true, "");
                        else
                            GUI.Toggle(cellRect, false, "");
                    }
                    break;
                case (int)ColumnId.Amplitude:
                    if (item.depth > 0)
                    {
                        item.amplitude = EditorGUI.Slider(cellRect, item.amplitude, 0.0F, 1.0F);
                    }
                    break;
                case (int)ColumnId.Duration:
                    if (item.depth > 0)
                    {
                        item.duration = EditorGUI.Slider(cellRect, item.duration, 0.0F, 10.0F);
                    }
                    break;
                case (int)ColumnId.Trigger:
                    if (item.depth > 0)
                    {
                        if (GUI.Button(cellRect, "Trigger"))
                            item.device.SendHapticImpulse(0, item.amplitude, item.duration);
                    }
                    break;
            }
        }
    }

    class XRDevicesDebuggerWindow : EditorWindow
    {
        static XRDevicesDebuggerWindow s_Instance;

        [SerializeField] Vector2 m_ScrollPosition;

        [SerializeField] bool m_ShowDeviceFeatures = true;
        [SerializeField] bool m_ShowDeviceHaptics = true;

        [SerializeField] Vector2 m_DeviceFeaturesTreeScrollPosition;
        [NonSerialized] XRDeviceFeaturesTreeView m_DeviceFeaturesTree;
        [SerializeField] TreeViewState m_DeviceFeaturesTreeState;
        [SerializeField] MultiColumnHeaderState m_DeviceFeaturesTreeHeaderState;

        [SerializeField] Vector2 m_DeviceHapticsTreeScrollPosition;
        [NonSerialized] XRDeviceHapticsTreeView m_DeviceHapticsTree;
        [SerializeField] TreeViewState m_DeviceHapticsTreeState;
        [SerializeField] MultiColumnHeaderState m_DeviceHapticsTreeHeaderState;

        [MenuItem("Window/XR Input Debugger", false, 2100)]

        public static void Init()
        {
            if (s_Instance == null)
            {
                s_Instance = GetWindow<XRDevicesDebuggerWindow>();
                s_Instance.Show();
                s_Instance.titleContent = new GUIContent("XR Input Debugger");
            }
            else
            {
                s_Instance.Show();
                s_Instance.Focus();
            }
        }

        public void OnEnable()
        {
            SetupDeviceFeaturesTree();
            SetupDeviceHapticsTree();
        }

        void SetupDeviceFeaturesTree()
        {
            if (m_DeviceFeaturesTreeState == null)
                m_DeviceFeaturesTreeState = new TreeViewState();
            m_DeviceFeaturesTree = XRDeviceFeaturesTreeView.Create(ref m_DeviceFeaturesTreeState, ref m_DeviceFeaturesTreeHeaderState);
            m_DeviceFeaturesTree.ExpandAll();
        }

        void SetupDeviceHapticsTree()
        {
            if (m_DeviceHapticsTreeState == null)
                m_DeviceHapticsTreeState = new TreeViewState();
            m_DeviceHapticsTree = XRDeviceHapticsTreeView.Create(ref m_DeviceHapticsTreeState, ref m_DeviceHapticsTreeHeaderState);
            m_DeviceHapticsTree.ExpandAll();
        }

        public void OnInspectorUpdate()
        {
            if (m_DeviceFeaturesTree != null)
            {
                m_DeviceFeaturesTree.Reload();
                m_DeviceFeaturesTree.Repaint();
            }
            if (m_DeviceHapticsTree != null)
            {
                m_DeviceHapticsTree.Reload();
                m_DeviceHapticsTree.Repaint();
            }

            Repaint();
        }

        public void OnGUI()
        {
            DrawToolbarGUI();
            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);

            if (m_ShowDeviceFeatures && m_DeviceFeaturesTree != null)
                DrawDeviceFeaturesGUI();
            if (m_ShowDeviceHaptics && m_DeviceHapticsTree != null)
                DrawDeviceHapticsGUI();

            EditorGUILayout.EndScrollView();
        }

        void DrawDeviceFeaturesGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Devices", GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            m_DeviceFeaturesTreeScrollPosition = EditorGUILayout.BeginScrollView(m_DeviceFeaturesTreeScrollPosition);
            var rect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true));
            m_DeviceFeaturesTree.OnGUI(rect);
            EditorGUILayout.EndScrollView();
        }

        void DrawDeviceHapticsGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Devices", GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            m_DeviceHapticsTreeScrollPosition = EditorGUILayout.BeginScrollView(m_DeviceHapticsTreeScrollPosition);
            var rect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true));
            m_DeviceHapticsTree.OnGUI(rect);
            EditorGUILayout.EndScrollView();
        }

        void DrawToolbarGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            m_ShowDeviceFeatures = GUILayout.Toggle(m_ShowDeviceFeatures, Contents.showDeviceFeatures, EditorStyles.toolbarButton);
            m_ShowDeviceHaptics = GUILayout.Toggle(m_ShowDeviceHaptics, Contents.showDeviceHaptics, EditorStyles.toolbarButton);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        static class Styles
        {
        }

        static class Contents
        {
            public static GUIContent noneContent = new GUIContent("None");
            public static GUIContent showDeviceFeatures = new GUIContent("Device Features");
            public static GUIContent showDeviceHaptics = new GUIContent("Device Haptics");
        }
    }
}

#endif // UNITY_EDITOR
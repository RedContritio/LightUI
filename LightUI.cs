using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuUIKit.GameObjects;
using UnityEngine.Assertions;
using UnityUIKit.Core;
using UnityUIKit.Core.GameObjects;
using UnityUIKit.GameObjects;

namespace LightUI
{
    public partial class LightUI
    {

        public const int defaultLineHeight = 60;

        private Manager manager = new Manager();

        private Stack<ManagedGameObject> layers = new Stack<ManagedGameObject>();


        /// <summary>
        /// 获取指定的对象（对象集合）
        /// 如果无匹配则返回null
        /// </summary>
        /// <param name="ID">要查询的对象ID</param>
        /// <returns></returns>
        public ManagedGameObject Find(string ID)
        {
            return manager.Find(ID);
        }

        /// <summary>
        /// 获取指定类型的对象集合
        /// </summary>
        /// <param name="type">要查询的对象类型</param>
        /// <returns></returns>
        public IEnumerable<ManagedGameObject> Find(LightUI.GUIElementType type)
        {
            return manager.Find(type);
        }

        /// <summary>
        /// 开始进行竖直布局
        /// </summary>
        /// <param name="expansion">是否可自动扩展</param>
        /// <param name="ID">该布局元素的ID</param>
        /// <param name="spacing">子元素之间的空格</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns>布局深度</returns>
        public int BeginVertical(Expansion expansion = Expansion.AutoSize, string ID = null, int spacing = 10, bool defaultActive = true)
        {
            int depth = layers.Count;
            ManagedGameObject obj = null;
            switch (expansion)
            {
                case Expansion.Fixed:
                    obj = CreateEmptyContainer(ID, Direction.Vertical, spacing, defaultActive: defaultActive);
                    break;
                case Expansion.AutoSize:
                    obj = CreateEmptyAutoSizeBox(ID, Direction.Vertical, spacing, defaultActive);
                    break;
            }
            layers.Push(obj);
            manager.Register(obj, ID);
            return depth;
        }

        /// <summary>
        /// 结束竖直布局，并返回该布局元素
        /// </summary>
        public ManagedGameObject EndVertical()
        {
            ManagedGameObject obj = layers.Pop();
            if (layers.Count > 0)
                layers.Peek().Children.Add(obj);
            return obj;
        }

        /// <summary>
        /// 开始进行水平布局
        /// </summary>
        /// <param name="expansion">是否可自动扩展</param>
        /// <param name="ID">该布局元素的ID</param>
        /// <param name="spacing">子元素之间的空格</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns>布局深度</returns>
        public int BeginHorizontal(Expansion expansion = Expansion.AutoSize, string ID = null, int spacing = 10, bool defaultActive = true)
        {
            int depth = layers.Count;
            ManagedGameObject obj = null;
            switch (expansion)
            {
                case Expansion.Fixed:
                    obj = CreateEmptyContainer(ID, Direction.Horizontal, spacing, defaultActive: defaultActive);
                    break;
                case Expansion.AutoSize:
                    obj = CreateEmptyAutoSizeBox(ID, Direction.Horizontal, spacing, defaultActive);
                    break;
            }
            layers.Push(obj);
            manager.Register(obj, ID);
            return depth;
        }

        /// <summary>
        /// 结束水平布局，并返回该水平布局元素
        /// </summary>
        public ManagedGameObject EndHorizontal()
        {
            ManagedGameObject obj = layers.Pop();
            if (layers.Count > 0)
                layers.Peek().Children.Add(obj);
            return obj;
        }

        /// <summary>
        /// 在当前布局内添加一个标签
        /// </summary>
        /// <param name="Text">标签文本</param>
        /// <param name="ID">注册用ID</param>
        /// <param name="Bold">是否加粗</param>
        /// <param name="OutLine">是否有轮廓线</param>
        /// <param name="style">标签样式</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuLabel Label(string Text, string ID = null, bool Bold = false, bool OutLine = false, TaiwuLabel.Style style = TaiwuLabel.Style.Subtitle, int width = 0, int height = defaultLineHeight, bool defaultActive = true)
        {
            TaiwuLabel label = CreateLabel(Text, ID, Bold, OutLine, style, width, height, defaultActive);
            manager.Register(label, ID);

            AddGameObjectToLayout(label);
            return label;
        }

        /// <summary>
        /// 在当前布局内添加一个Toggle（开关）
        /// </summary>
        /// <param name="Text">显示文本</param>
        /// <param name="isOn">默认是否激活</param>
        /// <param name="OnChange">激活状态改变时触发的回调函数</param>
        /// <param name="ID">注册ID</param>
        /// <param name="Bold">是否加粗</param>
        /// <param name="OutLine">是否有轮廓线</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="TipTitle">光标悬浮时提示文本的标题</param>
        /// <param name="TipContent">光标悬浮时的提示文本</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuToggle Toggle(string Text, bool isOn, Action<bool> OnChange, string ID = null, bool Bold = false, bool OutLine = false, int width = 0, int height = defaultLineHeight, string TipTitle = null, string TipContent = null, bool defaultActive = true)
        {
            TaiwuToggle toggle = CreateToggle(Text, isOn, OnChange, ID, Bold, OutLine, width, height, TipTitle, TipContent, defaultActive);
            manager.Register(toggle, ID);

            AddGameObjectToLayout(toggle);
            return toggle;
        }

        /// <summary>
        /// 在当前布局内添加一个滑条
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="defaultValue">初始值</param>
        /// <param name="OnChange">滑条位置改变时触发的回调函数</param>
        /// <param name="ID">注册ID</param>
        /// <param name="wholeNumber">是否整数</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="TipTitle">光标悬浮时提示文本的标题</param>
        /// <param name="TipContent">光标悬浮时的提示文本</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuSlider Slider(float minValue, float maxValue, float defaultValue, Action<float> OnChange, string ID, bool wholeNumber = true, int width = 0, int height = defaultLineHeight, string TipTitle = null, string TipContent = null, bool defaultActive = true)
        {
            TaiwuSlider slider = CreateSlider(minValue, maxValue, defaultValue, OnChange, ID, wholeNumber, width, height, TipTitle, TipContent, defaultActive);
            manager.Register(slider, ID);

            AddGameObjectToLayout(slider);
            return slider;
        }

        /// <summary>
        /// 在当前布局内添加一个按钮
        /// </summary>
        /// <param name="Text">按钮文本</param>
        /// <param name="OnClick">点击按钮后触发的回调函数</param>
        /// <param name="ID">注册ID</param>
        /// <param name="Bold">是否加粗</param>
        /// <param name="OutLine">是否有轮廓线</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="Interactable">该按钮是否可用</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuButton Button(string Text, Action OnClick, string ID = null, bool Bold = false, bool OutLine = false, int width = 0, int height = defaultLineHeight, bool Interactable = true, bool defaultActive = true)
        {
            TaiwuButton button = CreateButton(Text, OnClick, ID, Bold, OutLine, width, height, Interactable, defaultActive);
            manager.Register(button, ID);

            AddGameObjectToLayout(button);
            return button;
        }

        /// <summary>
        /// 在当前布局内添加一个文本编辑框
        /// </summary>
        /// <param name="defaultText">初始文本</param>
        /// <param name="PlaceHolder">文本为空的占位符</param>
        /// <param name="onValueChanged">每次文本改变时调用的回调函数</param>
        /// <param name="onEndEdit">结束编辑时调用的回调函数</param>
        /// <param name="ID">注册ID</param>
        /// <param name="ReadOnly">是否只读</param>
        /// <param name="inputType">输入类型，输入文本是否可视及是否纠错</param>
        /// <param name="contentType">内容类型，用于自动纠错</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuInputField InputField(string defaultText, string PlaceHolder, Action<string> OnValueChanged = null, Action<string> OnEndEdit = null, string ID = null, bool ReadOnly = false, UnityEngine.UI.InputField.InputType inputType = UnityEngine.UI.InputField.InputType.Standard, UnityEngine.UI.InputField.ContentType contentType = UnityEngine.UI.InputField.ContentType.Standard, bool defaultActive = true)
        {
            TaiwuInputField inputField = CreateInputField(defaultText, PlaceHolder, OnValueChanged, OnEndEdit, ID, ReadOnly, inputType, contentType, defaultActive);
            manager.Register(inputField, ID);

            AddGameObjectToLayout(inputField);
            return inputField;
        }

        /// <summary>
        /// 在当前布局内添加一个单选Toggle组
        /// </summary>
        /// <param name="Texts">Toggles的文本</param>
        /// <param name="onChanged">选中特定Toggle触发的回调函数</param>
        /// <param name="defaultValue">初始被选中的Toggle下标</param>
        /// <param name="ID">注册ID</param>
        /// <param name="Bold">是否加粗</param>
        /// <param name="OutLine">是否有轮廓线</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="spacing">子元素之间的空格</param>
        /// <param name="TipTitle">光标悬浮时提示文本的标题</param>
        /// <param name="TipContent">光标悬浮时的提示文本</param>
        /// <param name="direction">排布方向</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public ToggleGroup ToggleGroup(List<string> Texts, Action<int> onChanged, int defaultValue = 0, string ID = null, bool Bold = false, bool OutLine = false, int width = 0, int height = defaultLineHeight, int spacing = 10, string TipTitle = null, string TipContent = null, Direction direction = Direction.Horizontal, bool defaultActive = true)
        {
            ToggleGroup group = CreateToggleGroup(Texts, onChanged, defaultValue, ID, Bold, OutLine, width, height, spacing, TipTitle, TipContent, direction, defaultActive);
            manager.Register(group, ID);

            AddGameObjectToLayout(group);
            return group;
        }

        /// <summary>
        /// 在当前布局内添加一个空格
        /// </summary>
        /// <param name="width">空格宽度</param>
        /// <param name="height">空格高度</param>
        /// <returns></returns>
        public ManagedGameObject Space(int width, int height = defaultLineHeight)
        {
            Container space = CreateSpace(width, height);
            manager.Register(space);

            AddGameObjectToLayout(space);
            return space;
        }

        /// <summary>
        /// 创建一个文本标签
        /// </summary>
        /// <param name="Text">标签文本</param>
        /// <param name="ID">注册用ID</param>
        /// <param name="Bold">是否加粗</param>
        /// <param name="OutLine">是否有轮廓线</param>
        /// <param name="style">标签样式</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuLabel CreateLabel(string Text, string ID = null, bool Bold = false, bool OutLine = false, TaiwuLabel.Style style = TaiwuLabel.Style.Subtitle, int width = 0, int height = defaultLineHeight, bool defaultActive = true)
        {
            TaiwuLabel label = new TaiwuLabel()
            {
                Name = ID,
                Text = Text,
                UseBoldFont = Bold,
                UseOutline = OutLine,
                Element = {
                    PreferredSize = {width, height }
                },
                BackgroundStyle = style,
                DefaultActive = defaultActive
            };
            return label;
        }

        /// <summary>
        /// 创建一个 Toggle
        /// </summary>
        /// <param name="Text">显示文本</param>
        /// <param name="isOn">默认是否激活</param>
        /// <param name="OnChange">激活状态改变时触发的回调函数</param>
        /// <param name="ID">注册ID</param>
        /// <param name="Bold">是否加粗</param>
        /// <param name="OutLine">是否有轮廓线</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="TipTitle">光标悬浮时提示文本的标题</param>
        /// <param name="TipContent">光标悬浮时的提示文本</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuToggle CreateToggle(string Text, bool isOn, Action<bool> OnChange, string ID = null, bool Bold = false, bool OutLine = false, int width = 0, int height = defaultLineHeight, string TipTitle = null, string TipContent = null, bool defaultActive = true)
        {
            TaiwuToggle toggle = new TaiwuToggle()
            {
                Name = ID,
                Text = Text,
                isOn = isOn,
                onValueChanged = (bool value, Toggle _) => { OnChange?.Invoke(value); },
                Element = {
                    PreferredSize = { width, height }
                },
                UseBoldFont = Bold,
                UseOutline = OutLine,
                TipTitle = TipTitle,
                TipContant = TipContent,
                DefaultActive = defaultActive
            };
            return toggle;
        }

        /// <summary>
        /// 创建一个滑条
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="defaultValue">初始值</param>
        /// <param name="OnChange">滑条位置改变时触发的回调函数</param>
        /// <param name="ID">注册ID</param>
        /// <param name="wholeNumber">是否整数</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="TipTitle">光标悬浮时提示文本的标题</param>
        /// <param name="TipContent">光标悬浮时的提示文本</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuSlider CreateSlider(float minValue, float maxValue, float defaultValue, Action<float> OnChange, string ID, bool wholeNumber = true, int width = 0, int height = defaultLineHeight, string TipTitle = null, string TipContent = null, bool defaultActive = true)
        {
            TaiwuSlider slider = new TaiwuSlider()
            {
                Name = ID,
                MinValue = minValue,
                MaxValue = maxValue,
                Value = defaultValue,
                OnValueChanged = (float value, Slider _) => { OnChange?.Invoke(value); },
                WholeNumber = wholeNumber,
                Element = {
                    PreferredSize = { width, height }
                },
                TipTitle = TipTitle,
                TipContant = TipContent,
                DefaultActive = defaultActive
            };
            return slider;
        }

        /// <summary>
        /// 创建一个按钮
        /// </summary>
        /// <param name="Text">按钮文本</param>
        /// <param name="OnClick">点击按钮后触发的回调函数</param>
        /// <param name="ID">注册ID</param>
        /// <param name="Bold">是否加粗</param>
        /// <param name="OutLine">是否有轮廓线</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="Interactable">该按钮是否可用</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuButton CreateButton(string Text, Action OnClick, string ID = null, bool Bold = false, bool OutLine = false, int width = 0, int height = defaultLineHeight, bool Interactable = true, bool defaultActive = true)
        {
            TaiwuButton button = new TaiwuButton()
            {
                Name = ID,
                OnClick = (Button _) => { OnClick?.Invoke(); },
                Element = {
                    PreferredSize = { width, height }
                },
                Text = Text,
                UseBoldFont = Bold,
                UseOutline = OutLine,
                Interactable = Interactable,
                DefaultActive = defaultActive
            };
            return button;
        }

        /// <summary>
        /// 创建一个文本输入框
        /// </summary>
        /// <param name="defaultText">初始文本</param>
        /// <param name="PlaceHolder">文本为空的占位符</param>
        /// <param name="onValueChanged">每次文本改变时调用的回调函数</param>
        /// <param name="onEndEdit">结束编辑时调用的回调函数</param>
        /// <param name="ID">注册ID</param>
        /// <param name="ReadOnly">是否只读</param>
        /// <param name="inputType">输入类型，输入文本是否可视及是否纠错</param>
        /// <param name="contentType">内容类型，用于自动纠错</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuInputField CreateInputField(string defaultText, string PlaceHolder, Action<string> onValueChanged = null, Action<string> onEndEdit = null, string ID = null, bool ReadOnly = false, UnityEngine.UI.InputField.InputType inputType = UnityEngine.UI.InputField.InputType.Standard, UnityEngine.UI.InputField.ContentType contentType = UnityEngine.UI.InputField.ContentType.Standard, bool defaultActive = true)
        {
            TaiwuInputField inputField = new TaiwuInputField()
            {
                Name = ID,
                Text = defaultText,
                Placeholder = PlaceHolder,
                ReadOnly = ReadOnly,
                InputType = inputType,
                ContentType = contentType,
                OnValueChanged = (string text, InputField _) => { onValueChanged?.Invoke(text); },
                OnEndEdit = (string text, InputField _) => { onEndEdit?.Invoke(text); },
                DefaultActive = defaultActive
            };
            return inputField;
        }

        /// <summary>
        /// 创建一个Toggle单选组
        /// </summary>
        /// <param name="Texts">Toggles的文本</param>
        /// <param name="onChanged">选中特定Toggle触发的回调函数</param>
        /// <param name="defaultValue">初始被选中的Toggle下标</param>
        /// <param name="ID">注册ID</param>
        /// <param name="Bold">是否加粗</param>
        /// <param name="OutLine">是否有轮廓线</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="spacing">子元素之间的空格</param>
        /// <param name="TipTitle">光标悬浮时提示文本的标题</param>
        /// <param name="TipContent">光标悬浮时的提示文本</param>
        /// <param name="direction">排布方向</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public ToggleGroup CreateToggleGroup(List<string> Texts, Action<int> onChanged, int defaultValue = 0, string ID = null, bool Bold = false, bool OutLine = false, int width = 0, int height = defaultLineHeight, int spacing = 10, string TipTitle = null, string TipContent = null, Direction direction = Direction.Horizontal, bool defaultActive = true)
        {
            return CreateToggleGroup(Texts, onChanged, defaultValue, ID, Bold, OutLine, width, height, spacing, (int _) => { return TipTitle; }, (int _) => { return TipContent; }, direction, defaultActive);
        }

        /// <summary>
        /// 创建一个能具有不同提示内容的Toggle单选组
        /// </summary>
        /// <param name="Texts">Toggles的文本</param>
        /// <param name="onChanged">选中特定Toggle触发的回调函数</param>
        /// <param name="defaultValue">初始被选中的Toggle下标</param>
        /// <param name="ID">注册ID</param>
        /// <param name="Bold">是否加粗</param>
        /// <param name="OutLine">是否有轮廓线</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="spacing">子元素之间的空格</param>
        /// <param name="GetTipTitle">获取光标悬浮时提示文本标题的函数</param>
        /// <param name="GetTipContent">获取光标悬浮时提示文本的函数</param>
        /// <param name="direction">排布方向</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public ToggleGroup CreateToggleGroup(List<string> Texts, Action<int> onChanged, int defaultValue = 0, string ID = null, bool Bold = false, bool OutLine = false, int width = 0, int height = defaultLineHeight, int spacing = 10, Func<int, string> GetTipTitle = null, Func<int, string> GetTipContent = null, Direction direction = Direction.Horizontal, bool defaultActive = true)
        {
            List<ManagedGameObject> toggles = new List<ManagedGameObject>();

            for (int i = 0; i < Texts.Count; ++i)
            {
                int j = i;
                toggles.Add(CreateToggle(Texts[i], i == defaultValue,
                    (bool value) =>
                    {
                        if (value)
                            onChanged?.Invoke(j);
                    },
                    null, Bold, OutLine, width, height, GetTipTitle?.Invoke(i), GetTipContent?.Invoke(i)));
            }

            ToggleGroup toggleGroup = new ToggleGroup()
            {
                Name = ID,
                Element = {
                    PreferredSize = { width, height }
                },
                Group = {
                    Spacing = spacing,
                    Direction = direction
                },
                Children = toggles,
                DefaultActive = defaultActive
            };

            return toggleGroup;
        }

        /// <summary>
        /// 创建一个空的 AutoSizeBox
        /// </summary>
        /// <param name="ID">注册ID</param>
        /// <param name="direction">排布方向</param>
        /// <param name="spacing">子元素之间的空格</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public BoxAutoSizeModelGameObject CreateEmptyAutoSizeBox(string ID = null, Direction direction = Direction.Horizontal, int spacing = 0, bool defaultActive = true)
        {
            BoxAutoSizeModelGameObject box = new BoxAutoSizeModelGameObject()
            {
                Name = ID,
                Group = {
                    Direction = direction,
                    Spacing = spacing
                },
                Children = new List<ManagedGameObject>(),
                DefaultActive = defaultActive
            };
            return box;
        }

        /// <summary>
        /// 创建一个空的 Container
        /// </summary>
        /// <param name="ID">注册ID</param>
        /// <param name="direction">排布方向</param>
        /// <param name="spacing">子元素之间的空格</param>
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public Container CreateEmptyContainer(string ID = null, Direction direction = Direction.Horizontal, int spacing = 0, int width = 0, int height = defaultLineHeight, bool defaultActive = true)
        {
            Container container = new Container()
            {
                Name = ID,
                Element =
                {
                    PreferredSize = { width, height}
                },
                Group = {
                    Direction = direction,
                    Spacing = spacing
                },
                Children = new List<ManagedGameObject>(),
                DefaultActive = defaultActive
            };
            return container;
        }

        /// <summary>
        /// 创建一段空白
        /// </summary>
        /// <param name="width">空白宽度</param>
        /// <param name="height">空白长度</param>
        /// <returns></returns>
        public Container CreateSpace(int width, int height = defaultLineHeight)
        {
            return CreateEmptyContainer(width: width, height: height);
        }

        /// <summary>
        /// 添加组件到最顶层的布局中
        /// </summary>
        /// <param name="gameObject">要添加的组件</param>
        private void AddGameObjectToLayout(ManagedGameObject gameObject)
        {
            if (layers.Count <= 0)
                throw new Exception("Element is expected to set in Layout Element.");

            layers.Peek().Children.Add(gameObject);
        }
    }
}

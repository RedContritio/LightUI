using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuUIKit.GameObjects;
using UnityUIKit.GameObjects;

namespace LightUI
{
    public partial class LightUI
    {
        public TaiwuInputField InputField(ConfigEntry<string> Text)
        {
            return InputField(Text, null);
        }

        public TaiwuInputField InputField(ConfigEntry<string> Text, string placeHolder)
        {
            return InputField(Text, placeHolder, 0, defaultLineHeight);
        }
        public TaiwuInputField InputField(ConfigEntry<string> Text, string placeHolder, int width, int height = defaultLineHeight)
        {
            return InputField(Text, placeHolder, UnityEngine.UI.InputField.InputType.Standard, UnityEngine.UI.InputField.ContentType.Standard, width, height);
        }

        public TaiwuInputField InputField(ConfigEntry<string> Text, string placeHolder, UnityEngine.UI.InputField.InputType inputType, UnityEngine.UI.InputField.ContentType contentType, int width, int height = defaultLineHeight)
        {
            TaiwuInputField inputField = CreateInputField(Text.Value, placeHolder, null, onEndEdit: (string str) => { Text.Value = str; }, null, false, width, height, inputType, contentType);
            manager.Register(inputField);

            AddGameObjectToLayout(inputField);
            return inputField;
        }
        public TaiwuInputField InputField(string defaultText, string PlaceHolder, Action<string> OnValueChanged, Action<string> OnEndEdit)
        {
            return InputField(defaultText, PlaceHolder, OnValueChanged, OnEndEdit, 0);
        }

        public TaiwuInputField InputField(string defaultText, string PlaceHolder, Action<string> OnValueChanged, Action<string> OnEndEdit, int width, int height = defaultLineHeight)
        {
            return InputField(defaultText, PlaceHolder, OnValueChanged, OnEndEdit, UnityEngine.UI.InputField.InputType.Standard, UnityEngine.UI.InputField.ContentType.Standard, width, height);
        }

        public TaiwuInputField InputField(string defaultText, string PlaceHolder, Action<string> OnValueChanged, Action<string> OnEndEdit, UnityEngine.UI.InputField.InputType inputType, UnityEngine.UI.InputField.ContentType contentType, int width, int height = defaultLineHeight)
        {
            TaiwuInputField inputField = CreateInputField(defaultText, PlaceHolder, OnValueChanged, OnEndEdit, null, false, width, height, inputType, contentType);
            manager.Register(inputField);

            AddGameObjectToLayout(inputField);
            return inputField;
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
        public TaiwuInputField InputField(string defaultText, string PlaceHolder, Action<string> OnValueChanged, Action<string> OnEndEdit, string ID, bool ReadOnly, UnityEngine.UI.InputField.InputType inputType = UnityEngine.UI.InputField.InputType.Standard, UnityEngine.UI.InputField.ContentType contentType = UnityEngine.UI.InputField.ContentType.Standard, bool defaultActive = true)
        {
            TaiwuInputField inputField = CreateInputField(defaultText, PlaceHolder, OnValueChanged, OnEndEdit, ID, ReadOnly, inputType, contentType, defaultActive);
            manager.Register(inputField, ID);

            AddGameObjectToLayout(inputField);
            return inputField;
        }


        public TaiwuInputField CreateInputField(string defaultText, string PlaceHolder, Action<string> onValueChanged, Action<string> onEndEdit, string ID, bool ReadOnly, UnityEngine.UI.InputField.InputType inputType, UnityEngine.UI.InputField.ContentType contentType, bool defaultActive = true)
        {
            return CreateInputField(defaultText, PlaceHolder, onValueChanged, onEndEdit, ID, ReadOnly, 0, defaultLineHeight, inputType: inputType, contentType, defaultActive);
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
        /// <param name="width">预设宽度</param>
        /// <param name="height">预设高度</param>
        /// <param name="inputType">输入类型，输入文本是否可视及是否纠错</param>
        /// <param name="contentType">内容类型，用于自动纠错</param>
        /// <param name="defaultActive">默认是否可见</param>
        /// <returns></returns>
        public TaiwuInputField CreateInputField(string defaultText, string PlaceHolder, Action<string> onValueChanged, Action<string> onEndEdit, string ID, bool ReadOnly, int width, int height, UnityEngine.UI.InputField.InputType inputType, UnityEngine.UI.InputField.ContentType contentType, bool defaultActive = true)
        {
            TaiwuInputField inputField = new TaiwuInputField()
            {
                Name = ID,
                Text = defaultText,
                Placeholder = PlaceHolder,
                ReadOnly = ReadOnly,
                Element = 
                {
                    PreferredSize = { width, height}
                },
                InputType = inputType,
                ContentType = contentType,
                OnValueChanged = (string text, InputField _) => { onValueChanged?.Invoke(text); },
                OnEndEdit = (string text, InputField _) => { onEndEdit?.Invoke(text); },
                DefaultActive = defaultActive
            };
            return inputField;
        }
    }
}

using BepInEx.Configuration;
using System;
using TaiwuUIKit.GameObjects;
using UnityUIKit.GameObjects;

namespace LightUI
{
    public partial class LightUI
    {
        public TaiwuToggle Toggle(string Text, ConfigEntry<bool> bindValue)
        {
            return Toggle(Text, bindValue.Value, (bool value) => { bindValue.Value = value; });
        }

        public TaiwuToggle Toggle(string Text, bool isOn, Action<bool> OnChange)
        {
            return Toggle(Text, isOn, OnChange, 0, defaultLineHeight);
        }
        public TaiwuToggle Toggle(string Text, bool isOn, Action<bool> OnChange, int width, int height = defaultLineHeight)
        {
            return Toggle(Text, isOn, OnChange, null, null, width, height);
        }
        public TaiwuToggle Toggle(string Text, bool isOn, Action<bool> OnChange, string TipTitle, string TipContent, int width, int height = defaultLineHeight)
        {
            return Toggle(Text, isOn, OnChange, null, false, false, width, height, TipTitle, TipContent, true);
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
        public TaiwuToggle Toggle(string Text, bool isOn, Action<bool> OnChange, string ID, bool Bold, bool OutLine, int width, int height, string TipTitle, string TipContent, bool defaultActive)
        {
            TaiwuToggle toggle = CreateToggle(Text, isOn, OnChange, ID, Bold, OutLine, width, height, TipTitle, TipContent, defaultActive);
            manager.Register(toggle, ID);

            AddGameObjectToLayout(toggle);
            return toggle;
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
        public static TaiwuToggle CreateToggle(string Text, bool isOn, Action<bool> OnChange, string ID, bool Bold, bool OutLine, int width, int height, string TipTitle, string TipContent, bool defaultActive = true)
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
    }
}

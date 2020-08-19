using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuUIKit.GameObjects;
using UnityUIKit.Core;
using UnityUIKit.GameObjects;

namespace LightUI
{
    public partial class LightUI
    {
        public ToggleGroup ToggleGroup(List<string> Texts, Action<int> onChanged)
        {
            return ToggleGroup(Texts, onChanged, 0);
        }
        public ToggleGroup ToggleGroup(List<string> Texts, ConfigEntry<int> bindValue)
        {
            return ToggleGroup(Texts, (int value) => { bindValue.Value = value;}, bindValue.Value);
        }
        public ToggleGroup ToggleGroup(List<string> Texts, ConfigEntry<int> bindValue, int width, int height = defaultLineHeight)
        {
            return ToggleGroup(Texts, (int value) => { bindValue.Value = value; }, bindValue.Value, width, height);
        }
        public ToggleGroup ToggleGroup(List<string> Texts, Action<int> onChanged, int defaultValue)
        {
            return ToggleGroup(Texts, onChanged, defaultValue, 0, defaultLineHeight);
        }

        public ToggleGroup ToggleGroup(List<string> Texts, Action<int> onChanged, int defaultValue, int width, int height = defaultLineHeight)
        {
            return ToggleGroup(Texts, onChanged, defaultValue, null, new List<string>(), width, height);
        }
        public ToggleGroup ToggleGroup(List<string> Texts, Action<int> onChanged, int defaultValue, string TipTitle, List<string> TipContents, int width, int height = defaultLineHeight)
        {
            return ToggleGroup(Texts, onChanged, defaultValue, 10, TipTitle, TipContents, width, height);
        }

        public ToggleGroup ToggleGroup(List<string> Texts, Action<int> onChanged, int defaultValue, int spacing, string TipTitle, List<string> TipContents, int width, int height = defaultLineHeight)
        {
            ToggleGroup group = CreateToggleGroup(Texts, onChanged, defaultValue, null, false, false, width, height, spacing, TipTitle, TipContents, Direction.Horizontal, true);
            manager.Register(group);

            AddGameObjectToLayout(group);
            return group;
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
        public ToggleGroup ToggleGroup(List<string> Texts, Action<int> onChanged, int defaultValue, string ID, bool Bold, bool OutLine, int width, int height, int spacing, string TipTitle, string TipContent, Direction direction, bool defaultActive = true)
        {
            ToggleGroup group = CreateToggleGroup(Texts, onChanged, defaultValue, ID, Bold, OutLine, width, height, spacing, TipTitle, TipContent, direction, defaultActive);
            manager.Register(group, ID);

            AddGameObjectToLayout(group);
            return group;
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
        public ToggleGroup CreateToggleGroup(List<string> Texts, Action<int> onChanged, int defaultValue, string ID, bool Bold, bool OutLine, int width, int height, int spacing, string TipTitle, string TipContent, Direction direction, bool defaultActive = true)
        {
            List<string> tipContents = new List<string>();
            while(tipContents.Count < Texts.Count) tipContents.Add(TipContent);

            return CreateToggleGroup(Texts, onChanged, defaultValue, ID, Bold, OutLine, width, height, spacing, TipTitle, tipContents, direction, defaultActive);
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
        public ToggleGroup CreateToggleGroup(List<string> Texts, Action<int> onChanged, int defaultValue, string ID, bool Bold, bool OutLine, int width, int height, int spacing, string TipTitle, List<string> TipContents, Direction direction, bool defaultActive)
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
                    null, Bold, OutLine, width, height,TipTitle, (i < TipContents.Count() ? TipContents?.ElementAt(i) : null)));
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
    }
}

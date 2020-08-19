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
        public TaiwuButton Button(string Text, Action OnClick)
        {
            return Button(Text, OnClick, 0, defaultLineHeight);
        }

        public TaiwuButton Button(string Text, Action OnClick, int width, int height = defaultLineHeight)
        {
            return Button(Text, OnClick, false, false, 0, defaultLineHeight);
        }

        public TaiwuButton Button(string Text, Action OnClick, bool Bold, bool OutLine, int width, int height = defaultLineHeight)
        {
            return Button(Text, OnClick, null, Bold, OutLine, width, height, true, true);
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
        public TaiwuButton Button(string Text, Action OnClick, string ID, bool Bold, bool OutLine, int width, int height, bool Interactable, bool defaultActive)
        {
            TaiwuButton button = CreateButton(Text, OnClick, ID, Bold, OutLine, width, height, Interactable, defaultActive);
            manager.Register(button, ID);

            AddGameObjectToLayout(button);
            return button;
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
        public TaiwuButton CreateButton(string Text, Action OnClick, string ID , bool Bold, bool OutLine, int width, int height, bool Interactable, bool defaultActive)
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
    }
}

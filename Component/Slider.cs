using BepInEx.Configuration;
using System;
using TaiwuUIKit.GameObjects;
using UnityUIKit.GameObjects;

namespace LightUI
{
    public partial class LightUI
    {
        public TaiwuSlider Slider(int minValue, int maxValue, ConfigEntry<int> bindValue, int width, int height = defaultLineHeight)
        {
            return BindSlider(minValue, maxValue, bindValue, true, (int v) => { return (float)v; }, (float v) => { return (int)v; }, width, height);
        }
        public TaiwuSlider Slider(float minValue, float maxValue, ConfigEntry<float> bindValue, int width, int height = defaultLineHeight)
        {
            return BindSlider(minValue, maxValue, bindValue, false, (float v) => { return v; }, (float v) => { return v; }, width, height);
        }
        public TaiwuSlider Slider(double minValue, double maxValue, ConfigEntry<double> bindValue, int width, int height = defaultLineHeight)
        {
            return BindSlider(minValue, maxValue, bindValue, false, (double v) => { return (float)v; }, (float v) => { return (double)v; }, width, height);
        }
        public TaiwuSlider Slider(int minValue, int maxValue, int defaultValue, Action<int> OnChange, int width, int height = defaultLineHeight)
        {
            return Slider(minValue, maxValue, defaultValue, (float value) => { OnChange?.Invoke((int)value); }, true, width, height);
        }

        public TaiwuSlider Slider(float minValue, float maxValue, float defaultValue, Action<float> OnChange, int width, int height = defaultLineHeight)
        {
            return Slider(minValue, maxValue, defaultValue, (float value) => { OnChange?.Invoke(value); }, false, width, height);
        }
        public TaiwuSlider Slider(double minValue, double maxValue, double defaultValue, Action<double> OnChange, int width, int height = defaultLineHeight)
        {
            return Slider((float)minValue, (float)maxValue, (float)defaultValue, (float value) => { OnChange?.Invoke(value); }, false, width, height);
        }

        public TaiwuSlider Slider(float minValue, float maxValue, float defaultValue, Action<float> OnChange, bool wholeNumber, int width, int height = defaultLineHeight)
        {
            return Slider(minValue, maxValue, defaultValue, OnChange, wholeNumber, null, null, width, height);
        }

        public TaiwuSlider Slider(float minValue, float maxValue, float defaultValue, Action<float> OnChange, bool wholeNumber, string TipTitle, string TipContent, int width ,int height = defaultLineHeight)
        {
            return Slider(minValue, maxValue, defaultValue, OnChange, null, wholeNumber, width, height, TipTitle, TipContent, true);
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
        public TaiwuSlider Slider(float minValue, float maxValue, float defaultValue, Action<float> OnChange, string ID, bool wholeNumber, int width, int height, string TipTitle, string TipContent, bool defaultActive)
        {
            TaiwuSlider slider = CreateSlider(minValue, maxValue, defaultValue, OnChange, ID, wholeNumber, width, height, TipTitle, TipContent, defaultActive);
            manager.Register(slider, ID);

            AddGameObjectToLayout(slider);
            return slider;
        }

        private TaiwuSlider BindSlider<T>(T minValue, T maxValue, ConfigEntry<T> bindValue, bool wholeNumber, Func<T, float> conv1, Func<float, T> conv2, int width, int height = defaultLineHeight)
        {
            return Slider(conv1(minValue), conv1(maxValue), conv1(bindValue.Value), (float value) => { bindValue.Value = conv2(value); }, wholeNumber: wholeNumber, width, height);
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
        public static TaiwuSlider CreateSlider(float minValue, float maxValue, float defaultValue, Action<float> OnChange, string ID, bool wholeNumber, int width, int height, string TipTitle, string TipContent, bool defaultActive)
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
    }
}

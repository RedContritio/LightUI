using TaiwuUIKit.GameObjects;

namespace LightUI
{
    public partial class LightUI
    {
        public TaiwuLabel Label(string Text)
        {
            return Label(Text, 0);
        }

        public TaiwuLabel Label(string Text, int width, int height = defaultLineHeight)
        {
            return Label(Text, TaiwuLabel.Style.Subtitle, width, height);
        }
        public TaiwuLabel Label(string Text, TaiwuLabel.Style style, int width, int height = defaultLineHeight)
        {
            return Label(Text, false, false, TaiwuLabel.Style.Subtitle, width, height);
        }
        public TaiwuLabel Label(string Text, bool Bold, bool OutLine, TaiwuLabel.Style style, int width, int height = defaultLineHeight)
        {
            return Label(Text, null, Bold, OutLine, style, width, height);
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
        public TaiwuLabel Label(string Text, string ID, bool Bold, bool OutLine, TaiwuLabel.Style style, int width, int height = defaultLineHeight, bool defaultActive = true)
        {
            TaiwuLabel label = CreateLabel(Text, ID, Bold, OutLine, style, width, height, defaultActive);
            manager.Register(label, ID);

            AddGameObjectToLayout(label);
            return label;
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
        public static TaiwuLabel CreateLabel(string Text, string ID = null, bool Bold = false, bool OutLine = false, TaiwuLabel.Style style = TaiwuLabel.Style.Subtitle, int width = 0, int height = defaultLineHeight, bool defaultActive = true)
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
    }
}

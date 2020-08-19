using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        private ManagedGameObject lastObject = null;


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
            return EndLayer();
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
            return EndLayer();
        }

        public ManagedGameObject EndLayer()
        {
            ManagedGameObject obj = layers.Pop();
            if (layers.Count > 0)
                AddGameObjectToLayout(obj);
            return obj;
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

        public bool SetID(string ID)
        {
            return manager.UpdateKey(lastObject, lastObject.Name, ID);
        }

        public bool SetPreferredSize(int width, int height = defaultLineHeight)
        {
            try
            {
                UnityUIKit.Components.BoxElement.ComponentAttributes element = GetObjectField("Element") as UnityUIKit.Components.BoxElement.ComponentAttributes;
                element.PreferredSize = new List<float>{ width, height};
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void SetDefaultActive(bool active)
        {
            lastObject.DefaultActive = active;
        }

        public bool SetText(string Text)
        {
            return SetObjectField("Text", Text);
        }

        public bool SetBold(bool state = true)
        {
            return SetObjectField("UseBoldFont", state);
        }

        public bool SetOutline(bool state = true)
        {
            return SetObjectField("UseOutline", state);
        }

        private bool SetObjectField(string FieldName, object value)
        {
            try
            {
                lastObject.GetType().GetProperty(FieldName).SetValue(lastObject, value, null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private object GetObjectField(string FieldName)
        {
            try
            {
                return lastObject.GetType().GetProperty(FieldName).GetValue(lastObject);
            }
            catch
            {
                return null;
            }
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
            lastObject = gameObject;
        }
    }
}

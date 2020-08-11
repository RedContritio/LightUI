using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaiwuUIKit.GameObjects;
using UnityEngine;
using UnityUIKit.Core;
using UnityUIKit.Core.GameObjects;
using UnityUIKit.GameObjects;

namespace LightUI
{
    class Manager
    {
        private readonly Dictionary<GUI.GUIElementType, Type> TypeMap = new Dictionary<GUI.GUIElementType, Type>() {
            { GUI.GUIElementType.Label, typeof(TaiwuLabel)},
            { GUI.GUIElementType.Toggle, typeof(TaiwuToggle) },
            { GUI.GUIElementType.Slider, typeof(TaiwuSlider) },
            { GUI.GUIElementType.Button, typeof(TaiwuButton) },
            { GUI.GUIElementType.InputField, typeof(TaiwuInputField) },
            { GUI.GUIElementType.ToggleGroup, typeof(ToggleGroup) },
            { GUI.GUIElementType.Box, typeof(BoxAutoSizeModelGameObject) },
            { GUI.GUIElementType.Container, typeof(Container) }
        };

        public Dictionary<string, ManagedGameObject> ID2OBJ = new Dictionary<string, ManagedGameObject>();

        public HashSet<ManagedGameObject> OBJ_SET = new HashSet<ManagedGameObject>();

        /// <summary>
        /// 注册一个新组件
        /// 若有同ID组件，则会替换已有的。
        /// </summary>
        /// <param name="object">组件</param>
        /// <param name="ID">ID，字符串</param>
        /// <returns></returns>
        public bool Register(ManagedGameObject @object, string ID = null)
        {
            if(OBJ_SET.Contains(@object))
                return false;

            if(!string.IsNullOrEmpty(ID))
            {
                if(!ID2OBJ.ContainsKey(ID))
                    ID2OBJ.Add(ID, @object);
                else
                    ID2OBJ[ID] = @object;
            }
            OBJ_SET.Add(@object);

            return true;
        }

        /// <summary>
        /// 获取指定的对象（对象集合）
        /// </summary>
        /// <param name="ID">要查询的对象ID</param>
        /// <returns></returns>
        public ManagedGameObject Find(string ID)
        {
            if(ID2OBJ.TryGetValue(ID, out ManagedGameObject obj))
            {
                return obj;
            }
            return null;
        }

        /// <summary>
        /// 获取指定的对象集合
        /// </summary>
        /// <param name="type">要查询的对象类型</param>
        /// <returns></returns>
        public IEnumerable<ManagedGameObject> Find(GUI.GUIElementType type)
        {
            if(TypeMap.TryGetValue(type, out Type t))
            {
                foreach (ManagedGameObject @object in OBJ_SET)
                {
                    if(@object.GetType() == t)
                    {
                        yield return @object;
                    }
                }
            }
        }
    }
}

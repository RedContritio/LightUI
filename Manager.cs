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
        private readonly Dictionary<LightUI.GUIElementType, Type> TypeMap = new Dictionary<LightUI.GUIElementType, Type>() {
            { LightUI.GUIElementType.Label, typeof(TaiwuLabel)},
            { LightUI.GUIElementType.Toggle, typeof(TaiwuToggle) },
            { LightUI.GUIElementType.Slider, typeof(TaiwuSlider) },
            { LightUI.GUIElementType.Button, typeof(TaiwuButton) },
            { LightUI.GUIElementType.InputField, typeof(TaiwuInputField) },
            { LightUI.GUIElementType.ToggleGroup, typeof(ToggleGroup) },
            { LightUI.GUIElementType.Box, typeof(BoxAutoSizeModelGameObject) },
            { LightUI.GUIElementType.Container, typeof(Container) }
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
        public IEnumerable<ManagedGameObject> Find(LightUI.GUIElementType type)
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

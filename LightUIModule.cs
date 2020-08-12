using BepInEx;
using BepInEx.Logging;
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
    [BepInPlugin(GUID, "LightUI", Version)]
    [BepInProcess("The Scroll Of Taiwu Alpha V1.0.exe")]
    public class LightUIModule : BaseUnityPlugin
    {
        /// <summary>GUID</summary>
        public const string GUID = "0.LightUI";
        /// <summary>版本</summary>
        public const string Version = "1.0.0";

        public new static ManualLogSource Logger;

        private void Awake()
        {
            Logger = base.Logger;
        }

        private void Update()
        {
        }
    }
}

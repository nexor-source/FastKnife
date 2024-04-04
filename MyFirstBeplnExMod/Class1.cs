using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace FastKnife
{
    [BepInPlugin(modGUID, modName, modVersion)]
    // [BepInPlugin("nexor.MyFirstBepInExMod", "这是我的第一个BepIn插件", "1.0.0.0")]
    public class FastKnife : BaseUnityPlugin
    {
        private const string modGUID = "nexor.FastKnife";
        private const string modName = "FastKnife";
        private const string modVersion = "0.0.3";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static FastKnife Instance;

        // 在插件启动时会直接调用Awake()方法
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            // 使用Debug.Log()方法来将文本输出到控制台
            // Debug.Log("Hello, world!");
            harmony.PatchAll();
            ((FastKnife)this).Logger.LogInfo((object)"FastKnife 0.0.3 loaded.");
        }
    }
}

namespace FastKnife.Patches.Items
{
    [HarmonyPatch(typeof(KnifeItem))] // 目标类 KnifeItem
    [HarmonyPatch("HitKnife")] // 目标方法 HitKnife
    internal class KnifeItem_HitKnife_Patch
    {
        [HarmonyPostfix] // 后置补丁
        private static void Postfix(KnifeItem __instance)
        {
            // 使用反射获取私有变量 timeAtLastDamageDealt
            var timeAtLastDamageDealtField = typeof(KnifeItem).GetField("timeAtLastDamageDealt",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (timeAtLastDamageDealtField != null)
            {
                // 修改私有变量的值为 -0.38
                timeAtLastDamageDealtField.SetValue(__instance, -1.0f);
            }
            else
            {
                Debug.LogError("Failed to access timeAtLastDamageDealt field in KnifeItem class!");
            }
        }
    }
}
﻿using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EnforcerPlugin.Modules
{
    internal static class Buffs
    {
        internal static BuffDef protectAndServeBuff;
        internal static BuffDef energyShieldBuff;
        internal static BuffDef skateboardBuff;
        internal static BuffDef minigunBuff;

        internal static BuffDef smallSlowBuff;
        internal static BuffDef bigSlowBuff;
        internal static BuffDef impairedBuff;
        internal static BuffDef nemImpairedBuff;


        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        internal static void RegisterBuffs()
        {
            IL.RoR2.BuffCatalog.Init += FixBuffCatalog;

            protectAndServeBuff = AddNewBuff("Heavyweight", Assets.MainAssetBundle.LoadAsset<Sprite>("texBuffProtectAndServe"), EnforcerPlugin.characterColor, false, false);
            energyShieldBuff = AddNewBuff("EnergyShield", Assets.MainAssetBundle.LoadAsset<Sprite>("texBuffProtectAndServe"), EnforcerPlugin.characterColor, false, false);
            skateboardBuff = AddNewBuff("Heavyweight", Resources.Load<Sprite>("Textures/BuffIcons/texMovespeedBuffIcon"), EnforcerPlugin.characterColor, false, false);
            minigunBuff = AddNewBuff("MinigunStance", Assets.MainAssetBundle.LoadAsset<Sprite>("texBuffMinigun"), new Color(1, 0.7176f, 0.1725f), false, false);

            smallSlowBuff = AddNewBuff("NemSmallSelfSlow", Resources.Load<Sprite>("Textures/BuffIcons/texBuffSlow50Icon"), new Color(0.647f, 0.168f, 0.184f), false, true);
            bigSlowBuff = AddNewBuff("NemBigSelfSlow", Resources.Load<Sprite>("Textures/BuffIcons/texBuffSlow50Icon"), new Color(0.65f, 0.078f, 0.078f), false, true);
            impairedBuff = AddNewBuff("Impaired", Resources.Load<Sprite>("Textures/BuffIcons/texBuffCloakIcon"), Color.grey, false, true);
            nemImpairedBuff = AddNewBuff("NemImpaired", Resources.Load<Sprite>("Textures/BuffIcons/texBuffSlow50Icon"), Color.red, false, true);
        }

        internal static void FixBuffCatalog(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.Next.MatchLdsfld(typeof(RoR2Content.Buffs), nameof(RoR2Content.Buffs.buffDefs)))
            {
                return;
            }

            c.Remove();
            c.Emit(OpCodes.Ldsfld, typeof(ContentManager).GetField(nameof(ContentManager.buffDefs)));
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            buffDefs.Add(buffDef);

            return buffDef;
        }
    }
}

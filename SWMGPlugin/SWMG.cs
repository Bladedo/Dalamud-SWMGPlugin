using System;
using System.Runtime.InteropServices;
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using Lumina.Excel;
using SWMGPlugin.Helpers.AudioHelper;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace SWMGPlugin
{
    public unsafe class SWMG
    {
        private CachedSound swmg;

        private delegate void OnActionUsedDelegate(
            uint sourceId, 
            Character* sourceCharacter, 
            IntPtr pos, 
            IntPtr effectHeader,
            IntPtr effectArray,
            IntPtr effectTrail
            );

        private Hook<OnActionUsedDelegate> actionUsedHook;

        private ExcelSheet<Action> action;

        public SWMG()
        {
            swmg = AudioHelper.Instance.CacheFromData("swmg.wav");

            string actUsedSig = "40 55 53 57 41 54 41 55 41 56 41 57 48 8D AC 24 ?? ?? ?? ?? 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 45 70";
            actionUsedHook = Services.GameInteropProvider.HookFromSignature<OnActionUsedDelegate>(actUsedSig, OnActionUsed);

            action = Services.DataManager.GetExcelSheet<Action>()!;
        }

        private void OnActionUsed(
            uint sourceId,
            Character* sourceCharacter,
            IntPtr pos,
            IntPtr effectHeader,
            IntPtr effectArray,
            IntPtr effectTrail)
        {
            actionUsedHook.Original(sourceId, sourceCharacter, pos, effectHeader, effectArray, effectTrail);

            var row = action.GetRow((uint)Marshal.ReadInt32(effectHeader, 8));
            if (row == null)
            {
                return;
            }

            if (row.ActionCategory.Row != 9u) // limit break
            {
                return;
            }
            AudioHelper.Instance.PlayOneshot(swmg);
        }

        public void OnEnable()
        {
            actionUsedHook.Enable();
        }

        public void OnDisable()
        {
            actionUsedHook.Disable();
        }
    }
}

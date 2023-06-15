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

        private delegate void DOnActionUsed(
            uint source,
            Character* character,
            IntPtr position,
            IntPtr fxHeader,
            IntPtr fxArray,
            IntPtr fxTrail);

        private Hook<DOnActionUsed> actionUsedHook;

        private ExcelSheet<Action> action;

        public SWMG()
        {
            swmg = AudioHelper.Instance.CacheFromData("swmg.wav");

            var actUsedSig = Services.SigScanner.ScanText("40 55 53 57 41 54 41 55 41 56 41 57 48 8D AC 24 ?? ?? ?? ?? 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 45 70");
            actionUsedHook = Hook<DOnActionUsed>.FromAddress(actUsedSig, OnActionUsedDetour, false);

            action = Services.DataManager.GetExcelSheet<Action>()!;
        }

        private void OnActionUsedDetour(
            uint source,
            Character* character,
            IntPtr position,
            IntPtr fxHeader,
            IntPtr fxArray,
            IntPtr fxTrail)
        {
            actionUsedHook.Original(source, character, position, fxHeader, fxArray, fxTrail);

            var row = action.GetRow((uint)Marshal.ReadInt32(fxHeader, 8));
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

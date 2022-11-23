﻿using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XIVAutoAttack.Actions.BaseAction;
using XIVAutoAttack.Data;
using XIVAutoAttack.Helpers;
using XIVAutoAttack.Windows.ComboConfigWindow;
using XIVAutoAttack.Windows;
using XIVAutoAttack.Actions;
using Lumina.Data.Parsing;

namespace XIVAutoAttack.Combos.Script.Actions
{
    internal class CountDownAction
    {
        private BaseAction _action { get; set; }
        public ActionID ID { get; set; } = ActionID.None;
        MethodInfo _method;
        public string MethodName { get; set; } = string.Empty;

        public bool MustUse { get; set; }
        public bool Empty { get; set; }

        public float Time { get; set; }

        public CountDownAction()
        {

        }

        public CountDownAction(BaseAction act)
        {
            _action = act;
            ID = (ActionID)act.ID;
        }

        public CountDownAction(MethodInfo method)
        {
            _method = method;
            MethodName = method.Name;
        }

        public void DrawHeader(IScriptCombo combo)
        {
            float iconSize = 30;
            var size = new System.Numerics.Vector2(iconSize, iconSize);

            if (ID != ActionID.None && (_action == null || (ActionID)_action.ID != ID))
            {
                _action = combo.AllActions.FirstOrDefault(a => (ActionID)a.ID == ID);
            }
            if (!string.IsNullOrEmpty(MethodName) && (_method == null || _method.Name != MethodName))
            {
                _method = combo.GetType().GetMethodInfo(MethodName);
            }

            if (_action != null)
            {
                ImGui.Image(_action.GetTexture().ImGuiHandle, size);

                ImGui.SameLine();
                ComboConfigWindow.Spacing();

                var time = Time;
                ImGui.SetNextItemWidth(50);
                if(ImGui.DragFloat($"s##倒计时{GetHashCode()}", ref time))
                {
                    Time = time;
                }

                ImGui.SameLine();
                ComboConfigWindow.Spacing();

                var mustUse = MustUse;
                if (ImGui.Checkbox($"必须##必须{GetHashCode()}", ref mustUse))
                {
                    MustUse = mustUse;
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("跳过AOE判断，跳过提供的Buff判断。");
                }

                ImGui.SameLine();

                var empty = Empty;
                if (ImGui.Checkbox($"用光##用光{GetHashCode()}", ref empty))
                {
                    Empty = empty;
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("用完所有层数或者跳过连击判断。");
                }

                ImGui.SameLine();
                ComboConfigWindow.Spacing();

                ImGui.Text(_action.Name);
            }
            else if (_method != null)
            {
                ImGui.Image(IconSet.GetTexture(1).ImGuiHandle, size);

                ImGui.SameLine();
                ComboConfigWindow.Spacing();

                var time = Time;
                ImGui.SetNextItemWidth(50);
                if (ImGui.DragFloat($"s##倒计时{GetHashCode()}", ref time))
                {
                    Time = time;
                }

                ImGui.SameLine();
                ComboConfigWindow.Spacing();

                ImGui.Text(_method.GetMemberName());

                var desc = _method.GetMemberDescription();
                if (ImGui.IsItemHovered() && !string.IsNullOrEmpty(desc))
                {
                    ImGui.SetTooltip(desc);
                }
            }
        }

        public bool ShouldUse(IScriptCombo owner, out IAction act)
        {
            act = null;
            if (ID != ActionID.None && _action == null)
            {
                _action = owner.AllActions.FirstOrDefault(a => (ActionID)a.ID == ID);
            }
            if (!string.IsNullOrEmpty(MethodName) && (_method == null || _method.Name != MethodName))
            {
                _method = owner.GetType().GetMethodInfo(MethodName);
            }

            return _action?.ShouldUse(out act, MustUse, Empty) ?? false;
        }
    }
}

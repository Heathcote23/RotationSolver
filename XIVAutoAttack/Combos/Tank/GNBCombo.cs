using Dalamud.Game.ClientState.JobGauge.Types;
using Lumina.Excel.GeneratedSheets;
using System.Collections.Generic;
using XIVAutoAttack.Actions;
using XIVAutoAttack.Combos.CustomCombo;
using XIVAutoAttack.Configuration;

namespace XIVAutoAttack.Combos.Tank;

internal class GNBCombo : JobGaugeCombo<GNBGauge>
{
    internal override uint JobID => 37;
    internal override bool HaveShield => StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.RoyalGuard);
    private protected override PVEAction Shield => Actions.RoyalGuard;

    protected override bool CanHealSingleSpell => false;
    protected override bool CanHealAreaSpell => false;

    private static string ammoCound = "";

    internal struct Actions
    {
        public static readonly PVEAction
            //��������
            RoyalGuard = new(16142, shouldEndSpecial: true),

            //����ն
            KeenEdge = new(16137),

            //����
            NoMercy = new(16138),

            //�б���
            BrutalShell = new(16139),

            //αװ
            Camouflage = new(16140)
            {
                BuffsProvide = GeneralActions.Rampart.BuffsProvide,
                OtherCheck = PVEAction.TankDefenseSelf,
            },

            //��ħ��
            DemonSlice = new(16141),

            //���׵�
            LightningShot = new(16143),

            //Σ������
            DangerZone = new(16144),

            //Ѹ��ն
            SolidBarrel = new(16145),

            //������
            BurstStrike = new(16162)
            {
                OtherCheck = b => JobGauge.Ammo > 0,
            },

            //����
            Nebula = new (16148)
            {
                BuffsProvide = GeneralActions.Rampart.BuffsProvide,
                OtherCheck = PVEAction.TankDefenseSelf,
            },

            //��ħɱ
            DemonSlaughter = new (16149),

            //����
            Aurora = new PVEAction(16151, true)
            {
                BuffsProvide = new [] { ObjectStatus.Aurora },
            },

            //��������
            Superbolide = new (16152)
            {
                OtherCheck = PVEAction.TankBreakOtherCheck,
            },

            //������
            SonicBreak = new (16153),

            //�ַ�ն
            RoughDivide = new (16154, shouldEndSpecial: true)
            {
                ChoiceTarget = TargetFilter.FindMoveTarget
            },

            //����
            GnashingFang = new (16146)
            {
                OtherCheck = b => JobGauge.AmmoComboStep == 0 && JobGauge.Ammo > 0,
            },

            //���γ岨
            BowShock = new (16159),

            //��֮��
            HeartofLight = new (16160, true),

            //ʯ֮��
            HeartofStone = new (16161, true)
            {
                BuffsProvide = GeneralActions.Rampart.BuffsProvide,
                ChoiceTarget = TargetFilter.FindAttackedTarget,
            },

            //����֮��
            FatedCircle = new (16163)
            {
                OtherCheck = b => JobGauge.Ammo > (Level >= 88 ? 2 : 1),
            },

            //Ѫ��
            Bloodfest = new (16164)
            {
                OtherCheck = b => JobGauge.Ammo == 0,
            },

            //����
            DoubleDown = new (25760)
            {
                OtherCheck = b => JobGauge.Ammo >= 2,
            },

            //����צ
            SavageClaw = new (16147),

            //����צ
            WickedTalon = new (16150),

            //˺��
            JugularRip = new (16156)
            {
                OtherCheck = b => Service.IconReplacer.OriginalHook(16155) == JugularRip.ID,
            },

            //����
            AbdomenTear = new (16157)
            {
                OtherCheck = b => Service.IconReplacer.OriginalHook(16155) == AbdomenTear.ID,
            },

            //��Ŀ
            EyeGouge = new (16158)
            {
                OtherCheck = b => Service.IconReplacer.OriginalHook(16155) == EyeGouge.ID,
            },

            //������
            Hypervelocity = new (25759)
            {
                OtherCheck = b => Service.IconReplacer.OriginalHook(16155) == Hypervelocity.ID,
            };
    }
    internal override SortedList<DescType, string> Description => new ()
    {
        {DescType.��������, $"{Actions.Aurora.Action.Name}"},
        {DescType.��Χ����, $"{Actions.HeartofLight.Action.Name}"},
        {DescType.�������, $"{Actions.HeartofStone.Action.Name}, {Actions.Nebula.Action.Name}, {Actions.Camouflage.Action.Name}"},
        {DescType.�ƶ�, $"{Actions.RoughDivide.Action.Name}"},
    };

    private protected override ActionConfiguration CreateConfiguration()
    {
        return base.CreateConfiguration().SetCombo("GNB_Opener", 4, new string[]
        {
            "4GCD����",

        }, "����ѡ��");
    }

    private protected override bool BreakAbility(byte abilityRemain, out IAction act)
    {
        //����
        if (Level >= Actions.BurstStrike.Level && abilityRemain == 1 && Actions.NoMercy.ShouldUse(out act))
        {
            if (LastWeaponskill == Actions.KeenEdge.ID && JobGauge.Ammo == 1 && Actions.GnashingFang.RecastTimeRemain == 0 && !Actions.Bloodfest.IsCoolDown)
            {
                ammoCound = "4B";
                return true;
            }
            //3��������
            else if (JobGauge.Ammo == (Level >= 88 ? 3 : 2))
            {
                return true;
            }
            //2��������
            else if (JobGauge.Ammo == 2 && Actions.GnashingFang.RecastTimeRemain > 0)
            {
                return true;
            }
        }
        if (Level < Actions.BurstStrike.Level && abilityRemain == 1 && Actions.NoMercy.ShouldUse(out act)) return true;

        act = null;
        return false;
    }

    private protected override bool GeneralGCD(uint lastComboActionID, out IAction act)
    {
        //AOE
        //if (breakUseAmmo && Actions.DoubleDown.ShouldUse(out act, mustUse: true)) return true;
        if (Actions.FatedCircle.ShouldUse(out act)) return true;
        if (Actions.DemonSlaughter.ShouldUse(out act, lastComboActionID)) return true;
        if (Actions.DemonSlice.ShouldUse(out act, lastComboActionID)) return true;

        //����
        if ((JobGauge.Ammo == (Level >= 88 ? 3 : 2) && (StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.NoMercy) || Actions.NoMercy.RecastTimeRemain > 55)) ||    //3������
            (JobGauge.Ammo > 0 && Actions.NoMercy.RecastTimeRemain > 17 && Actions.NoMercy.RecastTimeRemain < 35) ||    //����������
            (JobGauge.Ammo == 3 && LastWeaponskill == Actions.BrutalShell.ID && Actions.NoMercy.RecastTimeRemain < 3) || 
            (JobGauge.Ammo == 1 && Actions.NoMercy.RecastTimeRemain > 55 && Actions.Bloodfest.RecastTimeRemain < 5) ||
            (JobGauge.Ammo == 1 && Actions.NoMercy.RecastTimeRemain > 55 && ((!Actions.Bloodfest.IsCoolDown && Level >= Actions.Bloodfest.Level) || Level < Actions.Bloodfest.Level)))
        {
            if (Actions.GnashingFang.ShouldUse(out act)) return true;
        }

        //������
        if (Actions.SonicBreak.ShouldUse(out act))
        {
            if (Actions.GnashingFang.RecastTimeRemain > 0 && StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.NoMercy)) return true;

            if (Level < Actions.DoubleDown.Level && StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.ReadyToRip) 
                && Actions.GnashingFang.RecastTimeRemain > 0) return true;
        }

        //����
        if (Actions.DoubleDown.ShouldUse(out act, mustUse: true))
        {
            if (Actions.SonicBreak.RecastTimeRemain > 0 && StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.NoMercy)) return true;

            if (StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.NoMercy) && Actions.NoMercy.RecastTimeRemain > 55 && Actions.Bloodfest.RecastTimeRemain < 5) return true;
        }

        //���������
        uint remap = Service.IconReplacer.OriginalHook(Actions.GnashingFang.ID);
        if (remap == Actions.WickedTalon.ID && Actions.WickedTalon.ShouldUse(out act)) return true;
        if (remap == Actions.SavageClaw.ID && Actions.SavageClaw.ShouldUse(out act)) return true;

        //������   
        if (Actions.BurstStrike.ShouldUse(out act))
        {
            if (Actions.SonicBreak.RecastTimeRemain > 0 && Actions.SonicBreak.RecastTimeRemain < 0.5) return false;

            if (StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.NoMercy) && 
                JobGauge.AmmoComboStep == 0 && 
                Actions.GnashingFang.RecastTimeRemain > 1) return true;

            if (LastWeaponskill == Actions.BrutalShell.ID && 
                (JobGauge.Ammo == (Level >= 88 ? 3 : 2) || 
                (Actions.Bloodfest.RecastTimeRemain < 6 && JobGauge.Ammo <= 2 && Actions.NoMercy.RecastTimeRemain > 10 && Level >= Actions.Bloodfest.Level))) return true;
        }

        //��������
        if (Actions.GnashingFang.RecastTimeRemain > 0 && Actions.GnashingFang.RecastTimeRemain < 0.5) return false;
        if (Actions.SolidBarrel.ShouldUse(out act, lastComboActionID)) return true;
        if (Actions.BrutalShell.ShouldUse(out act, lastComboActionID)) return true;
        if (Actions.KeenEdge.ShouldUse(out act, lastComboActionID)) return true;

        if (IconReplacer.Move && MoveAbility(1, out act)) return true;
        if (Actions.LightningShot.ShouldUse(out act)) return true;

        return false;
    }

    private protected override bool EmergercyAbility(byte abilityRemain, IAction nextGCD, out IAction act)
    {
        //��ʥ���� ���л�����ˡ�
        if (Actions.Superbolide.ShouldUse(out act)) return true;
        return false;
    }

    private protected override bool ForAttachAbility(byte abilityRemain, out IAction act)
    {
        //Σ������
        if (Actions.DangerZone.ShouldUse(out act))
        {
            //�Ǳ�����
            if (!StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.NoMercy)
            && ((Actions.GnashingFang.RecastTimeRemain > 20)
            || (Level < Actions.GnashingFang.Level) && Actions.NoMercy.IsCoolDown)) return true;

            //������
            if (StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.NoMercy))
            {
                //������ȴ��
                if (Actions.GnashingFang.RecastTimeRemain > 0) return true;
            }
        }

        //���γ岨
        if (Actions.BowShock.ShouldUse(out act, mustUse: true))
        {
            //������
            if (StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.NoMercy))
            {
                //����������ȴ��
                if (Actions.SonicBreak.RecastTimeRemain > 0) return true;
            }

            //����������ȴ��
            if (Actions.SonicBreak.IsCoolDown && Level < Actions.DoubleDown.Level)
            {
                //���γ岨
                if (Actions.BowShock.ShouldUse(out act, mustUse: true)) return true;
                //Σ������
                if (Actions.DangerZone.ShouldUse(out act)) return true;
            }
        }

        //����
        if (Actions.JugularRip.ShouldUse(out act)) return true;
        if (Actions.AbdomenTear.ShouldUse(out act)) return true;
        if (Actions.EyeGouge.ShouldUse(out act)) return true;
        if (Actions.Hypervelocity.ShouldUse(out act)) return true;

        //Ѫ��
        if (Actions.GnashingFang.RecastTimeRemain > 0 && Actions.Bloodfest.ShouldUse(out act)) return true;

        //��㹥��
        if (Actions.RoughDivide.Target.DistanceToPlayer() < 1 && !IsMoving)
        {  
            if (Actions.RoughDivide.ShouldUse(out act)) return true;
            if (StatusHelper.HaveStatusSelfFromSelf(ObjectStatus.NoMercy) && Actions.RoughDivide.ShouldUse(out act, emptyOrSkipCombo: true)) return true;
        }
        return false;
    }

    private protected override bool DefenceAreaAbility(byte abilityRemain, out IAction act)
    {
        if (Actions.HeartofLight.ShouldUse(out act, emptyOrSkipCombo: true)) return true;
        if (GeneralActions.Reprisal.ShouldUse(out act, mustUse: true)) return true;
        return false;
    }

    private protected override bool MoveAbility(byte abilityRemain, out IAction act)
    {
        //ͻ��
        if (Actions.RoughDivide.ShouldUse(out act, emptyOrSkipCombo: true)) return true;
        return false;
    }
    private protected override bool DefenceSingleAbility(byte abilityRemain, out IAction act)
    {
        if (abilityRemain == 2)
        {

            //����10%��
            if (Actions.HeartofStone.ShouldUse(out act)) return true;

            //���ƣ�����30%��
            if (Actions.Nebula.ShouldUse(out act)) return true;

            //���ڣ�����20%��
            if (GeneralActions.Rampart.ShouldUse(out act)) return true;

            //αװ������10%��
            if (Actions.Camouflage.ShouldUse(out act)) return true;
        }
        //���͹���
        //ѩ��
        if (GeneralActions.Reprisal.ShouldUse(out act)) return true;

        act = null;
        return false;
    }

    private protected override bool HealSingleAbility(byte abilityRemain, out IAction act)
    {
        if (Actions.Aurora.ShouldUse(out act, emptyOrSkipCombo: true) && abilityRemain == 1) return true;

        return false;
    }
}

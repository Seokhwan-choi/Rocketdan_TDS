using UnityEngine;

namespace TDS
{
    class DamageInst
    {
        public Character Attacker;
        public Character Defender;
        public double Damage;
    }

    // ������ ���
    static class DamageCalculator
    {
        public static DamageInst Create(Character attacker, Character defender)
        {
            DamageInst damageInst = new DamageInst();
            damageInst.Attacker = attacker;
            damageInst.Defender = defender;
            damageInst.Damage = attacker.Stat.Atk;

            return damageInst;
        }
    }
}
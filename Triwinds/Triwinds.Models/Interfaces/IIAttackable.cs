using System;

namespace Triwinds.Models.Interfaces
{
    public interface IIAttackable
    {
        UInt16 Range { get; set; }
        DamageRange Damage { get; set; }
    }
}

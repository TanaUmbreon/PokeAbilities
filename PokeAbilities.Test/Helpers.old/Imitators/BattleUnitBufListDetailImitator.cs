namespace PokeAbilities.Test.Helpers.Imitators
{
    /// <summary>
    /// <see cref="BattleUnitBufListDetail"/> クラスのメソッドを模倣し、疑似的に再現します。
    /// </summary>
    public class BattleUnitBufListDetailImitator
    {
		/// <summary>
		/// <see cref="BattleUnitBufListDetail"/>.AddNewKeywordBufInList(BufReadyType, KeywordBuf) メソッドの内部で実装されている、
		/// キーワード定義された状態を状態のインスタンスに変換する処理のみ模倣します。
		/// </summary>
		/// <param name="bufType">変換するキーワード定義された状態。</param>
		/// <param name="stack">状態の付与数。</param>
		/// <returns>キーワード定義に対応する状態のインスタンス。変換パターンが存在しない場合は null。</returns>
		public static BattleUnitBuf ToBattleUnitBuf(KeywordBuf bufType, int stack)
        {
			BattleUnitBuf buf = ToBattleUnitBuf(bufType);
			if (buf == null) { return buf; }

			buf.stack = stack;
			return buf;
        }

		/// <summary>
		/// <see cref="BattleUnitBufListDetail"/>.AddNewKeywordBufInList(BufReadyType, KeywordBuf) メソッドの内部で実装されている、
		/// キーワード定義された状態を状態のインスタンスに変換する処理のみ模倣します。
		/// </summary>
		/// <param name="bufType">変換するキーワード定義された状態。</param>
		/// <returns>キーワード定義に対応する状態のインスタンス。変換パターンが存在しない場合は null。</returns>
		private static BattleUnitBuf ToBattleUnitBuf(KeywordBuf bufType)
        {
			switch (bufType)
			{
				case KeywordBuf.Burn:
					return new BattleUnitBuf_burn();
				case KeywordBuf.Paralysis:
					return new BattleUnitBuf_paralysis();
				case KeywordBuf.Bleeding:
					return new BattleUnitBuf_bleeding();
				case KeywordBuf.Vulnerable:
					return new BattleUnitBuf_vulnerable();
				case KeywordBuf.Vulnerable_break:
					return new BattleUnitBuf_vulnerable_break();
				case KeywordBuf.Weak:
					return new BattleUnitBuf_weak();
				case KeywordBuf.Disarm:
					return new BattleUnitBuf_disarm();
				case KeywordBuf.Binding:
					return new BattleUnitBuf_binding();
				case KeywordBuf.Protection:
					return new BattleUnitBuf_protection();
				case KeywordBuf.BreakProtection:
					return new BattleUnitBuf_breakProtection();
				case KeywordBuf.Strength:
					return new BattleUnitBuf_strength();
				case KeywordBuf.Endurance:
					return new BattleUnitBuf_endurance();
				case KeywordBuf.Quickness:
					return new BattleUnitBuf_quickness();
				case KeywordBuf.Blurry:
					return new BattleUnitBuf_blurry();
				case KeywordBuf.Stun:
					return new BattleUnitBuf_stun();
				case KeywordBuf.DmgUp:
					return new BattleUnitBuf_dmgUp();
				case KeywordBuf.SlashPowerUp:
					return new BattleUnitBuf_slashPowerUp();
				case KeywordBuf.PenetratePowerUp:
					return new BattleUnitBuf_penetratePowerUp();
				case KeywordBuf.HitPowerUp:
					return new BattleUnitBuf_hitPowerUp();
				case KeywordBuf.DefensePowerUp:
					return new BattleUnitBuf_defensePowerUp();
				case KeywordBuf.WarpCharge:
					return new BattleUnitBuf_warpCharge();
				case KeywordBuf.Smoke:
					return new BattleUnitBuf_smoke();
				case KeywordBuf.NullifyPower:
					return new BattleUnitBuf_nullifyPower();
				case KeywordBuf.HalfPower:
					return new BattleUnitBuf_halfPower();
				case KeywordBuf.TeddyLove:
					return new PassiveAbility_105211.TeddyLove();
				case KeywordBuf.AllPowerUp:
					return new BattleUnitBuf_AllPowerUp();
				case KeywordBuf.TakeBpDmg:
					return new BattleUnitBuf_TakeBreakDamage2();
				case KeywordBuf.DecreaseSpeedTo1:
					return new BattleUnitBuf_DecreaseSpeedTo1();
				case KeywordBuf.Vibrate:
					return new BattleUnitBuf_viberate();
				case KeywordBuf.Decay:
					return new BattleUnitBuf_Decay();
				case KeywordBuf.BurnSpread:
					return new BattleUnitBuf_burnSpread();
				case KeywordBuf.RedMist:
					return new BattleUnitBuf_redMist();
				case KeywordBuf.Fairy:
					return new BattleUnitBuf_fairy();
				case KeywordBuf.CB_RedHoodTarget:
					return new BattleUnitBuf_RedHoodFinal_Mark();
				case KeywordBuf.CB_NothingSkin:
					return new BattleUnitBuf_Nothing_Skin();
				case KeywordBuf.CB_NothingMimic:
					return new BattleUnitBuf_Nothing_Mimic();
				case KeywordBuf.Resistance:
					return new BattleUnitBuf_Resistance();
				case KeywordBuf.HeavySmoke:
					return new BattleUnitBuf_HeavySmoke();
				case KeywordBuf.Arrest:
					return new BattleUnitBuf_Arrest();
				case KeywordBuf.ForbidRecovery:
					return new BattleUnitBuf_ForbidRecovery();
				case KeywordBuf.UpSurge:
					return new BattleUnitBuf_Upsurge();
				case KeywordBuf.KeterFinal_Eager:
					return new BattleUnitBuf_KeterFinal_Eager();
				case KeywordBuf.KeterFinal_FailLying:
					return new BattleUnitBuf_KeterFinal_FailLying();
				case KeywordBuf.KeterFinal_SuccessLying:
					return new BattleUnitBuf_KeterFinal_SuccessLying();
				case KeywordBuf.KeterFinal_ChangeCostAll:
					return new BattleUnitBuf_KeterFinal_ChangeCostAll();
				case KeywordBuf.KeterFinal_ChangeLibrarianHands:
					return new BattleUnitBuf_KeterFinal_ChangeLibrarianHands();
				case KeywordBuf.KeterFinal_DoubleEmotion:
					return new BattleUnitBuf_KeterFinal_DoubleEmotion();
				case KeywordBuf.KeterFinal_Light:
					return new BattleUnitBuf_KeterFinal_Light();
				case KeywordBuf.Nail:
					return new BattleUnitBuf_nail();
				case KeywordBuf.Alriune_Debuf:
					return new BattleUnitBuf_Alriune_Debuf();
				case KeywordBuf.CB_BigBadWolf_Scar:
					return new BattleUnitBuf_RedHoodFinal_Scar();
				default:
					return null;
			}
		}
	}
}

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PokeAbilities.Test.Helpers
{
    public static class Thrower
    {
        /// <summary>
        /// ラムダ式の式本体に指定したプロパティの値が null の場合、例外をスローします。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        public static void ThrowIfPropertyIsNull<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null) { throw new ArgumentNullException(nameof(propertyExpression)); }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("ラムダ式の本体がプロパティではありません。", nameof(propertyExpression));
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("ラムダ式の本体がプロパティではありません。", nameof(propertyExpression));
            }

            object value = (propertyExpression as ConstantExpression)?.Value;
            if (value == null)
            {
                throw new InvalidOperationException($"{property.Name} プロパティの値が null のためオブジェクトを生成できません。");
            }
        }

        /// <summary>
        /// 指定した敵キャラクター LOR ID に一致する敵キャラクターの XML 情報が見つからない場合、例外をスローします。
        /// </summary>
        /// <param name="enemyId"></param>
        public static void ThrowIfEnemyUnitNotFound(LorId enemyId)
        {
            if (Singleton<EnemyUnitClassInfoList>.Instance.GetData(enemyId) == null)
            {
                throw new InvalidOperationException($"EnemyUnitClassInfoList に ID '{enemyId.packageId}{enemyId.id}' の敵キャラクター情報が登録されていません。");
            }
        }

        /// <summary>
        /// 指定した case 値がその switch ステートメントで実装されていない旨の例外をスローします。
        /// </summary>
        /// <param name="caseValue"></param>
        public static void ThrowNotImplementedCase(string caseValue)
        {
            throw new NotImplementedException($"case {caseValue} に対応する処理は実装されていません。");
        }
    }
}

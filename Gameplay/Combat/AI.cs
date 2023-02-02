namespace BlueShadowMon
{
    public static class AI
    {

        private struct AiHelper
        {
            public AiHelper(Pet[] team)
            {
                leastHealth = team[0];
                mostPower = team[0];
                leastPower = team[0];
                mostArmor = team[0];
                leastArmor = team[0];
                foreach (Pet p in team.Skip(1))
                {
                    if (p[PetStat.Health] < leastHealth[PetStat.Health])
                        leastHealth = p;
                    if (p[PetStat.Power] > mostPower[PetStat.Power])
                        mostPower = p;
                    if (p[PetStat.Power] < leastPower[PetStat.Power])
                        leastPower = p;
                    if (p[PetStat.Armor] > mostArmor[PetStat.Armor])
                        mostArmor = p;
                    if (p[PetStat.Armor] < leastArmor[PetStat.Armor])
                        leastArmor = p;
                }
            }

            public Pet leastHealth;
            public Pet mostPower;
            public Pet leastPower;
            public Pet mostArmor;
            public Pet leastArmor;
        }

        /// <summary>
        /// AI make their move.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void Play(Pet activePet, List<Pet> allies, List<Pet> enemies)
        {
            if (activePet == null)
                throw new Exception("Current Pet is null!");


            // First, calculate th emost effective action to do
            // (deal damage? heal allies? apply buff?)
            AiHelper allyTeam = new(allies.ToArray());
            AiHelper enemyTeam = new(enemies.ToArray());
            Dictionary<string, int> scores = new()
            {
                { "Attack", 0},
                { "Heal", 0},
                { "PowerBuff", 0},
                { "PowerDebuff", 0},
            };


            // Attack with Health condition

            if (enemyTeam.leastPower[PetStat.Power] <= allyTeam.leastHealth[PetStat.Health])
            {
                scores["Attack"] += 5;
            }

            if (enemyTeam.mostPower[PetStat.Power] >= allyTeam.leastHealth[PetStat.Health])
            {
                scores["Attack"] += 20;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= allyTeam.leastHealth[PetStat.Health])
            {
                scores["Attack"] += 5;
            }


            // Attack with Armor Condition

            if (enemyTeam.mostPower[PetStat.Power] > allyTeam.mostArmor[PetStat.Armor])
            {
                scores["Attack"] += 10;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= allyTeam.mostArmor[PetStat.Armor])
            {
                scores["Attack"] += 5;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= allyTeam.leastArmor[PetStat.Armor])
            {
                scores["Attack"] += 5;
            }

            if (enemyTeam.mostPower[PetStat.Power] > allyTeam.leastArmor[PetStat.Armor])
            {
                scores["Attack"] += 20;
            }


            // Heal with Missing Health Condition

            if (enemyTeam.mostPower == enemyTeam.leastHealth && enemyTeam.mostPower.GetBonusStat(PetStat.Health, true) < -0.35f)
            {
                if (enemyTeam.mostPower == enemyTeam.leastArmor)
                {
                    scores["Heal"] += 30;
                }
            }
            if (enemyTeam.mostArmor == enemyTeam.leastHealth && enemyTeam.mostArmor.GetBonusStat(PetStat.Health, true) < -0.35f)
            {
                if (enemyTeam.mostArmor == enemyTeam.leastPower)
                {
                    scores["Heal"] += 15;
                }
            }


            // PowerDebuff with Power Condition

            if (enemyTeam.mostPower[PetStat.Power] <= allyTeam.mostPower[PetStat.Power])
            {
                scores["PowerDebuff"] += 20;
            }

            if (enemyTeam.mostPower[PetStat.Power] > allyTeam.mostPower[PetStat.Power])
            {
                scores["PowerDebuff"] += 10;
            }

            if (enemyTeam.mostPower[PetStat.Power] > allyTeam.leastPower[PetStat.Power])
            {
                scores["PowerDebuff"] += 5;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= allyTeam.leastPower[PetStat.Power])
            {
                scores["PowerDebuff"] += 10;
            }


            // PowerBuff with Armor Condition

            if (enemyTeam.mostPower[PetStat.Power] > allyTeam.mostArmor[PetStat.Power])
            {
                scores["PowerBuff"] += 10;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= enemyTeam.mostArmor[PetStat.Armor])
            {
                scores["PowerBuff"] += 20;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= enemyTeam.leastArmor[PetStat.Armor])
            {
                scores["PowerBuff"] += 20;
            }

            if (enemyTeam.mostPower[PetStat.Power] > enemyTeam.leastArmor[PetStat.Armor])
            {
                scores["PowerBuff"] += 5;
            }

            string bestAction = scores.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            // Then, select the most effective ability to use
            Ability ability;
            switch (bestAction)
            {
                case "Attack":
                    ability = Data.GetAbilityById[activePet.Abilities.Where(a => Data.GetAbilityById[a].Type == EffectType.Damage).First()];
                    break;
                case "Heal":
                    ability = Data.GetAbilityById[activePet.Abilities.Where(a => Data.GetAbilityById[a].Type == EffectType.Heal).First()];
                    break;
                case "PowerBuff":
                    ability = Data.GetAbilityById[activePet.Abilities.Where(a => Data.GetAbilityById[a].Type == EffectType.Buff).First()];
                    break;
                case "PowerDebuff":
                    ability = Data.GetAbilityById[activePet.Abilities.Where(a => Data.GetAbilityById[a].Type == EffectType.Debuff).First()];
                    break;
                default:
                    ability = Data.GetAbilityById[activePet.Abilities.First()];
                    break;
            }

            // Finally, chose targets and use it
            Pet[] targets;
            switch (ability.Type)
            {
                case EffectType.Damage:
                    targets = allies.ToArray();
                    break;
                case EffectType.Heal:
                    targets = enemies.ToArray();
                    break;
                case EffectType.Buff:
                    targets = enemies.ToArray();
                    break;
                case EffectType.Debuff:
                    targets = allies.ToArray();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (ability.CanTarget(EffectTarget.Self))
            {
                targets = new[] { activePet };
            }
            else if (!ability.CanTarget(EffectTarget.Multiple))
            {
                targets = new[] { targets.First() };
            }

            ability.UseOn(targets, activePet);
        }
    }
}

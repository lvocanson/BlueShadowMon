namespace BlueShadowMon
{
    public static class AI
    {

        private struct AiHelper
        {
            /// <summary>
            /// Calculate the most and least health, power and armor of a team.
            /// </summary>
            /// <param name="team"></param>
            public AiHelper(Pet[] team)
            {
                leastHealth = team[0];
                mostHealth = team[0];
                mostPower = team[0];
                leastPower = team[0];
                mostArmor = team[0];
                leastArmor = team[0];
                foreach (Pet p in team.Skip(1))
                {
                    if (p[PetStat.Health] < leastHealth[PetStat.Health])
                        leastHealth = p;
                    if (p[PetStat.Health] > mostHealth[PetStat.Health])
                        mostHealth = p;
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
            public Pet mostHealth;
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
            Dictionary<string, float> scores = new()
            {
                { "Attack", 0},
                { "Heal", 0},
                { "PowerBuff", 0},
                { "PowerDebuff", 0},
            };


            // Attack score

            scores["Attack"] += allyTeam.mostHealth[PetStat.Health] - allyTeam.leastHealth[PetStat.Health];

            if (enemyTeam.mostPower == allyTeam.leastHealth)
            {
                scores["Attack"] *= 1.5f;
            }

            // Heal score

            scores["Heal"] += (enemyTeam.mostHealth[PetStat.Health] - enemyTeam.leastHealth[PetStat.Health]) * 0.75f;

            if (enemyTeam.leastHealth.GetBonusStat(PetStat.Health, true) == 0)
            {
                scores["Heal"] = 0;
            }
            else if (enemyTeam.leastHealth.GetBonusStat(PetStat.Health, true) < -0.65f)
            {
                scores["Heal"] *= 2f;
            }
            else if (enemyTeam.leastHealth.GetBonusStat(PetStat.Health, true) > -0.30f)
            {
                scores["Heal"] *= 1.5f;
            }

            // PowerBuff

            if (activePet == enemyTeam.leastPower)
            {
                scores["PowerBuff"] += enemyTeam.mostPower[PetStat.Power];
            }
            else if (activePet == enemyTeam.mostPower)
            {
                scores["PowerBuff"] -= enemyTeam.leastPower[PetStat.Power];
            }

            // PowerDebuff

            if (allyTeam.mostPower.GetBonusStat(PetStat.Health) > -0.33f)
            {
                scores["PowerDebuff"] += allyTeam.mostPower[PetStat.Power];
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
            else if (!ability.CanTarget(EffectTarget.Multiple) || bestAction == "Heal")
            {
                targets = new[] { targets.First() };
            }

            ability.UseOn(targets, activePet);
        }
    }
}

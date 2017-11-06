using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using Microsoft.Practices.ObjectBuilder2;

namespace BusinessLogic.Services.Tools
{
    public class AutoSelectFunction
    {
        private const double lowerCoefficient = 0.005;

        private const double higherCoefficient = 0.006;

        private const int scale = 100;

        public static int Compare(Candidate candidate, Vacancy vacancy, int coefPrimary)
        {
            var func = 0;

            var primary = vacancy.VacancyPrimarySkill.Level * coefPrimary;

            if (candidate.CandidatePrimarySkill != null)
            {
                primary -= candidate.CandidatePrimarySkill.Level * coefPrimary;
            }

            primary = Math.Abs(primary);
            func += primary;

            int coefSec = 100 - coefPrimary;

            foreach (var vacancySecondarySkill in vacancy.VacancySecondarySkills)
            {
                var secondarySkillCandidate =
                    candidate.CandidateSecondarySkills?.FirstOrDefault(
                        canSkill => canSkill.TechSkill == vacancySecondarySkill.TechSkill);

                int secondaryFunc = vacancySecondarySkill.Level * coefSec;
                if (secondarySkillCandidate != null)
                {
                    secondaryFunc -= (secondarySkillCandidate.Level * coefSec);
                }
                func += Math.Abs(secondaryFunc);
            }

            return func;
        }

        public static double CompareByCandidate(Candidate candidate, Vacancy vacancy, int coefficient)
        {
            double pcoef = scale - coefficient;

            double scoef = coefficient;

            var res = 0.0;

            if (vacancy.VacancyPrimarySkill.Level > candidate.CandidatePrimarySkill.Level)
            {
                res += (vacancy.VacancyPrimarySkill.Level - candidate.CandidatePrimarySkill.Level) * lowerCoefficient * pcoef;
            }
            else
            {
                res += (candidate.CandidatePrimarySkill.Level - vacancy.VacancyPrimarySkill.Level) * higherCoefficient * pcoef;
            }

            if (candidate.CandidateSecondarySkills != null)
            {
                if (vacancy.VacancySecondarySkills != null)
                {
                    foreach (var cSkill in candidate.CandidateSecondarySkills)
                    {
                        var isFound = false;

                        foreach (var vSkill in vacancy.VacancySecondarySkills)
                        {
                            if (cSkill.TechSkill == vSkill.TechSkill)
                            {
                                if (vSkill.Level > cSkill.Level)
                                {
                                    res += (vSkill.Level - cSkill.Level) * scoef * lowerCoefficient;
                                }
                                else
                                {
                                    res += (cSkill.Level - vSkill.Level) * scoef * higherCoefficient;
                                }

                                isFound = true;

                                break;
                            }
                        }

                        if (!isFound)
                        {
                            res += scoef * higherCoefficient * candidate.CandidateSecondarySkills.Count;
                        }
                    }
                }
            }

            return res;
        }

        public static double CompareByVacancy(Vacancy vacancy, Candidate candidate, int coefficient)
        {
            double pcoef = scale - coefficient;

            double scoef = coefficient;

            var res = 0.0;

            if (candidate.CandidatePrimarySkill.Level > vacancy.VacancyPrimarySkill.Level)
            {
                res += (candidate.CandidatePrimarySkill.Level - vacancy.VacancyPrimarySkill.Level) * lowerCoefficient * pcoef;
            }
            else
            {
                res += (vacancy.VacancyPrimarySkill.Level - candidate.CandidatePrimarySkill.Level) * higherCoefficient * pcoef;
            }

            if (vacancy.VacancySecondarySkills != null)
            {
                if (candidate.CandidateSecondarySkills != null)
                {
                    foreach (var vSkill in vacancy.VacancySecondarySkills)
                    {
                        var isFound = false;

                        foreach (var cSkill in candidate.CandidateSecondarySkills)
                        {
                            if (vSkill.TechSkill == cSkill.TechSkill)
                            {
                                if (cSkill.Level > vSkill.Level)
                                {
                                    res += (cSkill.Level - vSkill.Level) * scoef * lowerCoefficient;
                                }
                                else
                                {
                                    res += (vSkill.Level - cSkill.Level) * scoef * higherCoefficient;
                                }

                                isFound = true;

                                break;
                            }
                        }

                        if (!isFound)
                        {
                            res += scoef * higherCoefficient * vacancy.VacancySecondarySkills.Count;
                        }
                    }
                }
            }

            return res;
        }
    }
}

using BusinessLogic.DBContext;
using BusinessLogic.Models;
using BusinessLogic.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Tools
{
    public static class VacancyFilter
    {
        public static bool Filter(VacancySearchModel vsm, Vacancy v)
        {
            if (vsm == null)
            {
                return true;
            }

            if (vsm.City != 0)
            {
                if (v.City != null)
                {
                    if (v.City.Value != vsm.City)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            if (vsm.Status != 0)
            {
                if (v.Status != vsm.Status)
                {
                    return false;
                }
            }

            if (vsm.PrimarySkill != null)
            {
                if (v.VacancyPrimarySkill != null)
                {
                    if (vsm.PrimarySkill.Id != 0)
                    {
                        if (v.VacancyPrimarySkill.TechSkill != vsm.PrimarySkill.Id)
                        {
                            return false;
                        }
                    }

                    if (vsm.PrimarySkill.Level != 0)
                    {
                        if (v.VacancyPrimarySkill.Level != vsm.PrimarySkill.Level)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            if (vsm.StartDate != null)
            {
                if (v.StartDate != null)
                {
                    if (DateTime.Compare(v.StartDate.Value, vsm.StartDate) < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            if (vsm.RequestDate != null)
            {
                if (v.RequestDate != null)
                {
                    if (DateTime.Compare(v.RequestDate.Value, vsm.RequestDate) < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            if (vsm.ProjectName != null)
            {
                if (v.ProjectName != null)
                {
                    var vacWords = v.ProjectName.Split(' ');

                    var searchWords = vsm.ProjectName.Split(' ');

                    if (LevenshteinDistanceCalculator.GetDistance(vsm.ProjectName, v.ProjectName) > LevenshteinDistanceCalculator.MaxAllowedDistance)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            if (vsm.VacancyName != null)
            {
                if (v.VacancyName != null)
                {
                    if (LevenshteinDistanceCalculator.GetDistance(v.VacancyName, vsm.VacancyName) > LevenshteinDistanceCalculator.MaxAllowedDistance)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}

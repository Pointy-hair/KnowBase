using BusinessLogic.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Helpers
{
    public class ObjectComparer
    {
        public bool IsEqual(CandidatePrimarySkill lhs, CandidatePrimarySkill rhs)
        {
            if (lhs == null)
            {
                if (rhs == null)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (rhs == null)
                {
                    return false;
                }
            }

            if (lhs.TechSkill != rhs.TechSkill)
            {
                return false;
            }

            if (lhs.Level != rhs.Level)
            {
                return false;
            }

            return true;
        }

        public bool IsEqual(VacancyPrimarySkill lhs, VacancyPrimarySkill rhs)
        {
            if (lhs == null)
            {
                if (rhs == null)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (rhs == null)
                {
                    return false;
                }
            }

            if (lhs.TechSkill != rhs.TechSkill)
            {
                return false;
            }

            if (lhs.Level != rhs.Level)
            {
                return false;
            }

            return true;
        }

        public bool IsEqual(CandidateSecondarySkill lhs, CandidateSecondarySkill rhs)
        {
            if (lhs.TechSkill != rhs.TechSkill)
            {
                return false;
            }

            if (lhs.Level != rhs.Level)
            {
                return false;
            }

            if (lhs == null)
            {
                if (rhs == null)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (rhs == null)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsEqual(VacancySecondarySkill lhs, VacancySecondarySkill rhs)
        {
            if (lhs.TechSkill != rhs.TechSkill)
            {
                return false;
            }

            if (lhs.Level != rhs.Level)
            {
                return false;
            }

            if (lhs == null)
            {
                if (rhs == null)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (rhs == null)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsEqual(Contact lhs, Contact rhs)
        {
            if (lhs == null)
            {
                if (rhs == null)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (rhs == null)
                {
                    return false;
                }
            }

            if (string.Compare(lhs.Email, rhs.Email, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (string.Compare(lhs.LinkedIn, rhs.LinkedIn, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (string.Compare(lhs.Phone, rhs.Phone, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            if (string.Compare(lhs.Skype, rhs.Skype, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            return true;
        }
    }
}

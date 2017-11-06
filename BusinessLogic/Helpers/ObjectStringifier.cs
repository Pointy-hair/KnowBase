using BusinessLogic.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Helpers
{
    public class ObjectStringifier
    {
        public string GetStr(CandidatePrimarySkill skill)
        {
            if (skill == null)
            {
                return null;
            }

            var result = skill.TechSkill.ToString() + " " + skill.Level.ToString();

            return result;
        }

        public string GetStr(VacancyPrimarySkill skill)
        {
            if (skill == null)
            {
                return null;
            }

            var result = skill.TechSkill.ToString() + " " + skill.Level.ToString();

            return result;
        }

        public string GetStr(CandidateSecondarySkill skill)
        {
            if (skill == null)
            {
                return null;
            }

            var result = skill.TechSkill.ToString() + " " + skill.Level.ToString();

            return result;
        }

        public string GetStr(VacancySecondarySkill skill)
        {
            if (skill == null)
            {
                return null;
            }

            var result = skill.TechSkill.ToString() + " " + skill.Level.ToString();

            return result;
        }

        public string GetStr(Contact contact)
        {
            if (contact == null)
            {
                return null;
            }

            var result = contact.Email + " " + contact.LinkedIn + " " + contact.Phone + " " + contact.Skype;

            return result;
        }
    }
}

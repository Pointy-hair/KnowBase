using BusinessLogic.DBContext;
using BusinessLogic.Models;

namespace BusinessLogic.Services.Tools
{
    public class CandidateFilter
    {
        public bool Filter(CandidateSearchModel searchOptions, Candidate candidate)
        {
            if (EqulasIdNullable(candidate.City, searchOptions.City))
            {
                return false;
            }

            if (EqulasIdNullable(candidate.CandidatePrimarySkill?.TechSkill, searchOptions.PrimarySkill))
            {
                return false;
            }

            if (EqualsId(candidate.HRM, searchOptions.HRM))
            {
                return false;
            }

            if (EqualsId(candidate.Status, searchOptions.Status))
            {
                return false;
            }

            if (EqulasIdNullable(candidate.CandidatePrimarySkill?.Level, searchOptions.Level))
            {
                return false;
            }

            if (ContainsString(searchOptions.LastNameRus, candidate.LastNameRus))
            {
                return false;
            }

            if (ContainsString(searchOptions.LastNameEng, candidate.LastNameEng))
            {
                return false;
            }

            if (ContainsString(searchOptions.Phone, candidate.Contact?.Phone))
            {
                return false;
            }

            if (ContainsString(searchOptions.Email, candidate.Contact?.Email))
            {
                return false;
            }

            if (ContainsString(searchOptions.Skype, candidate.Contact?.Skype))
            {
                return false;
            }

            return true;
        }

        private bool ContainsString(string candidate, string search)
        {
            if (candidate == null)
            {
                return false;
            }

            if (search == null)
            {
                return true;
            }

            return !candidate.Contains(search);
        }

        private bool EqualsId(int candidate, int? search)
        {
            if (search == null)
            {
                return false;
            }

            if (candidate == 0)
            {
                return true;
            }

            return candidate != search;
        }

        private bool EqulasIdNullable(int? candidate, int? search)
        {
            if (search == null)
            {
                return false;
            }

            if (candidate == 0)
            {
                return true;
            }

            return candidate != search;
        }

    }

}


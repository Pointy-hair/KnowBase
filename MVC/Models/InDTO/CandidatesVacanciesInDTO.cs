using System;
using System.Collections.Generic;

namespace MVC.Models.InDTO
{
    public class CandidatesVacanciesInDTO
    {
        public ICollection<int> Candidates { get; set; }

        public ICollection<int> Vacancies { get; set; }
    }
}
using System;

namespace MVC.Models
{
    public class TechSkillDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Nullable<int> Level { get; set; }

        public string Picture { get; set; }
    }
}
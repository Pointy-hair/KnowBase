//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessLogic.DBContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vacancy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vacancy()
        {
            this.Events = new HashSet<Event>();
            this.VacancySecondarySkills = new HashSet<VacancySecondarySkill>();
            this.Candidates = new HashSet<Candidate>();
        }
    
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public Nullable<int> City { get; set; }
        public int Status { get; set; }
        public string Link { get; set; }
        public Nullable<int> EngLevel { get; set; }
        public Nullable<int> Experience { get; set; }
        public string VacancyName { get; set; }
        public int HRM { get; set; }
        public Nullable<int> LastModifier { get; set; }
    
        public virtual City City1 { get; set; }
        public virtual EngLevel EngLevel1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Event> Events { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual VacancyStatus VacancyStatus { get; set; }
        public virtual VacancyPrimarySkill VacancyPrimarySkill { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancySecondarySkill> VacancySecondarySkills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}
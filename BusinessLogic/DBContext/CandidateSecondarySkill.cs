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
    
    public partial class CandidateSecondarySkill
    {
        public int Candidate { get; set; }
        public int TechSkill { get; set; }
        public int Level { get; set; }
    
        public virtual Candidate Candidate1 { get; set; }
        public virtual TechSkill TechSkill1 { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Repository.IRepositories;

namespace BusinessLogic.Repository.UnityRepositories
{
    public interface ITechInterviewRepository: ICreateRepository<TechInterview>, IReadRepository<TechInterview>,
        IUpdateRepository<TechInterview>
    {
    }
}

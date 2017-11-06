using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MVC.AutoMapperProfiles;
using MVC.Models;

namespace MVC.App_Start
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x => x.AddProfiles(new []
                {
                    typeof(DictionariesProfile),

                    typeof(UserProfile),

                    typeof(CandidateProfile),

                    typeof(VacancyProfile),

                    typeof(EventProfile),

                    typeof(InterviewProfile),

                    typeof(CalendarEventProfile),

                    typeof(CandidateElasticProfile),

                    typeof(VacancyElasticProfile)
                }
            ));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DBContext;
using BusinessLogic.Repository.AbstractRepositories;
using BusinessLogic.Repository.IRepositories;
using BusinessLogic.Repository.UnityRepositories;
using BusinessLogic.Unit_of_Work;

namespace BusinessLogic.Repository
{
    public class ContactRepository : Repository<Contact>, IContactRepository, IUpdateRepository<Contact>
    {
        public ContactRepository(IUnitOfWork context) : base(context)
        {
        }

        public Contact Update(Contact contact)
        {
            Contact cn = Read(contact.Id);
            cn.Email = contact.Email;
            cn.LinkedIn = contact.LinkedIn;
            cn.Phone = contact.Phone;
            cn.Skype = contact.Skype;

            return cn;
        }
    }
}

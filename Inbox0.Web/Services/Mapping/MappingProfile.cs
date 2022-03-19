using AutoMapper;
using Inbox0.Core.Models.Database;
using Inbox0.Web.Models.Forms;

namespace Inbox0.Web.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailForm, MailAccount>();
            CreateMap<MailAccount, EmailForm>();
        }
    }
}

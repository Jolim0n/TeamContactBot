using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamContactTelegramBot.Data.Users;

namespace TeamContactTelegramBot.Repositories.Users
{
    public class UserRepository : IUsersRepository
    {
        private IUsersUOW _uow;
        private UsersContext _context;
        //private static MapperConfiguration _conf;
        //private static IMapper _mapper;

        public UserRepository(IUsersUOW usersUOW, UsersContext context)
        {
            _uow = usersUOW;
            _context = context;
            //if (_conf == null)
            //{
            //    _conf = new MapperConfiguration(cfg =>
            //    {
            //        cfg.AddEXpressionMapping();
            //        cfg.CreateMap<Data.Users.Users, UsersDTO>.ReverseMap();
            //    });
            //    _conf.CompileMappings();
            //    _mapper = _conf.CreateMapper();
            //}
        }

        public async Task<List<Data.Users.Users>> GetListAsync()
        {
            using UsersContext context = new UsersContext();

            var usersInformation = context.DbUsers.ToList();

            if (usersInformation.Count == 0)
                return new List<Data.Users.Users>();

            return usersInformation.ToList();
        }

        public async Task<bool> CheckIfRegAsync(string log, string pass)
        {
            using UsersContext context = new UsersContext();

            var auto = context.DbUsers.FirstOrDefault(x => x.Login == log && x.Password == pass);

            if (auto == null)
                return false;
            else 
                return true;
        }

        public async Task<Data.Users.Users> AddAsync()
        {
            using UsersContext context = new UsersContext();

            var user = new Data.Users.Users { Code = "2", Name = "Test" };

            await context.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }
    }
}

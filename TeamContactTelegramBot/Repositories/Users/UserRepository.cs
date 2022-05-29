using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using TeamContactTelegramBot.Data.Users;
using TeamContactTelegramBot.Repositories.Users.Interfaces;

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

        public async Task<List<Data.Users.Users>> GetAllProgrammersAsync()
        {
            using UsersContext context = new UsersContext();

            var programmers = await context.DbUsers.Where(x => x.Role == 2).ToListAsync();

            if (programmers.Count == 0)
                return new List<Data.Users.Users>();

            return programmers;
        }

        public async Task<List<Data.Users.Users>> GetAllAnalystsAsync()
        {
            using UsersContext context = new UsersContext();

            var analysts = await context.DbUsers.Where(x => x.Role == 3).ToListAsync();

            if (analysts.Count == 0)
                return new List<Data.Users.Users>();

            return analysts;
        }

        public async Task<Data.Users.Users> GetAsync(int userRcd)
        {
            using UsersContext context = new UsersContext();

            var user = await context.DbUsers.FirstOrDefaultAsync(x => x.UsersId == userRcd);

            if (user == null)
                return new Data.Users.Users();

            return user;
        }

        public async Task<Data.Users.Users> AddAsync(Data.Users.Users user)
        {
            using UsersContext context = new UsersContext();

            await context.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<List<Data.Users.Users>> GetListAsync()
        {
            using UsersContext context = new UsersContext();

            var usersInformation = await context.DbUsers.ToListAsync();

            if (usersInformation.Count == 0)
                return new List<Data.Users.Users>();

            return usersInformation.ToList();
        }

        public async Task<(bool, byte)> CheckIfRegAsync(string log, string pass)
        {
            using UsersContext context = new UsersContext();

            var user = await context.DbUsers.FirstOrDefaultAsync(x => x.Login == log && x.Password == pass);

            if (user == null)
                return (false, 0);
            else 
                return (true, user.Role);
        }
    }
}

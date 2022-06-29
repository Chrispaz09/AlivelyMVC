using AlivelyMVC.Models;
using AlivelyMVC.ViewModels;
using AutoMapper;
using Task = AlivelyMVC.Models.Task;

namespace AlivelyMVC.Profiles
{
    public class AlivelyProfile : Profile
    {
        public AlivelyProfile()
        {
            CreateMap<User, UserViewModel>();

            CreateMap<UserViewModel, User>();

            CreateMap<Task, TaskViewModel>();

            CreateMap< TaskViewModel, Task>();

            CreateMap<SMARTGoal, SMARTGoalViewModel>();

            CreateMap<SMARTGoalViewModel , SMARTGoal>();
        }
    }
}

using AutoMapper;
using BudgetAPI.Models;
using BudgetAPI.DTOs;

namespace BudgetAPI.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // User Mappings
            CreateMap<User, UserResponseDto>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserLoginDto, User>();
            CreateMap<UserLoginResponseDto, User>();

            // Expense Mappings
            CreateMap<ExpenseCreateDto, Expense>();
            CreateMap<ExpenseUpdateDto, Expense>(); 
            CreateMap<Expense, ExpenseResponseDto>();

            // Budget Mappings
            CreateMap<BudgetCreateDto, Budget>();
            CreateMap<BudgetUpdateDto, Budget>();
            CreateMap<Budget, BudgetResponseDto>();
        }
    }
}

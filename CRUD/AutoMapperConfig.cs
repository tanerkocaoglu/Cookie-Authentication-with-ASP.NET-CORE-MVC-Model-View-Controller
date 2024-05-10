using AutoMapper;
using CRUD.Entities;
using CRUD.Models;

namespace CRUD
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			CreateMap<User, UserModel>().ReverseMap();
			CreateMap<User, CreateUserViewModel>().ReverseMap();
			
		}

	}
}

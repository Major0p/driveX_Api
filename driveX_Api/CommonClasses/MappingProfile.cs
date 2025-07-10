using AutoMapper;
using driveX_Api.Models.User;
using driveX_Api.DTOs.SignUp;

namespace driveX_Api.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpRequestDto, UserInfo>();
            CreateMap<UserInfo, SignUpRequestDto>();

        }
    }
}

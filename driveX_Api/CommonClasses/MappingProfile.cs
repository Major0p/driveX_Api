using AutoMapper;
using driveX_Api.Models.User;
using driveX_Api.DTOs.SignUp;
using driveX_Api.Models.File;
using driveX_Api.DTOs.File;

namespace driveX_Api.CommonClasses
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpRequestDto, UserInfo>();
            CreateMap<UserInfo, SignUpRequestDto>();
            CreateMap<Details, DetailsDto>();
            CreateMap<DetailsDto, Details>();
        }
    }
}

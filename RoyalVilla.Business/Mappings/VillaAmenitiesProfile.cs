namespace RoyalVilla_API.Mappings;

public class VillaAmenitiesProfile:Profile
{
    public VillaAmenitiesProfile()
    {
        CreateMap<VillaAmenities, VillaAmenitiesDto>()
            .ForMember(dest => dest.VillaName,
                opt => opt.MapFrom(src => src.Villa != null ? src.Villa.Name : null));

        CreateMap<CreateVillaAmenitiesDto, VillaAmenities>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Villa, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<UpdateVillaAmenitiesDto, VillaAmenities>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Villa, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<VillaAmenitiesDto, VillaAmenities>()
            .ForMember(dest => dest.Villa, opt => opt.Ignore())
            .ForMember(dest => dest.VillaId, opt => opt.Ignore());
    }
}

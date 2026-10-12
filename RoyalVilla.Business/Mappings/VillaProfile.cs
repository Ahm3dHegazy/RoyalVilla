namespace RoyalVilla_API.Mappings;

public class VillaProfile:Profile
{
    public VillaProfile()
    {
        CreateMap<CreateVillaDto, Villa>().ReverseMap();
        CreateMap<UpdateVillaDto, Villa>().ReverseMap();
        CreateMap<Villa, VillaDto>().ReverseMap();
    }
}
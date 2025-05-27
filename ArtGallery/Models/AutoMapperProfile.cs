using AutoMapper;

namespace ArtGallery.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //--Artifact Mapping
            CreateMap<ArtifactDTO, Artifact>()
                .ForMember(dest => dest.artifact_id, opt => opt.Ignore())
                .ForMember(dest => dest.created_date, opt => opt.Ignore())
                .ForMember(dest => dest.modified_date, opt => opt.Ignore())
                //Ignore FK mapping to manually st them later.
                .ForMember(dest => dest.artists, opt => opt.Ignore())
                .ForMember(dest => dest.tags, opt => opt.Ignore());

            //--Artist Mapping
            CreateMap<ArtistDTO, Artist>()
                .ForMember(dest => dest.artist_id, opt => opt.Ignore())
                .ForMember(dest => dest.created_date, opt => opt.Ignore())
                .ForMember(dest => dest.modified_date, opt => opt.Ignore())
                //Ignore FK mapping to manually set them later.
                .ForMember(dest => dest.artifacts, opt => opt.Ignore());

            //--Tag Mapping
            CreateMap<TagDTO, Tag>()
                .ForMember(dest => dest.tag_id, opt => opt.Ignore())
                //Ignore FK mapping to manually set them later.
                .ForMember(dest => dest.artifacts, opt => opt.Ignore());
        }
    }
}

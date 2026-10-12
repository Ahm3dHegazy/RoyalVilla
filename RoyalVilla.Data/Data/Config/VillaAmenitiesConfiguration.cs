namespace RoyalVilla_API.Data.Config;

public class VillaAmenitiesConfiguration:IEntityTypeConfiguration<VillaAmenities>
{
    public void Configure(EntityTypeBuilder<VillaAmenities> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasColumnType("VARCHAR(100)")
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasColumnType("VARCHAR(300)")
            .IsRequired();

        builder.HasOne(x => x.Villa)
            .WithMany(x => x.VillaAmenities)
            .HasForeignKey(x => x.VillaId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.ToTable("VillaAmenities");

        builder.HasData(LoadVillaAmenities());
    }

    private static VillaAmenities[] LoadVillaAmenities()
    {
        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return
        [
            // Villa 1
            new VillaAmenities { Id = 1, VillaId = 1, Name = "Private Pool", Description = "Heated outdoor pool with a sun deck and loungers.", CreatedAt = seedDate, UpdatedAt = seedDate },
            new VillaAmenities { Id = 2, VillaId = 1, Name = "Free Wi-Fi", Description = "High-speed fiber internet available throughout the villa.", CreatedAt = seedDate, UpdatedAt = seedDate },
            new VillaAmenities { Id = 3, VillaId = 1, Name = "Air Conditioning", Description = "Central air conditioning in all bedrooms and living areas.", CreatedAt = seedDate, UpdatedAt = seedDate },

            // Villa 2
            new VillaAmenities { Id = 4, VillaId = 2, Name = "Sea View", Description = "Panoramic sea view from the terrace and master bedroom.", CreatedAt = seedDate, UpdatedAt = seedDate },
            new VillaAmenities { Id = 5, VillaId = 2, Name = "BBQ Area", Description = "Outdoor barbecue area with seating for eight guests.", CreatedAt = seedDate, UpdatedAt = seedDate },
            new VillaAmenities { Id = 6, VillaId = 2, Name = "Private Parking", Description = "Covered parking space for up to three cars.", CreatedAt = seedDate, UpdatedAt = seedDate },

            // Villa 3
            new VillaAmenities { Id = 7, VillaId = 3, Name = "Gym", Description = "Fully equipped private gym with cardio and weight machines.", CreatedAt = seedDate, UpdatedAt = seedDate },
            new VillaAmenities { Id = 8, VillaId = 3, Name = "Garden", Description = "Landscaped garden with a seating area and fire pit.", CreatedAt = seedDate, UpdatedAt = seedDate },
            new VillaAmenities { Id = 9, VillaId = 3, Name = "Smart TV", Description = "55-inch smart TV with streaming apps in the living room.", CreatedAt = seedDate, UpdatedAt = seedDate }
        ];
    }
}
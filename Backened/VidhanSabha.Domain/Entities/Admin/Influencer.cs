using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin;

public class Tbl_Influencer
{
    public int Id { get; private set; }
    public int BoothId { get; private set; }
    public string Name { get; private set; }
    public int CategoryId { get; private set; }
    public int CastId { get; private set; }
    public string Mobile { get; private set; }
    public string Description { get; private set; }
    public bool Status { get; private set; } = true;

    // Multiple villages — ek Influencer ke multiple villages
    private readonly List<Tbl_InfluencerVillage> _villages = new();
    public IReadOnlyCollection<Tbl_InfluencerVillage> Villages => _villages.AsReadOnly();

    // Navigation
    public Tbl_Booth Booth { get; private set; } = null!;
    public Tbl_Cast? Cast { get; private set; }
    public Tbl_Category? Category { get; private set; }
    private Tbl_Influencer() { }

    public static Tbl_Influencer Create(
        int boothId,
        string name,
        int categoryId,
        int castId,
        string mobile,
        string description,
        List<int> villageIds)
    {
        var entity = new Tbl_Influencer
        {
            BoothId = boothId,
            Name = name,
            CategoryId = categoryId,
            CastId = castId,
            Mobile = mobile,
            Description = description,
        };
        entity.SetVillages(villageIds);
        return entity;
    }

    public void Update(
        int boothId,
        string name,
        int categoryId,
        int castId,
        string mobile,
        string description,
        List<int> villageIds)
    {
        BoothId = boothId;
        Name = name;
        CategoryId = categoryId;
        CastId = castId;
        Mobile = mobile;
        Description = description;
        SetVillages(villageIds);
    }

    private void SetVillages(List<int> villageIds)
    {
        var toRemove = _villages
            .Where(id => !villageIds.Contains(id.VillageId))
            .ToList();

        foreach (var id in toRemove)
            _villages.Remove(id); // ✅ EF sees this as DELETE
        var existingIds = _villages.Select(id => id.VillageId).ToHashSet();

        foreach (var vid in villageIds.Where(id => !existingIds.Contains(id)))
            _villages.Add(Tbl_InfluencerVillage.Create(vid)); // ✅ EF sees this as INSERT
    }

    public void Delete()
    {
        Status = false;

        foreach (var village in _villages)
        {
            village.Delete();
        }
    }
}

public class Tbl_InfluencerVillage
{
    public int Id { get; private set; }
    public int InfluencerId { get; private set; }
    public int VillageId { get; private set; }
    public bool Status { get; private set; } = true;

    public Tbl_Influencer Influencer { get; set; } = null!;
    public Tbl_Village? Village { get; set; } = null!;
    private Tbl_InfluencerVillage() { }
    public static Tbl_InfluencerVillage Create(int villageId) => new()
    {
        VillageId = villageId
    };
    public void Delete()
    {
        Status = false;
    }
}

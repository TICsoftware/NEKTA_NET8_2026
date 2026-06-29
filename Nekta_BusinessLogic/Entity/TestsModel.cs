using System.ComponentModel.DataAnnotations;

namespace Nekta_BusinessLogic.Entity;

public class Test
{
    public int TestId { get; set; }
    public string? Title { get; set; }
    public string? Specimen { get; set; }
    public string? TypeTest { get; set; }
    public string? Organ { get; set; }
    public string? Department { get; set; }
    public int covered_under_NABL { get; set; }

}

public class TestDirectory
{
    public Test_Detail? SearchTest { get; set; }
    public List<Options_List>? Specimen { get; set; }
    public List<Options_List>? TypeTest { get; set; }
    public List<Options_List>? Organ { get; set; }
    public List<Options_List>? Department { get; set; }
    public List<Options_List>? NABL_option { get; set; }
    public List<Test>? Tests { get; set; }

    public int NABL_option_1_count { get; set; }
    public int NABL_option_2_count { get; set; }

    public int TotalCount { get; set; }

    public ContentViewModel? Content { get; set; }
}

public class Test_Detail
{
    public string? Title { get; set; }
    public int? Specimen_Id { get; set; }
    public int? TypeTestId { get; set; }
    public int? OrganId { get; set; }
    public int? DepartmentId { get; set; }
    public int Is_covered_under_NABL { get; set; }
}
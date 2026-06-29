using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nekta_BusinessLogic.Entity
{
    public class AcademyResearchModel
    {
        public ContentViewModel AR_Content { get; set; }
        public List<ComponentGroup> AR_Components { get; set; } = new List<ComponentGroup>();

        public ContentViewModel Content { get; set; }
        public List<ComponentGroup> Components { get; set; } = new List<ComponentGroup>();
        public List<AnitaBorgesViewModel> AnitaBorgesList { get; set; } = new();
        public List<CommonViewModel> OpinionList { get; set; } = new();
        public List<CommonViewModel> TeacherAlwaysList { get; set; } = new();
        public List<MentorshipMomentsViewModel> MentorshipMomentsList { get; set; } = new();
        public List<CommonViewModel> LegacyList { get; set; } = new();


        public List<AcademyResearchCommonModel> ProgrammeOverviewList { get; set; } = new();
        public List<AcademyResearchCommonModel> FellowshipsWithUsList { get; set; } = new();
        public List<AcademyResearchCommonModel> EligibilityList { get; set; } = new();
        public List<AcademyResearchCommonModel> LearningList { get; set; } = new();
        public List<AcademyResearchCommonModel> ApplicationProcessList { get; set; } = new();
        public List<AcademyResearchCommonModel> OutcomeList { get; set; } = new();
        public List<AcademyResearchCommonModel> PartnerWithUsList { get; set; } = new();

        public List<AcademyResearchCommonModel> RecentPublicationsList { get; set; } = new();
        public int TotalCount { get; set; }

    }


    public class AcademyResearchCommonModel
    {
        public string GroupId { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string HmpgIntro { get; set; }
        public string DisplayTitle { get; set; }
        public string Content { get; set; }
        public string ComponentThumbnail { get; set; }
        public string ComponentThumbnailAltText { get; set; }
        public string ThumbnailImage { get; set; }
        public string ThumbnailAltText { get; set; }
        public string Url { get; set; }
        public string Url_Text { get; set; }
        public int Sequence { get; set; }
        public int IsBlock { get; set; }
        public string Video_path { get; set; }
        public string Video_poster { get; set; }
        public string Icon_Image { get; set; }
        public string Popup_Content { get; set; }
        public string Popup_Display_Title { get; set; }
        public string Section_title { get; set; }
    }

    public class AnitaBorgesViewModel
    {
        public string GroupId { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public string LHSIntro { get; set; }
        public string ThumbnailImage { get; set; }
        public string ThumbnailImageAlt { get; set; }
        public string RHSImage { get; set; }
        public string RHSImageAlt { get; set; }
        public int Sequence { get; set; }
    }

    public class CommonViewModel
    {
        public string GroupId { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string Content { get; set; }
        public string ThumbnailImage { get; set; }
        public string ThumbnailImageAlt { get; set; }
        public int Sequence { get; set; }
    }

    public class MentorshipMomentsViewModel
    {
        public string GroupId { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string DisplayTitle { get; set; }
        public string Content { get; set; }
        public string ThumbnailImage { get; set; }
        public string ThumbnailImageAlt { get; set; }
        public string BlockThumbnail { get; set; }
        public string BlockThumbnailAlt { get; set; }
        public string Url { get; set; }
        public int Sequence { get; set; }
        public int IsBlock { get; set; }
    }


}
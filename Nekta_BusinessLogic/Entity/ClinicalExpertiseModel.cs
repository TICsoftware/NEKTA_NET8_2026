using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nekta_BusinessLogic.Entity
{
    public class ClinicalExpertiseModel
    {
        public ContentViewModel? Content { get; set; }
        public List<ComponentGroup> Components { get; set; } = new();

        public List<ClinicalExpertiseCommonModel> ReviewCases_List { get; set; } = new();
        public List<ClinicalExpertiseCommonModel> RequestConsultation_List { get; set; } = new();
        public List<ClinicalExpertiseCommonModel> FromConsultation_List { get; set; } = new();
        public List<ClinicalExpertiseCommonModel> YourCase_List { get; set; } = new();
        public List<ClinicalExpertiseCommonModel> OncopathologyExperts_List { get; set; } = new();
        public List<ClinicalExpertiseCommonModel> RequestConsult_List { get; set; } = new();
        public List<ClinicalExpertiseCommonModel> FAQ_List { get; set; } = new();
    }



    public class ClinicalExpertiseCommonModel
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
        public string Component_Background_image { get; set; }
    }




}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nekta_BusinessLogic.Entity
{
    public class UpdatesModel
    {
        public ContentViewModel? Content { get; set; }
        public List<ComponentGroup> Components { get; set; } = new();

        public List<Common_List> Updates_Section_List { get; set; } = new();
        public List<Common_List> Annual_Report_Article_List { get; set; } = new();
        public List<Common_List> Media_Article_List { get; set; } = new();

        public int AnnualReportCount { get; set; } = new();
        public int MediaCount { get; set; } = new();
        public List<FinancialYearModel> AnnualReport_Year_List { get; set; } = new();

        public List<Common_List> Related_Media_Article_List { get; set; } = new();

        public List<MediaCommonModel> MediaSlider_List { get; set; } = new();

    }


    public class AnnualSearch
    {
        public List<Common_List> Annual_Report_List { get; set; } = new();
        public int AnnualtotalCount { get; set; } = new();
    }

    public class Common_List
    {
        public int ContId { get; set; }

        public int cont_parent_id { get; set; }
        public string ContTitle { get; set; }
        public string Cont_intro { get; set; }
        public string Cont_hmpg_intro { get; set; }
        public string PageName { get; set; }
        public string MastheadImage { get; set; }
        public string MobileMastheadImage { get; set; }

        public string Content { get; set; }

        public string BreadcrumPath { get; set; }
        public int Template_Master_ID { get; set; }

        public string cont_metadesc { get; set; }
        public string cont_metatag { get; set; }
        public string cont_window_title { get; set; }

        public string Thumbnail_img { get; set; }
        public string Thumbnail_img_alt_text { get; set; }

        public string Url { get; set; }
        public string Url_target { get; set; }
        public string external_url { get; set; }

        public string filepath { get; set; }

        public int Sequence { get; set; }
        public DateTime? cont_displaydate { get; set; }


    }


    public class FinancialYearModel
    {
        public string FinancialYear { get; set; }
    }


 

    public class MediaCommonModel
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
        public string Component_Icon_Image { get; set; }
        public string Popup_Content { get; set; }
        public string Popup_Display_Title { get; set; }
        public string Section_title { get; set; }

        public string Component_right_image { get; set; }
        public string Component_Right_image_alt { get; set; }

    }





}
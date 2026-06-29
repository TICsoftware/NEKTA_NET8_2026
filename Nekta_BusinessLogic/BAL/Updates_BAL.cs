using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Nekta_BusinessLogic.DAL;
using Nekta_BusinessLogic.Entity;
using Nekta_BusinessLogic;

namespace Nekta_BusinessLogic.BAL
{
    public class Updates_BAL : Updates_DAL
    {
        private readonly IConfiguration _configuration;
        public Updates_BAL(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public UpdatesModel GetContent_BAL(string pagename, int languageId, int geographyId)
        {
            var model = new UpdatesModel();

            var ds = GetContent_DAL(pagename, languageId, geographyId);

            if (ds.Tables.Count == 0)
                return model;

            // Content
            if (ds.Tables[0].Rows.Count > 0)
            {
                model.Content = MapContent(ds.Tables[0].Rows[0]);
            }

            // Updates Section
            if (ds.Tables[1].Rows.Count > 0)
            {
                model.Updates_Section_List = MapCommonList(ds.Tables[1]);
            }

            // Annual Report
            if (ds.Tables[2].Rows.Count > 0)
            {
                model.Annual_Report_Article_List = MapCommonList(ds.Tables[2]);

                if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                {
                    model.AnnualReportCount = Convert.ToInt32(ds.Tables[3].Rows[0]["TotalCount"]);
                }

                if (ds.Tables.Count > 6 && ds.Tables[6].Rows.Count > 0)
                {
                    model.AnnualReport_Year_List = ds.Tables[6]
                        .AsEnumerable()
                        .Select(row => new FinancialYearModel
                        {
                            FinancialYear = row["FinancialYear"]?.ToString()
                        }).ToList();
                }
            }

            // Media Articles
            if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
            {
                model.Media_Article_List = MapCommonList(ds.Tables[4]);

                if (ds.Tables.Count > 5 && ds.Tables[5].Rows.Count > 0)
                {
                    model.MediaCount = Convert.ToInt32(ds.Tables[5].Rows[0]["TotalCount"]);
                }
            }

            return model;
        }


        public UpdatesModel GetMediaInside_BAL(string pagename, int languageId, int geographyId)
        {
            var model = new UpdatesModel();

            var ds = GetContent_DAL(pagename, languageId, geographyId);

            if (ds.Tables.Count == 0)
                return model;

            // Content
            if (ds.Tables[0].Rows.Count > 0)
            {
                model.Content = MapContent(ds.Tables[0].Rows[0]);
            }

            if (ds.Tables[1].Rows.Count > 1)
            {
                model.Related_Media_Article_List = MapCommonList(ds.Tables[1]);
            }

            if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
            {
                var groupedData = GetGroupedComponents(ds.Tables[2]);
                model.Components = groupedData;

                model.MediaSlider_List = MapComponentCommon(groupedData, 1);
            }

            return model;
        }



        public List<Common_List> Updates_Articles_Load_BAL(int Cont_id, int PageNumber, int PageSize)
        {
            var obj = new List<Common_List>();

            var ds = Get_UpdatesArticles_load_DAL(Cont_id, PageNumber, PageSize);

            if (ds.Tables[0].Rows.Count > 0)
            {
                obj = MapCommonList(ds.Tables[0]);
            }


            return obj;
        }


        public AnnualSearch Get_AnnualReport_ByYear_BAL(int Cont_id, int FinancialYear = 0, int PageNumber = 1, int PageSize = 10)
        {
            var obj = new AnnualSearch();

            var ds = Get_AnnualReport_ByYear_DAL(Cont_id, FinancialYear, PageNumber, PageSize);

            // Data List
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                obj.Annual_Report_List = MapCommonList(ds.Tables[0]);
            }
            else
            {
                obj.Annual_Report_List = new List<Common_List>();
            }

            // Total Count
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                obj.AnnualtotalCount = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]);
            }

            return obj;
        }

        private List<Common_List> MapCommonList(DataTable table)
        {
            return table.AsEnumerable()
                .Select(row => new Common_List
                {
                    ContId = row.Field<int?>("cont_id") ?? 0,
                    cont_parent_id = row.Field<int?>("cont_parent_id") ?? 0,

                    ContTitle = !string.IsNullOrWhiteSpace(row["cont_hmpg_title"]?.ToString())
                        ? row["cont_hmpg_title"].ToString()
                        : row["cont_title"].ToString(),

                    Cont_intro = row["cont_intro"]?.ToString(),
                    Cont_hmpg_intro = row["cont_hmpg_intro"]?.ToString(),
                    PageName = row["cont_pagename"]?.ToString(),

                    Thumbnail_img = row["Hmpg_thumbnail"]?.ToString(),
                    Thumbnail_img_alt_text = row["Hmpg_thumbnail_alt_text"]?.ToString(),

                    external_url = row["cont_external_url"]?.ToString(),
                    filepath = row["MediafilePath"]?.ToString(),

                    Url = GetArticleUrl(row),
                    Url_target = GetArticleTarget(row),
                    cont_displaydate = table.Columns.Contains("cont_displaydate")
                        ? row.Field<DateTime?>("cont_displaydate")
                        : null,

                    Sequence = row.Table.Columns.Contains("cont_sequence")
                        ? row.Field<int?>("cont_sequence") ?? 0
                        : 0
                })
                .ToList();
        }


        private string GetArticleUrl(DataRow row)
        {
            var externalUrl = row["cont_external_url"]?.ToString();
            var mediaFile = row["MediafilePath"]?.ToString();
            var pageUrl = row["pageurl"]?.ToString();

            if (!string.IsNullOrWhiteSpace(externalUrl))
                return externalUrl;

            if (!string.IsNullOrWhiteSpace(mediaFile))
                return mediaFile;

            return pageUrl?.Replace("/global", "");
        }


        private string GetArticleTarget(DataRow row)
        {
            var externalUrl = row["cont_external_url"]?.ToString();
            var mediaFile = row["MediafilePath"]?.ToString();
            var pageUrlTarget = row["pageurl"]?.ToString();

            // External URL or File => Open in new tab
            if (!string.IsNullOrWhiteSpace(externalUrl) ||
                !string.IsNullOrWhiteSpace(mediaFile))
            {
                return "_blank";
            }

            // Check pageurl value
            if (string.Equals(pageUrlTarget, "self", StringComparison.OrdinalIgnoreCase))
            {
                return "_blank";
            }

            return "_self";
        }

        private ContentViewModel MapContent(DataRow row)
        {
            string baseurl = _configuration["AppSettings:BaseUrl"]?.TrimEnd('/') ?? "";
            var Image = row.Field<string>("masthead_image_path") ?? "";
            var canUrl = row.Field<string>("pageurl") ?? "";

            return new ContentViewModel
            {
                ContId = row.Field<int?>("cont_id") ?? 0,
                ContTitle = row.Field<string>("cont_title") ?? "",
                Cont_intro = row.Field<string>("cont_intro") ?? "",
                Cont_hmpg_intro = row.Field<string>("cont_hmpg_intro") ?? "",
                Content = row.Field<string>("content") ?? "",
                PageName = row.Field<string>("cont_pagename") ?? "",
                MastheadImage = row.Field<string>("masthead_image_path") ?? "",
                MobileMastheadImage = row.Field<string>("mobile_masthead_image_path") ?? "",
                Template_Master_ID = row.Field<int?>("Template_Master_ID") ?? 0,
                BreadcrumPath = row.Field<string>("BreadcrumPath") ?? "",
                cont_window_title = row.Field<string>("cont_window_title") ?? "",
                cont_metadesc = row.Field<string>("cont_metadesc") ?? "",
                cont_metatag = row.Field<string>("cont_metatag") ?? "",
                Hmpg_thumbnail = row.Field<string>("Hmpg_thumbnail") ?? "",
                Hmpg_thumbnail_alt_text = row.Field<string>("Hmpg_thumbnail_alt_text") ?? "",
                Masthead_image_Alt_text = row.Field<string>("Masthead_alt_text") ?? "",
                CanonicalUrl = Config_Application_Website.GetMetaUrl(baseurl, canUrl),
                cont_meta_image = Config_Application_Website.GetMetaUrl(baseurl, Image),
                cont_displaydate = row.Field<DateTime?>("cont_displaydate"),
            };
        }


        private List<ComponentGroup> GetGroupedComponents(DataTable table)
        {
            return table.AsEnumerable()
            .GroupBy(x => new
            {
                GroupId = x.Field<Guid>("context_group_id").ToString(),
                Sequence = Convert.ToInt32(x["component_sequence"]),
                IsBlock = Convert.ToInt32(x["is_block"])
            })
                .Select(group => new ComponentGroup
                {
                    GroupId = group.Key.GroupId,

                    Fields = group.Select(row => new ComponentField
                    {
                        GroupId = group.Key.GroupId,
                        FieldName = row.Field<string>("field_name") ?? "",
                        FieldKey = row.Field<string>("name_key") ?? "",
                        FieldValue = row.Field<string>("field_value") ?? "",
                        ImagePath = row.Field<string>("component_image_path") ?? "",
                        sequence = group.Key.Sequence,
                        IsBlock = group.Key.IsBlock,
                    }).ToList()
                })
                .OrderBy(g => g.Fields.FirstOrDefault()?.sequence ?? 0).ToList();
        }



        private List<MediaCommonModel> MapComponentCommon(List<ComponentGroup> data, int sequence)
        {
            return Config_Application_Website.MapComponent(data, sequence, (group, dict) => new MediaCommonModel
            {
                GroupId = group.GroupId,
                Title = Config_Application_Website.GetValue(dict, "Title", "Component Title"),
                Intro = Config_Application_Website.GetValue(dict, "Intro", "Component Intro"),
                HmpgIntro = Config_Application_Website.GetValue(dict, "Landing intro", "Component Landing intro"),
                DisplayTitle = Config_Application_Website.GetValue(dict, "Component Display title"),
                Content = Config_Application_Website.GetValue(dict, "Content", "Component Content"),
                ComponentThumbnail = Config_Application_Website.GetPath(group, "Component Thumbnail image"),
                ComponentThumbnailAltText = Config_Application_Website.GetValue(dict, "Component thumbnail image alt"),
                ThumbnailImage = Config_Application_Website.GetPath(group, "Thumbnail Image"),
                ThumbnailAltText = Config_Application_Website.GetValue(dict, "thumbnail image alt"),
                Url = Config_Application_Website.GetValue(dict, "Url", "Component URL"),
                Url_Text = Config_Application_Website.GetValue(dict, "Url text", "Component URL text"),
                //Url = Config_Application_Website.GetValue(dict, "Url"),
                //Url_Text = Config_Application_Website.GetValue(dict, "Url text"),
                Video_path = Config_Application_Website.GetPath(group, "Video"),
                Video_poster = Config_Application_Website.GetPath(group, "Video poster"),
                Icon_Image = Config_Application_Website.GetPath(group, "Icon image"),
                Component_Icon_Image = Config_Application_Website.GetPath(group, "component icon image"),
                Popup_Content = Config_Application_Website.GetValue(dict, "popup content"),
                Popup_Display_Title = Config_Application_Website.GetValue(dict, "Popup Display title"),
                Sequence = Config_Application_Website.GetIntValue(dict, "Sequence"),
                //Sequence = group.Fields.First().sequence,
                IsBlock = group.Fields.First().IsBlock,
                Section_title = Config_Application_Website.GetValue(dict, "Block Title", "Section title"),
                Component_right_image = Config_Application_Website.GetPath(group, "Component right image"),
                Component_Right_image_alt = Config_Application_Website.GetValue(dict, "Component Right image alt"),
            });
        }










    }
}
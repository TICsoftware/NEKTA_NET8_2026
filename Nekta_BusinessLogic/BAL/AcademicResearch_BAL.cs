using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Nekta_BusinessLogic.DAL;
using Nekta_BusinessLogic.Entity;

namespace Nekta_BusinessLogic.BAL
{
    public class AcademicResearch_BAL : Page_Manage_DAL
    {
        private readonly IConfiguration _configuration;
        public AcademicResearch_BAL(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public AcademyResearchModel GetAcademicResearch_BAL(string pagename, int languageId, int geographyId)
        {
            var model = new AcademyResearchModel();
            var ds = GetContentComponentData_DAL(pagename, languageId, geographyId);

            // Content
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.AR_Content = MapContent(ds.Tables[0].Rows[0]);
            }

            // Components
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                var groupedData = GetGroupedComponents(ds.Tables[1]);

                model.AR_Components = groupedData;
                model.AnitaBorgesList = MapComponentAnitaBorges(groupedData);
                model.OpinionList = MapComponentCommon(groupedData, 2);
                model.TeacherAlwaysList = MapComponentCommon(groupedData, 3);
                model.LegacyList = MapComponentCommon(groupedData, 5);
                model.MentorshipMomentsList = MapComponentMentorship(groupedData);
            }

            return model;
        }

        public AcademyResearchModel GetFellowshipsTraining_BAL(string pagename, int languageId, int geographyId)
        {
            var model = new AcademyResearchModel();
            var ds = GetContentComponentData_DAL(pagename, languageId, geographyId);

            // Content
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.Content = MapContent(ds.Tables[0].Rows[0]);
            }

            // Components
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                var groupedData = GetGroupedComponents(ds.Tables[1]);
                model.Components = groupedData;

                model.ProgrammeOverviewList = MapComponent_AR(groupedData, 1);
                model.FellowshipsWithUsList = MapComponent_AR(groupedData, 2);
                model.EligibilityList = MapComponent_AR(groupedData, 3);
                model.LearningList = MapComponent_AR(groupedData, 4);
                model.ApplicationProcessList = MapComponent_AR(groupedData, 5);
                model.OutcomeList = MapComponent_AR(groupedData, 6);
                model.PartnerWithUsList = MapComponent_AR(groupedData, 7);
            }

            return model;
        }



        public AcademyResearchModel GetPublicationsResources_BAL(string pagename, int languageId, int geographyId)
        {
            var model = new AcademyResearchModel();
            var ds = GetContentComponentListPaged_DAL(pagename, languageId, geographyId);

            // Content
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.Content = MapContent(ds.Tables[0].Rows[0]);
            }

            // Components
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                var groupedData = GetGroupedComponents(ds.Tables[1]);
                model.Components = groupedData;

                model.RecentPublicationsList = MapComponent_AR(groupedData, 1);

                if (ds.Tables[2].Rows.Count > 0)
                {
                    model.TotalCount = Convert.ToInt32(ds.Tables[2].Rows[0]["TotalCount"]);
                }

            }

            return model;
        }

        public AcademyResearchModel GetPublicationsResourcesPaging_BAL(int ContentID, int PageNumber, int PageSize)
        {
            AcademyResearchModel model = new AcademyResearchModel();

            DataSet ds = GetContentComponentsPaging_DAL(ContentID, PageNumber, PageSize);


            if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)
            {
                var groupedData = GetGroupedComponents(ds.Tables[0]);
                model.Components = groupedData;

                model.RecentPublicationsList = MapComponent_AR(groupedData, 1);

                if (ds.Tables[1].Rows.Count > 0)
                {
                    model.TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]);
                }

            }

            return model;
        }




        private ContentViewModel MapContent(DataRow row)
        {
            string baseurl = _configuration["AppSettings:BaseUrl"]?.TrimEnd('/') ?? "";
            var Image = row.Field<string>("masthead_image_path") ?? row.Field<string>("Hmpg_thumbnail") ?? "";
            var canUrl = row.Field<string>("pageurl") ?? "";

            return new ContentViewModel
            {
                ContId = row.Field<int?>("cont_id") ?? 0,
                ContTitle = row.Field<string>("cont_title") ?? "",
                Cont_intro = row.Field<string>("cont_intro") ?? "",
                Cont_hmpg_intro = row.Field<string>("cont_hmpg_intro") ?? "",
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
                CanonicalUrl = Config_Application_Website.GetMetaUrl(baseurl, canUrl),
                cont_meta_image = Config_Application_Website.GetMetaUrl(baseurl, Image)
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
                .OrderBy(g => g.Fields.FirstOrDefault()?.sequence ?? 0) // ✅ safe
                .ToList();
        }


        private List<AcademyResearchCommonModel> MapComponent_AR(List<ComponentGroup> data, int sequence)
        {
            return Config_Application_Website.MapComponent(data, sequence, (group, dict) => new AcademyResearchCommonModel
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
                Popup_Content = Config_Application_Website.GetValue(dict, "popup content"),
                Popup_Display_Title = Config_Application_Website.GetValue(dict, "Popup Display title"),
                Sequence = Config_Application_Website.GetIntValue(dict, "Sequence"),
                //Sequence = group.Fields.First().sequence,
                IsBlock = group.Fields.First().IsBlock,
                Section_title = Config_Application_Website.GetValue(dict, "Block Title", "Section title"),
            });
        }

        private List<AnitaBorgesViewModel> MapComponentAnitaBorges(List<ComponentGroup> data)
        {
            return Config_Application_Website.MapComponent(data, 1, (group, dict) => new AnitaBorgesViewModel
            {
                GroupId = group.GroupId,
                Title = Config_Application_Website.GetValue(dict, "Title"),
                Intro = Config_Application_Website.GetValue(dict, "Intro"),
                Content = Config_Application_Website.GetValue(dict, "Content"),
                LHSIntro = Config_Application_Website.GetValue(dict, "LHS Intro"),
                ThumbnailImage = Config_Application_Website.GetPath(group, "Thumbnail Image"),
                ThumbnailImageAlt = Config_Application_Website.GetValue(dict, "Thumbnail Image Alt"),
                RHSImage = Config_Application_Website.GetPath(group, "RHS Image"),
                RHSImageAlt = Config_Application_Website.GetValue(dict, "RHS Image Alt"),
                Sequence = group.Fields.First().sequence
            });
        }

        private List<CommonViewModel> MapComponentCommon(List<ComponentGroup> data, int sequence)
        {
            return Config_Application_Website.MapComponent(data, sequence, (group, dict) => new CommonViewModel
            {
                GroupId = group.GroupId,
                Title = Config_Application_Website.GetValue(dict, "Title"),
                Intro = Config_Application_Website.GetValue(dict, "Intro"),
                Content = Config_Application_Website.GetValue(dict, "Content"),
                ThumbnailImage = Config_Application_Website.GetPath(group, "Thumbnail Image"),
                ThumbnailImageAlt = Config_Application_Website.GetValue(dict, "Thumbnail Image Alt"),
                Sequence = group.Fields.First().sequence
            });
        }

        private List<MentorshipMomentsViewModel> MapComponentMentorship(List<ComponentGroup> data)
        {
            return Config_Application_Website.MapComponent(data, 4, (group, dict) => new MentorshipMomentsViewModel
            {
                GroupId = group.GroupId,
                Title = Config_Application_Website.GetValue(dict, "Title", "Component Title"),
                Intro = Config_Application_Website.GetValue(dict, "Intro", "Component Intro"),
                DisplayTitle = Config_Application_Website.GetValue(dict, "Component Display title"),
                Content = Config_Application_Website.GetValue(dict, "Content", "Component Content"),
                ThumbnailImage = Config_Application_Website.GetPath(group, "Thumbnail Image"),
                ThumbnailImageAlt = Config_Application_Website.GetValue(dict, "Thumbnail Image Alt"),
                BlockThumbnail = Config_Application_Website.GetPath(group, "Block Thumbnail"),
                BlockThumbnailAlt = Config_Application_Website.GetValue(dict, "Block Thumbnail Image Alt"),
                Url = Config_Application_Website.GetValue(dict, "Url"),
                Sequence = group.Fields.First().sequence,
                IsBlock = group.Fields.First().IsBlock
            });
        }






    }
}
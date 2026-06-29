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
    public class Events_BAL : Events_DAL
    {
        private readonly IConfiguration _configuration;
        public Events_BAL(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public EventsModel GetEventsContent_BAL(string pagename, int languageId, int geographyId)
        {
            var model = new EventsModel();
            var ds = GetEventsContent_DAL(pagename, languageId, geographyId);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.Content = MapContent(ds.Tables[0].Rows[0]);
            }

            // Components
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                var groupedData = GetGroupedComponents(ds.Tables[1]);
                model.Components = groupedData;

                model.UpcomingComponents_List = MapComponentCommon(groupedData, 1);
                model.PastComponents_List = MapComponentCommon(groupedData, 2);
                model.Stay_Updated_List = MapComponentCommon(groupedData, 3);
            }



            // Upcoming Events
            model.UpcomingEvents_List = MapEvents(GetTable(ds, 2));

            // Past Events
            model.PastEvents_List = MapEvents(GetTable(ds, 3));

            // Event Mode
            model.EventMode = MapOptions(
                GetTable(ds, 4),
                "EventModeName",
                "EventModeId"
            );

            // Event Type
            model.EventType = MapOptions(
                GetTable(ds, 5),
                "EventTypeName",
                "EventTypeId"
            );

            return model;
        }

//added for recurring events
 private DateTime GetNextRecurringDate(DateTime eventDate, int recurringDays)
    {
        DateTime nextDate = eventDate;

        while (nextDate < DateTime.Today)
        {
            nextDate = nextDate.AddDays(recurringDays);
        }

        return nextDate;
    }
        public List<PastEvent_List> Search_Past_Events_BAL(int EventTypeId, int EventModeId, DateTime? DateSearch)
        {
            var obj = new List<PastEvent_List>();

            var ds = Search_Past_Events_DAL(EventTypeId, EventModeId, DateSearch);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    obj.Add(new PastEvent_List()
                    {
                        EventCategory = row.Field<string>("EventCategory") ?? "",
                        EventId = row.Field<int?>("EventId") ?? 0,
                        Title = row.Field<string>("Title") ?? "",
                        EventTypeId = row.Field<int?>("EventTypeId") ?? 0,
                        EventTypeName = row.Field<string>("EventTypeName") ?? "",
                        EventModeId = row.Field<int?>("EventModeId") ?? 0,
                        EventModeName = row.Field<string>("EventModeName") ?? "",
                       // EventDate = row.Field<DateTime?>("EventDate") ?? DateTime.MinValue,
                        EventDate = row.Field<DateTime?>("EventDate")
                        ?? DateTime.MinValue,

            DisplayDate = row.Field<DateTime?>("DisplayDate")
                          ?? row.Field<DateTime?>("EventDate")
                          ?? DateTime.MinValue,

            IsRecurring = row.Field<bool?>("IsRecurring")
                          ?? false,

            RecurringAfterDays = row.Field<int?>("RecurringAfterDays"),
                        EventTime = row.Field<string>("EventTime") ?? "",
                        Intro = row.Field<string>("Intro") ?? "",
                        Content = row.Field<string>("Content") ?? "",
                        Speakers = row.Field<string>("Speakers") ?? "",
                        VideoUrl = row.Field<string>("VideoUrl") ?? "",
                        FilePath = row.Field<string>("FilePath") ?? "",
                        Status = row.Field<int?>("Status") ?? 0
                    });
                }
            }


            return obj;
        }


        public PastEvent_List GetEventsById_BAL(int EventId)
        {
            var obj = new PastEvent_List();

            var ds = GetEventsById_DAL(EventId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    obj.EventId = row.Field<int?>("EventId") ?? 0;
                    obj.Title = row.Field<string>("Title") ?? "";
                    obj.Intro = row.Field<string>("Intro") ?? "";
                    obj.Content = row.Field<string>("Content") ?? "";
                }
            }


            return obj;
        }



        private DataTable GetTable(DataSet ds, int index)
        {
            return ds.Tables.Count > index ? ds.Tables[index] : null;
        }

        // private List<PastEvent_List> MapEvents(DataTable table)
        // {
        //     if (table == null || table.Rows.Count == 0)
        //         return new List<PastEvent_List>();

        //     return table.AsEnumerable().Select(row => new PastEvent_List
        //     {
        //         EventCategory = row.Field<string>("EventCategory") ?? "",
        //         EventId = row.Field<int?>("EventId") ?? 0,
        //         Title = row.Field<string>("Title") ?? "",
        //         EventTypeId = row.Field<int?>("EventTypeId") ?? 0,
        //         EventTypeName = row.Field<string>("EventTypeName") ?? "",
        //         EventModeId = row.Field<int?>("EventModeId") ?? 0,
        //         EventModeName = row.Field<string>("EventModeName") ?? "",
        //         EventDate = row.Field<DateTime?>("EventDate") ?? DateTime.MinValue,
        //         EventTime = row.Field<string>("EventTime") ?? "",
        //         Intro = row.Field<string>("Intro") ?? "",
        //         Content = row.Field<string>("Content") ?? "",
        //         Speakers = row.Field<string>("Speakers") ?? "",
        //         VideoUrl = row.Field<string>("VideoUrl") ?? "",
        //         FilePath = row.Field<string>("FilePath") ?? "",
        //         Status = row.Field<int?>("Status") ?? 0,
        //         Event_encryptId = row.Field<int?>("EventId") == 0 ? "" : CryptoEngine.Encrypt(row.Field<int?>("EventId").ToString())
        //     }).ToList();
        // }

        private List<Options_List> MapOptions(
            DataTable table,
            string titleColumn,
            string idColumn)
     {
            if (table == null || table.Rows.Count == 0)
                 return new List<Options_List>();

            return table.AsEnumerable().Select(row => new Options_List
           {
                title = row.Field<string>(titleColumn) ?? "",
                id = row.Field<int?>(idColumn) ?? 0
            }).ToList();
        }
private List<PastEvent_List> MapEvents(DataTable table)
{
    List<PastEvent_List> list = new();

    if (table == null || table.Rows.Count == 0)
        return list;

    foreach (DataRow row in table.Rows)
    {
        list.Add(new PastEvent_List
        {
            EventCategory = row.Field<string>("EventCategory") ?? "",

            EventId = row.Field<int?>("EventId") ?? 0,

            Title = row.Field<string>("Title") ?? "",

            EventTypeId = row.Field<int?>("EventTypeId") ?? 0,
            EventTypeName = row.Field<string>("EventTypeName") ?? "",

            EventModeId = row.Field<int?>("EventModeId") ?? 0,
            EventModeName = row.Field<string>("EventModeName") ?? "",

            EventDate = row.Field<DateTime?>("EventDate")
                        ?? DateTime.MinValue,

            DisplayDate = row.Field<DateTime?>("DisplayDate")
                          ?? row.Field<DateTime?>("EventDate")
                          ?? DateTime.MinValue,

            IsRecurring = row.Field<bool?>("IsRecurring")
                          ?? false,

            RecurringAfterDays = row.Field<int?>("RecurringAfterDays"),

            EventTime = row.Field<string>("EventTime") ?? "",

            Intro = row.Field<string>("Intro") ?? "",

            Content = row.Field<string>("Content") ?? "",

            Speakers = row.Field<string>("Speakers") ?? "",

            VideoUrl = row.Field<string>("VideoUrl") ?? "",

            FilePath = row.Field<string>("FilePath") ?? "",

            Status = row.Field<int?>("Status") ?? 0,

            Event_encryptId =
                row.Field<int?>("EventId") == 0
                ? ""
                : CryptoEngine.Encrypt(
                    row.Field<int?>("EventId").ToString())
        });
    }

    return list;
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
                .OrderBy(g => g.Fields.FirstOrDefault()?.sequence ?? 0).ToList();
        }



        private List<AboutCommonModel> MapComponentCommon(List<ComponentGroup> data, int sequence)
        {
            return Config_Application_Website.MapComponent(data, sequence, (group, dict) => new AboutCommonModel
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
                Component_right_image = Config_Application_Website.GetPath(group, "Component right image"),
                Component_Right_image_alt = Config_Application_Website.GetValue(dict, "Component Right image alt"),
            });
        }



        public string Events_Registration_Insert_BAL(Entity.Register_Event obj)
        {
            DataTable dt=new();
            dt=Events_Registration_Insert_DAL(obj);
            if(dt.Rows.Count>0)
            {
                return dt.Rows[0]["Title"].ToString();
            }
            else
             return "";
        }

    }
}
using System.ComponentModel.DataAnnotations;

namespace Nekta_BusinessLogic.Entity;

public class Events
{
    public int Id { get; set; }
    public string? Title { get; set; }


}

public class EventsModel
{
    public ContentViewModel? Content { get; set; }
    public List<ComponentGroup> Components { get; set; } = new();

    public List<AboutCommonModel> UpcomingComponents_List { get; set; } = new();
    public List<AboutCommonModel> PastComponents_List { get; set; } = new();
    public List<AboutCommonModel> Stay_Updated_List { get; set; } = new();

    public List<PastEvent_List> UpcomingEvents_List { get; set; } = new();
    public List<PastEvent_List> PastEvents_List { get; set; } = new();

    public Events_Detail? SearchEvent { get; set; }
    public List<Options_List>? EventType { get; set; }
    public List<Options_List>? EventMode { get; set; }


    public int Past_Event_count { get; set; }
    public int Upcoming_Event_count { get; set; }
    public int TotalCount { get; set; }


}

public class Events_Detail
{
    public DateTime? SearchDate { get; set; }
    public int? EventType_Id { get; set; }
    public int? EventModeId { get; set; }
}


public class PastEvent_List
{
    public string EventCategory { get; set; }
    public int EventId { get; set; }
    public string Title { get; set; }
    public int EventTypeId { get; set; }
    public string EventTypeName { get; set; }
    public int EventModeId { get; set; }
    public string EventModeName { get; set; }
    public DateTime EventDate { get; set; }
    public string EventTime { get; set; }
    public string Intro { get; set; }
    public string Content { get; set; }
    public string Speakers { get; set; }
    public string VideoUrl { get; set; }
    public string FilePath { get; set; }
    public bool IsRecurring { get; set; }

public int? RecurringAfterDays { get; set; }


public DateTime actualDate  { get; set; }
public DateTime DisplayDate { get; set; }
    public int Status { get; set; }
    public string Event_encryptId { get; set; }

}

public class EventsCommonModel
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

    public string Component_right_image { get; set; }
    public string Component_Right_image_alt { get; set; }

}

public class Register_Event
{
    public int EventId { get; set; }
    public string? FullName { get; set; }
    public string? Contact_number { get; set; }
    public string? Email_Id { get; set; }
    public string? Designation { get; set; }

}
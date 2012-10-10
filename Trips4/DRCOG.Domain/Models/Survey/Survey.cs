using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DRCOG.Domain;
using DRCOG.Domain.Models;
using System.Web;

namespace DRCOG.Domain.Models.Survey
{
    public class Survey
    {

        //!! need to find a way here to get the certification form info to show.
        private int _buttonActionCode;

        public int Id { get; set; }
        public string Name { get; set; }
        //public string Year { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime AcceptedDate { get; set; }
        public bool ShowCertification { get; set; }

        public double OpeningDateForJavascript 
        {
            get
            {
                return OpeningDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            }
        }
        public double ClosingDateForJavascript
        {
            get
            {
                return ClosingDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            }
        }


        public List<Project> AgencyProjectList;
        public IList<Contact> AgencySponsorContacts;

        public ProjectSponsorsModel ProjectSponsorsModel { get; set; }

        public string ButtonActionText { get; set; }
        public int ButtonActionCode 
        {
            get { return _buttonActionCode; }
            set
            {
                _buttonActionCode = value;
                switch (value)
                {
                    case (int)SurveyButtonAction.Open:
                        //this.ButtonActionText = "Open";
                        //break;
                    case (int)SurveyButtonAction.SetClose:
                        //this.ButtonActionText = "Pick Closing";
                        //break;
                    case (int)SurveyButtonAction.Close:
                        //this.ButtonActionText = "Close Now";
                        //break;
                    case (int)SurveyButtonAction.SetOpen:
                        this.ButtonActionText = "Pick Dates";
                        break;
                    
                }
            }
        }

        public string GetStatusText()
        {
            if (this.OpeningDate.CompareTo(DateTime.UtcNow) <= 0)
            {
                if (!this.ClosingDate.Equals(DateTime.MinValue) && this.ClosingDate.CompareTo(DateTime.UtcNow) < 0) // we are closed
                {
                    this.ButtonActionCode = (int)SurveyButtonAction.Open;
                    return "Closed " + ClosingDate.ToShortDateString();
                }
                else
                {
                    if(this.OpeningDate.Equals(DateTime.MinValue)) // no dates set
                    {
                        this.ButtonActionCode = (int)SurveyButtonAction.SetOpen;
                        return "Not yet Opened";
                    }

                    // definitely open!!
                    this.ButtonActionCode = (int)SurveyButtonAction.Close;
                    return "Opened " + OpeningDate.ToShortDateString();
                }
            }

            this.ButtonActionCode = (int)SurveyButtonAction.Open;
            return "Opens " + OpeningDate.ToShortDateString();

        }

        public bool IsAdmin()
        {
            return HttpContext.Current.User.IsInRole("Survey Administrator") || HttpContext.Current.User.IsInRole("Administrator") ? true : false;
        }
        public bool IsOpen()
        {
            return (OpeningDate <= DateTime.Now && DateTime.Now < ClosingDate);
        }

        public bool IsEditable()
        {
            bool CanEdit;
            CanEdit = IsAdmin();
            if (CanEdit || IsOpen())
                return true;
            return false;
        }

        public SponsorOrganization SponsorsOrganization { get; set; }
    }

    public enum SurveyButtonAction
    {
        SetOpen = 1
        ,
        SetClose = 2
        ,
        Open = 3
        ,
        Close = 4
    }

    public class Surveys : CollectionBase
    {
        public int Add(Survey value)
        {
            return (List.Add(value));
        }

        public Survey Item(int index)
        {
            return (Survey)List[index];
        }
    }


}

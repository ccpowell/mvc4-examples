using System;
using System.Web.Mvc;
using System.Collections.Generic;
using DRCOG.Domain.Models;


namespace DRCOG.Domain.ViewModels.TIP
{
    public class StatusViewModel : TipBaseViewModel
    {

        public TipStatusModel TipStatus { get; set; }

        /// <summary>
        /// Flag indicating if the 
        /// </summary>
        /// <returns></returns>
        public bool IsEditable()
        {
            bool resp = !this.TipStatus.IsPrevious;
            if(!resp && (this.TipStatus.IsPending || this.TipStatus.IsCurrent)){
                resp = true;
            }
            return resp;
        }
        
        /// <summary>
        /// Lookup table associated with the Status of the TIP
        /// </summary>
        private Dictionary<int, string> _statusList;

        /// <summary>
        /// 
        /// </summary>
        public StatusViewModel()
        {
            _statusList = new Dictionary<int, string>();
        }

        /// <summary>
        /// Status list
        /// </summary>
        public Dictionary<int, string> StatusList
        {
            get { return _statusList; }
            set { _statusList = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public SelectList GetAvailableTipStatusSelectList()
        //{
        //    return new SelectList(this.StatusList, "key", "value");
        //}

    }
}

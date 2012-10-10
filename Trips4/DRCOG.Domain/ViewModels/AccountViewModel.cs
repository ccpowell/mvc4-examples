//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/6/2009 4:30:51 PM
// Description:
//
//======================================================
using System;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ViewModels
{
    /// <summary>
    /// Model for the AccountDetailView
    /// </summary>
    public class AccountViewModel
    {
        private AccountDetailModel _detailModel;
        private AccountSearchModel _searchModel;

        public AccountViewModel(AccountDetailModel detailModel, AccountSearchModel searchModel)
        {
            _detailModel = detailModel;
            _searchModel = searchModel;
        }

        public AccountDetailModel DetailModel
        {   //read only/ must be passed to the constructor that way we can be sure to always set currentuser on it when currentuser is set on this
            get { return _detailModel; }
        }

        public AccountSearchModel SearchModel
        {   //read only/ must be passed to the constructor that way we can be sure to always set currentuser on it when currentuser is set on this
            get { return _searchModel; }
        }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>The current user.</value>
        //public Account CurrentUser
        //{
        //    get
        //    {
        //        return base.CurrentUser;
        //    }
        //    set
        //    {
        //        //Set current user on the baseModel
        //        base.CurrentUser = value;
        //        ////Pass into the sub-models
        //        //if (_detailModel != null)
        //        //{
        //        //    _detailModel.CurrentUser = value;
        //        //}
        //        //if (_searchModel != null)
        //        //{
        //        //    _searchModel.CurrentUser = value;
        //        //}
        //    }
        //}
        
    }
}
